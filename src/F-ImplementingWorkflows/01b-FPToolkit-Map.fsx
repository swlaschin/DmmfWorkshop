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

/// Maps for option world
module OptionMap =

    add42 (Some 1)  // error

    let add42ToOption opt =
        if Option.isSome opt then
            let newVal = add42 opt.Value
            Some newVal
        else
            None

    let add42ToOption_v2 = Option.map add42

    add42ToOption (Some 1)   // OK
    add42ToOption_v2 (Some 1)   // OK
    Some 1 |> Option.map add42  // same thing

/// Maps for list world
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


/// Maps for async world
module AsyncMap =

    let async1 = async {
        return 1
        }

    add42 async1 // Error

    let asyncMap f xAsync = async {
        let! x = xAsync
        return f x
        }

    let async43 = async1 |> asyncMap add42
    async43 |> Async.RunSynchronously




