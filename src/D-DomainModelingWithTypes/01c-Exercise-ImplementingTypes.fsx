// =============================================
// Exercise: Implementing types
// Some of the types below have not yet been defined.
// Add simple definitions so that this file will compile.
//
// important:  types must be defined BEFORE they are referenced
// (e.g. earlier in the file )
//
// =============================================


// An alias for an string
type Text = string

// Use Record types for AND
type Name = {
  FirstName: Text
  MiddleInitial: Text
  LastName: Text
}

// A "wrapper" around an int. See below for usage instructions
type OrderId = OrderId of int



type OrderLine = {
    ProductId : ProductId  //TODO define a type for ProductId
    Qty: OrderQty   //TODO define a type for OrderQty
}

type Order = {
  OrderId : OrderId
  OrderLines : OrderLine list
}

// Use Choice types for OR
type PaymentMethod =
  | Cash
  | Card of CardInfo //TODO define a type for CardInfo
  | PayPal of EmailAddress //TODO define a type for EmailAddress

// Use Function types for workflows
type PlaceOrder =
  Order -> OrderPlaced //TODO define a type for OrderForm and OrderPlaced


// =============================
// Constructing and Destructuring records
// =============================

// create a record
let name = {FirstName="a"; MiddleInitial="b"; LastName="c"}

// extract a field from a record
let first = name.FirstName

// create a new record from an old one with some fields updated
let name2 = {name with FirstName="A"; MiddleInitial="B"}

// =============================
// Constructing and Destructuring Choices
// =============================

// to create, use one of the cases as a constructor function
let paymentMethod1 = Cash             // no extra data needed

let cardInfo = ??  //TODO Write some code to make this compile
let paymentMethod2 = Card cardInfo   // extra data (cardInfo) needed

let emailAddress = ??  //TODO Write some code to make this compile
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

// =============================
// Constructing and Destructuring Wrappers
//
// It's common to create "wrappers" around data
// =============================

// to create a wrapper, use the case/tag as a function
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



// =============================
// Constructing Function types
// =============================

// To implement a function based on a function type
// 1.  define a value in the normal way, but use the function type as the type annotation
//     let placeOrder : PlaceOrder = ...
// 2.  for the implementation, use a lambda with however many parameters are needed.

// here's an example of adding two numbers
type AddTwoNumbers = int -> int -> int  // the definition
let addTwoNumbers : AddTwoNumbers =  // the implementation
    fun n1 n2 ->
        n1 + n2

// Now try to implement the PlaceOrder function type from above
let placeOrder : PlaceOrder =
  fun input ->
    let output = ?? //TODO create an OrderPlaced event value here
    output