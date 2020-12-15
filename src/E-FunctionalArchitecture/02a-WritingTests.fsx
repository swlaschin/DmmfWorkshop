// =================================
// This file demonstrates how to test pure code
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

// the test framework
#load "Expecto.fsx"
open Expecto

// ----------------------------------
// Example of some simple tests
// ----------------------------------

let tests = testList "my tests" [
    testCase "a passing test" <| fun () ->
        let expected = 4
        Expect.equal expected (2+2) "2+2 = 4"

    testCase "a failing test" <| fun () ->
        let expected = 3
        Expect.equal expected (2+2) "2+2 = 3"

    testCase "a test that throws an exception" <| fun () ->
        failwith "invalid input"
    ]

Expecto.Api.runTest tests



