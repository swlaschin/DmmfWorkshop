namespace Layer2Fs.DomainTypes


// DomainTypes
type FsCustomerId = FsCustomerId of int
type FsCustomerName = FsCustomerName of string

type FsCustomer = {Id:FsCustomerId; Name:FsCustomerName}

// Repository
type IFsCustomerRepostory =
    abstract GetCustomer : FsCustomerId -> FsCustomer

// Enum and Choice types
type CardType = Visa | Mastercard
type CardNumber = CardNumber of string

type PaymentMethod =
  | Cash
  | Cheque of int
  | Card of CardType * CardNumber
