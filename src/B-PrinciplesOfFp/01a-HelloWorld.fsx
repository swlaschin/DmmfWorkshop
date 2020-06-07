// ================================
// My first F# program
// ================================

// It's traditional for tutorials to start with "hello world", so here it is in F#

printfn "Hello World"

(*

Even with such a simple bit of code, there are some interesting things to note:

* This snippet does not need a containing class.
* It can be run directly in an interactive environment
* There is a space between the `printfn` function and its parameter, rather than a parenthesis.
  We'll see why this is important soon!
*)


// ======================================
// tips on how to execute F# code interactively
// ======================================

(*
1. Highlight and evaluate small chunks at a time rather than the whole file
   That way, any errors are local
2. Defined values are global in memory and can be reused.
   If you get in trouble, it can be helpful to clear these.
   - in VS code, kill the F# terminal
   - in Visual Studio, do "Reset Interactive Session"
*)


// for the code below, try highlighting
// each non-comment line in turn
// and executing it

// -------------------
// declare a immutable value (not a "variable")
let myName = "Scott"
// and use it
printfn "my name is %s" myName


// -------------------
// define a function
let printName aName =
    printfn "Hello %s" aName

// and test it
let name = "Alice"
printName name


(*
A few things to note about this code:

* let is used for functions too.
  The `printName` function is defined using let,
  just like the `name` was.
  This is not a coincidence.
  In functional programming, functions are things just like strings and ints.

* Spaces again. `printName` has one parameter (aName)
  and we use spaces rather than parentheses and commas.

* No curly braces! Instead F# uses indentation to indicate blocks of code,
  in this case, the body of the printName function.

*)

// -------------------
// define a function with two parameters
let add x y = x + y
// and call it with 1 and 2
// notice that spaces are used to separate the parameters
add 1 2



// ====================================
// Function values vs simple values
// ====================================

// --------------------
// Simple Values

let x = 1
// val x : int = 1          // <======= look at the signature

// --------------------
// Function Values

let add1 x = x + 1
// val add1 : x:int -> int   // <======= look at the signature.
                             //          It has an arrow in it!



// STOP HERE -- more slides coming!
