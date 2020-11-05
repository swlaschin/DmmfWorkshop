// =============================================
// Exercise:
// Implement FizzBuzz as a pipeline
//
// Answered using a Record as an intermediate value
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



// Define a record structure to pass between the tests for 3,5,7 etc
type FizzBuzzData = {carbonated:string; number:int}

/// Test whether a data.number is divisible by 15
/// If true, return the "FizzBuzz" in data.carbonated.
/// BUT only do this if not already processed (data.carbonated is empty)
let handle15case fizzBuzzData =

    // is it already processed?
    if fizzBuzzData.carbonated <> "" then
        fizzBuzzData // leave alone

    // is it divisible?
    else if not (fizzBuzzData.number |> isDivisibleBy 15) then
        fizzBuzzData // leave alone

    // ok, handle this case
    else
        // create a new value which is carbonated
        {carbonated="FizzBuzz"; number=fizzBuzzData.number}
        // alternatively you can copy with update
        // {fizzBuzzData with carbonated=label}

/// A much more generic version of handle15case
/// --------------------------------------------
/// Test whether data.number is divisible by divisor
/// If true, return the label in data.carbonated.
/// BUT only do this if not already processed (data.carbonated is empty)
let handle divisor label fizzBuzzData =

    // is it already processed?
    if fizzBuzzData.carbonated <> "" then
        fizzBuzzData // leave alone

    // is it divisible?
    else if not (fizzBuzzData.number |> isDivisibleBy divisor) then
        fizzBuzzData // leave alone

    // ok, handle this case
    else
        // create a new value which is carbonated
        {carbonated=label; number=fizzBuzzData.number}
        // alternatively you can copy with update
        // {fizzBuzzData with carbonated=label}


// If still unprocessed at the end,
// convert data.number into a string,
// else return data.carbonated
let finalStep fizzBuzzData =
    if fizzBuzzData.carbonated = "" then
        string fizzBuzzData.number
    else
        fizzBuzzData.carbonated

// Finally, the main fizzBuzz function!
let fizzBuzz (n:int) :string =
    let initialData = {carbonated=""; number=n}

    initialData
    |> handle 15 "FizzBuzz"
    |> handle 3 "Fizz"
    |> handle 5 "Buzz"
    |> finalStep


// test it interactively
[1..30] |> List.map fizzBuzz





