// ================================================
// Syntax for List literals
// ================================================

// NOTE: In non-parameter usage, F# uses semicolons where C# uses commas.
// So for example, list literals use semicolons rather than commas. 

// try running this:
let twoToFive = [2;3;4;5]        // Square brackets create a list with
                                 // semicolon delimiters.

// notice how the interactive output changes
// val twoToFive : int list = [2; 3; 4; 5]

// try running this:
let oneToFive = 1 :: twoToFive   // :: creates list with new 1st element

// The result is [1;2;3;4;5]
// val oneToFive : int list = [1; 2; 3; 4; 5]

// try running this:
let zeroToFive = [0;1] @ twoToFive   // @ concats two lists

// The result is 
// val zeroToFive : int list = [0; 1; 2; 3; 4; 5]

// IMPORTANT: commas are never used as delimiters, only semicolons!

// you can also use "m..n" syntax for a range
let oneToTen = [1..10]            // try running this

// you can also use list comprehension.  Try running this:
let twoToTwenty = [for i in 1..10 do yield i*2 ]  
let twoToTen = [for i in 1..10 do if i%2=0 then yield i]  

// ================================================
// Part 5 - List functions - map, filter, sortBy
// ================================================

// There are some very useful functions that work on lists.

// They are very similar to the LINQ ones
//   List.map            similar to LINQ.Select
//   List.filter         similar to LINQ.Where   
//   List.sortBy         similar to LINQ.OrderBy
//   etc

// They normally have two parameters: a function that acts on each item, and the list itself.
// List.map [selectFunction] [listToOperateOn]
// List.filter [whereFunction] [listToOperateOn]
// List.sortBy [orderByFunction] [listToOperateOn]

// In this example, the "selectFunction" is "add1" and the listToOperateOn is "1..10"
let add1 x = x + 1
List.map add1 [1..10]

// In this example, do "map" first, with two args, then do "sum" on the result.
let sumOfSquaresTo100_v1 =
   let square x = x * x
   let squares = List.map square [1..100]
   List.sum squares 

// The List.map could be inlined, but it needs parentheses!
// Without the parens, "List.map" would be passed as an arg to List.sum
let sumOfSquaresTo100_v2 =
   let square x = x * x
   List.sum ( List.map square [1..100] )

// Generally though, list operations are written using pipes, which makes them easier to read.

// Here is the same sumOfSquares function written using pipes
let sumOfSquaresTo100_piped =
   let square x = x * x
   [1..100] |> List.map square |> List.sum  // "square" was defined earlier


// And here are other examples of List functions.
// Run each one in turn and make sure you understand what is happening.
let add1 x = x + 1
[1..10] |> List.map add1

let square x = x * x
[1..10] |> List.map square

let isEven x = x % 2 = 0
[1..10] |> List.filter isEven

let isGreaterThan5 x = x > 5
[1..10] |> List.filter isGreaterThan5 

let negative x = -x
[1..10] |> List.sortBy negative 

// sort by boolean result
[1..10] |> List.sortBy isEven

// we can also use the string wrapper functions we created earlier in conjunction with the list functions
["hello"; "goodbye"] |> List.filter (startsWith "h")
["hello"; "goodbye"] |> List.map (replace "o" "-")

// other useful functions are "length" and "head" (first element) and "sum" and "average"
[1..10] |> List.head
[1..10] |> List.sortBy negative |> List.head
[1..10] |> List.filter isGreaterThan5 |> List.head

[1..10] |> List.filter isEven |> List.length
[1..10] |> List.filter isGreaterThan5 |> List.length

[1..10] |> List.sum

[1..10] |> List.map float |> List.average   // average only works with floats. How did I convert the ints to floats?


// Exercise: Use List.filter find strings that contain a "h"

(*
["alice"; "bob"; "hello"; "hi"] |> List.filter what?? 
*)

// Follow up: Use List.filter find strings that contain both an "h" and an "o"



// ================================================
// Part 13 - Pattern Matching with Lists
// ================================================

// Lists have their own pattern matching syntax:

// [] matches an empty list
// [x] matches a list with exactly one element
// [x;y] matches a list with exactly two elements

// first::rest matches a list at least one element. 
//   "first" is bound to the first element
//   "rest" is bound to the the rest of the list (which might be empty)

let listMatchingExample aList =
    match aList with
    | [] -> printfn "the list is empty"
    | [x] -> printfn "the list has one element and it is %i" x
    | first::rest -> 
        printfn "the list has more than one element and the first element is %i" first

// try running this
listMatchingExample [] 

// try running this
listMatchingExample [1]

// try running this
listMatchingExample [1;2]

// try running this
listMatchingExample [1..10]

// try running this -- why does it not compile?
listMatchingExample ["hello"]   


// Exercise - what happens if the "first::rest" case is moved above the "[x]" case in the example?

// ================================================
// Part 14 - Recursion with lists
// ================================================

// A function can call itself to run the same logic on a smaller set of data.

let rec listMatchingExampleWithRecursion aList =
    match aList with
    | [] -> printfn "the list is empty"
    | [x] -> printfn "the list has one element and it is %i" x
    | first::rest -> 
        printfn "the list has more than one element and the first element is %i" first
        listMatchingExampleWithRecursion rest

listMatchingExampleWithRecursion [1;2]
listMatchingExampleWithRecursion [1;2;3]

// Tips:
//   * Use the "rec" keyword
//   * You often need to pass in some extra parameters to keep track of the state

// For example, to count the elements of list, we have a list and a sumSoFar

let rec sumOfElements aList sumSoFar =
    match aList with
    | [] -> // we're done, return the sumSoFar 
        sumSoFar 
    | first::rest -> // we're not done, so take the first and add it to the sumSoFar 
        let newSumSoFar = first + sumSoFar
        let smallerList = rest
        sumOfElements smallerList newSumSoFar 

// try running this
let sumSoFar = 0
let list = [1..10]
let sum = sumOfElements list sumSoFar 

