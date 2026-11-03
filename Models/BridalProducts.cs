using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace Lab_8.Models
{
    public class BridalProduct
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product title is required.")]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Image { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be zero or greater.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or greater.")]
        public int Quantity { get; set; }

        public string Status { get; set; } = "Available";
    }
}