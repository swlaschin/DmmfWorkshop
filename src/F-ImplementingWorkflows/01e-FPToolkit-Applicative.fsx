// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// FP Toolkit: Applicative
// ========================================


// ===================================
// Applicative for Option
// ===================================

module Option =

    let lift2 f opt1 opt2 =
        match (opt1,opt2) with
        | Some x, Some y -> Some (f x y)
        | _ -> None

    let lift3 f opt1 opt2 opt3 =
        match (opt1,opt2,opt3) with
        | Some x, Some y, Some z -> Some (f x y z)
        | _ -> None

module OptionApplicativeExamples =

    let add x y = x + y
    let threeParams x y z = x + y + z

    (Option.lift2 add) (Some 1) (Some 2)
    (Option.lift2 add) (Some 1) None
    (Option.lift2 add) None (Some 1)

    (Option.lift3 threeParams) (Some 1) (Some 2) (Some 3)
    (Option.lift3 threeParams) (Some 1) None (Some 3)

// ===================================
// Applicative for Result
// ===================================

module Result =

    let lift2 f r1 r2 =
        match (r1,r2) with
        | Ok x, Ok y -> Ok (f x y)
        | Error e1, Ok _ -> Error e1
        | Ok _, Error e2 -> Error e2
        | Error e1, Error e2 -> Error (e1 @ e2)

    let lift3 f r1 r2 r3 =
        match r1,r2,r3 with
        | Ok x, Ok y, Ok z -> Ok (f x y z)
        | Error e1, Ok _, Ok _ -> Error e1
        | Ok _, Error e2, Ok _ -> Error e2
        | Ok _, Ok _, Error e3 -> Error e3
        | Error e1, Error e2, Ok _ -> Error (e1 @ e2)
        | Ok _, Error e2, Error e3 -> Error (e2 @ e3)
        | Error e1, Ok _, Error e3 -> Error (e1 @ e3)
        | Error e1, Error e2, Error e3 -> Error (e1 @ e2 @ e3)



module ResultApplicativeExamples =

    let add x y = x + y
    let threeParams x y z = x + y + z

    (Result.lift2 add) (Ok 1) (Ok 2) |> printfn "%A"
    (Result.lift2 add) (Error ["bad"]) (Ok 2) |> printfn "%A"
    (Result.lift2 add) (Error ["bad"]) (Error ["oops"]) |> printfn "%A"

    (Result.lift3 threeParams) (Ok 1) (Ok 2) (Ok 4) |> printfn "%A"

// ===================================
// Applicative for List
// ===================================

module List =

    // one kind of list applicative
    let lift2 f list1 list2 =
        List.map2 f list1 list2

    // another kind of list applicative
    let crossProduct f list1 list2 =
        [ for x in list1 do
          for y in list2 do
             yield f x y
        ]

module ListApplicativeExample =

    let add x y = x + y

    (List.lift2 add) [1;2;3] [10;100;1000]
    (List.crossProduct add) [1;2;3] [10;100;1000]
