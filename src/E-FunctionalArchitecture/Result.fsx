namespace global  // note use of GLOBAL namespace

open System

//==============================================
// Helpers for Result type and AsyncResult type
//==============================================

// F# VERSION DIFFERENCE
// The "Result" type is built-in to F# 4.1 and newer (from VS2017),
// so uncomment the Result type if you are using an older version of F#

(*
/// The Result type represents a choice between success and failure
type Result<'success, 'failure> =
    | Ok of 'success
    | Error of 'failure
*)

/// Functions for Result type (functor and monad).
/// For applicatives, see Validation.
[<RequireQualifiedAccess>]  // RequireQualifiedAccess forces the `Result.xxx` prefix to be used
module Result =

    // F# VERSION DIFFERENCE
    // The `map`, `mapError` and `bind` functions are built-in to F# 4.1 and newer (from VS2017),
    // so uncomment these if you are using an older version of F#
    (*
    let map f result =
        match result with
        | Ok success -> Ok (f success)
        | Error err -> Error err

    let mapError f result =
        match result with
        | Ok success -> Ok success
        | Error err -> Error (f err)

    let bind f result =
        match result with
        | Ok success -> f success
        | Error err -> Error err
    *)

    // F# VERSION DIFFERENCE
    // The `map`, `mapError` and `bind` functions are in a different module in F# 4.1 and newer (from VS2017),
    // so these aliases make them available in this module.
    // In older versions of F#, where the functions are defined above, please comment them out
    let map = Result.map
    let mapError = Result.mapError
    let bind = Result.bind

    // Like `map` but with a unit-returning function
    let iter (f : _ -> unit) result =
        map f result |> ignore

    /// Apply a Result<fn> to a Result<x> monadically
    let apply fR xR =
        match fR, xR with
        | Ok f, Ok x -> Ok (f x)
        | Error err1, Ok _ -> Error err1
        | Ok _, Error err2 -> Error err2
        | Error err1, Error _ -> Error err1


    // combine a list of results, monadically
    let sequence aListOfResults =
        let (<*>) = apply // monadic
        let (<!>) = map
        let cons head tail = head::tail
        let consR headR tailR = cons <!> headR <*> tailR
        let initialValue = Ok [] // empty list inside Result

        // loop through the list, prepending each element
        // to the initial value
        List.foldBack consR aListOfResults initialValue



    //-----------------------------------
    // Lifting

    /// Lift a two parameter function to use Result parameters
    let map2 f x1 x2 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2

    /// Lift a three parameter function to use Result parameters
    let map3 f x1 x2 x3 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3

    /// Lift a four parameter function to use Result parameters
    let map4 f x1 x2 x3 x4 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3 <*> x4

    /// Lift a five parameter function to use Result parameters
    let map5 f x1 x2 x3 x4 x5 =
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3 <*> x4 <*> x5

    /// Apply a monadic function with two parameters
    let bind2 f x1 x2 = map2 f x1 x2 |> bind id

    /// Apply a monadic function with three parameters
    let bind3 f x1 x2 x3 = map3 f x1 x2 x3 |> bind id

    //-----------------------------------
    // Predicates

    /// Predicate that returns true on success
    let isOk =
        function
        | Ok _ -> true
        | Error _ -> false

    /// Predicate that returns true on failure
    let isError xR =
        xR |> isOk |> not

    /// Lift a given predicate into a predicate that works on Results
    let filter pred =
        function
        | Ok x -> pred x
        | Error _ -> true


    //-----------------------------------
    // Mixing simple values and results

    /// On success, return the value. On error, return a default value
    let ifError defaultVal =
        function
        | Ok x -> x
        | Error _ -> defaultVal


    //-----------------------------------
    // Mixing options and results

    /// Apply a monadic function to an Result<x option>
    let bindOption f xR =
        match xR with
        | Some x -> f x |> map Some
        | None -> Ok None

    /// Convert an Option into a Result. If none, use the passed-in errorValue
    let ofOption errorValue opt =
        match opt with
        | Some v -> Ok v
        | None -> Error errorValue

    /// Convert a Result into an Option
    let toOption xR =
        match xR with
        | Ok v -> Some v
        | Error _ -> None

    /// Convert the Error case into an Option
    /// (useful with List.choose to find all errors in a list of Results)
    let toErrorOption =
        function
        | Ok _ -> None
        | Error err -> Some err


//==============================================
// Computation Expression for Result
//==============================================

[<AutoOpen>]
module ResultComputationExpression =

    type ResultBuilder() =
        member __.Return(x) = Ok x
        member __.Bind(x, f) = Result.bind f x

        member __.ReturnFrom(x) = x
        member this.Zero() = this.Return ()

        member __.Delay(f) = f
        member __.Run(f) = f()

        member this.While(guard, body) =
            if not (guard())
            then this.Zero()
            else this.Bind( body(), fun () ->
                this.While(guard, body))

        member this.TryWith(body, handler) =
            try this.ReturnFrom(body())
            with e -> handler e

        member this.TryFinally(body, compensation) =
            try this.ReturnFrom(body())
            finally compensation()

        member this.Using(disposable:#System.IDisposable, body) =
            let body' = fun () -> body disposable
            this.TryFinally(body', fun () ->
                match disposable with
                    | null -> ()
                    | disp -> disp.Dispose())

        member this.For(sequence:seq<_>, body) =
            this.Using(sequence.GetEnumerator(),fun enum ->
                this.While(enum.MoveNext,
                    this.Delay(fun () -> body enum.Current)))

        member this.Combine (a,b) =
            this.Bind(a, fun () -> b())

    let result = new ResultBuilder()

