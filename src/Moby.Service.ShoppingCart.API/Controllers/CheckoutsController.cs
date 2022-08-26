using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moby.Service.ShoppingCart.API.Messages;
using Moby.Service.ShoppingCart.API.Models.Dto;
using Moby.Service.ShoppingCart.API.Repository;
using Moby.ServiceBus;

namespace Moby.Service.ShoppingCart.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckoutsController : ControllerBase
{
    private readonly ICartManager _cartManager;
    private readonly IMessageBusManager _messageBusManager;
    private readonly IConfiguration _configuration;
    protected ResponseDto Response;

    public CheckoutsController(ICartManager cartManager, IMessageBusManager messageBusManager, IConfiguration configuration)
    {
        _cartManager = cartManager;
        Response = new();
        _messageBusManager = messageBusManager;
        _configuration = configuration;
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

            var connectionString = _configuration.GetConnectionString("AzureServiceBus");

            await _messageBusManager.PublishMessage(checkoutMessage, "checkouttopic", connectionString);

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
