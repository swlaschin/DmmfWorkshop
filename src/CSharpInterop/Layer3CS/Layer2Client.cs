using System;
using Layer2Fs;
using Layer2Fs.DomainTypes;
using Microsoft.FSharp.Core;

namespace Layer3CS
{
    public class Layer2Client
    {
        public FsCustomer CreateFsCustomer()
        {
            var id = FsCustomerId.NewFsCustomerId(1);
            var name = FsCustomerName.NewFsCustomerName("Alice");
            var fsCust = new FsCustomer(id, name);
            Console.WriteLine("Id={0}, Name={1}", fsCust.Id.Item, fsCust.Name.Item);
            return fsCust;
        }

        public FSharpOption<CommonTypes.String50> CreateString50()
        {
            // var string50_v1 = new CommonTypes.String50("hello");

            var string50 = CommonTypes.createString50("hello");
            return string50;
        }

        public FsCustomer ChangeFsCustomerName(FsCustomer fsCust, string newName)
        {
            var newFsCust = fsCust.WithNewName(newName);
            return newFsCust;
        }

        public PaymentMethod CreateCardPayment()
        {
            var cardType = CardType.Visa;
            var cardNo = CardNumber.NewCardNumber("1234");
            var paymentMethod = Layer2Fs.CsApi.CreateCardPayment(cardType, cardNo);
            return paymentMethod;
        }

        public void ProcessPayment(PaymentMethod paymentMethod)
        {
            Layer2Fs.CsApi.ProcessPayment(
                paymentMethod,
                () => Console.WriteLine("Paid in cash"),
                checkNo => Console.WriteLine("Paid by cheque {0}", checkNo),
                (cardType, cardNumber) => Console.WriteLine("Paid by card CardType={0}, CardNumber={1}", cardType, cardNumber)
                );
        }

    }
}

