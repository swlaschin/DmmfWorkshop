// --------------------------
//  Q. `sprintf` is a function that is similar to `printfn` except that it returns a string rather than writing to the console.
//  Create a `sprintName` function that uses `sprintf` instead of `printfn`. What is its signature?

let sprintName aName =
    sprintf "%s" aName
// val sprintName :
//    aName:string -> string

// --------------------------
//  Q. Using the `let` syntax, create a function that multiplies its argument by two. What is its signature?

let multiplyBy2 x = x * 2
// val multiplyBy2 :
//    x:int -> int

// --------------------------
//  Q. Write a `sayGreeting` function that takes two parameters: `greeting` and `name`, separated by spaces.
//  If you pass in `"Hello"` as the greeting and `"Alice"` as the name, the result should be `"Hello Alice"`.
//  What is the signature of this function?

let sayGreeting greeting aName =
    sprintf "%s %s" greeting aName

// val sayGreeting :
//    greeting:string -> aName:string -> string

sayGreeting "Hello" "Alice"  // "Hello Alice"

// --------------------------
//  Q. Rewrite `let add x y = x + y` using lambdas. There are two alternatives. Compare the signatures of these alternative functions with the original.


// example to copy
let add2 = fun x -> x + 2

// without a lambda
let add_v1 x y =
    x + y
// val add_v1 : x:int -> y:int -> int

// with a one-parameter lambda
let add_v2 x =
    fun y -> x + y
// val add_v2 : x:int -> y:int -> int

// alternative to v2 using an inner function instead of a lambda
let add_v2a x =
    let innerFn y = x + y
    innerFn // return
// val add_v2a : x:int -> y:int -> int

// with a two-parameter lambda
let add_v3 =
    fun x y -> x + y
// val add_v3 : x:int -> y:int -> int



