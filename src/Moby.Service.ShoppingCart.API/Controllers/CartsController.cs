using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moby.Service.ShoppingCart.API.Messages;
using Moby.Service.ShoppingCart.API.Models.Dto;
using Moby.Service.ShoppingCart.API.Repository;

namespace Moby.Service.ShoppingCart.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly ICartManager _cartManager;
    protected ResponseDto Response;

    public CartsController(ICartManager cartManager)
    {
        _cartManager = cartManager;
        Response = new();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCartById(string userId)
    {
        try
        {
            var cart = await _cartManager.GetCartByUserIdAsync(userId);

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

    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] CartDto cartToCreate)
    {
        try
        {
            var cart = await _cartManager.CreateCartAsync(cartToCreate);

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

    [HttpPut]
    public async Task<IActionResult> UpdateCart([FromBody] CartDto cartToUpdate)
    {
        try
        {
            var cart = await _cartManager.UpdateCartAsync(cartToUpdate);

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

    [HttpDelete("{cartDetailsId:int}")]
    public async Task<IActionResult> RemoveProductFromCart(int cartDetailsId)
    {
        try
        {
            var isSuccessDeletion = await _cartManager.RemoveProductFromCartAsync(cartDetailsId);

            Response.Results = isSuccessDeletion;

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

    [HttpDelete("/api/[controller]/clear/{cartId:int}")]
    public async Task<IActionResult> ClearCartByIdAsync(int cartId)
    {
        try
        {
            var isSuccessDeletion = await _cartManager.ClearCartByIdAsync(cartId);

            Response.Results = isSuccessDeletion;

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
