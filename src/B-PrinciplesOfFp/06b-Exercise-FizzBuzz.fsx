// =============================================
// Exercise:
// Implement FizzBuzz as a pipeline
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

// a straightforward implementation
let simpleFizzBuzz n =
    if n |> isDivisibleBy 15 then
        "FizzBuzz"    // NOTE no return keyword needed
    else if n |> isDivisibleBy 3 then
        "Fizz"
    else if n |> isDivisibleBy 5 then
        "Buzz"
    else
        string n

// test it on the numbers up to 30
[1..30] |> List.map simpleFizzBuzz

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



F# TIPS:
* There is no "return" - the last value in the function is a return
* To check for divisibility use the helper function
  "isDivisibleBy" defined above
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

// define a record type
type MyData = {something:string; somethingElse:int}

// to create a value
let myData = {something="hello"; somethingElse=42}

// to copy/update a value
let myData2 = {myData with somethingElse=42}

// to access a field in the record
let something = myData.something
*)


// fizzBuzz takes an int and returns a string
let fizzBuzz (n:int) :string =
    // handle 15 with "FizzBuzz"
    // then handle 3 with "Fizz"
    // then handle 5 with "Buzz"
    // then handle what's left

// test it on the numbers up to 30
[1..30] |> List.map fizzBuzz

