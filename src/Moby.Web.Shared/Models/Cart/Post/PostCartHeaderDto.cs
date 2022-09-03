using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.Web.Shared.Models.Cart.Post;
public class PostCartHeaderDto
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string CouponCode { get; set; } = string.Empty;
}
