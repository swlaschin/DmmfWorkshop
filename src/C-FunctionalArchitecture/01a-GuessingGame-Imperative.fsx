// =================================
// Guessing game- Imperative implementation
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

// =====================================
// This is the imperative implementation
// * IO is mixed in with regular code
// * state is held in mutable variables
// =====================================


let mutable lowerBound = 1
let mutable upperBound = 100
let mutable exitGame = false

let rec guessingLoop() =
    // Step 3. Given the bounds, make a guess 
    //         (or if you know the answer go to step 7)

    if lowerBound = upperBound then
        // step 7. (jump from 3) If you know the answer, show "Your number is %i"
        printfn "Your number is %i" lowerBound
        // exit loop
        ()

    else
        // make a new guess
        let guess = (lowerBound + upperBound) / 2

        // step 4. Show the guess with the text "Is your number bigger than %i? [y,n,q]"
        printfn "Is your number bigger than %i? [y,n,q]" guess

        let mutable inputIsValid = false
        while not inputIsValid && not exitGame do
            // step 5. Accept input
            let input = System.Console.ReadLine()
            match input with
            | "y" ->
                // step 6a. If the input is "y" or "n" then 
                //             update the bounds and go to step 3
                inputIsValid <- true
                lowerBound <- guess + 1   // update the bounds
                guessingLoop()            // go to step 3 
            | "n" ->
                // step 6a. If the input is "y" or "n" then 
                //             update the bounds and go to step 3
                inputIsValid <- true
                upperBound <- guess
                guessingLoop()
            | "q" ->
                // step 6b. If the input is "q" then 
                //             show "Game over. Thanks for playing" and quit
                printfn "Game over. Thanks for playing"

                // quit the loop and the game
                exitGame <- true
                ()
            | _ ->
                // step 6c. If the input is something else then 
                //             show "Please enter [y,n,q]" and go to step 5 
                printfn "Please enter [y,n,q]"
                ()

let rec mainLoop() =

    // start the game

    // Step 1. Ask "Think of a number between 1 and 100"
    printfn "Think of a number between 1 and 100"

    // Step 2. Set the bounds for a new game to 1..100
    lowerBound <- 1
    upperBound <- 100
    exitGame <- false

    guessingLoop()

    // get more input
    let mutable inputIsValid = false
    while not inputIsValid && not exitGame do
        // step 8. Show "Do you want to play again or quit? [y,q]"
        printfn "Do you want to play again or quit? [y,q]"

        // step 9. Accept input
        let input = System.Console.ReadLine()

        match input with
        | "y" ->
            //step 10a. If the input is "y" then 
            //             go to step 1
            inputIsValid <- true
            mainLoop()     // go to step 1
        | "q" ->
            //step 10b. If the input is "q" then 
            //             show "Game over. Thanks for playing" and quit

            printfn "Game over. Thanks for playing"
            inputIsValid <- true
            () // quit the loop
        | _ ->
            //step 10c. If the input is something else then 
            //             show "Please enter [y,q]" and go to step 9 
            printfn "Please enter [y,q]"

// to start the game, type this in the F# terminal
(*
mainLoop();;
*)

// or to run from the command line
(*
dotnet fsi 01a-GuessingGame-Imperative.fsx
*)

// if you need to kill the game!
// * in Visual Studio, "Reset Interactive Session"
// * in VS Code, kill the terminal

