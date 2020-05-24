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


// --------------------
// Use Choices for OR
// --------------------

type MyChoice =
    | Choice1 of Choice1Data
    | Choice2 of Choice2Data

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


type OrderLine = ??

(*
------------------------------------------
data Blog =
  BlogName
  AND list of BlogPosts

data BlogName = all printable chars, maxlen = 100
*)

type Blog = ??


(*
------------------------------------------
data TShirtColor = Black OR Red OR Blue
data TShirt =
  Large (with TShirtColor)
  OR Small (with TShirtColor)
*)

type TShirtColor = ??
type TShirt = ??



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
type PlayMove = Move * GameState -> MoveResult * GameState
