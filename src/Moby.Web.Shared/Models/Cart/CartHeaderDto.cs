using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.Web.Shared.Models.Cart;
public class CartHeaderDto
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string CouponCode { get; set; }

    public decimal Total { get; set; }

    public decimal TotalAfterDiscount { get; set; }

    public DateTime PickUpDate { get; set; } = DateTime.Now;

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string CardNumber { get; set; }

    public string Cvv { get; set; }

    public string ExpiryDate { get; set; }
}
