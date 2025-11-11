using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductService _productService;

        public IndexModel(ProductService productService)
        {
            _productService = productService;
        }

        public List<Product> Products { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Vérifier les droits admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Auth/Login");
            }

            Products = await _productService.GetAllProductsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string name, decimal price, int quantity, string imageUrl)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product != null)
                {
                    product.Name = name;
                    product.Price = price;
                    product.Quantity = quantity;
                    product.ImageUrl = imageUrl;

                    await _productService.UpdateProductAsync(product);
                    TempData["Success"] = "Produit modifié avec succès!";
                }
                return RedirectToPage();
            }
            catch (Exception)
            {
                TempData["Error"] = "Erreur lors de la modification du produit";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                TempData["Success"] = "Produit supprimé avec succès!";
                return RedirectToPage();
            }
            catch (Exception)
            {
                TempData["Error"] = "Erreur lors de la suppression du produit";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostCreateAsync(string name, decimal price, int quantity, string imageUrl)
        {
            try
            {
                var product = new Product
                {
                    Name = name,
                    Price = price,
                    Quantity = quantity,
                    ImageUrl = imageUrl
                };

                await _productService.CreateProductAsync(product);
                TempData["Success"] = "Produit créé avec succès!";
                return RedirectToPage();
            }
            catch (Exception)
            {
                TempData["Error"] = "Erreur lors de la création du produit";
                return RedirectToPage();
            }
        }

        // ❌ SUPPRIMEZ COMPLÈTEMENT CETTE MÉTHODE - ELLE N'EST PAS UTILE DANS L'ADMIN
        // public async Task<IActionResult> OnPostAddToCart(int productId)
        // {
        //     ... tout le code ...
        // }
    }
}