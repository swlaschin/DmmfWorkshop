// =================================
// This file demonstrates how to do move IO to the edges
// and keep your core code pure and testable
//
// This is based on the previous example, except that all the IO is now Async.
// Async "contaminates" your code -- once it happens somewhere, the rest of you code
// needs to adapt to it.
//
// if the IO is kept separate from the pure code, the pure code is not contaminated with async!
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


(*
========================================
Example 1
========================================
Read two strings from input and compare them.
Print whether the first is bigger, smaller, or equal to the second
*)

// Async versions of read and write
module IO =
    let readLine() = async {
        return System.Console.ReadLine()
        }
    let writeLine str = async {
        printfn "%s" str
        }

module Example1_Impure =

    let compare_two_strings() = async {
        do! IO.writeLine "Enter the first value"
            // use do! when there is no return value in the Async

        let str1 = System.Console.ReadLine()
        do! IO.writeLine "Enter the second value"
        let! str2 = IO.readLine()
            // use let! when there is a return value in the Async

        if str1 > str2 then
            do! IO.writeLine "The first value is bigger"
        else if str1 < str2 then
            do! IO.writeLine "The first value is smaller"
        else
            do! IO.writeLine "The values are equal"
        }

// impure test
(*
Example1_Impure.compare_two_strings() |> Async.RunSynchronously
*)


// -------------------------
// Pure version doesn't change when async IO is used
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

    let compare_two_strings() = async {
        // impure section
        do! IO.writeLine "Enter the first value"
        let! str1 = IO.readLine()
        do! IO.writeLine "Enter the second value"
        let! str2 = IO.readLine()

        // pure section
        let result = Example1_Pure_Core.compare_two_strings str1 str2

        // impure section
        match result with
        | Bigger ->
            do! IO.writeLine "The first value is bigger"
        | Smaller ->
            do! IO.writeLine "The first value is smaller"
        | Equal ->
            do! IO.writeLine "The values are equal"
        }

// impure test
(*
Example1_Pure_Shell.compare_two_strings()  |> Async.RunSynchronously
*)

