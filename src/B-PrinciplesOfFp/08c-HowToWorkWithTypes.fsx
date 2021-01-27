// =============================
// HOWTO:
// Constructing and destructuring records, wrappers, choices, etc.
// =============================


// ----------------------------------
// 1. Constructing and Destructuring records
// ----------------------------------

type PersonalName = {   // A record type used for modeling "AND" relations
    FirstName: string
    LastName: string
    }

// to create
let name = {
    FirstName="Alice"
    LastName="Smith"
    }
// to extract
let first = name.FirstName

// ----------------------------------
// 2. Constructing and Destructuring Choices
// ----------------------------------

type CardInfo = {
    CardNumber:string
    // etc
    }
type EmailAddress = string

type PaymentMethod =
  | Cash
  | Card of CardInfo
  | PayPal of EmailAddress

// To construct the Cash case of PaymentMethod, use "Cash" as a constructor.
// No extra data is needed
let paymentMethod1 = Cash                // no extra data needed


// To construct the Card case of PaymentMethod, use "Card" as a constructor,
// with "cardInfo" as extra information
let cardInfo = {CardNumber="123"}
let paymentMethod2 = Card cardInfo



// To construct the PayPal case of PaymentMethod, use "PayPal" as a constructor,
// with "emailAddress" as extra information
let emailAddress = "abc@example.com"
let paymentMethod3 = PayPal emailAddress


// to destructure a choice type, use pattern matching
let printMethod paymentMethod =
  match paymentMethod with
  | Cash ->                         // each match is a bit like a lambda, with an ->
        printfn "Cash"
  | Card cardInfo ->                // cardInfo is available in the pattern match
        printfn "Card with %A" cardInfo
  | PayPal emailAddress ->          // emailAddress is available in the pattern match
        printfn "PayPal with %A" emailAddress

paymentMethod1 |> printMethod
paymentMethod2 |> printMethod
paymentMethod3 |> printMethod

// ----------------------------------
// 3. Constructing and Destructuring Wrappers
// ----------------------------------

type OrderId = OrderId of int   // A "wrapper" around an int.

// to create a wrapper, use the case as a function
let orderId = OrderId 99

// to deconstruct a wrapper, there are a number of ways
// Approach 1: use pattern matching with one case
let value1 =
    match orderId with
    | OrderId i -> i // return the inner value

// Approach 2: use pattern matching on the LEFT hand side
let (OrderId value2) = orderId
// value2 is now 99

// Approach 3: in functions you can use the pattern matching directly in the parameter
let printOrderId (OrderId value) =
    printfn "OrderId = %i" value

// test
printOrderId orderId   // output is "OrderId = 99"


// ----------------------------------
// 4. Constructing Function types
// ----------------------------------

// To implement a function based on a function type
// 1.  define a value in the normal way, but use the function type as the type annotation
//     let placeOrder : PlaceOrder = ...
// 2.  for the implementation, use a lambda with however many parameters are needed.

// here's an example of adding two numbers
type AddTwoNumbers = int -> int -> int  // the definition
let addTwoNumbers : AddTwoNumbers =     // the implementation
    fun n1 n2 ->
        n1 + n2

