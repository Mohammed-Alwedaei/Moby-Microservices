namespace Moby.Service.ShoppingCart.API.Models;

public class CartModel
{
    public CartHeaderModel CartHeader { get; set; }

    public IEnumerable<CartDetailsModel> CartDetails { get; set; }  
}
