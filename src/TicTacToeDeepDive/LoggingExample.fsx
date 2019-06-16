let add1 x = x + 1

List.map add1 [1..10]













let logInput x = printf "Input is: %i. " x; x
let logOutput x = printfn "Output is: %i. " x; x

let loggedAdd1 = logInput >> add1 >> logOutput

List.map loggedAdd1 [1..10]