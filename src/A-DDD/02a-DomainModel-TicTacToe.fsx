// Demonstrates the TicTacToe domain written as code
module rec TicTacToe

// ==============================
// Basic F# Syntax for types
// (see also fsharp-basic-syntax.fsx in parent directory)
// ==============================

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


//============================================
// Your code starts here


// 1. The "PlayMove" workflow
// The input is a pair: information from outside + game state loaded from storage
//     This could be refactored to a single record with two fields.
// The output is a pair: information to return to outside + game state to save to storage
type PlayMove =
   // everything on this side is input
   MoveInformation * GameState -> MoveResult * GameState
   //                             everything on this side is output

// you might also expose a version where the internal state is hidden
type PlayMoveWebAPI =
   MoveInformation -> MoveResult


// 2. MoveInformation is modeled as Player AND Position

// MoveInformation could be implemented as a tuple
(*
type MoveInformation = Player * Position
*)

// Or, MoveInformation could be implemented as a record
type MoveInformation = {
    Player : Player
    Position : Position
    }

// 3. Player is modeled as X OR O
(*
type Player =
    | X
    | O
*)
type Player = X | O  // shorter version of above, like an enum

// 4. Position is modeled as Row AND Column

// Position could be implemented as a tuple
(*
type Position = Row * Column
*)
// Or, Position could be implemented as a record
type Position = {
    Row: Row
    Column : Column
    }

// 5. Row and Column are simple type with constraints
type Row = int // between 1..3
type Column = int // between 1..3

// 6. MoveResult is Win OR Draw OR NextTurn
type MoveResult =
    | Win of Player
    | Draw
    | NextTurn of Player

// 7. The game state is the state of all the cells
type GameState = CellState list

type CellState =
    Position * Player option

