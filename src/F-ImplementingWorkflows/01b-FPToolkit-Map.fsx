// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================



// ========================================
// FP Toolkit: Map
// ========================================

let add42 x = x + 42
add42 1  // 43

// ===================================
// 1A: Map for Options
// ===================================

module OptionMap =

    // An implementation of map for options.
    // This is the same as the built-in function Option.map
    let map f =
       fun opt ->
            match opt with
            | Some x -> Some (f x)
            | None -> None

//==============================
// 1B: Some examples of using map for Options
//==============================


// --------------------
// the wrong way!
add42 (Some 1)  // error

// --------------------
// the ugly way
let add42ToOption opt =
    if Option.isSome opt then
        let newVal = add42 opt.Value
        Some newVal
    else
        None

Some 1 |> add42ToOption       // OK

// --------------------
// the long-winded way
let add42ToOption_v2 = OptionMap.map add42
Some 1 |> add42ToOption_v2    // OK

// --------------------
// the elegant way
Some 1 |> OptionMap.map add42

// --------------------
// the built-in way, using Option.map
Some 1 |> Option.map add42

// --------------------
// play with your own examples
let lowercase (s:string) = s.ToLower()
Some "ABC" |> Option.map lowercase 
Some "ABC" |> Option.map String.length

// ===================================
// 2A: Map for Results
// ===================================

module ResultMap =

    // An implementation of map for Results.
    // This is the same as the built-in function Result.map
    let map f =
       fun res ->
            match res with
            | Ok x -> Ok (f x)
            | Error e -> Error e



//==============================
// 2B: Some examples of using map for Results
//==============================

// --------------------
// the wrong way!
add42 (Ok 1)  // error

// --------------------
// the ugly way
let add42ToResult res =
    match res with
    | Ok x ->
        let newVal = add42 x
        Ok newVal
    | Error e ->
        Error e

add42ToResult (Ok 1) |> sprintf "%A"      // OK

// --------------------
// the long-winded way
let add42ToResult_v2 x = (ResultMap.map add42) x
Ok 1 |> add42ToResult_v2 |> sprintf "%A"   // OK

// --------------------
// the elegant way
Ok 1 |> ResultMap.map add42 |> sprintf "%A"

// --------------------
// the built-in way, using Result.map
Ok 1 |> Result.map add42 |> sprintf "%A"

// --------------------
// play with your own examples
let lowercase (s:string) = s.ToLower()
Ok "ABC" |> Result.map lowercase |> sprintf "%A" 
Ok "ABC" |> Result.map String.length |> sprintf "%A"


//==============================
// 3: Some examples of using map for Lists
//==============================

// --------------------
// the ugly way
let add42ToEach list =
    let mutable newList = []
    for item in list do
        let newItem = add42 item
        newList <- newList @ [newItem]

    // return
    newList

add42ToEach [1;2;3]        // OK

// --------------------
// the long-winded way
let add42ToEach_v2 = List.map add42
add42ToEach_v2 [1;2;3]     // OK

// --------------------
// the elegant way, using the built-in map
[1;2;3] |> List.map add42  // same thing


// --------------------
// play with your own examples
let lowercase (s:string) = s.ToLower()
["ABC";"DEF"] |> List.map lowercase |> sprintf "%A" 
["ABC";"D"] |> List.map String.length |> sprintf "%A"

// ===================================
// 4A: Map for Async
// ===================================

module AsyncMap =

    // An implementation of map for async.
    let map f xAsync = async {
        let! x = xAsync // same as "x = await xAsync"
        return f x
        }

//==============================
// 4B: Examples of using map for Async
//==============================

// define an Async value
let async1 = async {
    return 1
    }

// --------------------
// the wrong way
add42 async1 // Error


// --------------------
// the elegant way, using the map function
let async43 = async1 |> AsyncMap.map add42

async43 |> Async.RunSynchronously  // it needs to be run!




