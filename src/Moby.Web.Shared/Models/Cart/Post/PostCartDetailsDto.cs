using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.Web.Shared.Models.Cart.Post;
public class PostCartDetailsDto
{
    public int Id { get; set; }

    public int CartHeaderId { get; set; }

    public virtual CartHeaderDto? CartHeader { get; set; }

    public int ProductId { get; set; }

    public virtual ProductDto Product { get; set; }

    public int Count { get; set; }
}
