/// The `Validation` type is the same as the `Result` type
/// but with a *list* for failures rather than a single value.
/// This allows `Validation` types to be combined
/// by combining their errors ("applicative-style")
type Validation<'Success,'Failure> =
    Result<'Success,'Failure list>


module Validation =

    /// Convert a result into a validation,
    /// with a list of errors
    let ofResult (xR:Result<_,_>) :Validation<_,_> =
        match xR with
        | Ok x -> Ok x
        | Error x -> Error [x]

    // One-liner using Result.mapError
    let ofResult_v2 (xR:Result<_,_>) :Validation<_,_> =
        xR |> Result.mapError List.singleton

//-----------------------------------
// Lifting
//-----------------------------------

// implementations using explicit pattern matching
module LiftImplementation_v1 =

    /// Lift a two parameter function to use Validation parameters
    let lift2 f (x1V:Validation<_,_>) (x2V:Validation<_,_>) =
        match (x1V,x2V) with
        | Ok x1, Ok x2 -> Ok (f x1 x2)
        | Ok x1, Error e2 -> Error e2
        | Error e1, Ok x2 -> Error e1
        | Error e1, Error e2 -> Error (e1 @ e2) // concat two lists

    /// Lift a three parameter function to use Validation parameters
    let lift3 f (x1V:Validation<_,_>) (x2V:Validation<_,_>) (x3V:Validation<_,_>) =
        match (x1V,x2V,x3V) with
        | Ok x1, Ok x2, Ok x3  -> Ok (f x1 x2 x3)
        | Ok x1, Error e2, Ok x3 -> Error e2
        | Error e1, Ok x2, Ok x3 -> Error e1
        | Error e1, Error e2, Ok x3 -> Error (e1 @ e2)
        | Ok x1, Ok x2, Error e3  -> Error e3
        | Ok x1, Error e2, Error e3 -> Error (e2 @ e3)
        | Error e1, Ok x2, Error e3 -> Error (e1 @ e3)
        | Error e1, Error e2, Error e3 -> Error (e1 @ e2 @ e3)

// implementations using Apply
module LiftImplementation_v2 =

    /// Apply a Validation<fn> to a Validation<x> applicatively
    let apply (fV:Validation<_,_>) (xV:Validation<_,_>) :Validation<_,_> =
        match fV, xV with
        | Ok f, Ok x -> Ok (f x)
        | Error errs1, Ok _ -> Error errs1
        | Ok _, Error errs2 -> Error errs2
        | Error errs1, Error errs2 -> Error (errs1 @ errs2)

    let lift2 f (x1V:Validation<_,_>) (x2V:Validation<_,_>) =
        apply (apply (Ok f) x1V) x2V

    let lift3 f (x1V:Validation<_,_>) (x2V:Validation<_,_>) (x3V:Validation<_,_>) =
        apply (apply (apply (Ok f) x1V) x2V) x3V

    let lift3_v2 f (x1V:Validation<_,_>) (x2V:Validation<_,_>) (x3V:Validation<_,_>) =
        apply (lift2 f x1V x2V) x3V

// implementations using the Apply infix operator <*>
module LiftImplementation_v3 =

    let apply = LiftImplementation_v2.apply

    let lift2 f (x1V:Validation<_,_>) (x2V:Validation<_,_>) =
        let ( <*> ) = apply
        (Ok f) <*> x1V <*> x2V

    let lift3 f (x1V:Validation<_,_>) (x2V:Validation<_,_>) (x3V:Validation<_,_>) =
        let ( <*> ) = apply
        (Ok f) <*> x1V <*> x2V <*> x3V

// implementations using <*> and map as well
module LiftImplementation_v4 =

    let apply = LiftImplementation_v2.apply
    let map = Result.map

    let lift2 f x1 x2 =
        let (<!>) = map
        let (<*>) = apply
        // looks very similar to "f x1 x2"
        f <!> x1 <*> x2

    let lift3 f x1 x2 x3 =
        let (<!>) = map
        let (<*>) = apply
        // looks very similar to "f x1 x2 x3"
        f <!> x1 <*> x2 <*> x3
