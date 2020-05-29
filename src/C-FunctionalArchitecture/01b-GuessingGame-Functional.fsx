// =================================
// Guessing game - Functional implementation
//
// How the game works:
// * Think of a number between 1..100
// * The computer guesses a number (e.g. 50) and asks
//   "is your number bigger than the guess"
// * You say: yes or no
// * The computer guesses again,
//   or if it knows, tells you your number
// =================================

(*
Implementation details:

1. Ask "Think of a number between 1 and 100"
2. Set the bounds for a new game to 1..100
3. Given the bounds, make a guess 
   (or if you know the answer go to step 7)
4. Show the guess with the text "Is your number bigger than %i? [y,n,q]"
5. Accept input
6a. If the input is "y" or "n" then 
       update the bounds and go to step 3
6b. If the input is "q" then 
       show "Game over. Thanks for playing" and quit
6c. If the input is something else then 
       show "Please enter [y,n,q]" and go to step 5 
----
7. (jump from 3) If you know the answer, show "Your number is %i"
8. Show "Do you want to play again or quit? [y,q]"
9. Accept input
10a. If the input is "y"  then 
   go to step 1
10b. If the input is "q" then 
   show "Game over. Thanks for playing" and quit
10c. If the input is something else then 
   show "Please enter [y,q]" and go to step 9 

*)


// =================================
// This is the functional implementation
// * IO is at the edges
// * state is immutable
// =================================




// ==============================
// Model
// ==============================

type Bounds = {
    lowerBound : int
    upperBound : int
    }

type Guess = int

// The overall state
type GameState =
    | FirstGuess of Guess * Bounds
    | AnotherGuess of Guess * Bounds
    | KnownNumber of Guess
    | GameOver

// Step 3. Given the bounds, make a guess 
//         (or if you know the answer go to step 7)
let makeAnotherGuess bounds =
    if bounds.lowerBound = bounds.upperBound then
        // we know for sure
        KnownNumber bounds.lowerBound
    else
        // make a another guess
        let guess = (bounds.lowerBound + bounds.upperBound) / 2
        AnotherGuess (guess, bounds)

// start of new game
let initialGameState =
    let bounds = {lowerBound=1; upperBound=100}
    let guess = (bounds.lowerBound + bounds.upperBound) / 2
    FirstGuess (guess, bounds)

// ==============================
// Update
// ==============================

// Input/Events from the user overall
type Msg =
    | Higher
    | NotHigher
    | PlayAgain
    | Quit


let update msg gameState =
    match gameState,msg with
    | FirstGuess (currentGuess,bounds), Higher
    | AnotherGuess (currentGuess,bounds), Higher  ->
        // update bounds and guess again
        makeAnotherGuess {bounds with lowerBound = currentGuess + 1}
    | FirstGuess (currentGuess,bounds), NotHigher
    | AnotherGuess (currentGuess,bounds), NotHigher  ->
        // update bounds and guess again
        makeAnotherGuess {bounds with upperBound = currentGuess}
    | _, PlayAgain ->
        // reset bounds and start again
        initialGameState
    | _, Quit  ->
        GameOver
    | GameOver, _ ->
        failwith "Should not receive any input when game is over"
    | _, _ ->
        failwith "Invalid combination of input and gamestate"

// ==============================
// View
// ==============================

/// When the input key matches the keypress,
/// the corresponding msg will be sent back into the game
type Command = { keypress:string; msg:Msg }

/// Things the user interface can do
type UiElement =
    // show a message
    | Show of message:string
    // accept a keystroke and try to match it to one of the commands
    | Input of Command list

let isYourNumberBiggerThan guess =
    let elem1 =
        Show (sprintf "Is your number bigger than %i? [y,n,q]" guess)
    let elem2 =
        let commands = [
            {keypress="y"; msg=Higher}
            {keypress="n"; msg=NotHigher}
            {keypress="q"; msg=Quit}
        ]
        Input commands
    [elem1; elem2]

// Convert the game state into a list of commands
let view gameState  =
    match gameState with
    | FirstGuess (guess,bounds) ->
        let elem1 =
            let message = sprintf "Think of a number between %i and %i " bounds.lowerBound bounds.upperBound
            Show message
        let uiElements = isYourNumberBiggerThan guess
        elem1 :: uiElements
    | AnotherGuess (guess,bounds) ->
        let uiElements = isYourNumberBiggerThan guess
        uiElements
    | KnownNumber guess ->
        let elem1 =
            Show (sprintf "Your number is %i" guess)
        let elem2 =
            Show (sprintf "Do you want to play again or quit? [y,q]")
        let elem3 =
            let commands = [
                {keypress="y"; msg=PlayAgain}
                {keypress="q"; msg=Quit}
            ]
            Input commands
        [elem1; elem2; elem3]
    | GameOver ->
        let elem =
            Show "Game over. Thanks for playing"
        [elem]

// ==============================
// Main game loop
// ==============================

// The interface for all possible interactions with the user
type UserInterface = {
    Show : string -> unit
    GetInputString : unit -> string
    }

// given a Cmd as output, call the user interface
// and return an optional Msg for input
let handleCmd ui cmd  =
    match cmd with
    | Show msg ->
        ui.Show msg
        None
    | Input actions ->
        let rec loopUntilValidKey() =
            let key = ui.GetInputString()
            // does this match any of the actions
            match actions |> List.tryFind (fun action -> action.keypress = key) with
            | Some action ->
                action.msg
            | None ->
                let validKeys =
                    actions
                    |> List.map (fun action -> action.keypress)
                    |> String.concat ", "
                ui.Show (sprintf "Please enter [%s]" validKeys)
                loopUntilValidKey()
        loopUntilValidKey() |> Some

let rec gameLoop io gameState =
    let cmds = view gameState
    let msg = cmds |> List.choose (handleCmd io) |> List.tryHead
    match msg with
    | Some msg ->
        let newState = update msg gameState
        gameLoop io newState
    | None ->
        ()

// ==============================
// Top level
// ==============================

let io = {
    Show = fun msg -> System.Console.WriteLine msg
    GetInputString = fun () -> System.Console.ReadLine()
}

// start the game
gameLoop io initialGameState

// to start the game, type this in the F# terminal
(*
gameLoop io initialGameState;;
*)

// or to run from the command line
(*
dotnet fsi 01a-GuessingGame-Functional.fsx
*)

// if you need to kill the game!
// * in Visual Studio, "Reset Interactive Session"
// * in VS Code, kill the terminal


