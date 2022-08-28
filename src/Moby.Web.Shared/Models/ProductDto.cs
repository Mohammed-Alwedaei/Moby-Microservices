using Newtonsoft.Json;

namespace Moby.Web.Shared.Models;

public class ProductDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    [JsonProperty("Description")]
    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? ImageUrl { get; set; }
}