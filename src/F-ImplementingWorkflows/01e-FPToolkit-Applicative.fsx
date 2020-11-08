// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// FP Toolkit: Applicative
// ========================================

let oneParamFn (x:int) = x + 1
let twoParamFn (x:int) (y:int) = x + y
let threeParamFn (x:int) (y:int) (z:int) = x + y + z

// ===================================
// Applicative for Option
// ===================================

module Option =


    // "lift" a 2-parameter function to Option world
    let lift2 f opt1 opt2 =
        match (opt1,opt2) with
        | Some x, Some y -> Some (f x y)
        | _ -> None

    // "lift" a 3-parameter function to Option world
    let lift3 f opt1 opt2 opt3 =
        match (opt1,opt2,opt3) with
        | Some x, Some y, Some z -> Some (f x y z)
        | _ -> None

    // A more generic approach!
    // All we need to how to apply a wrapped function to a wrapped value
    // Option<f> -> Option<x> -> Option<f(x)>
    let ap optF optX =
        match (optF, optX) with
        | Some f, Some x -> Some (f x)
        | _ -> None

    // alternative implementation of lift2 using "ap" and "return"
    let lift2_v2 f xOpt yOpt =
        let retn = Some
        ap (ap (retn f) xOpt) yOpt

    // alternative implementation of lift3 using ap
    let lift3_v2 f xOpt yOpt zOpt =
        let retn = Some
        ap (ap (ap (retn f) xOpt) yOpt) zOpt

module OptionApplicativeExamples =


    // Error - function does not work in Option world
    oneParamFn (Some 1)
    // Ok
    (Option.map oneParamFn) (Some 1)

    // Error - function does not work in Option world
    twoParamFn (Some 1) (Some 2)
    // Ok
    (Option.lift2 twoParamFn) (Some 1) (Some 2)
    (Option.lift2 twoParamFn) (Some 1) None
    (Option.lift2 twoParamFn) None     (Some 2)

    // Error - function does not work in Option world
    threeParamFn (Some 1) (Some 2) (Some 3)
    // Ok
    (Option.lift3 threeParamFn) (Some 1) (Some 2) (Some 3)
    (Option.lift3 threeParamFn) (Some 1) None     (Some 3)


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

    // A more generic approach!
    // All we need to how to apply a wrapped function to a wrapped value
    // Result<f> -> Result<x> -> Result<f(x)>
    let ap resF resX =
        match (resF, resX) with
        | Ok f, Ok x -> Ok (f x)
        | _ -> None

    // alternative implementation of lift2 using "ap" and "return"
    let lift2_v2 f xRes yRes =
        let retn = Ok
        ap (ap (retn f) xRes) yRes

    // alternative implementation of lift3 using ap
    let lift3_v2 f xRes yRes zRes =
        let retn = Ok
        ap (ap (ap (retn f) xRes) yRes) zRes


module ResultApplicativeExamples =

    // Error - function does not work in Result world
    oneParamFn (Ok 1)
    // Ok
    (Result.map oneParamFn) (Ok 1)  |> printfn "%A"
    (Result.map oneParamFn) (Error "bad") |> printfn "%A"

    // Error - function does not work in Result world
    twoParamFn (Ok 1) (Ok 2)
    // Ok
    (Result.lift2 twoParamFn) (Ok 1) (Ok 2) |> printfn "%A"
    (Result.lift2 twoParamFn) (Ok 1) (Error ["oops"])     |> printfn "%A"
    (Result.lift2 twoParamFn) (Error ["bad"]) (Error ["oops"]) |> printfn "%A"

    // Error - function does not work in Result world
    threeParamFn (Ok 1) (Ok 2) (Ok 3)
    // Ok
    (Result.lift3 threeParamFn) (Ok 1) (Ok 2) (Ok 3) |> printfn "%A"
    (Result.lift3 threeParamFn) (Ok 1) (Error ["bad"]) (Ok 3)


    /// Validation example
    let nameOrError1 = Ok "Alice"
    let nameOrError2  = Error ["name must not be blank"]
    let emailOrError1 = Ok "a@example.com"
    let emailOrError2 = Error ["bad email"]

    type Contact = {name:string; email:string}
    let makeContact name email = {name=name; email=email}

    (Result.lift2 makeContact) nameOrError1 emailOrError1 |> printfn "%A"
    (Result.lift2 makeContact) nameOrError1 emailOrError2 |> printfn "%A"
    (Result.lift2 makeContact) nameOrError2 emailOrError2 |> printfn "%A"

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
