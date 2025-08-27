using Common.AppSettings;
using Data.Data;
using Data.Entities;
using Service.Interfaces;
using Service.Models.Company;

namespace Service.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly EcommercedbContext _db;

        public CompanyService(EcommercedbContext db)
        {
            _db = db;
        }

        //method to adding the new company
        public long? AddCompany(AddCompanyModel model)
        {
            var company = _db.TblCompanies.Any(x => x.CompanyName.ToLower() == model.CompanyName.ToLower() && x.Email == model.Email); //checks wheather the company name and email already exist or not.
            if (company)
            {
                return null;
            }
            var AddedCompany = new TblCompany();
            {
                AddedCompany.CompanyName = model.CompanyName;
                AddedCompany.CompanyAddress = model.CompanyAddress;
                AddedCompany.ContactNumber = model.ContactNumber;
                AddedCompany.Email = model.Email;
                AddedCompany.CountryId = model.CountryId;
            }

            _db.TblCompanies.Add(AddedCompany);
            _db.SaveChanges();

            return AddedCompany.CompanyId;
        }

        public bool CompanyStatus(long CompanyId, bool status)
        {
            var company = _db.TblCompanies.Find(CompanyId);
            if (company != null && company.DeletedAt == null && company.Status == 2)
            {
                company.IsActive = status;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //method for deleting the company
        public bool DeleteCompany(long CompanyId)
        {
            var company = _db.TblCompanies.FirstOrDefault(x => x.CompanyId == CompanyId && x.DeletedAt == null); //get the id from database of company which we want to delete and also checks wheather it's already deleted?
            if (company != null)
            {
                company.DeletedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //method for get the details of company by CompanyId
        public CompanyModel GetCompanyDetailById(long CompanyId)
        {
            var Country = _db.TblCountries.ToList();
            var company = _db.TblCompanies.Where(x => x.CompanyId == CompanyId && x.IsActive == true && x.DeletedAt == null && x.Status == 2).FirstOrDefault(); //find the details of company by Id from table
            if (company != null)
            {
                return new CompanyModel
                {
                    CompanyName = company.CompanyName,
                    CompanyAddress = company.CompanyAddress,
                    ContactNumber = company.ContactNumber,
                    Email = company.Email,
                    Country = company.Country.CountryName,
                };
            }
            else
            {
                return null;
            }
        }

        public List<CompanyModel> GetCompanyDetails()
        {
            List<CompanyModel> CompanyList = (from c in _db.TblCompanies 
                                              where( c.IsActive == true && c.DeletedAt == null && c.Status == 2)
                                              select new CompanyModel
                                              {
                                                  CompanyName = c.CompanyName,
                                                  CompanyAddress = c.CompanyAddress,
                                                  ContactNumber = c.ContactNumber,
                                                  Country = c.Country.CountryName,
                                                  Email = c.Email,

                                              }).ToList();
            return CompanyList;
        }

        //method for updating the company details
        public ResponseModel UpdateCompany(long CompanyId, UpdateCompanyModel model)
        {
            var companydetails = _db.TblCompanies.Where(x => (x.CompanyName == model.CompanyName && x.Email == model.Email) && x.CompanyId != model.CompanyId).AsQueryable(); //checks wheather the company name and email already exist or not
            if (companydetails.Any())
            {
                return null;
            }

            var company = _db.TblCompanies.Find(CompanyId); //get the id of company which we want to update
            if (company != null && company.DeletedAt == null && company.Status == 2)
            {
                company.CompanyName = model.CompanyName;
                company.CompanyAddress = model.CompanyAddress;
                company.ContactNumber = model.ContactNumber;
                company.Email = model.Email;
                company.CountryId = model.CountryId;
                company.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
                return new ResponseModel()
                {
                    Id = CompanyId,
                    Message = "Company Updated Successfully.",
                    StatusCode = 200,
                };
            }

            return new ResponseModel()
            {
                Id = CompanyId,
                Message = "Company doesn't Exist.",
                StatusCode = 401,
            };
        }
    }
}
