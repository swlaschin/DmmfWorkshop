(*
Some of the types below have not yet been defined.
Add simple definitions so that this file will compile.
*)

// Use Record types for AND
type Name = {
  FirstName: string
  MiddleInitial: string
  LastName: string
}

// Use single case unions for wrapper types
type OrderId = OrderId of int
type ProductId = ProductId of int
type OrderQty = OrderQty of int

type OrderLine = {
    ProductId : ProductId
    Qty: OrderQty
    }

type Order = {
  OrderId : OrderId
  // important:  types must be defined BEFORE they are referenced
  // (e.g. earlier in the file )
  OrderLines : OrderLine list //TODO define a type for OrderLine
}

type CardInfo = {
    CardNumber : string
    ExpiryMonth : int
    ExpiryYear : int
    }

type EmailAddress = EmailAddress of string

// Use Choice types for OR
type PaymentMethod =
  | Cash
  | Card of CardInfo //TODO define a type for CardInfo
  | PayPal of EmailAddress //TODO define a type for EmailAddress


type OrderPlaced = {
   Order : Order
   Timestamp : System.DateTime
   }

// Use Function types for workflows
type PlaceOrder =
  Order -> OrderPlaced //TODO define a type for OrderForm and OrderPlaced


// =============================
// Constructing and Destructuring records
// =============================

// to create
let name = {FirstName="a"; MiddleInitial="b";LastName="c"}
// to extract
let first = name.FirstName

// =============================
// Constructing and Destructuring Choices
// =============================

// to create, use one of the cases as a function
let paymentMethod1 = Cash

//TODO Write some code to make this compile
let cardInfo = {
    CardNumber = "1234"
    ExpiryMonth = 1
    ExpiryYear = 2024
    }
let paymentMethod2 = Card cardInfo

//TODO Write some code to make this compile
let emailAddress = EmailAddress "scott@example.com"
let paymentMethod3 = PayPal emailAddress

// to destructure a choice type, use pattern matching
let printMethod paymentMethod =
  match paymentMethod with
  | Cash -> printfn "Cash"
  | Card cardInfo -> printfn "Card with %A" cardInfo
  | PayPal emailAddress -> printfn "PayPal with %A" emailAddress

paymentMethod1 |> printMethod
paymentMethod2 |> printMethod
paymentMethod3 |> printMethod

// =============================
// Constructing and Destructuring Single Choice Wrappers
// =============================

// to create a wrapper, use the case as a function
let orderId = OrderId 99

// to deconstruct a wrapper, there are a number of ways
// Approach 1: use pattern matching with one case
let value1 =
    match orderId with
    | OrderId i -> i // return the inner value

// Approach 2: use pattern matching on the LEFT hand side
let (OrderId value2) = orderId
// value2 is now 99s

// Approach 3: in functions you can use the pattern matching directly in the parameter
let printOrderId (OrderId value) =
    printfn "OrderId = %i" value

// test
printOrderId orderId   // output is "OrderId = 99"



// =============================
// Constructing Function types
// =============================

// To implement a function based on a function type
// 1.  define a value in the normal way, but use the function type as the type annotation
//     let placeOrder : PlaceOrder = ...
// 2.  for the implementation, use a lambda with however many parameters are needed.

// here's example of adding two numbers
type AddTwoNumbers = int -> int -> int  // the definition
let addTwoNumbers : AddTwoNumbers =  // the implementation
    fun n1 n2 ->
        n1 + n2

// Now try to implement the PlaceOrder function type from above
let placeOrder : PlaceOrder =
  fun input ->
    //TODO create an OrderPlaced event value here
    let output = {
        Order = input
        Timestamp = System.DateTime.UtcNow
        }
    output