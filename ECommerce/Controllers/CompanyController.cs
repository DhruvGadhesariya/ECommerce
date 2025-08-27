using Common.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.Company;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [AllowAnonymous]
        [HttpGet("/companydetails/{CompanyId}")]
        public IActionResult CompanyDetailsById(long CompanyId)
        {
            CompanyModel Response = _companyService.GetCompanyDetailById(CompanyId);
            if (Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Admin,Supplier")]
        [HttpPost("/addcompany")]
        public IActionResult AddCompany(AddCompanyModel model)
        {
            long? Response = _companyService.AddCompany(model);
            if (Response != null)
            {
                return Ok(Response);
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Company Already Exist!"
                });
            }
        }

        [Authorize(Roles = "Admin,Supplier")]
        [HttpPut("/updatecompany/{CompanyId}")]
        public IActionResult UpdateCompany(long CompanyId, UpdateCompanyModel model)
        {
            if (CompanyId != model.CompanyId)
            {
                return BadRequest();
            }
            ResponseModel Response = _companyService.UpdateCompany(CompanyId, model);
            if (Response != null)
            {
                return Ok(Response);
            }
            else if (Response?.Message == "Company Doesn't Exist!")
            {
                return Ok(new ResponseModel()
                {
                    Id = CompanyId,
                    StatusCode = 401,
                    Message = "Company Doesn't Exist!"
                });
            }
            else
            {
                return Ok(new ResponseModel()
                {

                    StatusCode = 401,
                    Message = "Company Already Exist!"
                });
            }
        }

        [Authorize(Roles = "Admin,Supplier")]
        [HttpDelete("/deletecompany/{CompanyId}")]
        public IActionResult DeleteCompany(long CompanyId)
        {
            bool Company = _companyService.DeleteCompany(CompanyId);
            if (Company == true)
            {
                return Ok("Company Removed Successfully");
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    Id = CompanyId,
                    StatusCode = 401,
                    Message = "Company doesn't Exist!"
                });
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("/companydetails")]
        public IActionResult GetCompanyDetails()
        {
            List<CompanyModel> Response = _companyService.GetCompanyDetails();
            if(Response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Response);
            }
        }

        [Authorize(Roles ="Admin,Supplier")]
        [HttpPost("/updateCompanystatus/{CompanyId}")]
        public IActionResult CompanyStatus(long CompanyId, bool Status)
        {
            bool Response = _companyService.CompanyStatus(CompanyId, Status);
            if (Response == true)
            {
                return Ok("Status Updated Successfully");
            }
            else
            {
                return Ok(new ResponseModel()
                {
                    StatusCode = 401,
                    Message = "Company doesn't Exist!"
                });
            }
        }
    }
}
