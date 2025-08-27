using Common.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.Product;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles ="Admin,Supplier")]
        [HttpGet("/productdetails/{ProductId}")]
        public IActionResult ProductDetailById(long ProductId)
        {
            ProductModel Resposne = _productService.GetProductDetailById(ProductId);
            if (Resposne == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Resposne);
            }

        }

        [Authorize(Roles = "Supplier")]
        [HttpPost("/addproduct")]
        public IActionResult AddProduct(AddProductModel model)
        {
            long? Response = _productService.AddProduct(model);
            if (Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Product Already Exist!"
                });
            }
        }

        [Authorize(Roles = "Supplier")]
        [HttpPut("/updateproduct/{ProductId}")]
        public IActionResult UpdateProduct(long ProductId, UpdateProductModel model)
        {
            if (ProductId != model.ProductId)
            {
                return BadRequest();
            }
            ResponseModel Response = _productService.UpdateProduct(ProductId, model);
            if (Response != null)
            {
                return Ok("Product Updated Successfully!");
            }
            else if (Response?.Message == "Product Doesn't Exist!")
            {
                return Ok(new ResponseModel()
                {
                    Id = ProductId,
                    StatusCode = 401,
                    Message = "Product Doesn't Exist!"
                });
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Product Already Exist!"
                });
            }
        }

        [Authorize(Roles = "Supplier")]
        [HttpDelete("/deleteproduct/{ProductId}")]
        public IActionResult DeleteProduct(long ProductId)
        {
            bool Response = _productService.DeleteProduct(ProductId);
            if (Response == true)
            {
                return Ok("Product Deleted Successfully!");
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    Id = ProductId,
                    StatusCode = 401,
                    Message = "Product Already Exist!"
                });
            }
        }

        [Authorize(Roles = "Supplier")]
        [HttpPost("/productimage")]
        public IActionResult ProductImages([FromForm]ProductImageModel model)
        {
            string Response = _productService.ProductImage(model);
            if(Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Product doesn't Exist!"
                });

            }
        }

        [AllowAnonymous]
        [HttpPost("/productdetails")]
        public IActionResult ProductDetails()
        {
            List<ProductModel> Response = _productService.GetProductDetails();
            if (Response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Response);
            }
        }
    }
}
