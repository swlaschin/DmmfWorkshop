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
    let map2 f opt1 opt2 =
        match (opt1,opt2) with
        | Some x, Some y -> Some (f x y)
        | _ -> None

    // "lift" a 3-parameter function to Option world
    let map3 f opt1 opt2 opt3 =
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

    // alternative implementation of map2 using "ap" and "return"
    let map2_v2 f xOpt yOpt =
        let retn = Some
        ap (ap (retn f) xOpt) yOpt

    // alternative implementation of map3 using ap
    let map3_v2 f xOpt yOpt zOpt =
        let retn = Some
        ap (ap (ap (retn f) xOpt) yOpt) zOpt

module OptionApplicativeExamples =

    // Error - function does not work in Option world
    // oneParamFn (Some 1)
    // Ok
    (Option.map oneParamFn) (Some 1)

    // Error - function does not work in Option world
    // twoParamFn (Some 1) (Some 2)
    // Ok
    (Option.map2 twoParamFn) (Some 1) (Some 2)
    (Option.map2 twoParamFn) (Some 1) None
    (Option.map2 twoParamFn) None     (Some 2)

    // Error - function does not work in Option world
    // threeParamFn (Some 1) (Some 2) (Some 3)
    // Ok
    (Option.map3 threeParamFn) (Some 1) (Some 2) (Some 3)
    (Option.map3 threeParamFn) (Some 1) None     (Some 3)


// ===================================
// Applicative for Result
// ===================================

module Result =

    let map2 f r1 r2 =
        match (r1,r2) with
        | Ok x, Ok y -> Ok (f x y)
        | Error e1, Ok _ -> Error e1
        | Ok _, Error e2 -> Error e2
        | Error e1, Error e2 -> Error (e1 @ e2)

    let map3 f r1 r2 r3 =
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
        | Error e1, Ok x -> Error e1
        | Ok f, Error e2 -> Error e2
        | Error e1, Error e2 -> Error (List.append e1 e2)

    // alternative implementation of map2/3 using "ap" and "return"
    let map2_v2 f xRes yRes =
        let retn = Ok
        ap (ap (retn f) xRes) yRes

    let map3_v2 f xRes yRes zRes =
        let retn = Ok
        ap (ap (ap (retn f) xRes) yRes) zRes

    // alternative implementation of map2/3 using <!> and <*>
    let map2_v3 f xRes yRes =
        let (<!>) = Result.map
        let (<*>) = ap
        f <!> xRes <*> yRes

    let map3_v3 f xRes yRes zRes =
        let (<!>) = Result.map
        let (<*>) = ap
        // f xRes yRes zRes
        f <!> xRes <*> yRes <*> zRes

module ResultApplicativeExamples =

    // Error - function does not work in Result world
    // oneParamFn (Ok 1)
    // Ok
    (Result.map oneParamFn) (Ok 1)  |> printfn "%A"
    (Result.map oneParamFn) (Error "bad") |> printfn "%A"

    // Error - function does not work in Result world
    // twoParamFn (Ok 1) (Ok 2)
    // Ok
    (Result.map2 twoParamFn) (Ok 1) (Ok 2) |> printfn "%A"
    (Result.map2 twoParamFn) (Ok 1) (Error ["oops"])     |> printfn "%A"
    (Result.map2 twoParamFn) (Error ["bad"]) (Error ["oops"]) |> printfn "%A"

    // Error - function does not work in Result world
    // threeParamFn (Ok 1) (Ok 2) (Ok 3)
    // Ok
    (Result.map3 threeParamFn) (Ok 1) (Ok 2) (Ok 3) |> printfn "%A"
    (Result.map3 threeParamFn) (Ok 1) (Error ["bad"]) (Ok 3)

module ValidationExample =
    // domain type
    type Contact = {name:string; email:string}
    let makeContact name email = {name=name; email=email}

    // results from validation
    let nameOrError1 = Ok "Alice"
    let nameOrError2  = Error ["name must not be blank"]
    let emailOrError1 = Ok "a@example.com"
    let emailOrError2 = Error ["bad email"]

    // combine the results from validation
    (Result.map2 makeContact) nameOrError1 emailOrError1 |> printfn "%A"
    (Result.map2 makeContact) nameOrError1 emailOrError2 |> printfn "%A"
    (Result.map2 makeContact) nameOrError2 emailOrError2 |> printfn "%A"

// ===================================
// Applicative for List
// ===================================

module List =

    // one kind of list applicative
    let map2 f list1 list2 =
        List.map2 f list1 list2

    // another kind of list applicative
    let crossProduct f list1 list2 =
        [ for x in list1 do
          for y in list2 do
             yield f x y
        ]

module ListApplicativeExample =

    let add x y = x + y

    (List.map2 add) [1;2;3] [10;100;1000]
    (List.crossProduct add) [1;2;3] [10;100;1000]
