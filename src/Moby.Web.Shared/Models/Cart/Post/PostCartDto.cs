using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.Web.Shared.Models.Cart.Post;
public class PostCartDto
{
    public PostCartHeaderDto CartHeader { get; set; }

    public IEnumerable<PostCartDetailsDto> CartDetails { get; set; }
}
