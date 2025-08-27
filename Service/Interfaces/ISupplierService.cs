using Common.AppSettings;
using Service.Models.Supplier;

namespace Service.Interfaces
{
    public interface ISupplierService
    {
        public SupplierModel GetSupplierDetailsById(short SupplierId);

        public long? AddSupplier(AddSupplierModel model);

        public ResponseModel UpdateSupplier(short SupplierId,UpdateSupplierModel model);

        public bool DeleteSupplier(short SupplierId);

        public List<SupplierModel> GetSupplierDetails();

        public bool SupplierStatus(short SupplierId, bool status);
    }
}
