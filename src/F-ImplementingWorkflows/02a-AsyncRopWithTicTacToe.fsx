(*
TicTacToe using async

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

let checkValidSquare gameState newMove =
  let squareIsPlayed existingMove =
    existingMove.Square = newMove.Square
  if gameState.Moves |> List.exists squareIsPlayed then
    Error SquareAlreadyPlayed
  else
    Ok newMove

let checkValidPlayer gameState newMove =
  if gameState.PlayerToMove <> newMove.Player then
    Error NotYourTurn
  else
    Ok newMove

let otherPlayer player =
  match player with
  | X -> O
  | O -> X

let updateGameState gameState newMove =
  {gameState with
     Moves = newMove :: gameState.Moves
     PlayerToMove = otherPlayer gameState.PlayerToMove
     Result = KeepPlaying
  }


/// pure workflow function with no I/O
let pureWorkflow gameState newMove =
  newMove
  |> checkValidSquare gameState
  |> Result.bind (checkValidPlayer gameState)
  |> Result.map (updateGameState gameState)

/// This is defined AFTER the domain logic so that
/// it can't accidentally be used!
module DB =

    let mutable gameState = initialGameState

    let loadGameState() =
      async.Return gameState

    let saveGameState newGameState =
      gameState <- newGameState
      async.Return ()

/// The main module combines all the components
module Api =

    /// final workflow function with I/O
    let makeMove newMove =
      async {
        // IO here
        let! gameState = DB.loadGameState()

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
            do! DB.saveGameState gameState
        | Error _  ->
            () //ignore

        // final output
        return result
      }

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

Api.newGame()      |> Async.RunSynchronously
Api.makeMove move1 |> Async.RunSynchronously
Api.makeMove move2 |> Async.RunSynchronously
Api.makeMove move3 |> Async.RunSynchronously