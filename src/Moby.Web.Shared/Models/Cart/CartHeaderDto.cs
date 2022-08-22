using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.Web.Shared.Models.Cart;
public class CartHeaderDto
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? CouponCode { get; set; }
}
