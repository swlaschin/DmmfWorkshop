// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// Tool #4a - Kleisli
// ========================================

module OptionKliesli =

    // three Option-returning functions to be chained together
    let doSomething x = Some x
    let doSomethingElse x = Some x
    let doAThirdThing x = Some x

    let kleisli f1 f2 =
        f1 >> (Option.bind f2)

    // common symbol
    let ( >=> ) = kleisli 

    let example input = 
        let combined = doSomething >=> doSomethingElse >=> doAThirdThing
        combined input

module ResultKliesli =

    // three Result-returning functions to be chained together
    let doSomething x = Ok x
    let doSomethingElse x = Ok x
    let doAThirdThing x = Ok x

    let kleisli f1 f2 =
        f1 >> (Result.bind f2)

    // common symbol
    let ( >=> ) = kleisli 

    let example input = 
        let combined = doSomething >=> doSomethingElse >=> doAThirdThing
        combined input

module AsyncKliesli =
    
    // three Async-returning functions to be chained together
    let doSomething x = async.Return x
    let doSomethingElse x = async.Return x
    let doAThirdThing x = async.Return x

    // helper
    let asyncBind f x = async.Bind(x,f)

    let kleisli f1 f2 =
        f1 >> (asyncBind f2)

    // common symbol
    let ( >=> ) = kleisli 

    let example input = 
        let combined = doSomething >=> doSomethingElse >=> doAThirdThing
        combined input

