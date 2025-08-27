using Common.AppSettings;
using Service.Models.Company;

namespace Service.Interfaces
{
    public interface ICompanyService
    {
        public CompanyModel GetCompanyDetailById(long CompanyId);

        public long? AddCompany(AddCompanyModel model);

        public ResponseModel UpdateCompany(long CompanyId, UpdateCompanyModel model);

        public bool DeleteCompany(long CompanyId);

        public List<CompanyModel> GetCompanyDetails();

        public bool CompanyStatus(long CompanyId, bool status);
    }
}
