// =================================
// This file demonstrates how to define different kinds of total functions
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

// ============================
// Exception based design
// ============================

module ExceptionBasedDesign =

    let twelveDividedBy i =
        12 / i

// test
ExceptionBasedDesign.twelveDividedBy 4
ExceptionBasedDesign.twelveDividedBy 0


// ============================
// Extend the outputs: Option based design
// ============================

module ExtendedOutputDesign =

    let twelveDividedBy i =
        if i = 0 then
            None
        else
            Some (12 / i)


// test
ExtendedOutputDesign.twelveDividedBy 4
ExtendedOutputDesign.twelveDividedBy 0

let showExtendedOutputResult n =
    let opt = ExtendedOutputDesign.twelveDividedBy n
    match opt with
    | Some i ->
        printfn "12/%i is %i" n i
    | None ->
        printfn "12/%i is not allowed" n

showExtendedOutputResult 4
showExtendedOutputResult 0

// ============================
// Constrain the inputs: NonZeroInteger
// ============================

module ConstrainedTypes =

    type NonZeroInteger = private NonZeroInteger of int
    module NonZeroInteger =
        let create i = if i = 0 then None else Some (NonZeroInteger i)
        let value (NonZeroInteger i) = i

open ConstrainedTypes

module ConstrainedInputDesign =

    let twelveDividedBy nz =
        12 / NonZeroInteger.value nz


let showConstrainedInputResult n =
    // try to create a valid input OUTSIDE the function
    let nzOpt = NonZeroInteger.create n

    match nzOpt with
    | Some nz ->
        // If it is OK, we can call the function safely!
        ConstrainedInputDesign.twelveDividedBy nz
        |> printfn "12/%i is %i" n
    | None ->
        printfn "Input is not a NonZeroInteger"

// test
showConstrainedInputResult 4
showConstrainedInputResult 0


