//----------------------------------------------------------
//  Q. Write a `sumEven` function that sums up only even numbers
//  in a list. Implement it using `List.fold`

// TIP if/then/else is written
//   if something then
//     result
//   else
//     other result

let isEven i = (i%2 = 0)

let sumEven aList =
    let initialValue = 0
    let action sumSoFar i =
        if isEven i then
            sumSoFar + i
        else
            sumSoFar
    List.fold action initialValue aList

// test
sumEven [1..10]  // result = 30
sumEven [1; 3; 5; 6; 10]  // result = 16

//----------------------------------------------------------
//  Q. Write a `alternatingSum` function that alternately adds and subtracts
//  elements in a list to the running total. Implement it using `List.fold`
//
// TIP: if you need to keep track of two values, use a tuple or a record
//  `fst` gets the first element of a tuple
//  `snd` gets the first element of a tuple


let alternatingSum aList =
    let initialValue = (false,0)
    let action (isNeg,sumSoFar) x =
        if isNeg then
            (false,sumSoFar-x)
        else
            (true ,sumSoFar+x)
    List.fold action initialValue aList
    |> snd  // second element of tuple

// test
alternatingSum [1..10]  // result = -5
alternatingSum [1; 1; 99]  // result = 99
