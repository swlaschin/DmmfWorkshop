// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// Tool #4a - Kleisli
// ========================================

// ===================================
// Kleisli for Options
// ===================================

module OptionKliesli =

    // three Option-returning functions to be chained together
    let doSomething x = Some x
    let doSomethingElse x = Some x
    let doAThirdThing x = Some x

    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (Option.bind f2)

    // define the common symbol for the the kleisli function
    let ( >=> ) = kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli = doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

// ===================================
// Kleisli for Results
// ===================================

module ResultKliesli =

    // three Result-returning functions to be chained together
    let doSomething x = Ok x
    let doSomethingElse x = Ok x
    let doAThirdThing x = Ok x

    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (Result.bind f2)

    // define the common symbol for the the kleisli function
    let ( >=> ) = kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli = doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

// ===================================
// Kleisli for Lists
// ===================================

module ListKleisli =

    // three List-returning functions to be chained together
    let doSomething x = [x+1; x+2]
    let doSomethingElse x = [x+10; x+20]
    let doAThirdThing x = [x+100; x+200]

    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (List.collect f2)

    // define the common symbol for the the kleisli function
    let ( >=> ) = kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli = doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

    // test the code
    example 5


// ===================================
// Kleisli for Async
// ===================================

module AsyncKliesli =

    // three Async-returning functions to be chained together
    let doSomething x = async.Return x
    let doSomethingElse x = async.Return x
    let doAThirdThing x = async.Return x

    // helper
    let asyncBind f x = async.Bind(x,f)

    // define the kleisli function
    let kleisli f1 f2 =
        f1 >> (asyncBind f2)

    // define the common symbol for the the kleisli function
    let ( >=> ) = kleisli

    let example input =
        // create a new function by composing
        let composedWithKliesli = doSomething >=> doSomethingElse >=> doAThirdThing
        // call the new function
        composedWithKliesli input

