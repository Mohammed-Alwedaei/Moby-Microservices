using System.ComponentModel.DataAnnotations;

namespace Mango.Services.Product.API.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(350)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(250)]
        public string? ImageUrl { get; set; }
    }
}
