// =================================
// Anagram game - Low Level Interpreter implementation
// using computation expressions
// =================================


(*
In this implementation, a computation expression is used to hide the
details of the continuations.

What's nice about this is that it hides the continuation passing

    // using continations
    WriteLine (sprintf "The anagram is '%s'" anagram, fun () ->
    WriteLine ("Please enter your word, or <enter> to see the word, or '.' to quit", fun () ->
    ReadLine (fun input ->

    // using a CE is cleaner
    do! writeLine (sprintf "The anagram is '%s'" anagram)
    do! writeLine "Please enter your word, or <enter> to see the word, or '.' to quit"
    let! input = readLine()
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


type Program<'a> =
    //                (params for IO)       (handle response from interpreter)
    | ReadLine     of (*none*)              (string -> Program<'a>)
    | WriteLine    of string              * (unit -> Program<'a>)
    | Exit
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
        | ReadLine next ->
            ReadLine(next >> bindP f)
        | WriteLine(str,next) ->
            WriteLine(str,next >> bindP f)
        | Exit ->
            Exit

    // define a computation expression builder
    type ProgramBuilder() =
        member this.Return(x) = returnP x
        member this.Bind(x,f) = bindP f x

    // create an instance of the computation expression builder
    let programCE = ProgramBuilder()

    // helper functions
    let stop x = Stop x
    let readLine () = ReadLine(stop)
    let writeLine str = WriteLine(str,stop)
    let exit = Exit

// =============================================
// All the pure code -- completely deterministic and testable
// =============================================

open ProgramCE

module Pure =

    let printGameState gameState : Program<_> =
        programCE {
            do! writeLine (sprintf "You have played %i rounds, and solved %i anagrams" gameState.Rounds gameState.AnagramsSolved)
            }

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
            do! writeLine "\n== new round ==\n"
            do! writeLine (sprintf "The anagram is '%s'" anagram)
            do! writeLine "Please enter your word, or <enter> to see the word, or '.' to quit"

            // Step 3. Accept input
            let! input = readLine()

            match input with

            // Step 4a. If the input is "." then quit
            | "." ->
                do! writeLine (sprintf "The word was '%s'" target)
                do! writeLine "Game over. Thanks for playing"
                do! printGameState gameState
                do! exit

            // Step 4b. If the input is CR then show the answer
            | "" ->
                do! writeLine (sprintf "The word was '%s'" target)
                do! printGameState gameState
                do! play gameState  // play again with current gameState

            // Step 4c & 4d.
            | guess ->
                // 4c. If the input is an anagram of the displayed word AND is a valid word
                if guess |> gameState.IsDictionaryWord && Anagram.isAnagram guess anagram then
                    // increment AnagramsSolved
                    let gameState = {gameState with AnagramsSolved = gameState.AnagramsSolved + 1}
                    do! writeLine "Solved!"
                    do! printGameState gameState
                    do! play gameState  // play again with current gameState
                // 4d
                else
                    do! writeLine (sprintf "Failed! The word was '%s'" target)
                    do! printGameState gameState
                    do! play gameState  // play again with current gameState

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

    let writeLine str =
        printfn "%s" str

    let readLine() =
        System.Console.ReadLine()

// =============================================
// Top level code
// =============================================

// interpret a Program object
let rec interpret (program:Program<_>) =
    match program with
    | ReadLine next ->
        let input = Impure.readLine()
        let newProgram = next input
        interpret newProgram
    | WriteLine (str,next) ->
        Impure.writeLine str
        let newProgram = next()
        interpret newProgram
    | Exit ->
        () // exit loop
    | Stop _ ->
        () // exit loop


// the main "play" function that creates an initial Program and then
// starts intepreting it
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
dotnet fsi 04f-AnagramGame-LowLevelInterpreterCE.fsx
*)
// which will make the following code work
if System.Environment.CommandLine.Contains(__SOURCE_FILE__) then
    // running from comment line
    play()