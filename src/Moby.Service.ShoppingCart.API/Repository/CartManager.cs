﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moby.Service.ShoppingCart.API.DbContexts;
using Moby.Service.ShoppingCart.API.Models;
using Moby.Service.ShoppingCart.API.Models.Dto;

namespace Moby.Service.ShoppingCart.API.Repository;

public class CartManager : ICartManager
{
    private ApplicationDbContext _db;
    private IMapper _mapper;

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
            .FirstOrDefaultAsync(h => h.Id == cartToBeCreated.CartHeader.Id);

        if (cartHeaderFromDb is null)
        {
            //Create cart header
            _db.CartHeaders.Add(cartToBeCreated.CartHeader);
            await _db.SaveChangesAsync();

            //Create cart details and reference the previous cart header
            cartToBeCreated.CartDetails.FirstOrDefault().Id = cartToBeCreated.CartHeader.Id;

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
        if (cartDetailsFromDb is null)
        {
            cartToBeUpdated.CartDetails.FirstOrDefault().Id = cartHeaderFromDb.Id;
            cartToBeUpdated.CartDetails.FirstOrDefault().Product = null;

            _db.CartDetails.Add(cartToBeUpdated.CartDetails.FirstOrDefault());

            await _db.SaveChangesAsync();
        }
        else
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
            var cartDetails = await _db.CartDetails.FirstOrDefaultAsync(d => d.Id == cartDetailsId);

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

    public async Task<bool> ClearCartByIdAsync(int cartId)
    {
        var cartHeaderFromDb = await _db.CartHeaders
            .FirstOrDefaultAsync(h => h.Id == cartId);

        if (cartHeaderFromDb is not null)
        {
            _db.CartDetails.RemoveRange(_db.CartDetails.Where(h => h.Id == cartHeaderFromDb.Id));

            _db.CartHeaders.Remove(cartHeaderFromDb);

            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }
}