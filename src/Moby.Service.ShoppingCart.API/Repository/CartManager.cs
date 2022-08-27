using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moby.ServiceBus;
using Moby.Services.ShoppingCart.API.DbContexts;
using Moby.Services.ShoppingCart.API.Models;
using Moby.Services.ShoppingCart.API.Models.Dto;

namespace Moby.Services.ShoppingCart.API.Repository;

public class CartManager : ICartManager
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CartManager(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CartDto> GetCartByUserIdAsync(string userId)
    {
        var cartHeaderFromDb = await _db.CartHeaders
            .FirstOrDefaultAsync(h => h.UserId == userId);

        var cart = new CartModel()
        {
            CartHeader = cartHeaderFromDb
        };

        cart.CartDetails = _db.CartDetails
            .Where(d => d.CartHeaderId == cartHeaderFromDb.Id)
            .Include(d => d.Product);

        return _mapper.Map<CartModel, CartDto>(cart);
    }

    public async Task<CartDto> CreateCartAsync(CartDto cart)
    {
        var cartToBeCreated = _mapper.Map<CartDto, CartModel>(cart);

        //If product is null create one
        var productFromDb = await _db.Products
            .FirstOrDefaultAsync(p => p.Id == cartToBeCreated.CartDetails.FirstOrDefault().ProductId);

        if (productFromDb is null)
        {
            _db.Products.Add(cartToBeCreated.CartDetails.FirstOrDefault().Product);
            await _db.SaveChangesAsync();
        }

        //If cart header is null create one
        var cartHeaderFromDb = await _db.CartHeaders
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.UserId == cartToBeCreated.CartHeader.UserId);

        if (cartHeaderFromDb is null)
        {
            //Create cart header
            _db.CartHeaders.Add(cartToBeCreated.CartHeader);
            await _db.SaveChangesAsync();

            //Create cart details and reference the previous cart header
            cartToBeCreated.CartDetails.FirstOrDefault().CartHeaderId = cartToBeCreated.CartHeader.Id;

            cartToBeCreated.CartDetails.FirstOrDefault().Product = null;
            _db.CartDetails.Add(cartToBeCreated.CartDetails.FirstOrDefault());

            await _db.SaveChangesAsync();
        }

        var cartDetailsFromDb = await _db.CartDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == cartToBeCreated.CartDetails.FirstOrDefault().Id
                                      && d.CartHeaderId == cartToBeCreated.CartHeader.Id);

        if (cartDetailsFromDb is null)
        {
            var createdCartHeader = await _db.CartHeaders
                .FirstOrDefaultAsync(h => h.UserId == cartToBeCreated.CartHeader.UserId);

            cartToBeCreated.CartDetails.FirstOrDefault().CartHeaderId = createdCartHeader.Id;
            cartToBeCreated.CartDetails.FirstOrDefault().Product = null;

            _db.CartDetails.Add(cartToBeCreated.CartDetails.FirstOrDefault());

            await _db.SaveChangesAsync();
        }

        return _mapper.Map<CartModel, CartDto>(cartToBeCreated);
    }

    public async Task<CartDto> UpdateCartAsync(CartDto cart)
    {
        var cartToBeUpdated = _mapper.Map<CartDto, CartModel>(cart);

        var cartHeaderFromDb = await _db.CartHeaders
            .FirstOrDefaultAsync(h => h.Id == cartToBeUpdated.CartHeader.Id);

        var cartDetailsFromDb = await _db.CartDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == cartToBeUpdated.CartDetails.FirstOrDefault().Id
                                      && d.CartHeaderId == cartToBeUpdated.CartHeader.Id);
        //Create cart details 
        if (cartDetailsFromDb is not null)
        {
            cartToBeUpdated.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
            _db.CartDetails.Update(cartToBeUpdated.CartDetails.FirstOrDefault());
            await _db.SaveChangesAsync();
        }

        return _mapper.Map<CartModel, CartDto>(cartToBeUpdated);
    }

    public async Task<bool> RemoveProductFromCartAsync(int cartDetailsId)
    {
        try
        {
            var cartDetails = await _db.CartDetails
                .FirstOrDefaultAsync(d => d.Id == cartDetailsId);

            var totalCartProductsCount = _db.CartDetails
                .Where(d => d.CartHeaderId == cartDetails.CartHeaderId)
                .Count();

            _db.CartDetails.Remove(cartDetails);

            if (totalCartProductsCount == 1)
            {
                var cartHeaderToRemove = _db.CartHeaders
                    .Where(h => h.Id == cartDetails.CartHeaderId)
                    .FirstOrDefault();

                _db.CartHeaders.Remove(cartHeaderToRemove);
            }

            await _db.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ClearCartByIdAsync(string userId)
    {
        var cartHeaderFromDb = await _db.CartHeaders
            .FirstOrDefaultAsync(h => h.UserId == userId);

        if (cartHeaderFromDb is not null)
        {
            _db.CartDetails.RemoveRange(_db.CartDetails.Where(h => h.Id == cartHeaderFromDb.Id));

            _db.CartHeaders.Remove(cartHeaderFromDb);

            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> ApplyCoupon(string userId, string couponCode)
    {
        var cartHeaderFromDb = await _db.CartHeaders
            .FirstOrDefaultAsync(h => h.UserId == userId);

        cartHeaderFromDb.CouponCode = couponCode;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveCoupon(string userId)
    {
        var cartHeaderFromDb = await _db.CartHeaders
            .FirstOrDefaultAsync(h => h.UserId == userId);

        cartHeaderFromDb.CouponCode = "";

        await _db.SaveChangesAsync();

        return true;
    }
}
