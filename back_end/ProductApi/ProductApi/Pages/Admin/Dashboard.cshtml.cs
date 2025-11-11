using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _environment;

        public DashboardModel(IProductService productService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _environment = environment;
        }

        public IList<Product> Products { get; set; } = new List<Product>();
        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public decimal TotalValue { get; set; }
        public string UserName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Auth/Login");
            }

            UserName = HttpContext.Session.GetString("UserName") ?? "Admin";
            Products = await _productService.GetAllProductsAsync();

            TotalProducts = Products.Count;
            LowStockCount = Products.Count(p => p.Quantity > 0 && p.Quantity <= 10);
            OutOfStockCount = Products.Count(p => p.Quantity == 0);
            TotalValue = Products.Sum(p => p.Price * p.Quantity);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string name, decimal price, int quantity, string imageUrl, IFormFile imageFile)
        {
            try
            {
                string finalImageUrl = await ProcessImageAsync(imageFile, imageUrl);

                var product = new Product
                {
                    Name = name,
                    Price = price,
                    Quantity = quantity,
                    ImageUrl = finalImageUrl
                };

                await _productService.CreateProductAsync(product);
                TempData["Success"] = "Produit créé avec succès!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur: {ex.Message}";
                return RedirectToPage();
            }
        }

        private async Task<string> ProcessImageAsync(IFormFile imageFile, string imageUrl)
        {
            // Priorité au fichier uploadé
            if (imageFile != null && imageFile.Length > 0)
            {
                return await SaveUploadedFileAsync(imageFile);
            }

            // Sinon utiliser l'URL si fournie
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return imageUrl;
            }

            // Sinon image par défaut
            return "/images/products/default-product.jpg";
        }

        private async Task<string> SaveUploadedFileAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var productsFolder = Path.Combine("images", "products");
            var fullPath = Path.Combine(_environment.WebRootPath, productsFolder, fileName);

            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{productsFolder}/{fileName}";
        }





        // 👇 MODIFIEZ CETTE MÉTHODE POUR PRENDRE IFormFile
        public async Task<IActionResult> OnPostEditAsync(int id, string name, decimal price, int quantity, IFormFile imageFile, string currentImageUrl)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product != null)
                {
                    product.Name = name;
                    product.Price = price;
                    product.Quantity = quantity;

                    // Gérer la nouvelle image si fournie
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        product.ImageUrl = await SaveUploadedFileAsync(imageFile);
                    }
                    else
                    {
                        // Garder l'image actuelle
                        product.ImageUrl = currentImageUrl ?? product.ImageUrl;
                    }

                    await _productService.UpdateProductAsync(product);
                    TempData["Success"] = "Produit modifié avec succès!";
                }
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la modification: {ex.Message}";
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
    }
}