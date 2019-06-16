#load "TicTacToe1_Server.fsx"
// -----------------------------------------------------------
// TicTacToe client - uses the version 1 implementation directly
// -----------------------------------------------------------

open TicTacToe1_Server.TicTacToeDomain
open TicTacToe1_Server.TicTacToeImplementation // access the implementation directly

let moveResult1 = newGameState

let moveResult2 =
    match moveResult1 with
    | OToMove (gameState, possibleMoves) ->
        let nextMove = PlayerOMove (Left, Top)  // set the next move here
        oMove gameState nextMove
    | _ -> failwith "not expected"

let moveResult3 =
    match moveResult2 with
    | XToMove (gameState, possibleMoves) ->
        let nextMove = PlayerXMove (Left, Center)  // set the next move here
        xMove gameState nextMove
    | _ -> failwith "not expected"

let moveResult4 =
    match moveResult3 with
    | OToMove (gameState, possibleMoves) ->
        let nextMove = PlayerOMove (Middle, Top) // set the next move here
        oMove gameState nextMove
    | _ -> failwith "not expected"

let moveResult5 =
    match moveResult4 with
    | XToMove (gameState, possibleMoves) ->
        let nextMove = PlayerXMove (Middle, Center) // set the next move here
        xMove gameState nextMove
    | _ -> failwith "not expected"
