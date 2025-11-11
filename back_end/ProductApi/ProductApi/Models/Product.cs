using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Product
    {
        public int Id { get; set; } // Clé primaire

        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0.01, 10000, ErrorMessage = "Le prix doit être entre 0.01 et 10 000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(0, 1000, ErrorMessage = "La quantité doit être entre 0 et 1000")]
        public int Quantity { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}