using Common.AppSettings;
using Data.Data;
using Data.Entities;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.Models.Product;

namespace Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly EcommercedbContext _db;
        private readonly Imageurl _options;

        public ProductService(EcommercedbContext db, IOptions<Imageurl> options)
        {
            _db = db;
            _options = options.Value;
        }

        //Method for adding the Product
        public long? AddProduct(AddProductModel model)
        {
            var Product = _db.TblProducts.Any(x => x.ProductName.ToLower() == model.ProductName.ToLower() && x.SupplierId == model.SupplierId); //checks wheather the productname and supplier already exist or not.
            if (Product)
            {
                return null;

            }

            var AddedProduct = new TblProduct();
            {
                AddedProduct.ProductName = model.ProductName;
                AddedProduct.Description = model.Description;
                AddedProduct.Price = model.Price;
                AddedProduct.StokeKeepingUnit = model.StokeKeepingUnit;
                AddedProduct.CategoryId = model.CategoryId;
                AddedProduct.CompanyId = model.CompanyId;
                AddedProduct.SubCategoryId = model.SubCategoryid;
                AddedProduct.SupplierId = model.SupplierId;
                AddedProduct.AvailableStock = model.AvailableStock;

            }

            _db.TblProducts.Add(AddedProduct);
            _db.SaveChanges();

            return AddedProduct.ProductId;
        }

        //Method for deleting the Product
        public bool DeleteProduct(long ProductId)
        {
            var Product = _db.TblProducts.FirstOrDefault(x => x.ProductId == ProductId && x.DeletedAt == null); //get the id from database of Product which we want to delete and also checks wheather it's already deleted?
            if (Product != null)
            {
                Product.DeletedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Method for get detail of the Product
        public ProductModel GetProductDetailById(long productId)
        {
            var Category = _db.TblCategories.ToList();
            var Subcategory = _db.TblSubCategories.ToList();
            var Company = _db.TblCompanies.ToList();
            var Supplier = _db.TblSuppliers.ToList();
            var product = _db.TblProducts.Where(x => x.ProductId == productId && x.DeletedAt == null && x.Status == 2).FirstOrDefault();  //find the details of product by Id from table
            if (product != null)
            {
                return new ProductModel
                {
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Discount = product.Discount,
                    Price = product.Price,
                    Category = product.Category.CategoryName,
                    SubCategory = product.SubCategory.SubCategoryName,
                    Company = product.Company.CompanyName,
                    Supplier = product.Supplier.SupplierName,
                    AvailableStock = product.AvailableStock,
                };
            }
            else
            {
                return null;
            }
        }

        public List<ProductModel> GetProductDetails()
        {
            List<ProductModel> ProductList = (from p in _db.TblProducts
                                              where (p.DeletedAt == null && p.Status == 2)
                                              select new ProductModel
                                              {
                                                  ProductName = p.ProductName,
                                                  Description = p.Description,
                                                  Price = p.Price,
                                                  AvailableStock = p.AvailableStock,
                                                  Discount = p.Discount,
                                                  Category = p.Category.CategoryName,
                                                  SubCategory = p.SubCategory.SubCategoryName,
                                                  Company = p.Company.CompanyName,
                                                  Supplier = p.Supplier.SupplierName,

                                              }).ToList();
            return ProductList;
        }

        public string ProductImage(ProductImageModel model)
        {
            var product = _db.TblProducts.FirstOrDefault(x => x.ProductId == model.ProductId && x.DeletedAt == null);
            if (product == null)
            {
                return null;
            }
            int count = 1;
            string rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            foreach (var image in model.ProductImage)
            {
                string filename = $"Product{product.ProductId}-Image-{count}{Path.GetExtension(image.FileName)}";
                string filePath = Path.Combine(rootDirectory, filename);

                _db.TblProductMedias.Add(new TblProductMedia
                {
                    ProductId = product.ProductId,
                    MediaName = filename,
                    MediaPath = Path.Combine(_options.Mediapath, filename),
                    MediaType = "img"
                });

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                count++;
            }
            _db.SaveChanges();
            return "Image Uploaded Successfully";
        }

        //Method for updating the Product detail
        public ResponseModel UpdateProduct(long ProductId, UpdateProductModel model)
        {
            var productdetails = _db.TblProducts.Where(x => (x.ProductName == model.ProductName && x.SupplierId == model.SupplierId) && x.ProductId != model.ProductId).AsQueryable(); //checks wheather the product name and supplier already exist or not
            if (productdetails.Any())
            {
                return null;
            }

            var Product = _db.TblProducts.Find(ProductId); //get the id of product which we want to update
            if (Product != null && Product.DeletedAt == null && Product.Status == 2)
            {
                Product.ProductName = model.ProductName;
                Product.Description = model.Description;
                Product.Price = model.Price;
                Product.StokeKeepingUnit = model.StokeKeepingUnit;
                Product.CategoryId = model.CategoryId;
                Product.CompanyId = model.CompanyId;
                Product.SubCategoryId = model.SubCategoryid;
                Product.SupplierId = model.SupplierId;
                Product.AvailableStock = model.AvailableStock;
                Product.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
                return new ResponseModel()
                {
                    Id = ProductId,
                    Message = "Product Updated Successfully!",
                    StatusCode = 200,
                } ;
            }

            return new ResponseModel()
            {
                Id = ProductId,
                Message = "Product Doesn't Exist!",
                StatusCode = 401,
            };
        }
    }
}
