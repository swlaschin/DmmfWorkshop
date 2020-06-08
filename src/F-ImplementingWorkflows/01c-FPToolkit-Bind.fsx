// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// FP Toolkit: bind
// ========================================


// ===================================
// 1: Bind for Options
// ===================================

module OptionHelpers =

    // ----------------------------------
    // define a helper function
    // to make composition easy
    let ifSomeDo f (opt:'a option) =
        if opt.IsSome then
            f opt.Value
        else
            None

/// Some examples of using bind for Options
module OptionBindExamples =

    // three Option-returning functions to be chained together
    let doSomething x = 
        if x % 2 = 0 then
            Some x
        else
            None

    let doSomethingElse x = 
        if x % 3 = 0 then
            Some x
        else
            None

    let doAThirdThing x = 
        if x % 5 = 0 then
            Some x
        else
            None

    // --------------------
    // the really ugly way
    let example_v1 input =
        let x = doSomething input
        if Option.isSome x then
            let y = doSomethingElse (x.Value)
            if Option.isSome y then
                let z = doAThirdThing (y.Value)
                if Option.isSome z then
                    let result = z.Value
                    Some result
                else
                    None
            else
                None
        else
            None

    // ----------------------------------
    // Using the helper function
    let example_v2 input =
        doSomething input
        |> OptionHelpers.ifSomeDo doSomethingElse
        |> OptionHelpers.ifSomeDo doAThirdThing

    // ----------------------------------
    // Or use the built-in Option.bind function
    let example_v3 input =
        doSomething input
        |> Option.bind doSomethingElse
        |> Option.bind doAThirdThing

    // test the code
    example_v3 2
    example_v3 6
    example_v3 30

// ===================================
// 2: Bind for Results
// ===================================

module ResultBindExamples =

    // three Result-returning functions
    // to be chained together
    let doSomething x =
        if x % 2 = 0 then
            Ok x
        else
            Error "not / by 2"

    let doSomethingElse x =
        if x % 3 = 0 then
            Ok x
        else
            Error "not / by 3"

    let doAThirdThing x =
        if x % 5 = 0 then
            Ok x
        else
            Error "not / by 5"

    let example input =
        doSomething input
        |> Result.bind doSomethingElse
        |> Result.bind doAThirdThing

    // test the code
    example 2
    example 6
    example 30

// ===================================
// 3: Bind for Lists
// ===================================

module ListBindExamples =

    // three List-returning functions to be chained together
    let doSomething x = [x+1; x+2]
    let doSomethingElse x = [x+10; x+20]
    let doAThirdThing x = [x+100; x+200]

    // A helper to make things consistent.
    // In F#, bind for lists is List.collect. In C# it is SelectMany
    let listBind = List.collect

    let example input =
        doSomething input
        |> listBind doSomethingElse
        |> listBind doAThirdThing

    // test the code
    example 5

// ===================================
// 4: Bind for Async
// ===================================

module AsyncBindExamples =

    // three Async-returning functions to be chained together
    let doSomething x = async.Return x
    let doSomethingElse x = async.Return x
    let doAThirdThing x = async.Return x

    // helper
    let asyncBind f x = async.Bind(x,f)

    let example input =
        doSomething input
        |> asyncBind doSomethingElse
        |> asyncBind doAThirdThing

