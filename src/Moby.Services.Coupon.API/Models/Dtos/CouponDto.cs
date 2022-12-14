using System.ComponentModel.DataAnnotations;

namespace Moby.Services.Coupon.API.Models.Dtos;

public class CouponDto
{
    [Key]
    public int Id { get; set; }

    public string Code { get; set; }

    public decimal DiscountAmount { get; set; }
}
