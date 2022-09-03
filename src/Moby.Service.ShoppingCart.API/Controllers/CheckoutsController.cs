using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moby.ServiceBus;
using Moby.Services.ShoppingCart.API.Messages;
using Moby.Services.ShoppingCart.API.Models.Dto;
using Moby.Services.ShoppingCart.API.Repository;

namespace Moby.Services.ShoppingCart.API.Controllers;

[Route("api/[controller]")]
[Authorize("ReadAccess")]
[ApiController]
public class CheckoutsController : ControllerBase
{
    private readonly ICartManager _cartManager;
    private readonly IMessageBusManager _messageBusManager;
    private readonly IConfiguration _configuration;
    private readonly ICouponManager _couponManager;
    protected ResponseDto Response;

    public CheckoutsController(ICartManager cartManager, 
        IMessageBusManager messageBusManager, 
        IConfiguration configuration, 
        ICouponManager couponManager)
    {
        _cartManager = cartManager;
        Response = new();
        _messageBusManager = messageBusManager;
        _configuration = configuration;
        _couponManager = couponManager;
    }

    [HttpPost]
    public async Task<IActionResult> Checkout([FromBody] CheckoutMessage checkoutMessage)
    {
        try
        {
            var checkout = checkoutMessage;
            var cartFromDb = await _cartManager.GetCartByUserIdAsync(checkoutMessage.UserId);

            if (cartFromDb is null)
                return BadRequest();

            checkout.Details = cartFromDb.CartDetails;

            if (!string.IsNullOrEmpty(checkout.CouponCode))
            {
                var coupon = await _couponManager.GetCouponAsync(checkout.CouponCode);

                if (coupon.DiscountAmount != checkout.TotalAfterDiscount)
                {
                    Response.IsSuccess = false;
                    Response.Errors = new List<string>
                    {
                        "The discount amount has changed, Please confirm"
                    };
                    Response.Message = "The discount amount has changed, Please confirm";

                    return BadRequest(Response);
                }
            }

            var connectionString = _configuration.GetConnectionString("AzureServiceBus");

            await _messageBusManager.PublishMessage(checkoutMessage, "checkouttopic", connectionString);
            await _cartManager.ClearCartByIdAsync(checkoutMessage.UserId);

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
