/// =============================================
/// Think of a number
/// =============================================

(*

Think of a number.
Add one to it.
Square it.
Subtract one.
Divide by the number you first thought of.
Subtract the number you first thought of.
The answer is TWO!

Challenge, write this using a piping model.
Use the code below as a starting point
*)

// uncomment this code to start
// by removing (* and *)
let thinkOfANumber numberYouThoughtOf =
    let addOne x = x + 1
    let squareIt x = x * x
    let subtractOne x = x - 1
    let divideByTheNumberYouFirstThoughtOf x = x / numberYouThoughtOf
    let subtractTheNumberYouFirstThoughtOf x = x - numberYouThoughtOf

    // define these functions
    // then combine them using piping

    numberYouThoughtOf
    |> addOne
    |> squareIt
    |> subtractOne
    |> divideByTheNumberYouFirstThoughtOf
    |> subtractTheNumberYouFirstThoughtOf

// test your implementation
thinkOfANumber 10
thinkOfANumber 11
thinkOfANumber 12

// what happens if you use a very large number?
thinkOfANumber 12000


