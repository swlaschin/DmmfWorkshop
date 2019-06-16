open System

module ConstrainedTypes =

    type NonZeroInteger = private NonZeroInteger of int

    module NonZeroInteger =

        let create i =
            if i = 0 then
                None
            else 
                Some (NonZeroInteger i)

        let value (NonZeroInteger i) = i


open ConstrainedTypes

// ============================
// Exception based design
// ============================

let twelveDividedBy_exn i =
    12 / i

// test
twelveDividedBy_exn 0

twelveDividedBy_exn 4

// ============================
// Option based design
// ============================


let twelveDividedBy_opt i =
    if i = 0 then
        None 
    else 
        Some (12 / i)


// test
let result0 = twelveDividedBy_opt 0

match result0 with
| Some i -> 
    i |> printfn "Answer is %i"
| None -> 
    printfn "Output is not available"

let result1 = twelveDividedBy_opt 1

match result1 with
| Some i -> 
    i |> printfn "Answer is %i"
| None -> 
    printfn "Output is not available"

// ============================
// NonZero based design
// ============================

let twelveDividedBy_nz nz =
    12 / NonZeroInteger.value nz


// test
let nz0 = NonZeroInteger.create 0 

match nz0 with
| Some nz -> 
    twelveDividedBy_nz nz 
    |> printfn "Answer is %i"
| None -> 
    printfn "Input is not a NonZeroInteger"

let nz1 = NonZeroInteger.create 1 

match nz1 with
| Some nz -> 
    twelveDividedBy_nz nz
    |> printfn "Answer is %i"
| None -> 
    printfn "Input is not a NonZeroInteger"
