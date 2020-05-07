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
let addToPreviousMoves (move:Move) (previousMoves:MoveList) =  // return a new list
    move::previousMoves

// Count how many times a possible next move occurs
// in conjection with a previous move.
// This is a recursive function that loops through the
// list just as we implemented earlier.
let rec countOfNextMoveGivenPreviousMove nextMove previousMove count list =
    match list with
    | [] -> count
    | [x] -> count // need two elements at least
    | x::y::rest ->
        let newCount =
            if x = nextMove && y = previousMove then
                count + 1
            else
                count // unchanged
        let newList = y::rest
        countOfNextMoveGivenPreviousMove nextMove previousMove newCount newList

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
    let rCount = countOfNextMoveGivenPreviousMove Rock previousMove 0 list
    let pCount = countOfNextMoveGivenPreviousMove Paper previousMove 0 list
    let sCount = countOfNextMoveGivenPreviousMove Scissors previousMove 0 list
    if rCount > pCount && rCount > sCount then
        // opponent most likely to play R
        Rock
    else if pCount > sCount then
        // opponent most likely to play P
        Paper
    else
        Scissors

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
    | Paper -> Scissors
    | Scissors -> Rock
    | Quit -> Quit

// Get a move from the human
let rec getMoveFromInput() =
    printfn "Please enter r,p,s or q"
    let str = System.Console.ReadLine()
    match str with
    | "r" | "R" -> Rock
    | "p" | "P" -> Paper
    | "s" | "S" -> Scissors
    | "q" ->
        Quit
    | _ ->
        printfn "Try again"
        getMoveFromInput()

// print the winner between the human and the computer
let printWinner humanMove computerMove score =
    match (humanMove,computerMove) with
    | Rock,Scissors | Paper,Rock | Scissors,Paper ->
        printfn "Computer=%A" computerMove
        let newScore = score + 1
        printfn "Human wins. Score=%i" newScore
        newScore // return
    | Scissors,Rock | Rock,Paper | Paper,Scissors ->
        printfn "Computer=%A" computerMove
        let newScore = score - 1
        printfn "Computer wins. Score=%i" newScore
        newScore // return
    | _ ->
        printfn "Computer=%A" computerMove
        printfn "draw"
        score

/// Play one round of the game, and then loop if not quit
let rec playGame previousMove previousMoves score =
    // play
    let humanMove = getMoveFromInput()
    if humanMove <> Quit then
        let computerMove = moveToPlay previousMove previousMoves
        let newScore = printWinner humanMove computerMove score

        // go round again
        let previousMoves = previousMoves |> addToPreviousMoves humanMove
        let previousMove = humanMove
        playGame previousMove previousMoves newScore
    else
        printfn "Quitting with score: %i" score

// start
let start() =
    playGame Rock [] 0




// to play from the command line:
//   fsi 12b-Exercise-RockPaperScissors(answer).fsx
