

using Moby.Services.Order.API.Models;
using Moby.Services.Order.API.Models.Dto;

namespace Moby.Services.Order.API.Messages;

public class CheckoutMessage
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? CouponCode { get; set; }

    public decimal Total { get; set; }

    public decimal? TotalAfterDiscount { get; set; }

    public DateTime PickUpDate { get; set; } = DateTime.Now;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CardNumber { get; set; }

    public string? ExpiryDate { get; set; }

    public string? Cvv { get; set; }

    public int? ItemsCount { get; set; }

    public IEnumerable<CartDetailsDto>? Details { get; set; }
}
