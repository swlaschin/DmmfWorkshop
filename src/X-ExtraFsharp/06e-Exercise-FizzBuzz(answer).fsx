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

Challenge, write this using a piping model.
See if you can define ONE test function and reuse it
for 3, 5, and 15 using partial application,
just like we did with roman numbers.

TIP:
There is no "return" - the last value in the function is a return

TIP: to check for divisibility use the helper function
"isDivisibleBy" defined below

TIP: to create a string from int use %i
sprintf "%i" 123

TIP:
if/then expressions look like

if x then
   y
else
   z


TIP:
* you will probably need to define an intermediate data structure

type Data = {something:string; somethingElse:bool};
// to create and access
let data = {something="hello"; somethingElse=false};
let something = data.something


*)

// uncomment this code to start

// helper function
let isDivisibleBy divisor n =   // question: why put the divisor first?
    (n % divisor) = 0

type Data = {carbonated: string; number: int}

let test divisor label data =
    // unprocessed
    if data.carbonated = "" then
        if data.number |> isDivisibleBy divisor then
            {carbonated=label; number = -1}
        else
            data // leave alone
    else
        data // leave alone

let finalResult data =
    // unprocessed
    if data.carbonated = "" then
        sprintf "%i" data.number
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

for i in [1..31] do
    printfn "%s" (fizzBuzz i)


// This code is very ugly!
// For extra credit, tidy it up so that test3, test5, and test15 can be combined into one function
