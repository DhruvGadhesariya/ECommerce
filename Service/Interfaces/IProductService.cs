using Common.AppSettings;
using Service.Models.Product;

namespace Service.Interfaces
{
    public interface IProductService
    {
        public long? AddProduct(AddProductModel model);

        public ProductModel GetProductDetailById(long productId);

        public ResponseModel UpdateProduct(long ProductId, UpdateProductModel model);

        public bool DeleteProduct(long ProductId);

        public string ProductImage(ProductImageModel model);

        public List<ProductModel> GetProductDetails();

    }
}
