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


(*
printfn experiments
*)

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



// STOP HERE -- more slides coming!





// ====================================
// strict type checking

1 + 1.5
1 + int 1.5
float 1 + 1.5
1 + "2"
1 + int "2"
string 1 + "2"

// ====================================
// mutability

let x = 10
x = 11
// x <- 11



// ====================================
// defining a function

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

//Simple Value

let x = 1
// val x : int = 1                   // <=========== look at the signature

//Function Value

let add1 x = x + 1
//val add1 : x:int -> int            // <=========== look at the signature

// ====================================
// Three different ways to define a function


module Function_V1 =  // this is a module, used to group code together

    let add1 x =
        x + 1   // no "return" keyword. Last expression is returned

module Function_V2 =

    let add1 x =
        let result = x + 1
        result    // return result

module Function_V3 =

    let add1 =
        fun x -> x + 1   // lambda expression uses "fun"


Function_V1.add1 41  // Result => 42
Function_V2.add1 41  // Result => 42
Function_V3.add1 41  // Result => 42


