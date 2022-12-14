using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.Web.Shared.Models.Cart;
public class CartDetailsDto
{
    public int Id { get; set; }

    public int? CartHeaderId { get; set; }

    public virtual CartHeaderDto? CartHeader { get; set; }

    public int ProductId { get; set; }

    public virtual ProductDto? Product { get; set; }

    public int Count { get; set; }
}
