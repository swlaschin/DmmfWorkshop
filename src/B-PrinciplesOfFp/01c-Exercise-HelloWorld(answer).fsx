// --------------------------
//  Q. `sprintf` is a function that is similar to `printfn` except that it returns a string rather than writing to the console.
//  Create a `sprintName` function that uses `sprintf` instead of `printfn`. What is its signature?

let sprintName aName =
    sprintf "%s" aName

// The type signature is:
// val sprintName :  aName:string -> string

// --------------------------
//  Q. Using the `let` syntax, create a function that multiplies its argument by two. What is its signature?

let multiplyBy2 x = x * 2

// The type signature is:
// val multiplyBy2 :  x:int -> int

// --------------------------
//  Q. Write a `sayGreeting` function that takes two parameters: `greeting` and `name`, separated by spaces.
//  If you pass in `"Hello"` as the greeting and `"Alice"` as the name, the result should be `"Hello Alice"`.
//  What is the signature of this function?

let sayGreeting greeting aName =
    sprintf "%s %s" greeting aName

// The type signature is:
// val sayGreeting : greeting:string -> aName:string -> string

sayGreeting "Hello" "Alice"  // "Hello Alice"

