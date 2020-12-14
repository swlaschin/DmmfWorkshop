// =================================
// This file contains additional functions for ROP
// =================================


/// Convert a "dead-end" function into a function
/// that also returns the input
let tee (f:'a -> unit) result =
    f result
    result

/// Convert an exception throwing function into a useful function.
/// * "exceptionThrowingFunction" is the function you want to trap.
/// * "handler" converts exceptions to your error type.
let catch exceptionThrowingFunction (handler:exn->'MyError) oneTrackInput =
    try
        Ok (exceptionThrowingFunction oneTrackInput)
    with
    | ex ->
        Error (handler ex)

/// Like the original "catch" but adapted to accept
/// a *two track* input rather than a one track input
let catchR exceptionThrowingFunction handler twoTrackInput =
    // catch' is a switch/points function...
    let catch' = catch exceptionThrowingFunction handler
    // so we need to use "bind" to convert it to two-track
    twoTrackInput |> Result.bind catch'

