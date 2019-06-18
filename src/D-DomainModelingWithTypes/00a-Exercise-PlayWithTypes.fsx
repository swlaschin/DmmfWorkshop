
// ===========================================
// Use Record types for AND
// ===========================================

type Name = {
  FirstName: string
  MiddleInitial: string
  LastName: string
}

// =============================
// Constructing and Destructuring Records
// =============================

// to create
let name = {FirstName="a"; MiddleInitial="b";LastName="c"}
// to extract
let first = name.FirstName
// to destructure
let {FirstName=f; MiddleInitial=m;LastName=l} = name

// ===========================================
// Use single-case choice types for wrappers
// ===========================================

type CustomerId = CustomerId of int

module CustomerId =

  // to construct a wrapper type...

  let create id =
    // use the case name as a function
    CustomerId id

  // to extract the value, here are three implementations

  /// Very explicit implementation (rarely used)
  let value1 custId =
    match custId with
    | CustomerId id -> id

  /// Alternative implementation using inline pattern matching
  let value2 custId =
    let (CustomerId innerId) = custId
    innerId

  /// Alternative implementation using inline pattern matching
  /// in the parameter itself.
  /// This is the most common implementation approach
  let value3 (CustomerId innerId) = innerId

// ===========================================
// Aliases are used sometimes for fast and loose modeling.
// These is NOT a wrapper type, just an alias.
// ===========================================

type Date = System.DateTime
type OrderQty = int



// =============================
// AND-ing types together
// A complex record containing other types
// =============================

type Order = {
  OrderId : OrderId // define a type for OrderId
  // important:  types must be defined BEFORE they are referenced
  // (e.g. earlier in the file )
  OrderLines : OrderLine list // define a type for OrderLine
}


// ===========================================
// OR-ing types together
// A complex choice type
// ===========================================

type PaymentMethod =
  | Cash
  | Card of CardInfo // define a type for CardInfo
  | PayPal of EmailAddress // define a type for EmailAddress

// ===========================================
// Use Function types for workflows
// ===========================================

type PlaceOrder =
  OrderForm -> OrderPlaced // define a type for OrderForm and OrderPlaced



// =============================
// Constructing and Destructuring Choices
// =============================

// to create, use one of the cases as a function
let paymentMethod1 = Cash

let cardInfo = ?? // use the type you defined earlier
let paymentMethod2 = Card cardInfo

let emailAddress = ?? // use the type you defined earlier
let paymentMethod3 = PayPal EmailAddress

// to destructure, use pattern matching
let printMethod paymentMethod =
  match paymentMethod with
  | Cash -> printfn "Cash"
  | Card cardInfo -> printfn "%A" cardInfo
  | PayPal emailAddress -> printfn "%A" emailAddress

// test with different payment methods
printMethod paymentMethod1
printMethod paymentMethod2
printMethod paymentMethod3


// =============================
// Constructing Function types
// =============================

// use the defintion of PlaceOrder you defined earlier
let placeOrder : PlaceOrder =
  fun input ->
    let output = ??? // create a OrderPlaced value here
    output