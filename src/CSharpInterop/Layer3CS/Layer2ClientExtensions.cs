using System;
using Layer2Fs.DomainTypes;

namespace Layer3CS
{
    public static class Layer2ClientExtensions
    {
        public static FsCustomer WithNewName(this FsCustomer fsCust, string newName)
        {
            var name = FsCustomerName.NewFsCustomerName(newName);
            var newFsCust = new FsCustomer(fsCust.Id, name);
            return newFsCust;
        }
    }
}

