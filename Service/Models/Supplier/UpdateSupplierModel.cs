using System.ComponentModel.DataAnnotations;

namespace Service.Models.Supplier
{
    public class UpdateSupplierModel
    {
        [Required]
        public short SupplierId { get; set; }

        [Required]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

    }
}
