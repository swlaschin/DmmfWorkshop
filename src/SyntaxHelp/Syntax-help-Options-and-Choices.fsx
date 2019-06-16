
// ================================================
// "Option" types 
// ================================================

// F# doesn't allow nulls by default -- you must use an Option type
// and then pattern match.  
// Some(..) and None are roughly analogous to Nullable wrappers
let validValue = Some 99
let invalidValue = None

// In this example, match..with matches the "Some" and the "None",
// and also unpacks the value in the "Some" at the same time.
let printOption input =
   match input with
    | Some i -> printfn "input is an int=%d" i
    | None -> printfn "input is missing"

printOption validValue
printOption invalidValue

// Exercise  - Why don't you need a "_" case in addition to Some and None?

// Exercise - what happens if you leave off the Some case in "printOption" above?

// Exercise - what happens if you leave off the None case in "printOption" above?


// ================================================
// "Choice" types 
// ================================================

// Choice types (called "Discriminated Unions") are data structures with a set of choices
// A value must be only *one* of these choices at a time.

type CardType = 
    | Visa
    | Mastercard

// They look like C# enums, but unlike enums, they can have data associated with each choice
type PaymentMethod = 
  | Cash
  | Cheque of int
  | Card of CardType * string   // recognize this type as a tuple?


// To create a choice type, use the "constructor" for that choice

// for enum-style choices, it is simple
let visa = Visa
let mc = Mastercard

// for choices with extra data, call the constructor, passing in the required data at the same time

let cheque = Cheque 42
let card = Card (visa,"1234")

// to get data out, you must pattern matching for each case
let printPayment paymentMethod = 
    match paymentMethod with
    | Cash ->  printfn "Paid in cash"
    | Cheque checkNo -> printfn "Paid by cheque: %i" checkNo
    | Card (cardType,cardNo) -> printfn "Paid with %A %A" cardType cardNo


// each case looks a bit like a lambda, with an arrow after the pattern is matched
//  | [choice] [associated data] -> action

// example
let paymentMethod1 = Card(Visa, "1234")
printPayment paymentMethod1

let paymentMethod2 = Cheque 42
printPayment paymentMethod2

let paymentMethod3 = Cash
printPayment paymentMethod3

