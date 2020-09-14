// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// FP Toolkit: Kleisli
// ========================================

// ===================================
// Kleisli for Options
// ===================================

module Option =

    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (Option.bind f2)

module OptionKliesliExamples =

    // three Option-returning functions to be chained together
    let doSomething x = Some (x+1)
    let doSomethingElse x = Some (x+10)
    let doAThirdThing x = Some (x+100)


    // define the common symbol for the the kleisli function
    let ( >=> ) = Option.kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli = doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

    // test the code
    example 5 |> printfn "%A"

// ===================================
// Kleisli for Results
// ===================================

module Result =
    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (Result.bind f2)

module ResultKliesliExample =

    // three Result-returning functions to be chained together
    let doSomething x = Ok (x+1)
    let doSomethingElse x = Ok (x+10)
    let doAThirdThing x = Ok (x+100)

    // define the common symbol for the the kleisli function
    let ( >=> ) = Result.kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli = doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

    // test the code
    example 5 |> printfn "%A"

// ===================================
// Kleisli for Lists
// ===================================

module List =
    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (List.collect f2)

module ListKleisliExamples =

    // three List-returning functions to be chained together
    let doSomething x = [x+1; x+2]
    let doSomethingElse x = [x+10; x+20]
    let doAThirdThing x = [x+100; x+200]

    // define the common symbol for the the kleisli function
    let ( >=> ) = List.kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli =
            doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

    // test the code
    example 5


// ===================================
// Kleisli for Async
// ===================================

module Async =
    // helper
    let bind f x = async.Bind(x,f)

    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (bind f2)

module AsyncKliesliExamples =

    // three Async-returning functions to be chained together
    let doSomething x = async.Return (x+1)
    let doSomethingElse x = async.Return (x+10)
    let doAThirdThing x = async.Return (x+100)

    // define the common symbol for the the kleisli function
    let ( >=> ) = Async.kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli = doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

    // test the code
    example 2 |> Async.RunSynchronously