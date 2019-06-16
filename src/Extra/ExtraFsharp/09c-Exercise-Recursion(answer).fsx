/// =============================================
/// Recursion exercises
/// =============================================


//----------------------------------------------------------
//  Q. Count down to 0 and then count back up to 3.

let rec countdownThenCountUp number countUp =
    match number with
    | 3 ->
        printfn "%i" number
        if countUp then
            // stop
            printfn "Done!"
        else
            countdownThenCountUp (number - 1) countUp
    | 2 | 1 ->
        printfn "%i" number
        let nextNumber = if countUp then (number + 1) else (number - 1)
        countdownThenCountUp nextNumber countUp
    | 0 ->
        printfn "Go!"
        countdownThenCountUp 1 true
    | _ ->
        failwith "should not happen"

// test
countdownThenCountUp 3 false



//----------------------------------------------------------
// Q. Using list pattern matching, sum the items in a list
// Hide any extra parameters by creating an inner "loop" function

let sum inputList =

    // inner function
    let rec loop aList sumSoFar =
        match aList with
        | [] ->
            sumSoFar // done -- return it
        | first::rest ->
            let sumSoFar = sumSoFar + first
            loop rest sumSoFar

    loop inputList 0

// test
[1..10] |> sum