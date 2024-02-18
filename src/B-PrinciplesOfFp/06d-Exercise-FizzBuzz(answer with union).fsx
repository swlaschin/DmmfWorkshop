// =============================================
// Exercise:
// Implement FizzBuzz as a pipeline
//
// Answered using a Choice as an intermediate value
// =============================================


(*
Definition of FizzBuzz:
    Given a number
    If it is divisible by 3, return "Fizz"
    If it is divisible by 5, return "Buzz"
    If it is divisible by 3 and 5, return "FizzBuzz"
    Otherwise return the number as a string
    Do NOT print anything
*)

// helper function
let isDivisibleBy divisor n =   // question: why put the divisor first?
    (n % divisor) = 0

(*

Exercise:

Rewrite this using a piping model.

let fizzBuzz n =
    n
    |> handle15case
    |> handle3case
    |> handle5case
    |> finalStep


After getting this to work, see if you can define a
single "handle" function that can be reused for 3, 5, and 15.

*)


// a "choice" data structure to pass between the tests for 3,5,7 etc
type FizzBuzzData =
    | Handled of string
    | Unhandled of int

/// Test whether a number is divisible by 15
/// If true, return the "FizzBuzz" in the Handled choice
/// BUT only do this if not already handled
let handle15case fizzBuzzData =
    match fizzBuzzData with
    // if it is already handled
    | Handled label ->
        fizzBuzzData // leave alone

    // if it is not handled
    | Unhandled number ->
        // is it divisible?
        if not (number |> isDivisibleBy 15) then
            fizzBuzzData // leave alone
        // ok, handle this case
        else
            // create a new value which is handled
            Handled "FizzBuzz"

/// A much more generic version of handle15case
/// --------------------------------------------
/// Test whether a number is divisible by divisor
/// If true, return the label in the Handled choice
/// BUT only do this if not already handled
let handle divisor label fizzBuzzData =
    match fizzBuzzData with
    // if it is already handled
    | Handled _ ->
        fizzBuzzData // leave alone

    // if it is not handled
    | Unhandled number ->
        // is it divisible?
        if not (number |> isDivisibleBy divisor) then
            fizzBuzzData // leave alone
        // ok, handle this case
        else
            // create a new value which is handled
            Handled label

// If still unhandled at the end,
// convert number into a string,
// else return the Handled value
let finalStep fizzBuzzData =
    match fizzBuzzData with
    // if it is already handled
    | Handled str ->
        str // use the string

    // if it is not handled
    | Unhandled number ->
        string number // convert to string


// Finally, the main fizzBuzz function!
let fizzBuzz (n:int) :string =
    let initialData = Unhandled n

    initialData
    |> handle 15 "FizzBuzz"
    |> handle 3 "Fizz"
    |> handle 5 "Buzz"
    |> finalStep


// test
[1..30] |> List.map fizzBuzz


// =============================================
// Bonus: Parallel FizzBuzz
// =============================================

// by breaking down the logic into small composable functions,
// we can mix and match them in new ways.

// For example, we can run that same handle function in "parallel"
// and then we can eliminate the need for the "15" handler

/// Combine two fizzbuzz results
let combineHandlers handler1 handler2  =
    fun input ->
        let result1 = handler1 input
        let result2 = handler2 input
        match (result1,result2) with
        | Handled s1, Handled s2 -> Handled (s1 + s2)
        | Handled s1, Unhandled _ -> Handled s1
        | Unhandled _, Handled s2 -> Handled s2
        | Unhandled i, Unhandled _ -> Unhandled i


// The handlers run "in parallel"
// We don't need to have special case for 15, etc
let fizzBuzzParallel (n:int) :string =
    let initialData = Unhandled n

    let parallelHandler =
        [
        handle 3 "Fizz"
        handle 5 "Buzz"
        handle 7 "Zap"
        ]
        |> List.reduce combineHandlers

    initialData
    |> parallelHandler
    |> finalStep


// test
[1..35] |> List.map fizzBuzzParallel
