
// Use Record types for AND
type Name = {
  FirstName: string
  MiddleInitial: string
  LastName: string
}

type Order = {
  OrderId : OrderId // define a type for OrderId
  // important:  types must be defined BEFORE they are referenced
  // (e.g. earlier in the file )
  OrderLines : OrderLine list // define a type for OrderLine
}

// Use Choice types for OR
type PaymentMethod =
  | Cash
  | Card of CardInfo // define a type for CardInfo
  | PayPal of EmailAddress // define a type for EmailAddress

// Use Function types for workflows
type PlaceOrder =
  OrderForm -> OrderPlaced // define a type for OrderForm and OrderPlaced


// =============================
// Constructing and Destructuring records
// =============================

// to create
let name = {FirstName="a"; MiddleInitial="b";LastName="c"}
// to extract
let first = name.FirstName
// to destructure
let {FirstName=f; MiddleInitial=m;LastName=l} = name

// =============================
// Constructing and Destructuring Choices
// =============================

// to create, use one of the cases as a function
let paymentMethod1 = Cash

let cardInfo = ??
let paymentMethod2 = Card cardInfo

let emailAddress = ??
let paymentMethod3 = PayPal EmailAddress

// to destructure, use pattern matching
let printMethod paymentMethod =
  match paymentMethod with
  | Cash -> printfn "Cash"
  | Card cardInfo -> printfn "%A" cardInfo
  | PayPal emailAddress -> printfn "%A" emailAddress

paymentMethod1 |> printMethod


// =============================
// Constructing Function types
// =============================

let placeOrder : PlaceOrder =
  fun input ->
    let output = // create OrderPlaced event value here
    output