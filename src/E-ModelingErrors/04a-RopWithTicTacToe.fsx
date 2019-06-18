(*
TicTacToe using Result
*)

#load "Result.fsx"

module Domain =
    type Player = X | O

    type Square = {
      Row: int
      Col: int
    }

    type Move = {
      Player : Player
      Square : Square
    }

    type MoveError =
      | SquareAlreadyPlayed
      | NotYourTurn
      | GameFinished

    type MoveResult =
      | Draw
      | Winner of Player
      | KeepPlaying
      | BadMove of MoveError

    type PlayMove = Move -> MoveResult


// =========================
// Internal steps
// =========================

open Domain

type GameState = {
    Moves: Move list
    PlayerToMove : Player
    Result : MoveResult
}


//----------------
// game state database
//----------------

let initialGameState = {
    Moves=[]
    PlayerToMove = X
    Result=KeepPlaying
    }


//----------------
// internal steps
//----------------

/// Has the square been played before
let checkValidSquare gameState newMove =
  // helper function for use in List.exists below
  let squareIsPlayed existingMove =
    existingMove.Square = newMove.Square

  // do the test by checking all the previous moves
  if gameState.Moves |> List.exists squareIsPlayed then
    Error SquareAlreadyPlayed
  else
    Ok newMove

/// Is it the correct players turn?
let checkValidPlayer gameState newMove =
  if gameState.PlayerToMove <> newMove.Player then
    Error NotYourTurn
  else
    Ok newMove

/// A helper function to return the next player
let otherPlayer player =
  match player with
  | X -> O
  | O -> X

/// Update the game state after a new move
let updateGameState gameState newMove =
  {gameState with
     Moves = newMove :: gameState.Moves
     PlayerToMove = otherPlayer gameState.PlayerToMove
     Result = KeepPlaying
  }


/// The core workflow - a "pure" function with no I/O.
/// That is, the gameState is provided as a parameter
/// and we do NOT know about the database.
/// This is what we should unit test.
let pureWorkflow gameState newMove =
  newMove
  |> checkValidSquare gameState
  |> Result.bind (checkValidPlayer gameState)
  |> Result.map (updateGameState gameState)


/// This is defined AFTER the domain logic so that
/// it can't accidentally be used!
module DB =
    /// the "database"
    let mutable gameState = initialGameState

    /// Load game from database
    let loadGameState() =
      gameState

    /// Save game to database
    let saveGameState newGameState =
      gameState <- newGameState

/// The main module combines all the components
module Api =

    /// The top-level workflow function with I/O
    /// It loads from the database, calls the pure domain logic,
    /// and then saves state back to the database.
    let makeMove newMove =
        // IO here
        let gameState = DB.loadGameState()

        // pure
        let result = pureWorkflow gameState newMove

        // make MoveResult
        let moveResult =
          match result with
          | Ok gameState ->
              gameState.Result
          | Error err ->
              BadMove err

        // IO here
        match result with
        | Ok gameState ->
            DB.saveGameState gameState
        | Error _  ->
            () //ignore

        // final output
        result


    /// initialize game
    let newGame() =
        // IO here
        DB.saveGameState initialGameState


// ==================================
// test data
// ==================================

let move1={Player=X; Square={Row=1;Col=1}}
let move2={Player=O; Square={Row=1;Col=2}}
let move3={Player=X; Square={Row=2;Col=1}}

Api.newGame()
Api.makeMove move1
Api.makeMove move2
Api.makeMove move3