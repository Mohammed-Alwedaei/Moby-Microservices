using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moby.Services.Coupon.API.Models.Dtos;
using Moby.Services.Coupon.API.Repository;

namespace Moby.Services.Coupon.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CouponsController : ControllerBase
{
    private readonly ICouponManager _couponManager;
    protected ResponseDto Response;

    public CouponsController(ICouponManager couponManager)
    {
        _couponManager = couponManager;
        this.Response = new();
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetCouponByCode(string code)
    {
        try
        {
            var cart = await _couponManager.GetCouponByCodeAsync(code);

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
