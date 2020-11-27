// =================================
// Anagram game - Low Level Interpreter implementation
// =================================


(*
This is the Interpreter implementation
* The "Pure.play" function returns a "program" with instructions for the shell I/O
* The top level shell interprets those instructions by reading or writing
  However it could be interpreted in another way, e.g. with a GUI
* In this implementation, the instructions are "low level" 
  such as ReadLine, WriteLine, etc.
  The exact text to display is now part of the core logic.

What's good about this:
* Low level instructions are easier to write an interpreter for

What's bad about this:
* It's easy to have too much detail in the pure code now
  It looks very like the imperative code

    // ------------------------------
    // using high level instructions
    // ------------------------------
    GetInput (anagram,fun input ->
        match input with

    // ------------------------------
    // using low level instructions
    // ------------------------------
    WriteLine ("\n== new round ==\n", fun () ->
    WriteLine (sprintf "The anagram is '%s'" anagram, fun () ->
    WriteLine ("Please enter your word, or <enter> to see the word, or '.' to quit", fun () ->
    ReadLine (fun input ->

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


type Program =
    //                (params for IO)       (handle response from interpreter)
    | ReadLine     of (*none*)              (string -> Program)
    | WriteLine    of string              * (unit -> Program)
    | Exit         

// =============================================
// All the pure code -- completely deterministic and testable
// =============================================

module Pure =

    let printGameState gameState next : Program =
        WriteLine (sprintf "You have played %i rounds, and solved %i anagrams" gameState.Rounds gameState.AnagramsSolved, next)

    let rec play (gameState:GameState) : Program =
        // update the game state
        let gameState = {gameState with Rounds = gameState.Rounds + 1}

        // Step 1. Pick a word (the TARGET) from the wordlist
        let target = gameState.RandomDictionaryWords |> Seq.head
        let anagram =
            Anagram.permutations target
            |> Seq.filter (gameState.IsDictionaryWord >> not)
            |> Seq.head

        // Step 2. tell the UI to print the anagram and instructions and get input
        WriteLine ("\n== new round ==\n", fun () ->
        WriteLine (sprintf "The anagram is '%s'" anagram, fun () ->
        WriteLine ("Please enter your word, or <enter> to see the word, or '.' to quit", fun () ->

        // Step 3. Accept input
        ReadLine (fun input ->

        match input with

        // Step 4a. If the input is "." then quit
        | "." ->
            WriteLine (sprintf "The word was '%s'" target, fun () ->
            WriteLine ("Game over. Thanks for playing", fun () -> 
            printGameState gameState (fun () -> 
            Exit )))

        // Step 4b. If the input is CR then show the answer
        | "" ->
            WriteLine (sprintf "The word was '%s'" target, fun () ->
            printGameState gameState (fun () ->
            play gameState))  // play again with current gameState

        // Step 4c & 4d.
        | guess ->
            // 4c. If the input is an anagram of the displayed word AND is a valid word
            if guess |> gameState.IsDictionaryWord && Anagram.isAnagram guess anagram then
                // increment AnagramsSolved
                let gameState = {gameState with AnagramsSolved = gameState.AnagramsSolved + 1}
                WriteLine ("Solved!", fun () ->
                printGameState gameState (fun () ->
                play gameState))  // play again with current gameState
            // 4d
            else
                WriteLine(sprintf "Failed! The word was '%s'" target, fun () ->
                printGameState gameState (fun () ->
                play gameState))  // play again with current gameState

        ))))  // must match up all the brackets!

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
let rec interpret (program:Program) =
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
dotnet fsi 04e-AnagramGame-LowLevelInterpreter.fsx
*)
// which will make the following code work
if System.Environment.CommandLine.Contains(__SOURCE_FILE__) then
    // running from comment line
    play()