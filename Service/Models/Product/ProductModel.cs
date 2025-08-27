namespace Service.Models.Product
{
    public class ProductModel
    {
        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public decimal? Discount { get; set; }

        public string SubCategory { get; set; } = string.Empty;

        public string Supplier { get; set; } = string.Empty;

        public int? AvailableStock { get; set; }
    }
}
