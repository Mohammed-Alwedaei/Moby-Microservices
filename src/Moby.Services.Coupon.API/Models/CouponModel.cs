using System.ComponentModel.DataAnnotations;

namespace Moby.Services.Coupon.API.Models;

public class CouponModel
{
    [Key]
    public int Id { get; set; }

    public string Code { get; set; }

    public double DiscountAmount { get; set; }
}
