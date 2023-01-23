// =================================
// This file demonstrates how to test
// the pure core code 
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
