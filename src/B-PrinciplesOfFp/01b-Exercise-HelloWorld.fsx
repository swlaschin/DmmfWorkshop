// Example of a simple function
let printName aName =
    printfn "Hello %s" aName

// the type signature looks like this
// val printName : aName:string -> unit

// test it by running this interactively, one line at a time
let name = "Alice"
printName name


// --------------------------
// Q. `sprintf` is a function that is similar to `printfn` except
// that it returns a string rather than writing to the console.
// Create a `sprintName` function that uses `sprintf` instead
// of `printfn`. What is its signature?  How does it compare with "printName"

let sprintName aName = ??

// --------------------------
// Q. Using the `let` syntax, create a `multipliedByTwo` function
// that multiplies its argument by two. What is its signature?

let multipliedByTwo x = ??

// --------------------------
// Q. Write a `sayGreeting` function that takes two
// parameters: `greeting` and `name`, separated by spaces.
// If you pass in `"Hello"` as the greeting and
// `"Alice"` as the name, the result should be `"Hello Alice"`.
// What is the signature of this function?

let sayGreeting ?? ?? = ??

