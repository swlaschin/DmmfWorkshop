// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================



// ========================================
// Tool #2 - Map
// ========================================

let add42 x = x + 42
add42 1  // 43

// ===================================
// Map for Options
// ===================================

module OptionMap =

    add42 (Some 1)  // error

    let add42ToOption opt =
        if Option.isSome opt then
            let newVal = add42 opt.Value
            Some newVal
        else
            None

    // An implementation of map for options.
    // This is the same as the built-in function Option.map
    let optionMap f =
       fun opt ->
            match opt with
            | Some x -> Some (f x)
            | None -> None


    let add42ToOption_v2 = Option.map add42

    add42ToOption (Some 1)   // OK
    add42ToOption_v2 (Some 1)   // OK
    Some 1 |> Option.map add42  // same thing

// ===================================
// Map for Results
// ===================================

module ResultMap =

    add42 (Ok 1)  // error

    let add42ToResult res =
        match res with
        | Ok x -> 
            let newVal = add42 x
            Ok newVal
        | Error e ->
            Error e

    // An implementation of map for Results.
    // This is the same as the built-in function Result.map
    let resultMap f =
       fun res ->
            match res with
            | Ok x -> Ok (f x)
            | Error e -> Error e


    let add42ToResult_v2 x = (Result.map add42) x 

    add42ToResult (Ok 1) |> sprintf "%A"      // OK
    add42ToResult_v2 (Ok 1) |> sprintf "%A"   // OK
    Ok 1 |> Result.map add42 |> sprintf "%A"  // same thing


// ===================================
// Map for Lists
// ===================================

module ListMap =

    let add42ToEach list =
        let mutable newList = []
        for item in list do
            let newItem = add42 item
            newList <- newList @ [newItem]

        // return
        newList

    let add42ToEach_v2 = List.map add42

    add42ToEach [1;2;3]        // OK
    add42ToEach_v2 [1;2;3]     // OK
    [1;2;3] |> List.map add42  // same thing


// ===================================
// Map for Async
// ===================================

module AsyncMap =

    let async1 = async {
        return 1
        }

    add42 async1 // Error

    // An implementation of map for async.
    let asyncMap f xAsync = async {
        let! x = xAsync // same as "x = await xAsync"
        return f x
        }

    let async43 = async1 |> asyncMap add42
    async43 |> Async.RunSynchronously




