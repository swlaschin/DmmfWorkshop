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
let isDivisibleBy n divisor =  // question: why put the divisor first?
    (n % divisor) = 0


// fizzBuzz takes an int and returns a string
let fizzBuzz (aNumber:int) :string =
    // something with 15 "FizzBuzz"
    // then do something with 3 "Fizz"
    // then do something with 5 "Buzz"
    // then do something with what's left

// test it
for i in [1..31] do
    printfn "%s" (fizzBuzz i)



