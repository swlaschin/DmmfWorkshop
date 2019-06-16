// ================================================
// Decorator pattern in FP
// ================================================

// Create an input log function and an output log function
// and then use them to create "logged" version of add1

let add1 x = x + 1

let logTheInput x =
    printf "In=%i; " x
    x

let logTheOutput x =
    printfn "Out=%i; " x
    x

let add1Logged x =
    x |> logTheInput |> add1 |> logTheOutput

// test
add1Logged 4

// test
[1..10] |> List.map add1Logged