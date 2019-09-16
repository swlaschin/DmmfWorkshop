// ================================================
// Multiple arguments
// ================================================


// can pass two things as a single value -- a tuple
let addTuple (x,y) = x + y


// but can also have ONE argument and then return a new function
let makeAddingFunction x =
    fun y -> x + y
// signature
// val makeAddingFunction : x:int -> y:int -> int

// ========>>> this is called "currying"

// in F# this is done automatically

let add x y = x + y  // exactly the same as makeAddingFunction!

// signature
// val add : x:int -> y:int -> int


// proof
let add1 = add 1  // returns a NEW function

// ========>>> this is called "partial application"



// ================================================
// Partial Application in action
// ================================================

// the benefit of partial application is that you can write a general function and bake in parameters as needed

let formatMessage message str = printfn "%s: %s" message str
let hello =  formatMessage "Hello"
let goodbye =  formatMessage"Goodbye"

hello "Scott"
goodbye "Scott"


// Partial application is an incredibly simple yet powerful idea.  Understanding this is one of the keys to "thinking functionally".
//
// Partial application can be used anywhere you want to bake in parameters.

// let add x y = x + y

// You can now define "add2" in terms of this, rather than defining a new function.
// In this case we want to bake in a "2" as one of the parameters.

//let add2 x = add 2 x
//let add2 x = (add 2) x
let add2 = add 2  // only one parameter supplied to add!


// Here is "double" defined in terms of a more generic "multiply" function.
let multiply x y = x * y
let double = multiply 2

// It is very common to use partial application in conjunction with the piping style

// Rather than defining intermediate functions "add2" and "double":

5
|> add2
|> double  // = 14

5
|> add 2
|> multiply 2  // = 14


// And of course we can extend this to more complex list transformations too:

[1..10]
|> List.map (add 1)
|> List.map (multiply 2)






