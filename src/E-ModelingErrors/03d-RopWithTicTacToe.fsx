// =================================
// This file demonstrates the TicTacToe
// with Result and error types to document things that
// can go wrong.
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// Load a file with library functions for Result
#load "Result.fsx"

// =========================
// The domain model
// =========================

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

    type GameState = {
        Moves: Move list
        PlayerToMove : Player
        Result : MoveResult
    }

    /// Make a move and return the new GameState
    type MakeMove = Move -> Result<GameState,MoveError>


// =========================
// Pure implementation code (no I/O)
// =========================


/// the core logic of the game
module Implementation =
    open Domain

    let initialGameState = {
        Moves=[]
        PlayerToMove = X
        Result=KeepPlaying
        }

    /// Is the move valid?
    let checkValidSquare gameState newMove =

        // helper function
        let squareIsPlayed existingMove =
            existingMove.Square = newMove.Square

        // check if the square is already played
        if gameState.Moves |> List.exists squareIsPlayed then
            Error SquareAlreadyPlayed
        else
            Ok newMove

    /// Is the player valid?
    let checkValidPlayer gameState newMove =
        if gameState.PlayerToMove <> newMove.Player then
            Error NotYourTurn
        else
            Ok newMove

    /// What is the other player
    let otherPlayer currentPlayer =
        match currentPlayer with
        | X -> O
        | O -> X

    /// Update the game state with a normal move
    /// NOTE we are not handling the logic for the end of the game yet!
    let updateGameState gameState newMove =
        {gameState with
             Moves = newMove :: gameState.Moves
             PlayerToMove = otherPlayer gameState.PlayerToMove
             Result = KeepPlaying
        }

    /// The pure workflow function with no I/O
    let pureGameWorkflow gameState newMove =
      newMove
      |> checkValidSquare gameState
      |> Result.bind (checkValidPlayer gameState)
      |> Result.map (updateGameState gameState)


// =========================
// Impure implementation code (with I/O)
// =========================

/// The database to store the game state
module Database =
    open Implementation

    // the "database" is just a mutable variable in memory :)
    let mutable gameState = initialGameState

    /// Load the game state from the database
    let loadGameState() =
      gameState

    /// Save the game state to the database
    let saveGameState newGameState =
      gameState <- newGameState


/// final workflow function with I/O
let makeMove : Domain.MakeMove =
    fun newMove ->
        // IO here
        let gameState = Database.loadGameState()

        // pure code
        let result = Implementation.pureGameWorkflow gameState newMove

        // make a MoveResult from the output of the game logic
        let moveResult =
          match result with
          | Ok gameState ->
              gameState.Result
          | Error err ->
              Domain.BadMove err

        // IO here
        match result with
        | Ok gameState ->
            Database.saveGameState gameState
        | Error _  ->
            () //ignore

        // final output
        result


/// initialize game
let newGame() =
    // IO here
    Database.saveGameState Implementation.initialGameState


// ==================================
// play a game
// ==================================

open Domain

let xMove1={Player=X; Square={Row=1;Col=1}}
let oMove1={Player=O; Square={Row=1;Col=2}}
let xMove2={Player=X; Square={Row=2;Col=1}}

newGame()
makeMove xMove1  // OK
makeMove xMove2  // Error NotYourTurn
makeMove oMove1  // OK
makeMove oMove1  // Error SquareAlreadyPlayed
makeMove xMove2  // OK
