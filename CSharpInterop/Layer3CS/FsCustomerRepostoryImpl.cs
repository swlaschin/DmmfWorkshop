using Layer2Fs.DomainTypes;

namespace Layer3CS
{
    public class FsCustomerRepostoryImpl: IFsCustomerRepostory
    {
        public FsCustomer GetCustomer(FsCustomerId id)
        {
            var name = FsCustomerName.NewFsCustomerName("Alice");
            var fsCust = new FsCustomer(id, name);
            return fsCust;
        }
    }
}
