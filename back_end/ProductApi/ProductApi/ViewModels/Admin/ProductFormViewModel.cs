using System.ComponentModel.DataAnnotations;

namespace ProductApi.ViewModels.Admin
{
    public class ProductFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        [Display(Name = "Nom du produit")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0.01, 10000, ErrorMessage = "Le prix doit être entre 0.01 et 10 000 €")]
        [Display(Name = "Prix (€)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(0, 1000, ErrorMessage = "La quantité doit être entre 0 et 1000")]
        [Display(Name = "Quantité en stock")]
        public int Quantity { get; set; }

        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        [Display(Name = "URL de l'image")]
        public string? ImageUrl { get; set; }

        public bool IsEdit => Id.HasValue && Id > 0;
        public string FormTitle => IsEdit ? "Modifier le produit" : "Nouveau produit";
        public string SubmitText => IsEdit ? "Modifier" : "Créer";
    }
}
