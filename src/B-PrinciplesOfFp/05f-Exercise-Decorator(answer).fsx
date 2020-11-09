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


// ===========================================
// Exercise: define the logging functions here
// ===========================================

let logTheInput x =
    printf "Hello In=%A; " x
    x

let logTheOutput x =
    printfn "Out=%i; " x; x

// ===========================================
// Exercise: define the logged version of add1
// ===========================================

let add1Logged x =
    x |> logTheInput |> add1 |> logTheOutput

// alternative implementation using composition
// instead of piping
let add1Logged_v2 =
    // note that there is no "x" parameter
    logTheInput >> add1 >> logTheOutput


// and test it
add1Logged 4
List.map add1Logged [1..10]      // do "add1Logged" for each element of a list

// =============================================
// now try logging with a string function
// =============================================

let sayHello name = "hello" + name
sayHello "alice"

// Exercise: define the logged version of sayHello
let sayHelloLogged x =
    x |> logTheInput |> sayHello |> logTheOutput
    // Why does this code have a compiler error?
    // How can you fix it?

sayHelloLogged "alice"

