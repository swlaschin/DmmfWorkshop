/// =============================================
/// FizzBuzz
/// =============================================


(*

Given a number
If it is divisible by 3, return "Fizz"
If it is divisible by 5, return "Buzz"
If it is divisible by 3 and 5, return "FizzBuzz"
Otherwise return the number as a string
Do NOT print anything


Exercise: 

Write this using a piping model.
See if you can define ONE function and reuse it
for 3, 5, and 15 using partial application,
just like we did with "replace" in the roman number example


F# TIPS:
* There is no "return" - the last value in the function is a return
* To check for divisibility use the helper function
  "isDivisibleBy" defined below
* To create a string from int use the "string" function
  string 123
* In F#, if/then/else expressions look like
    if x then
        y
    else
        z

DESIGN HINT:
You will probably need to define an intermediate data structure
to pass data around.

type Data = {something:string; somethingElse:int};
// to create a value
let data = {something="hello"; somethingElse=42};
// to access a field
let something = data.something
*)


// helper function
let isDivisibleBy divisor n =   // question: why put the divisor first?
    (n % divisor) = 0

// Data structure to pass between the tests for 3,5,7 etc
type Data = {carbonated: string; number: int}

/// Test whether a data.number is divisible by divisor 
/// If true, return the label in data.carbonated.
/// BUT only do this if data.carbonated is empty
let test divisor label data =
    // unprocessed
    if data.carbonated = "" then
        if data.number |> isDivisibleBy divisor then
            {carbonated=label; number = -1}
        else
            data // leave alone
    else
        data // leave alone

// If still unprocessed at the end, 
// convert data.number into a string, 
// else return data.carbonated
let finalResult data =
    if data.carbonated = "" then
        string data.number
    else
        data.carbonated

// fizzBuzz takes an int and returns a string
let fizzBuzz (aNumber:int) :string =
    let data = {carbonated=""; number=aNumber}
    data
    |> test 15 "FizzBuzz"
    |> test 3 "Fizz"
    |> test 5 "Buzz"
    |> finalResult

// test
for i in [1..31] do
    printfn "%s" (fizzBuzz i)


