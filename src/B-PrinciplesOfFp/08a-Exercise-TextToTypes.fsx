// =================================
// Exercise : Converting textual domain model to F# types
// =================================

(*
Exercise
For each of the textual domain models below, convert them into a corresponding F# type

// IMPORTANT:  types must be defined BEFORE they are referenced
// (e.g. earlier in the file )

*)

(*
// --------------------
// Use Functions for workflows
// --------------------

// a single input and a single output
type Workflow = InputData -> OutputData

// a pair of inputs and a pair of outputs
type Workflow2 = InputData * State -> OutputData * State

// --------------------
// Use Records or tuples for AND
// --------------------

// a record with named fields
type ContactInfo = {
    // FieldName : FieldType
    Name : Name
    Address : Address
    }

// a pair
type ContactInfo = Name * Address

// a triplet
type ContactInfo = Name * Address * Email


(*
// Question: When to use tuples vs records?

// 1) a tuple has no named fields so you must construct and deconstruct explicitly
let info = name,address  // construct
let name,address = info  // deconstruct

// 2) a record has named fields so it is better when the types aren't self-documenting
type PersonalName = string * string                // unclear
type PersonalName = {first:string; last:string}    // better

// 3) Use records if the data *always* belongs together. Tuples are more casual.
// As always: it depends!
*)


// --------------------
// Use Choices for OR
// --------------------

type Choice1Data = ...
type Choice2Data = ...

type MyChoice =
    | Choice1 of Choice1Data
    | Choice2 of Choice2Data

// choices are used for "enums" as well
type DrinkSize = Big | Small

// --------------------
// Use type aliases for primitives
// --------------------

// document constraints in a comment
type OrderQuantity = int // must be > 0
type EmailAddress = string // must contain @ symbol

// --------------------
// Use list and option if needed
// --------------------

type Order = {
    OrderLines : OrderLine list
    DeliveryAddress : Address option // optional data
    }

*)

// TIP: this is a useful helper definition for unknown types
type undefined = exn

(*
------------------------------------------
data PersonalName = FirstName AND LastName
*)

type PersonalName = ??

(*
------------------------------------------
data OrderLine =
    OrderId
    AND Product
    AND OrderQuantity
*)

type OrderId = ??
type Product = ??
type OrderQuantity = ??
type OrderLine = ??

(*
------------------------------------------
data Blog =
  BlogName
  AND list of BlogPosts

data BlogName = all printable chars, maxlen = 100
*)

type BlogName = ??
type BlogPost = ??
type Blog = ??


(*
------------------------------------------
data TShirtColor = Black OR Red OR Blue
data TShirtSize = Large OR Small
data TShirt = TShirtSize AND TShirtColor

data HoodieColorSmall = Black OR White
data HoodieColorLarge = Red OR Blue
data Hoodie =
  Large (with HoodieColorLarge)
  OR Small (with HoodieColorSmall)
*)

type TShirtColor = ??
type TShirt = ??

type Hoodie = ??


(*
------------------------------------------
data ExpressShippingType = OneDay OR TwoDay
data ShippingMethod =
  Standard
  OR Express (with ExpressShippingType)
*)

type ExpressShippingType = ??
type ShippingMethod = ??


(*
------------------------------------------
workflow PlayMove =
    inputs: Move AND GameState
    outputs: MoveResult AND (new)GameState
*)

type Move = undefined
type MoveResult = undefined
type GameState = undefined

type PlayMove = Move * GameState -> ??
            // ^pair of inputs      ^output

// could also be written in curried style like this
type PlayMove_v2 = Move -> GameState -> ??
                // ^param1 ^param2      ^output
