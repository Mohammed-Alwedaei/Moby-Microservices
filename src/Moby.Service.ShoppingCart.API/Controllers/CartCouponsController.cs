using Microsoft.AspNetCore.Mvc;
using Moby.Service.ShoppingCart.API.Models.Dto;
using Moby.Service.ShoppingCart.API.Repository;

namespace Moby.Service.ShoppingCart.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CartCouponsController : ControllerBase
{
    private readonly ICartManager _cartManager;
    protected ResponseDto Response;

    public CartCouponsController(ICartManager cartManager)
    {
        _cartManager = cartManager;
        Response = new();
    }

    [HttpPost]
    [Route("{userId}/{couponCode}")]
    public async Task<IActionResult> ApplyCouponToCart(string userId, string couponCode)
    {
        try
        {
            var isApplied = await _cartManager.ApplyCoupon(userId, couponCode);


            Response.Results = true;

            return Ok(Response);
        }
        catch (Exception exception)
        {
            Response.IsSuccess = false;

            Response.Errors = new()
            {
                exception.Message
            };

            return BadRequest(Response);
        }
    }

    [HttpPost]
    [Route("{userId}")]
    public async Task<IActionResult> RemoveCouponToCart(string userId)
    {
        try
        {
            var cart = await _cartManager.RemoveCoupon(userId);

            Response.Results = cart;

            return Ok(Response);
        }
        catch (Exception exception)
        {
            Response.IsSuccess = false;

            Response.Errors = new()
            {
                exception.Message
            };

            return BadRequest(Response);
        }
    }
}
