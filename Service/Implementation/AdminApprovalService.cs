using Data.Data;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class AdminApprovalService : IAdminApprovalService
    {
        private readonly EcommercedbContext _db;

        public AdminApprovalService(EcommercedbContext db)
        {
            _db = db;
        }
        public string CompanyApprovalStatus(long CompanyId, byte? Status)
        {
            var Company = _db.TblCompanies.Find(CompanyId);
            if (Company is not null)
            {
                if (Status == 1)
                {
                    Company.Status = 1;
                    return "Status of company is pending.";
                }
                else if (Status == 2)
                {
                    Company.Status = 2;
                    return "Company is Approved.";
                }
                else
                {
                    Company.Status = 3;
                    return "Admin declined your request.";
                }
                _db.SaveChanges();
                return "Status Updated Successfully";
            }
            else
            {
                return "Company Doesn't Exist!";
            }
        }

        public string SupplierApprovalStatus(short SupplierId, byte? Status)
        {
            var Supplier = _db.TblSuppliers.Find(SupplierId);
            if (Supplier is not null)
            {
                if (Status == 1)
                {
                    Supplier.Status = 1;
                    return "Status of Supplier is pending.";
                }
                else if (Status == 2)
                {
                    Supplier.Status = 2;
                    return "Supplier is Approved.";
                }
                else
                {
                    Supplier.Status = 3;
                    return "Admin declined your request.";
                }
                _db.SaveChanges();
                return "Status Updated Successfully";
            }
            else
            {
                return "Supplier Doesn't Exist!";
            }
        }
    }
}
