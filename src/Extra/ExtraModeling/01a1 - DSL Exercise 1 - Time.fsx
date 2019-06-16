// ================================================
// DSL Exercise: Create a DSL to report the relative time
//
// ================================================

(*
// Design the types to work with these syntax examples
let example1 = getDate 5 Days Ago
let example2 = getDate 1 Hour Hence

// the C# equivalent would probably be more like this:
// getDate().Interval(5).Days().Ago()
// getDate().Interval(1).Hour().Hence()

*)

// set up the vocabulary
type DateScale = what??
type DateDirection = what??

// define a function that matches on the vocabulary
let getDate (interval:int) (scale:DateScale) (direction:DateDirection) =
    let signedHours = ??
    // create a date given the hours difference
    System.DateTime.Now.AddHours(float signedHours)

// test some examples
let example1 = getDate 5 Days Ago
let example2 = getDate 1 Hour Hence
