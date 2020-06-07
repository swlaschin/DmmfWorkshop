// =============================================
// Exercise: Implementing types
//
// Here is a domain model with various types.
//
// Some of the types below have NOT yet been defined.
// Your task is to add simple definitions so that this file will compile.
//
// IMPORTANT:  types must be defined BEFORE they are referenced
// (e.g. earlier in the file )
//
// =============================================

// Uncomment these to see all the undefined types
// and not-implemented code
(*
type undefined = exn
let notImplemented() = failwith "not implemented"
*)

type Text = string      // An alias for an string

type PersonalName = {   // A record type used for modeling "AND" relations
  FirstName: Text
  LastName: Text
}

type OrderId = OrderId of int   // A "wrapper" around an int.

/// Must be > 0 and <= 100
type OrderQty = OrderQty of int  // For wrapper types, document any
                                 // constraints in the type comment

/// Must be 6 characters with leading "Z"
type LegacyProductId = LegacyProductId of string

/// Must be 8 characters with leading "N"
type NewProductId = undefined

/// A choice type is used here to distinguish between
/// two product ids that have different constraints
type ProductId =
    | LegacyProduct of LegacyProductId
    | NewProduct of NewProductId    //TODO define a type for NewProductId


type OrderLine = {
    ProductId : ProductId
    Qty: OrderQty
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


type OrderPlaced = undefined

// Use Function types for workflows
type PlaceOrder =
  Order -> OrderPlaced //TODO define a type for OrderPlaced


// =============================
// HOWTO:
// Constructing and destructuring records, wrappers, choices, etc.
// =============================


// ----------------------------------
// 1. Constructing and Destructuring records
// ----------------------------------

// to create
let name = {FirstName="a"; LastName="c"}
// to extract
let first = name.FirstName

// ----------------------------------
// 2. Constructing and Destructuring Choices
// ----------------------------------

// To construct the Cash case of PaymentMethod, use "Cash" as a constructor.
// No extra data is needed
let paymentMethod1 = Cash                // no extra data needed

//TODO Write some code to make this compile
let cardInfo = notImplemented()

// To construct the Card case of PaymentMethod, use "Card" as a constructor,
// with "cardInfo" as extra information
let paymentMethod2 = Card cardInfo

//TODO Write some code to make this compile
let emailAddress = notImplemented()

// To construct the PayPal case of PaymentMethod, use "PayPal" as a constructor,
// with "emailAddress" as extra information
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

// Now try to implement the PlaceOrder function type from above
let placeOrder : PlaceOrder =
  fun input ->
    //TODO create an OrderPlaced event value here
    let output = notImplemented()
    output