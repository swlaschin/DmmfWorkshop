// ================================================
// Exercise:
// Decorator pattern in FP
// ================================================

// Exercise:
//
// Create an input log function and an output log function
// and then use them to create a "logged" version of add1, like this
(*
let logTheInput x = ??
let logTheOutput x = ??
let add1Logged x = logTheInput, then add1, then logTheOutput
*)

// ===========================================
// original function to be logged
// ===========================================

let add1 x = x + 1

// test
add1 4
List.map add1 [1..10]      // do "add1" for each element of a list
[1..10] |> List.map add1   // same thing!



// ===========================================
// define the logging functions
// ===========================================

let logTheInput x =
    printf "Hello In=%A; " x
    x

let logTheOutput x =
    printfn "Out=%i; " x; x

// ===========================================
// define the logged version of add1
// ===========================================

let add1Logged x =
    x |> logTheInput |> add1 |> logTheOutput

let add1Logged x =
    x |> logTheInput |> add1 |> logTheOutput


// test
add1Logged 4
List.map add1Logged [1..10]      // do "add1Logged" for each element of a list
[1..10] |> List.map add1Logged