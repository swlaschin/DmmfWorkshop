//===================================
// functions as things
//
// Demonstrates:
//   * functions as input
//   * functions as output
//   * partial application
//   * functions as parameters
//===================================

// --------------------------------
// define some simple functions
// --------------------------------

module FunctionsAsValues =

    let add1 x = x + 1
    // val add1 : x:int -> int

    let multiplyBy2 x = x * 2
    // val times2 : x:int -> int

    let three = add1 2 // result is 3
    let six = multiplyBy2 3 // result is 6

    let q = add1
    // val q : (int -> int)

    let r = multiplyBy2
    // val r : (int -> int)

    let threeAgain = q 2 // result is 3
    let sixAgain = r 3 // result is 6

// --------------------------------
// Functions vs. Lambdas
// --------------------------------

module FunctionsAndLambdas =

    // a one-parameter function defined in the usual way
    let addOne_v1 x =
        x + 1
    // val addOne_v1 : x:int -> int

    // a one-parameter function defined as a lambda.
    // Note: "addOne_v2" is a "thing" with no parameters.
    let addOne_v2 =
        fun x -> x + 1
    // val add_v2 : x:int -> int


    // a two-parameter function defined in the usual way
    let add_v1 x y =
        x + y
    // val add_v1 : x:int -> y:int -> int

    // a two-parameter function defined as a lambda
    // Note: "add_v2" is a "thing" with no parameters.
    let add_v2 =
        fun x y -> x + y
    // val add_v2 : x:int -> y:int -> int

    // a two-parameter function defined as a mixture.
    // "x" is a parameter of "add_v2" but "y" is a parameter of the lambda.
    let add_v3 x =
        fun y -> x + y
    // val add_v3 : x:int -> y:int -> int

    // alternative to v3 using an inner function
    let add_v3a x =
        let innerFn y = x + y
        innerFn // return
    // val add_v3a : x:int -> (int -> int)


// --------------------------------
// functions as data
// --------------------------------

module ListOfFunctions =

    let add1 x = x + 1
    let multiplyBy2 x = x * 2
    let subtract3 x = x - 3

    let listOfFunctions = [add1; multiplyBy2; subtract3]


    for fn in listOfFunctions do
        let result = fn 100
        printfn "If 100 is the input, the output is %i" result

    // Result =>
    // If 100 is the input, the output is 101
    // If 100 is the input, the output is 200
    // If 100 is the input, the output is 97


// --------------------------------
// functions need to have parameters!
// --------------------------------

module StrictEvaluation =

    let hello1 = printfn "Hello"
    // console output shows "Hello" immediately

    let hello2() = printfn "Hello"

    // assign the function value
    let z = hello2

    // evaluate later
    z()
    z()

// --------------------------------
// functions as input
// --------------------------------

module FunctionsAsInput =
    open FunctionsAsValues

    let evalWith5ThenAdd2 fn =
        fn 5 + 2     // same as fn(5) + 2

    // val evalWith5ThenAdd2 : fn:(int -> int) -> int

    // test
    evalWith5ThenAdd2 add1 // result => 8
    evalWith5ThenAdd2 multiplyBy2 // result => 12

    let square x = x * x     // an int -> int function
    evalWith5ThenAdd2 square // result => 27

// --------------------------------
// functions as output
// --------------------------------

module FunctionsAsOutput =
    open FunctionsAsValues

    (*
    let add1 x = x + 1
    let add2 x = x + 2
    let add3 x = x + 3
    *)

    let adderGenerator numberToAdd =
        // return a lambda
        fun x -> numberToAdd + x

    // val adderGenerator :
    //    int -> (int -> int)

    // test
    let add1 = adderGenerator 1
    add1 2   // result => 3

    let add100 = adderGenerator 100
    add100 2   // result => 102


module FunctionsAsOutput_v2 =
    open FunctionsAsValues

    let adderGenerator numberToAdd =
        // define a nested inner function
        let innerFn x =
            numberToAdd + x
        // return the inner function
        innerFn

    // val adderGenerator :
    //    int -> (int -> int)


    // test
    let add1 = adderGenerator 1
    add1 2   // result => 3

    let add100 = adderGenerator 100
    add100 2   // result => 102


    let multiplierGenerator numberToMultiply =
        let innerFn x =
            numberToMultiply * x
        // return the inner function
        innerFn

    let multiplyBy2 = multiplierGenerator 2
    multiplyBy2 3   // result => 6

    let multiplyBy100 = multiplierGenerator 100
    multiplyBy100 2   // result => 200


// --------------------------------
// partial application
// --------------------------------

module PartialApplication =
    open FunctionsAsValues

    // Example of extracting ints
    do
        let x = 1 + 2 + 5
        let y = 1 + 2 + 10
        printfn "x is %i. y is %i" x y

    do
        let x = (1 + 2) + 5
        let y = (1 + 2) + 10
        printfn "x is %i. y is %i" x y

    do
        let three = 1 + 2
        let x = three + 5
        let y = three + 10
        printfn "x is %i. y is %i" x y

    do
        printfn "Hello %s" "Alice"
        printfn "Hello %s" "Bob"

    do
        (printfn "Hello %s") "Alice"
        (printfn "Hello %s") "Bob"

    do
        let printName = printfn "Hello %s"
        printName "Alice"
        printName "Bob"

    do
        let printName = printfn "Hello %s" (* missingParam *)

        //to use, supply this missing parameter
        printName (* missingParam *)
        printName "Alice"
        printName "Bob"

    module Add_V1 =

        let three = 1 + 2
        let four  = 1 + 2

    module Add_V2 =

        let three = (+) 1 2
        let four  = (+) 1 3

    module Add_V3 =

        let three = ( (+) 1 ) 2
        let four  = ( (+) 1 ) 3

    module Add_V4 =

        let add1 = (+) 1
        let three = add1 2
        let four  = add1 3

        2 * 3
        (*) 2 3

    module Add_5 =

        let add1 = (+) 1
        add1 2   // result => 3

        let multiplyBy2 = (*) 2
        multiplyBy2 3   // result => 6

        let equals3 = (=) 3
        equals3 3   // result => true

// --------------------------------
// functions as parameters - Higher Order Functions
// --------------------------------

module FunctionsAsParameters =
    open FunctionsAsValues

    let inputList = [1..5] // same as [1;2;3;4;5]

    let listAfterAdd1 =
        // pass "add1" as the element transformer
        List.map add1 inputList
        // result => [2; 3; 4; 5; 6]

    let listAfterMultiplyBy2 =
        // pass "multiplyBy2" as the element transformer
        List.map multiplyBy2 inputList
        // result => [2; 4; 6; 8; 10]

    // Use partial application to define a function that needs an int
    let hello = sprintf "Hello %i"

    let listAfterHello =
        // pass "hello" as the element transformer
        List.map hello inputList
        // result => ["Hello 1"; "Hello 2"; etc ]

