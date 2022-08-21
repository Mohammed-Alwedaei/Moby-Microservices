namespace Moby.Service.ShoppingCart.API.Models;

public class CartHeaderModel
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? CouponCode { get; set; } 
}
