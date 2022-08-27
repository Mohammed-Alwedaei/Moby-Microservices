using Moby.Services.ShoppingCart.API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace Moby.Services.ShoppingCart.API.Models.Dto;

public class CartDetailsDto
{
    public int Id { get; set; }

    public int CartHeaderId { get; set; }

    public CartHeaderDto CartHeader { get; set; }

    public int ProductId { get; set; }

    public ProductDto Product { get; set; }

    public int Count { get; set; }
}
