namespace Moby.Service.ShoppingCart.API.Models.Dto;

public class CartHeaderDto
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? CouponCode { get; set; } 
}
