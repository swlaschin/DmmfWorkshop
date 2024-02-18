namespace global  // note use of GLOBAL namespace

open System

//==============================================
// The `Validation` type is the same as the `Result` type but with a *list* for failures
// rather than a single value. This allows `Validation` types to be combined
// by combining their errors ("applicative-style")
//==============================================

type Validation<'Success,'Failure> =
    Result<'Success,'Failure list>

/// Functions for the `Validation` type (mostly applicative)
[<RequireQualifiedAccess>]  // RequireQualifiedAccess forces the `Validation.xxx` prefix to be used
module Validation =

    let map f (x:Validation<_,_>) :Validation<_,_> =
        Result.map f x

    let bind f (x:Validation<_,_>) :Validation<_,_> =
        Result.bind f x

    /// Apply a Validation<fn> to a Validation<x> applicatively
    let apply (fV:Validation<_,_>) (xV:Validation<_,_>) :Validation<_,_> =
        match fV, xV with
        | Ok f, Ok x -> Ok (f x)
        | Error errs1, Ok _ -> Error errs1
        | Ok _, Error errs2 -> Error errs2
        | Error errs1, Error errs2 -> Error (errs1 @ errs2)

    //-----------------------------------
    // Lifting

    /// Lift a two parameter function to use Validation parameters
    let map2 f x1 x2 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2

    /// Lift a three parameter function to use Validation parameters
    let map3 f x1 x2 x3 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3

    /// Lift a four parameter function to use Validation parameters
    let map4 f x1 x2 x3 x4 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3 <*> x4

    /// Lift a five parameter function to use Validation parameters
    let map5 f x1 x2 x3 x4 x5 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3 <*> x4 <*> x5


    // combine a list of Validation, applicatively
    let sequence (aListOfValidations:Validation<_,_> list) =
        let (<*>) = apply
        let (<!>) = Result.map
        let cons head tail = head::tail
        let consR headR tailR = cons <!> headR <*> tailR
        let initialValue = Ok [] // empty list inside Result

        // loop through the list, prepending each element
        // to the initial value
        List.foldBack consR aListOfValidations initialValue

    //-----------------------------------
    // Converting between Validations and other types

    let ofResult xR :Validation<_,_> =
        xR |> Result.mapError List.singleton

    let toResult (xV:Validation<_,_>) :Result<_,_> =
        xV



