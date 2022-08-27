﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace Moby.Services.ShoppingCart.API.Models;

public class CartDetailsModel
{
    public int Id { get; set; }

    public int CartHeaderId { get; set; }

    [ForeignKey(nameof(CartHeaderId))]
    public CartHeaderModel CartHeader { get; set; }

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public ProductModel Product { get; set; }

    public int Count { get; set; }
}
