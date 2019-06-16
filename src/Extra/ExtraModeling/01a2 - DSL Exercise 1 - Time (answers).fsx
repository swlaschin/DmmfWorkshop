// ================================================
// DSL Exercise: Create a DSL to report the relative time
//
// ================================================

(*
// Design the types to work with these syntax examples
let example1 = getDate 5 Days Ago
let example2 = getDate 1 Hour Hence


*)

// set up the vocabulary
type DateScale = Hour | Hours | Day | Days | Week | Weeks
type DateDirection = Ago | Hence | FromNow 

// define a function that matches on the vocabulary
let getDate (interval:int) (scale:DateScale) (direction:DateDirection) =
    let absHours = match scale with
                   | Hour | Hours -> 1 * interval
                   | Day | Days -> 24 * interval
                   | Week | Weeks -> 24 * 7 * interval
    let signedHours = match direction with
                      | Ago -> -1 * absHours 
                      | Hence | FromNow ->  absHours 
    // create a date given the hours difference                      
    System.DateTime.Now.AddHours(float signedHours)

// test some examples
getDate 5 Days Ago |> printfn "%O"
getDate 1 Hour FromNow |> printfn "%O"
getDate 1 Day FromNow |> printfn "%O"


