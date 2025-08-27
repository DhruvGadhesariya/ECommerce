using Common.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.Supplier;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [Authorize(Roles ="Admin,Supplier")]
        [HttpGet("/supplierdetails/{SupplierId}")]
        public IActionResult SupplierDetailsById(short SupplierId)
        {
            SupplierModel Response = _supplierService.GetSupplierDetailsById(SupplierId);
            if (Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpPost("/addsupplier")]
        public IActionResult AddSupplier(AddSupplierModel model)
        {
            long? Response = _supplierService.AddSupplier(model);
            if (Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Supplier Already Exist!"
                });
            }
        }

        [Authorize(Roles = "Supplier")]
        [HttpPut("/updatesupplier/{SupplierId}")]
        public IActionResult UpdateSupplier(short SupplierId, UpdateSupplierModel model)
        {
            if (SupplierId != model.SupplierId)
            {
                return BadRequest();
            }
            ResponseModel Response = _supplierService.UpdateSupplier(SupplierId, model);
            if (Response != null)
            {
                return Ok("Supplier Updated successfully");
            }
            else if (Response?.Message == "Supplier Doesn't Exist!")
            {
                return Ok(new ResponseModel()
                {
                    Id = SupplierId,
                    StatusCode = 401,
                    Message = "Supplier Doesn't Exist!"
                });
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Supplier Already Exist!"
                });
            }
        }

        [Authorize(Roles = "Supplier,Admin")]
        [HttpDelete("/deletesupplier/{SupplierId}")]
        public IActionResult DeleteSupplier(short SupplierId)
        {
            bool supplier = _supplierService.DeleteSupplier(SupplierId);
            if (supplier == true)
            {
                return Ok("Supplier Removed Successfully");
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    Id = SupplierId,
                    StatusCode = 401,
                    Message = "Supplier Doesn't Exist!"
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/supplierdetails")]
        public IActionResult GetSupplierDetails()
        {
            List<SupplierModel> Response = _supplierService.GetSupplierDetails();
            if (Response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Response);
            }
        }

        [Authorize(Roles = "Admin,Supplier")]
        [HttpPost("/updateSupplierstatus/{SupplierId}")]
        public IActionResult SupplierStatus(short SupplierId, bool Status)
        {
            bool Response = _supplierService.SupplierStatus(SupplierId, Status);
            if (Response == true)
            {
                return Ok("Status Updated Successfully");
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Supplier doesn't Exist!"
                });
            }
        }
    }
}
