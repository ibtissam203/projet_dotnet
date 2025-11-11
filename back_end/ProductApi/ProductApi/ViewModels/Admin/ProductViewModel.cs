namespace ProductApi.ViewModels.Admin
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Propriétés calculées pour la vue
        public string StockStatus
        {
            get
            {
                if (Quantity == 0) return "Rupture de stock";
                if (Quantity <= 5) return "Stock très faible";
                if (Quantity <= 10) return "Stock faible";
                return "En stock";
            }
        }

        public string StockClass
        {
            get
            {
                if (Quantity == 0) return "stock-out";
                if (Quantity <= 5) return "stock-very-low";
                if (Quantity <= 10) return "stock-low";
                return "stock-ok";
            }
        }

        public string PriceFormatted => Price.ToString("C");
        public bool IsLowStock => Quantity <= 10;
        public bool IsOutOfStock => Quantity == 0;
        public bool HasImage => !string.IsNullOrEmpty(ImageUrl);
        public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy");
    }
}
