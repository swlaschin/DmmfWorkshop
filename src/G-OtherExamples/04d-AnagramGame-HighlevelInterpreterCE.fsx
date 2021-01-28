// =================================
// Anagram game - High Level Interpreter implementation
// using computation expressions
// =================================

(*
In this implementation, a computation expression is used to hide the
details of the continuations.

What's nice about this is that it hides the continuation passing

    // using continations
    GetInput (anagram,fun input ->
        match input with

    // using a CE is cleaner
    let! input = getInput anagram
    match input with
*)

#load "AnagramDictionary.fsx"
open AnagramDictionary

// =============================================
// The domain with the state and the program types
// =============================================

type GameState = {
    Rounds : int
    AnagramsSolved : int
    IsDictionaryWord : string -> bool
    RandomDictionaryWords : string seq
}

type Target = string
type Anagram = string
type Input = string

type Program<'a> =
    //                (params for IO)       (handle response from interpreter)
    | GetInput     of Anagram             * (Input -> Program<'a>)
    | RevealTarget of GameState * Target  * (unit -> Program<'a>)
    | CorrectGuess of GameState           * (unit -> Program<'a>)
    | FailedGuess  of GameState * Target  * (unit -> Program<'a>)
    | Exit         of GameState * Target
    // added to keep CE generic
    | Stop         of 'a

// =============================================
// Code to build the Computation Expression
// =============================================

module ProgramCE =

    let returnP (x:'a) :Program<'a> =
        Stop x

    let rec bindP (f:'a->Program<'b>) (program:Program<'a>) : Program<'b> =
        match program with
        | Stop x ->
            f x
        | GetInput(anagram,next) ->
            GetInput(anagram,next >> bindP f)
        | RevealTarget(gameState,target,next) ->
            RevealTarget(gameState,target,next >> bindP f)
        | CorrectGuess(gameState,next) ->
            CorrectGuess(gameState,next >> bindP f)
        | FailedGuess(gameState,target,next) ->
            FailedGuess(gameState,target,next >> bindP f)
        | Exit(gameState,target) ->
            Exit(gameState,target)  // bind does not apply

    // define a computation expression builder
    type ProgramBuilder() =
        member this.Return(x) = returnP x
        member this.Bind(x,f) = bindP f x

    // create an instance of the computation expression builder
    let programCE = ProgramBuilder()

    // helper functions
    let stop x = Stop x
    let getInput anagram  = GetInput (anagram,stop)
    let revealTarget gameState target = RevealTarget (gameState,target,stop)
    let correctGuess gameState = CorrectGuess (gameState,stop )
    let failedGuess gameState target = FailedGuess (gameState,target,stop)
    let exit gameState target = Exit(gameState,target)

// =============================================
// All the pure code -- completely deterministic and testable
// =============================================

open ProgramCE

module Pure =

    // using a computation, the pure code can be written l
    let rec play (gameState:GameState) : Program<_> =
        programCE {
            // update the game state
            let gameState = {gameState with Rounds = gameState.Rounds + 1}

            // Step 1. Pick a word (the TARGET) from the wordlist
            let target = gameState.RandomDictionaryWords |> Seq.head
            let anagram =
                Anagram.permutations target
                |> Seq.filter (gameState.IsDictionaryWord >> not)
                |> Seq.head

            // Step 2. tell the UI to print the anagram and instructions and get input
            let! input = getInput anagram

            // Step 3. Accept input
            match input with

            // Step 4a. If the input is "." then quit
            | "." ->
                do! exit gameState target

            // Step 4b. If the input is CR then show the answer
            | "" ->
                do! revealTarget gameState target
                do! play gameState   // play again

            // Step 4c & 4d.
            | guess ->
                // 4c. If the input is an anagram of the displayed word AND is a valid word
                if guess |> gameState.IsDictionaryWord && Anagram.isAnagram guess anagram then
                    // increment AnagramsSolved
                    let gameState = {gameState with AnagramsSolved = gameState.AnagramsSolved + 1}
                    do! correctGuess gameState
                    do! play gameState   // play again
                // 4d
                else
                    do! failedGuess gameState target
                    do! play gameState   // play again
        }

// =============================================
// All the impure code -- this could be made async to make it obvious!
// =============================================

module Impure =

    let initialGameState() =
        let wd =
            let filename = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"WordList10000.txt")
            WordDictionary.load filename
        let randomDictionaryWords = wd |> WordDictionary.randomWordGenerator 4 8
        let isDictionaryWord = WordDictionary.isWord wd
        {
        Rounds = 0
        AnagramsSolved = 0
        IsDictionaryWord = isDictionaryWord
        RandomDictionaryWords = randomDictionaryWords
        }

    /// Print the game state
    let printGameState (gameState:GameState) =
        printfn "You have played %i rounds, and solved %i anagrams" gameState.Rounds gameState.AnagramsSolved

    /// Print the anagram and instructions
    let printAnagramAndInstructions anagram =
        printfn "\n== new round ==\n"
        printfn "The anagram is '%s'" anagram
        printfn "Please enter your word, or <enter> to see the word, or '.' to quit"

    /// Print the target (in response to CR)
    let printTarget target =
        printfn "The word was '%s'" target

    let printSolved() =
        printfn "Solved!"

    let printFailed target =
        printfn "Failed! The word was '%s'" target

    /// Print the game over message (in response to ".")
    let printGameOver target =
        printfn "The word was '%s'" target
        printfn "Game over. Thanks for playing"

    let readLine() =
        System.Console.ReadLine()

// =============================================
// Top level code
// =============================================

// interpret a Program object
let rec interpret (program:Program<_>) =
    match program with
    | GetInput (anagram, next) ->
        Impure.printAnagramAndInstructions anagram
        let input = Impure.readLine()
        let newProgram = next input
        interpret newProgram
    | RevealTarget (gameState,target,next) ->
        Impure.printTarget target
        Impure.printGameState gameState
        let newProgram = next()
        interpret newProgram
    | CorrectGuess (gameState,next) ->
        Impure.printSolved()
        Impure.printGameState gameState
        let newProgram = next()
        interpret newProgram
    | FailedGuess (gameState,target,next) ->
        Impure.printFailed target
        Impure.printGameState gameState
        let newProgram = next()
        interpret newProgram
    | Exit (gameState,target) ->
        Impure.printGameOver target
        Impure.printGameState gameState
        // exit loop
    | Stop _ ->
        ()
        // exit loop

// the top level function that mixes pure and impure code in a loop
let play() =

    let initialGameState = Impure.initialGameState()
    let initialProgram = Pure.play initialGameState
    interpret initialProgram

// to start the game interactively,
// 1. highlight all code and execute
// 2. type this in the F# interactive terminal
(*
play();;
*)
// if you need to kill the game!
// * in Visual Studio, "Reset Interactive Session"
// * in VS Code, kill the terminal

// or to run from the command line, do
(*
dotnet fsi 04d-AnagramGame-HighLevelInterpreterCE.fsx
*)
// which will make the following code work
if System.Environment.CommandLine.Contains(__SOURCE_FILE__) then
    // running from comment line
    play()