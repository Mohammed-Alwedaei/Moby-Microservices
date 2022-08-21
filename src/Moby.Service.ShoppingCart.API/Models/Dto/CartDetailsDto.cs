using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace Moby.Service.ShoppingCart.API.Models.Dto;

public class CartDetailsDto
{
    public int Id { get; set; }

    public int CartHeaderId { get; set; }

    [ForeignKey(nameof(CartHeaderId))]
    public virtual CartHeaderDto? CartHeader { get; set; }

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual ProductModel? Product { get; set; }

    public int Count { get; set; }
}
