using System.ComponentModel.DataAnnotations;

namespace Moby.Services.ShoppingCart.API.Models;

public class CartHeaderModel
{
    [Key]
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? CouponCode { get; set; }
}
