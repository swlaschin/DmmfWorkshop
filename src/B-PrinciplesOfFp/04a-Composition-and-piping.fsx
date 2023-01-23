﻿// ================================================
// Composition
// ================================================

// Exercise: Execute each chunk of code in this file and
// and make you understand how it works.

(*
In object-oriented programming, we focus on behavior and messages.
Objects have internal state that is changed by calling methods or receiving messages.

In functional programming, the focus is on *transformation* of data.

Unlike the object-oriented approach, data structures are kept separate from the functions that act on them.

Data structures are thus "dumb"; it is the functions that do all the work,
transforming input data structures to (possibly different) output data structures.

One of my favorite analogies for this is to think of functions as little bits of railroad track,
with a tunnel in the middle that tranforms the input into an output.

Here are some simple function definitions:
*)

let add1 x = x + 1
let double x = x * 2
let square x = x * x

(*
The compiler outputs the following text:

val add1 : int -> int
val double : int -> int
val square : int -> int

You can read the arrow as "transforms",
so `add1 : x:int -> int` means that the function `add1` takes
an `int` parameter called `x` and transforms it to another `int`.

One way to apply this transformation is by calling the function
in a familiar way, putting the parameter after the function, like this:

*)


// ================================================
// Composition examples
// ================================================

(*
Composition in most programming languages means nesting the function calls, like this
*)

add1(5)                   // = 6
double(add1(5))           // = 12
square(double(add1(5)))   // = 144


(*
In functional languages, new "track" is created by joining small pieces of "track".

Here's some examples of composition in use:
*)

// create a new bigger track...
let add1_double = add1 >> double

// ... and call this new function
add1_double 5


// create a new bigger track...
let add1_double_square = add1 >> double >> square

// ... and call this new function
add1_double_square 5    // 144


// ================================================
// Piping examples
// ================================================


(*
But it is common in functional programming to chain a set of transformations (that is, functions) together. To do this we using a "pipe" model,
in which the output of one function is sent as the input to the next function in the chain.
This is, of course, similar to the UNIX "pipes and filters" pattern.

In F# the pipe operator is written `|>` and piping works left to right.

Here's some examples of piping in use:
*)

5 |> add1                     // = 6
5 |> add1 |> double           // = 12
5 |> add1 |> double |> square // = 144

(*
As the chains get longer, we often make it more readable by putting each step on a new line, like this:
*)

5
|> add1
|> double
|> square // = 144

// you can make it into a function like this

let addDoubleSquare x =
    x
    |> add1
    |> double
    |> square

addDoubleSquare 5 // 144

// ================================================
// Composition vs Piping
// ================================================

(*
Composition combines two functions to make a new function.char
It doesn't need any extra data.
*)
let add1_double_composed = add1 >> double

(*
Piping NEEDS an initial value to send down the pipe
*)

let fiveAdd1Doubled = 5 |> add1 |> double
let sixAdd1Doubled = 6 |> add1 |> double
let xAdd1Doubled x = x |> add1 |> double




