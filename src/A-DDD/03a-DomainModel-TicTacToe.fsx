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


// The "PlayMove" workflow
// The input is a pair: information from outside + game state loaded from storage
//     This could be refactored to a single record with two fields.
// The output is a pair: information to return to outside + game state to save to storage
type PlayMove = MoveInformation * GameState -> MoveResult * GameState

// MoveInformation is Position AND Player
type MoveInformation = {
    Position : Position
    Player : Player
    }

// Player is X OR O
type Player =
    | X
    | O

// Position is Row AND Column
type Position = {
    Row: Row
    Column : Column
    }

// Row and Column are simple type with constraints
type Row = int // between 1..3
type Column = int // between 1..3

// MoveResult is Win OR Draw OR NextTurn
type MoveResult =
    | Win of Player
    | Draw
    | NextTurn of Player


// GameState is a list of the CellStates
type GameState = CellState list

// CellState is a Position AND optionally, if a player has played there
type CellState = {
    Position : Position
    Played : Player option
    }

