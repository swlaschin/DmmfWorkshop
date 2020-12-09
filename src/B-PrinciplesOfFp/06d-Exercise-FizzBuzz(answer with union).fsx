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
    | Processed of string
    | Unprocessed of int

/// Test whether a number is divisible by 15
/// If true, return the "FizzBuzz" in the Processed choice
/// BUT only do this if not already processed
let handle15case fizzBuzzData =
    match fizzBuzzData with
    // if it is already processed
    | Processed label ->
        fizzBuzzData // leave alone

    // if it is not processed
    | Unprocessed number ->
        // is it divisible?
        if not (number |> isDivisibleBy 15) then
            fizzBuzzData // leave alone
        // ok, handle this case
        else
            // create a new value which is processed
            Processed "FizzBuzz"

/// A much more generic version of handle15case
/// --------------------------------------------
/// Test whether a number is divisible by divisor
/// If true, return the label in the Processed choice
/// BUT only do this if not already processed
let handle divisor label fizzBuzzData =
    match fizzBuzzData with
    // if it is already processed
    | Processed _ ->
        fizzBuzzData // leave alone

    // if it is not processed
    | Unprocessed number ->
        // is it divisible?
        if not (number |> isDivisibleBy divisor) then
            fizzBuzzData // leave alone
        // ok, handle this case
        else
            // create a new value which is processed
            Processed label

// If still unprocessed at the end,
// convert number into a string,
// else return the Processed value
let finalStep fizzBuzzData =
    match fizzBuzzData with
    // if it is already processed
    | Processed str ->
        str // use the string

    // if it is not processed
    | Unprocessed number ->
        string number // convert to string


// Finally, the main fizzBuzz function!
let fizzBuzz (n:int) :string =
    let initialData = Unprocessed n

    initialData
    |> handle 15 "FizzBuzz"
    |> handle 3 "Fizz"
    |> handle 5 "Buzz"
    |> finalStep


// test
[1..30] |> List.map fizzBuzz





