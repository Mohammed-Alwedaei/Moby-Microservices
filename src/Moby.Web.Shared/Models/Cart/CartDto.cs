namespace Moby.Web.Shared.Models.Cart;

public class CartDto
{
    public CartHeaderDto CartHeader { get; set; }

    public IEnumerable<CartDetailsDto> CartDetails { get; set; }
}
