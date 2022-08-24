using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.Web.Shared.Models.Cart;
public class CouponDto
{
    public int Id { get; set; }

    public string Code { get; set; }

    public decimal DiscountAmount { get; set; }
}
