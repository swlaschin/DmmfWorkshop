// ================================================
// Decorator pattern in FP
// ================================================

// Create an input log function and an output log function
// and then use them to create a "logged" version of add1


let add1 x = x + 1

let logTheInput x = ??  // use printfn

let logTheOutput x = ??  // use printfn

// TIP for add1Logged use piping "|>"
let add1Logged x =
    x |> ??


// test
add1Logged 4

// test
[1..10] |> List.map add1Logged