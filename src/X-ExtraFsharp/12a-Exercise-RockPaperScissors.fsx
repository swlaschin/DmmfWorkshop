// ================================================
// Rock paper scissors
// ================================================

(*
Write a program that plays rock-paper-scissors

There are three moves: Rock, Paper and Scissors

Keep track of the opponents moves in a list,
and use this to calculate a probability of their next move.
*)

type Move = Rock | Paper | Scissors | Quit
type MoveList = Move list

// Add the opponents move of the lost of previous moves
let addOpponentMove (move:Move) (previousMoves:MoveList) =  // return a new list
    ??

// Count how many times a possible next move occurs
// in conjection with a previous move.
// This is a recursive function that loops through the
// list just as we implemented earlier.

let rec countOfNextMoveGivenPreviousMove nextMove previousMove count list =
    ??

// test
(*
let list = [Rock;Paper;Paper;Paper]
countOfNextMoveGivenPreviousMove Rock Paper 0 list  // expect 1
countOfNextMoveGivenPreviousMove Paper Paper 0 list  // expect 2
countOfNextMoveGivenPreviousMove Scissors Paper 0 list  // expect 0
*)


// using "countOfNextMoveGivenPreviousMove" return the most common
// move given a previousMove
let findMostCommonMoveGivenPreviousMove (previousMove:Move) (list:MoveList) :Move =
    ??
(*
// test
let previousMoves = [Rock;Paper;Scissors;Rock;Paper;Scissors]
findMostCommonMoveGivenPreviousMove Paper previousMoves  // expect Rock
findMostCommonMoveGivenPreviousMove Scissors previousMoves  // expect Paper
*)

// Given a previous move by the opponent, return the best move
// to play against that.
let moveToPlay previousMove list =
    let opponentsMove = findMostCommonMoveGivenPreviousMove previousMove list
    match opponentsMove with
    | Rock -> Paper
    | Paper -> ??
    | Scissors -> ??
    | Quit -> Quit

// Get a move from the human
let rec getMoveFromInput() =
    printfn "Please enter r,p,s or q"
    let str = System.Console.ReadLine()
    match str with
    | "r" | "R" -> Rock
    | "p" | "P" -> ??
    | "s" | "S" -> ??
    | "q" ->
        Quit
    | _ ->
        printfn "Try again"
        getMoveFromInput()

// print the winner between the human and the computer
let printWinner humanMove computerMove score =
    match (humanMove,computerMove) with
    | ?,? | ?,? |  ?,? ->
        let newScore = score + 1
        printfn "Human wins. Score=%i" newScore
        newScore // return
    | ?,? | ?,? | ?,? ->
        let newScore = score - 1
        printfn "Computer wins. Score=%i" newScore
        newScore // return
    | _ ->
        printfn "draw"
        score

/// Play one round of the game, and then loop if not quit
let rec playGame previousMove previousMoves score =
    // play
    let humanMove = getMoveFromInput()
    if ?? then
        let computerMove = moveToPlay previousMove previousMoves
        let newScore = printWinner humanMove computerMove score

        // go round again
        let previousMoves = ??
        let previousMove = humanMove
        playGame previousMove previousMoves newScore
    else
        printfn "Quitting with score: %i" score

// start
let start() =
    playGame Rock [] 0


start()

// to play from the command line:
//   fsi 12a-Exercise-RockPaperScissors.fsx

