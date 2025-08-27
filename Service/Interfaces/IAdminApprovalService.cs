using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAdminApprovalService
    {
        public string CompanyApprovalStatus(long CompanyId, byte? Status);

        public string SupplierApprovalStatus(short SupplierId, byte? Status);
    }
}
