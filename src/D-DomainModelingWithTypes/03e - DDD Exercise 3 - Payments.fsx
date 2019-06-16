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

type CardType = Visa | Mastercard | ??
type CardNumber = CardNumber of string
type ChequeNumber = ??
type EmailAddress = ??
type BitcoinAddress = ??

type PaymentMethod =
    | Cash
    | Cheque of ChequeNumber
    | Card of ??
    | PayPal of ??
    | Bitcoin of ??

type PaymentAmount = ??

type Payment = ??

let printPaymentMethod (paymentMethod:PaymentMethod) =
    match paymentMethod with
    | Cash ->  ??
    | Cheque checkNo -> ??
    | Card (cardType,cardNo) -> ??

let printPayment (payment:Payment) =
    ??

let makePayment (amount:float) (paymentMethod:PaymentMethod) :Payment =
    ??

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
