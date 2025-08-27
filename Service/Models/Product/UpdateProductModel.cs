using System.ComponentModel.DataAnnotations;

namespace Service.Models.Product
{
    public class UpdateProductModel
    {
        [Required]
        public long ProductId { get; set; }

        [Required(ErrorMessage = "Please enter the Name of the product.")]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 15, ErrorMessage = "Description must be at least 15 characters long.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter the Price of product.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter the avaiable stock of product.")]
        public int? AvailableStock { get; set; }

        [Required(ErrorMessage = "Please enter the supplierId.")]
        public short SupplierId { get; set; }

        [Required(ErrorMessage = "Please enter the categoryId.")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter the companyId.")]
        public long? CompanyId { get; set; }

        [Required(ErrorMessage = "Please enter the subcategoryId.")]
        public int? SubCategoryid { get; set; }

        [Required(ErrorMessage = "Please enter the SKU.")]
        public string StokeKeepingUnit { get; set; } = null!;
    }
}
