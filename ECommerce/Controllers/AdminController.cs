using Common.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminApprovalService _adminApprovalService;

        public AdminController(IAdminApprovalService adminApprovalService)
        {
            _adminApprovalService = adminApprovalService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/companyStatus")]
        public IActionResult CompanyApprovalStaus(long CompanyId, byte? Status)
        {
            string Response = _adminApprovalService.CompanyApprovalStatus(CompanyId, Status);
            if (Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return BadRequest(Response);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("/supplierStatus")]
        public IActionResult SupplierApprovalStatus(short SupplierId, byte? Status)
        {
            string Response = _adminApprovalService.SupplierApprovalStatus(SupplierId, Status);
            if (Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return BadRequest(Response);
            }
        }
    }
}
