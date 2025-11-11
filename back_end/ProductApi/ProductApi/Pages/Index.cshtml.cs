using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using ProductApi.Services;

namespace ProductApi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IJSRuntime _jsRuntime;

        public IndexModel(IProductService productService, IJSRuntime jsRuntime)
        {
            _productService = productService;
            _jsRuntime = jsRuntime;
        }

        public List<ProductApi.Models.Product> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _productService.GetAllProductsAsync();
        }

       
    }
}