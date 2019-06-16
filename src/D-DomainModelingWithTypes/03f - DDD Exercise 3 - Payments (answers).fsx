// ================================================
// DDD Exercise: Model a payment taking system
//
// ================================================

(*
The payment taking system should accept
* Cash
* Credit cards
* Cheques
* Paypal
* Bitcoin

A payment consists of a:
* payment
* non-negative amount

After designing the types, create functions that will:

* print a payment method
* print a payment, including the amount
* create a new payment from an amount and method

*)

type CardType = Visa | Mastercard
type CardNumber = CardNumber of string
type ChequeNumber = ChequeNumber of int
type EmailAddress = EmailAddress of string
type BitcoinAddress = BitcoinAddress of string

type PaymentMethod =
    | Cash
    | Cheque of ChequeNumber
    | Card of CardType * CardNumber
    | PayPal of EmailAddress
    | Bitcoin of BitcoinAddress

type PaymentAmount =
    PaymentAmount of float

type Payment = {
    paymentMethod: PaymentMethod
    paymentAmount: PaymentAmount
    }

let printPaymentMethod (paymentMethod:PaymentMethod) =
    match paymentMethod with
    | Cash ->  printfn "Paid in cash"
    | Cheque checkNo -> printfn "Paid by cheque: %A" checkNo
    | Card (cardType,cardNo) -> printfn "Paid with %A %A" cardType cardNo
    | PayPal emailAddress -> printfn "Paid with PayPal %A" emailAddress
    | Bitcoin bitcoinAddress -> printfn "Paid with BitCoin %A" bitcoinAddress

let printPayment (payment:Payment) =
    // get the data out using a match -- or could define a helper "value" function
    match payment.paymentAmount
        with (PaymentAmount amount) -> printf "Amount: %g. " amount
    printPaymentMethod payment.paymentMethod

let makePayment (amount:float) (paymentMethod:PaymentMethod) :Payment =
    {paymentMethod=paymentMethod; paymentAmount=PaymentAmount amount}

// examples
let paymentMethod1 = Cash
let paymentMethod2 = Cheque (ChequeNumber 42)
let paymentMethod3 = Card (Visa, CardNumber "1234")
let paymentMethod4 = PayPal (EmailAddress "me@example.com")
let paymentMethod5 = Bitcoin (BitcoinAddress "1234")

let payment1 = makePayment 42.0 paymentMethod1
let payment2 = makePayment 123.0 paymentMethod2
let payment3 = makePayment 123.0 paymentMethod3

// highlight and run
printPaymentMethod paymentMethod1
printPaymentMethod paymentMethod2
printPaymentMethod paymentMethod3
printPaymentMethod paymentMethod4
printPaymentMethod paymentMethod5

printPayment payment1
printPayment payment2
printPayment payment3
