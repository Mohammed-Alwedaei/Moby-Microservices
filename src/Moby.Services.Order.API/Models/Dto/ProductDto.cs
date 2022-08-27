using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moby.Services.Order.API.Models.Dto;

public class ProductDto
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? ImageUrl { get; set; }
}
