namespace ProductApi.ViewModels.Admin
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public decimal TotalValue { get; set; }
        public List<ProductViewModel> Products { get; set; } = new();
        public string UserName { get; set; } = string.Empty;

        // Propriétés calculées pour la vue
        public string TotalValueFormatted => TotalValue.ToString("C");
        public bool HasLowStock => LowStockCount > 0;
        public bool HasOutOfStock => OutOfStockCount > 0;
        public string AlertStatus => HasOutOfStock ? "danger" : HasLowStock ? "warning" : "success";
    }
}
