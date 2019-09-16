// ================================================
// Everything is an expression
// ================================================

let x1 = fun () -> 1

let x2 = match 1 with
         | 1 -> "a"
         | _ -> "b"

let x3 = if true then "a" else "b"

let x4 = for i in [1..10]
            do printf "%i" i

let x5 = try
            let result = 1 / 0
            printfn "%i" result
         with
         | e ->
            printfn "%s" e.Message

// for branches, ALL parts have to match!

let testA x =
    match x with
    | 1 -> "a"
    | _ -> 99


let testB x =
    if x then 1 else "a"


let testC x =
    if x then 1

