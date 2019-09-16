// ================================================
// Higher Order Functions
// ================================================


// "Parameterize all the things"

let printListA() =
   for i in [1..10] do
      printfn "the number is %i" i

let printListB aList =
   for i in aList do
      printfn "the number is %i" i

let printListC anAction aList =
   for i in aList do
      anAction i

let printAction i = printfn "the number is %i" i
[1..10] |> printListC printAction


// ================================================
// Fold
// ================================================

// -----------------------------------------
// imperative versions with mutable accumulator
// -----------------------------------------
module ImperativeVersion =

    let sum aList =
       let mutable sumSoFar = 0
       for element in aList do
          sumSoFar <- sumSoFar + element
       sumSoFar

    // try it
    let answer1 = sum [1..10]
    // answer1 = 55

    printfn "answer=%i" answer1


    let product aList =
       let mutable productSoFar = 1
       for element in aList do
          productSoFar <- productSoFar * element
       productSoFar

    // try it
    let answer2 = product [1..10]
    // answer2 = 3628800
    printfn "answer=%i" answer2

(*

What do these implementations have in common?

The looping logic!

As programmers, we are told to remember the DRY principle ("don’t repeat yourself"),
yet here we have repeated almost exactly the same loop logic each time.


Let's see if we can extract just the differences between these three methods:

| Function  | Initial value | Inner loop logic |
| Sum       | sum=0         | Add the i'th element to the running total |
| Product   | product=1     | Multiply the i'th element with the running total |

Is there a way to strip the duplicate code and focus on the just the setup and inner loop logic?

Yes there is.

Here are the same three functions in F#:
*)


// -----------------------------------------
// imperative versions with refactored accumulator and loop action
// -----------------------------------------

module MutableRefactored =

    let sum aList =

       // code specific to "sum"
       let initialValue = 0
       let action sumSoFar element =
            sumSoFar + element

       // common code
       let mutable accumulator = initialValue
       for element in aList do
          accumulator <- action accumulator element
       accumulator

    let product aList =

        // code specific to "product"
       let initialValue = 1
       let action productSoFar element =
            productSoFar * element

       // common code
       let mutable accumulator = initialValue
       for element in aList do
          accumulator <- action accumulator element
       accumulator

    sum [1..10] |> printfn "sum=%i"
    product [1..10] |> printfn "product=%i"



// -----------------------------------------
// fold
// -----------------------------------------

module UsingFold =

    let listProcessor action initialValue aList =
       let mutable accumulator = initialValue
       for element in aList do
          accumulator <- action accumulator element
       accumulator

    //=== Using common function to define new functions ===


    let sum aList =
        let initialValue = 0
        let action sumSoFar element =
            sumSoFar + element
        listProcessor action initialValue aList

    let sum1to10 = sum [1..10]
    // sum1to10 = 3628800

    let product aList =
        let initialValue = 1
        let action productSoFar element =
            productSoFar * element
        listProcessor action initialValue aList

    let product1to10 = product [1..10]
    // product1to10 = 3628800

    printfn "sum1to10 = %i" sum1to10
    printfn "product1to10 = %i" product1to10


    //=== Using listProcessor directly ===
    let sum1to10_v2 = listProcessor (+) 0 [1..10]

    let product1to10_v2 = listProcessor (*) 1 [1..10]

module UsingListFold =

    //=== Using List.fold directly ===
    let sum1to10 = List.fold (+) 0 [1..10]
    let product1to10 = List.fold (*) 1 [1..10]


module UsingListFoldWithPartialApplication =

    //=== Using List.fold with partial application ===
    let sum = List.fold (+) 0
    let product = List.fold (*) 1

    let sum1to10 = sum [1..10]
    let product1to10 = product [1..10]

    printfn "sum1to10 = %i" sum1to10
    printfn "product1to10 = %i" product1to10

let product n =
    let initialValue = 1
    let action productSoFar x = productSoFar * x
    [1..n] |> List.fold action initialValue

//test
product 10


(*
All fold functions have the same pattern:

* Set up the initial value
* Set up an action function that will be performed on each element inside the loop.
* Call the library function List.fold.
  This is a powerful, general purpose function which starts with the initial value and
  then runs the action function for each element in the list in turn.

The action function always has two parameters:
* a running total (or state) and
* the list element to act on (called "x" in the above examples).

By using List.fold and avoiding any loop logic at all, the F# code gains a number of benefits:

* The key program logic is emphasized and made explicit.
  The important differences between the functions become very clear,
  while the commonalities are pushed to the background.

* The boilerplate loop code has been eliminated, and as a result the
  code is more condensed

* There can never be a error in the loop logic (such as off-by-one)
  because that logic is not exposed to us.

*)


