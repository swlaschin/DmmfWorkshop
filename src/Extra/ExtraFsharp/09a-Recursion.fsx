// ================================================
// Recursion
// ================================================

// A function can call itself to run the same logic on a smaller set of data.

// Tips:
//   * Use the "rec" keyword
//   * You often need to pass in some extra parameters to keep track of the state

let rec countdown number =
    match number with
    | 3
    | 2
    | 1 ->
        printfn "%i" number
        countdown (number - 1)
    | 0 ->
        printfn "Go!"
    | _ ->
        failwith "should not happen"

countdown 3

// ================================================
// Recursion on lists
// ================================================

// list recursion always has at least two patterns
//  * empty list
//  * non-empty list (first::rest)

let rec printItems aList =
    match aList with
    | [] ->
        ()        // stop. Question: why return unit?
    | first::rest ->
        printfn "%A" first
        printItems rest


printItems [1;2]
printItems [1;2;3]


// For example, to count the elements of a list,
// we have a list and a countSoFar

let rec listCount aList countSoFar =
    match aList with
    | [] ->
        // we're done, return the countSoFar
        countSoFar
    | first::rest ->
        // we're not done, so increment the countSoFar and repeat
        let newCountSoFar = countSoFar + 1
        let smallerList = rest
        listCount smallerList newCountSoFar

// try running this
let list = [1..10]
listCount list 0

