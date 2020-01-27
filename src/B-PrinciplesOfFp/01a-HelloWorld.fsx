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



let myName = "Scott"
printfn "my name is %s" myName

let add x y = x + y
add 1 2

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


// ======================================
// Playing around with printfn and sprintf
// ======================================

"hello"  // string
42       // int
3.141    // float
true     // bool
[1;2;3]  // list

printfn "%s" "hello"  // %s string
printfn "%i" 42       // %i int

printfn "%f" 3.15   // %f float
printfn "%g" 3.15   // %g float
printfn "%0.1f" 3.15   //with formatting
printfn "%0.9f" 3.15   //with formatting

printfn "%b" false    // %b bool
printfn "%A" [1..3]   // %A anything
printfn "%s is %i years old" "Alice" 42


let printSquares n =
   for i in [1..n] do
      let sq = i*i
      printfn "%i" sq

printSquares 5

// ======================================
// sprintf is like printfn except that it returns a string
// ======================================


let x = sprintf "%i" 42     // "42"

// C# equivalents to printfn and sprintf
// printfn = Console.WriteLn
// sprintf = String.Format

sprintf "%f" 3.15   // "3.150000"



// STOP HERE -- more slides coming!





// ====================================
// F# does not allow implicit casting
// ====================================

1 + 1.5
1 + int 1.5
float 1 + 1.5
1 + "2"
1 + int "2"
string 1 + "2"

// ====================================
// mutability
// ====================================

let x = 10
x = 11        // this is wrong
// x <- 11    // this is correct


// ====================================
// defining a function
// ====================================

let printName aName =
    printfn "Hello %s" aName

// test
let name = "Alice"
printName name

// define another function
let add x y = x + y

// test
add 1 2


(*
A few things to note about this:
* let is used for functions too.
  The `printName` function is defined using let, just like the `name` was.
  This is not a coincidence.
  In functional programming, functions are things just like strings and ints.
* Spaces again. `printName` has one parameter (aName)
  and we use spaces rather than parentheses and commas.
* No curly braces! Instead F# uses indentation to indicate blocks of code,
  in this case, the body of the printName function.
*)


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
//val add1 : x:int -> int   // <======= look at the signature. It has an arrow in it!


// --------------------
// Modules

/// this is a module, used to group code together
module MyModule =

    let add2 x =
        x + 2   // no "return" keyword. Last expression is returned

MyModule.add2 40  // Result => 42

// "open" is same as "using" in C#
open MyModule
add2 40

