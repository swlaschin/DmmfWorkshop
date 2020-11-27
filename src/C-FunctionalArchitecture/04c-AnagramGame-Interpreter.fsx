// =================================
// Anagram game - Interpreter implementation
//
// How the game works:
//
// In each round of the game, the player is presented with an
// anagram of a six-letter word where that anagram is not itself
// a word (so ANTLER might be presented as AELNRT but not LEARNT).
//
// The user can enter an answer, or hit return to see the answer,
// or enter a full stop to quit the game completely.
// If there is more than one solution to an anagram,
// all of them are valid (so ANTLER, LEARNT and RENTAL would all be correct
// answers to AELNRT)
//
// At the end of each round, the user is shown the number of rounds
// attempted and the number of anagrams solved.
// =================================

(*
Implementation details:

For each round:
1. Pick a word (the TARGET) from the wordlist and
   make an anagram of it such that the anagram is not also a valid word
2. Print the anagram and instructions
   "Please enter your guess, or <enter> to see the word, or '.' to quit"
3. Accept input from user
4a. If the input is "." then quit
        show "Game over. Thanks for playing"
        show the game state
        quit
4b. If the input is CR then show the answer
        show the TARGET
        update the game state: increment number of rounds
        show the game state
        and go to step 1
4c. If the input is an anagram of the displayed word AND is a valid word
        show "Solved!"
        update the game state: increment number of rounds, increment number of anagrams solved.
        show the game state
        and go to step 1
4d. If the input is NOT an anagram of the displayed word OR is not a valid word
        show "Failed!"
        update the game state: increment number of rounds
        show the game state
        and go to step 1
4. Quit (from 3a)
    show the game state
    exit

*)

// =====================================
// This is the Interpreter implementation
// * The "Pure.play" function returns a "program" with instructions for the shell I/O
// * The top level shell interprets those instructions by reading or writing
//   However it could be interpreted in another way, e.g. with a GUI
// =====================================

#load "AnagramDictionary.fsx"
open AnagramDictionary

// =============================================
// All the pure code
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

type Program =
    //                (params for IO)       (handle response from interpreter)
    | GetInput     of Anagram             * (Input -> Program)
    | RevealTarget of GameState * Target  * (unit -> Program)
    | CorrectGuess of GameState           * (unit -> Program)
    | FailedGuess  of GameState * Target  * (unit -> Program)
    | Exit         of GameState * Target

// =============================================
// All the pure code -- completely deterministic and testable
// =============================================

module Pure =

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
        GetInput (anagram,fun input ->

            // Step 3. Accept input
            match input with

            // Step 4a. If the input is "." then quit
            | "." ->
                Exit (gameState,target)

            // Step 4b. If the input is CR then show the answer
            | "" ->
                RevealTarget (gameState,target,fun () -> play gameState)

            // Step 4c & 4d.
            | guess ->
                // 4c. If the input is an anagram of the displayed word AND is a valid word
                if guess |> gameState.IsDictionaryWord && Anagram.isAnagram guess anagram then
                    // increment AnagramsSolved
                    let gameState = {gameState with AnagramsSolved = gameState.AnagramsSolved + 1}
                    CorrectGuess (gameState,fun () -> play gameState)
                // 4d
                else
                    FailedGuess (gameState,target,fun () -> play gameState)
            )

// =============================================
// All the impure code -- this could be made async to make it obvious!
// =============================================

module Impure =
    open Pure

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

// intepret the program
let rec interpret (program:Program) =
    match program with
    | GetInput (anagram,next) ->
        Impure.printAnagramAndInstructions anagram
        let input = System.Console.ReadLine()
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


// the top level function that mixes pure and impure code in a loop
let play() =

    let initialGameState = Impure.initialGameState()
    let initialProgram = Pure.play initialGameState
    interpret initialProgram

// to start the game,
// 1. highlight all code and execute
// 2. type this in the F# interactive terminal
(*
play();;
*)

// or to run from the command line, uncomment the "mainLoop" section above
// and then do
(*
dotnet fsi 03c-AnagramGame-Imperative.fsx
*)

// if you need to kill the game!
// * in Visual Studio, "Reset Interactive Session"
// * in VS Code, kill the terminal

