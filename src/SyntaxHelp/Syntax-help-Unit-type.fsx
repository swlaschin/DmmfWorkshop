// ================================================
// Part 6. Special Types - unit
// ================================================

// F# has some special types that might be new to you: unit, tuples, and automatic generics.

// Let's start with the "unit" type.

// The unit type is similar to "void" and is used for inputs and outputs that have no value.
// The type "unit" has one value, written as "()"

// Try running this:
let myUnit = ()

// pay attention to the output of the interactive window
//    val myUnit : unit = ()

// "unit" is the type. "()" is the value,



// Try running this:
let myUnit2 = printfn "The printfn returns nothing, so unit is used"

// pay attention to the output of the interactive window
//    val myUnit2 : unit = ()
// The result of printfn is nothing


// This function has an input but no output. Try running it:
let unitOutput x = printfn "x=%i" x
// pay attention to the output of the interactive window
//    val unitOutput : x:int -> unit

// This function has an output but no input. Try running it:
let unitInput() = 1
// pay attention to the output of the interactive window
//    val unitInput : unit -> int


// Predict what the signature of this will be.
// Also, predict whether "hello" will be printed immediately
let example6a = 
    printfn "hello"

// Predict what the signature of this will be.
// Also, predict whether "hello" will be printed immediately
let example6b() = 
    printfn "hello"

// Predict what the signature of this will be.
// Also, predict whether "hello" will be printed immediately
let example6c = 
    printfn "hello"
    1

// Predict what the signature of this will be.
// Also, predict whether "hello" will be printed immediately
let example6d() = 
    printfn "hello"
    1

// Predict what happens if I evaluate "example6a" twice.
// Also, predict whether "hello" will be printed or not.
// (Try running the code below)
example6a 
example6a 

// Predict what happens if I evaluate "example6b" twice.
// Also, predict whether "hello" will be printed or not.
example6b
example6b

// Predict what happens if I evaluate "example6b()" twice.
// Also, predict whether "hello" will be printed or not.
example6b()
example6b()

