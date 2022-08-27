using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moby.Services.Order.API.Models;

public class OrderDetailsModel
{
    [Key]
    public int Id { get; set; }

    public int CartHeaderId { get; set; }

    [ForeignKey(nameof(CartHeaderId))]
    public virtual OrderHeaderModel CartHeader { get; set; }

    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal ProductPrice { get; set; }

    public int Count { get; set; }
}
