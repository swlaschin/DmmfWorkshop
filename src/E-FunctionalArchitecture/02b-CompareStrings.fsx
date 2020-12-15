// =================================
// This file demonstrates how to do move IO to the edges
// and keep your core code pure and testable
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


(*
REQUIREMENTS:

Read two strings from input and compare them.
Print whether the first is bigger, smaller, or equal to the second
*)

//========================================
// Impure implementation
//========================================

module Impure =

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
Impure.compare_two_strings()
*)

//========================================
// pure implementation
//========================================

module PureCore =

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
PureCore.compare_two_strings "a" "b"
PureCore.compare_two_strings "a" "a"
PureCore.compare_two_strings "b" "a"


//========================================
// implementation of shell/api
//========================================

// The shell layer handles the I/O
// and then calls the pure code in PureCore
module Shell =
    open PureCore

    let compare_two_strings() =
        // impure section
        printfn "Enter the first value"
        let str1 = System.Console.ReadLine()
        printfn "Enter the second value"
        let str2 = System.Console.ReadLine()

        // pure section
        let result = PureCore.compare_two_strings str1 str2

        // impure section
        match result with
        | Bigger ->
            printfn "The first value is bigger"
        | Smaller ->
            printfn "The first value is smaller"
        | Equal ->
            printfn "The values are equal"

// execute the shell
(*
Shell.compare_two_strings()
*)

//========================================
// Tests for the pure implementation
//========================================

// the test framework
#load "Expecto.fsx"
open Expecto

let tests = testList "Comparison tests" [
    testCase "smaller" <| fun () ->
        let expected = PureCore.Smaller
        let actual = PureCore.compare_two_strings "a" "b"
        Expect.equal expected actual "a < b"

    testCase "equal" <| fun () ->
        let expected = PureCore.Equal
        let actual = PureCore.compare_two_strings "a" "a"
        Expect.equal expected actual "a = a"

    testCase "bigger" <| fun () ->
        let expected = PureCore.Bigger
        let actual = PureCore.compare_two_strings "b" "a"
        Expect.equal expected actual "b > a"

    ]

Expecto.Api.runTest tests
