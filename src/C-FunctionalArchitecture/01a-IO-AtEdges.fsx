// =================================
// This file demonstrates how to do move IO to the edges
// and keep your core code pure and testable
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


(*
========================================
Example 1
========================================
Read two strings and compare them.
Print whether the first is bigger, smaller, or equal to the second
*)

module Example1_Impure =

    let compare_two_strings() =
        printfn "Enter the first value"
        let str1 = System.Console.ReadLine()
        printfn "Enter the second value"
        let str2 = System.Console.ReadLine()

        if str1 > str2 then
            printfn "The first value is bigger"
        else if str1 < str2 then
            printfn "The first value is smaller"
        else
            printfn "The values are equal"

// impure test
(*
Example1_Impure.compare_two_strings()
*)

// -------------------------
// Pure version
// -------------------------
module Example1_Pure_Core =

    type ComparisonResult =
        | Bigger
        | Smaller
        | Equal

    let compare_two_strings str1 str2 =
        if str1 > str2 then
            Bigger
        else if str1 < str2 then
            Smaller
        else
            Equal

// It's easy to unit test a pure function
Example1_Pure_Core.compare_two_strings "a" "b"
Example1_Pure_Core.compare_two_strings "a" "a"


// The shell layer handles the I/O
// and then calls the pure code in Example1_Pure_Core
module Example1_Pure_Shell =
    open Example1_Pure_Core

    let compare_two_strings() =
        // impure section
        printfn "Enter the first value"
        let str1 = System.Console.ReadLine()
        printfn "Enter the second value"
        let str2 = System.Console.ReadLine()

        // pure section
        let result = Example1_Pure_Core.compare_two_strings str1 str2

        // impure section
        match result with
        | Bigger ->
            printfn "The first value is bigger"
        | Smaller ->
            printfn "The first value is smaller"
        | Equal ->
            printfn "The values are equal"

// impure test
(*
Example1_Pure_Shell.compare_two_strings()
*)

(*
========================================
Example 2
========================================
Build a list of strings by reading strings from the console
until an empty string is read, then print them.
*)

module Example2_Impure =

    let buildListOfStrings() =
        let mutable listOfStrings = []
        let mutable finished = false
        while not finished do
            printfn "Enter a string or <CR> to stop"
            let str = System.Console.ReadLine()
            if System.String.IsNullOrEmpty(str) then
                finished <- true
            else
                listOfStrings <- List.append listOfStrings [str]

        printfn "The list is %A" listOfStrings

// impure test
(*
Example2_Impure.buildListOfStrings()
*)

// -------------------------
// Pure version
// -------------------------
module Example2_Pure_Core =

    type State = {
        listOfStrings : string list
        finished : bool
        }

    // it's useful to define a standard initial state
    let initialState = {
        listOfStrings=[]
        finished=false
        }

    /// process a single string and update the state
    let processOneString (state:State) str =
        if System.String.IsNullOrEmpty(str) then
            {state with finished = true}
        else
            {state with listOfStrings = List.append state.listOfStrings [str] }

    /// loop with the prompt until the state is "finished"
    let buildListOfStrings prompt =

        // define an inner loop function
        let rec loop state =
            // get the string from the user
            let str = prompt()
            // update the state
            let newState = processOneString state str
            // continue or exit?
            if not newState.finished then
                loop newState // loop again with the new state
            else
                newState.listOfStrings  // return the state

        // start the loop with the initial state
        loop initialState

// It's easy to unit test a pure function
let state0 = Example2_Pure_Core.initialState
let state1 = Example2_Pure_Core.processOneString state0 "a"
let state2 = Example2_Pure_Core.processOneString state1 "b"
let state3 = Example2_Pure_Core.processOneString state2 ""


// can also unit test by providing a
// custom prompt function with no I/O
let prompt =
    let enum = (seq {"a"; "b"; ""}).GetEnumerator()
    fun () ->
        // each time it is called, move to the next element...
        enum.MoveNext() |> ignore
        // .. and return it
        enum.Current

Example2_Pure_Core.buildListOfStrings prompt


// The shell layer handles the I/O
// and then calls the pure code in Example2_Pure_Core
module Example2_Pure_Shell =

    /// loop with the prompt until finished
    let buildListOfStrings() =

        // impure section
        let prompt() =
            printfn "Enter a string or <CR> to stop"
            System.Console.ReadLine()

        // pure section
        let listOfStrings = Example2_Pure_Core.buildListOfStrings prompt

        // impure section
        printfn "The list is %A" listOfStrings

// impure test
(*
Example2_Pure_Shell.buildListOfStrings()
*)
