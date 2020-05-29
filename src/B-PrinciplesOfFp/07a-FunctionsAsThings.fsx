//===================================
// Functions as things
//===================================
//
// Demonstrates:
//   * functions as values
//   * named functions vs anonymous functions (lambdas)
//   * functions as input
//   * functions as output
//   * partial application
//   * function transformers
//
// Exercise:
//    look at, execute, and understand all the code in this file
//
//===================================

// --------------------------------
// Part 1: Functions are things that can be assigned to values
// --------------------------------

module FunctionsAsValues =

    // define two simple functions
    let add1 x = x + 1
    let multiplyBy2 x = x * 2

    // and test them
    let three = add1 2 // result is 3
    let six = multiplyBy2 3 // result is 6

    // You can assign a function value to another value, like this
    let q = add1
    // q is a function AND a value -- a "function value"
    // val q : (int -> int)

    // Again we can assign a function value to another value, like this
    let r = multiplyBy2
    // val r : (int -> int)

    // and now we can use these values as functions
    let threeAgain = q 2 // result is 3
    let sixAgain = r 3 // result is 6

// --------------------------------
// Part 2: Functions can be defined in multiple ways
// --------------------------------

module FunctionsAndLambdas =

    // -----------------------------------
    // Two different ways to define a one parameter function
    // -----------------------------------

    // A one-parameter function defined in the usual way (a named function)
    let increment_v1 x =
        x + 1
    // val increment_v1 : x:int -> int

    // A one-parameter function defined as a value equal to a lambda.
    // Note: "increment_v2" is a "thing" with no parameters.
    let increment_v2 =
        fun x -> x + 1
    // val increment_v2 : x:int -> int

    // -----------------------------------
    // Four different ways to define a two parameter function
    // -----------------------------------

    // A two-parameter function defined in the usual way
    let add_v1 x y = x + y
    // val add_v1 : x:int -> y:int -> int

    // A two-parameter function defined as a lambda
    // Note: "add_v2" is a "thing" with no parameters.
    let add_v2 = fun x y -> x + y
    // val add_v2 : x:int -> y:int -> int

    // A two-parameter function defined as a mixture of normal definition and lambda
    // * "x" is a parameter of "add_v3"
    // * "y" is a parameter of the lambda.
    let add_v3 x = fun y -> x + y
    // val add_v3 : x:int -> y:int -> int

    // another alternative to v3
    // returning an inner function rather than a lambda
    let add_v4 x =
        let innerFn y = x + y
        innerFn
    // val add_v4 : x:int -> (int -> int)

    (*
    Question: Which way is better?

    Answer: It depends!
    * Named functions are easier to understand, especially if is long
    * Lambdas are easier for inline functions that are short and don't have names

    // good use of a named function
    let myFunc x =
       lots of code
       lots of code
       lots of code
       lots of code

    // good use of a lambda
    [1..10] |> List.map (fun i -> i + 1)

    *)



// --------------------------------
// Part 3: Functions are things that can be put in lists, etc
// --------------------------------

module ListOfFunctions =

    // define some simple functions
    let add1 x = x + 1
    let multiplyBy2 x = x * 2
    let subtract3 x = x - 3

    // put them in a list
    let listOfFunctions = [add1; multiplyBy2; subtract3]

    // loop through the list and for each function, execute it
    for fn in listOfFunctions do
        let result = fn 100
        printfn "If 100 is the input, the output is %i" result

    // Result =>
    // If 100 is the input, the output is 101
    // If 100 is the input, the output is 200
    // If 100 is the input, the output is 97

    // you can use map on the list
    let someInts =
        listOfFunctions
        |> List.map (fun fn -> fn 100)

    // you can transform each function into a new function
    let newListOfFunctions =
        listOfFunctions
        |> List.map (fun fn ->
            let newFn x = (fn x) + 42
            newFn // return this
            ) // end of the lambda

    let someMoreInts =
        newListOfFunctions
        |> List.map (fun fn -> fn 100)


    // another way of doing the same thing
    let newListOfFunctions_v2 =
        let add42 x = x + 42
        listOfFunctions
        |> List.map (fun fn -> fn >> add42)


    let someMoreInts_v2 =
        newListOfFunctions_v2
        |> List.map (fun fn -> fn 100)

// --------------------------------
// Part 4: Functions need to have parameters!
// --------------------------------

module StrictEvaluation =

    // hello1 is a unit value
    let hello1 = printfn "Hello"
    // console output shows "Hello" immediately

    // hello2 is a function value
    let hello2() = printfn "Hello"

    // assign the function value to value
    let z = hello2

    // evaluate later
    z     // doesnt do anything
    z()   // runs the function
    z()   // runs the function again

// --------------------------------
// Part 5: Functions can be used as input parameters
// --------------------------------

module FunctionsAsInput =


    // define a function with a function parameter
    let evalWith5ThenAdd2 fn =
        fn 5 + 2     // same as fn(5) + 2
    // val evalWith5ThenAdd2 : fn:(int -> int) -> int

    // define some simple functions
    let add1 x = x + 1
    let multiplyBy2 x = x * 2

    // it will work with ANY int -> int function
    evalWith5ThenAdd2 add1 // result => 8
    evalWith5ThenAdd2 multiplyBy2 // result => 12
    evalWith5ThenAdd2 (fun i -> i + 42) // result => 12

// --------------------------------
// Part 5: Functions can be the output of a function
// --------------------------------

module FunctionsAsOutput =

    let adderGenerator numberToAdd =
        fun x -> numberToAdd + x  // returns a lambda with numberToAdd baked in

    // val adderGenerator :
    //    int -> (int -> int)

    // test
    let add1 = adderGenerator 1
    add1 2   // result => 3
    add1 42   // result => 43

    let add100 = adderGenerator 100
    add100 2   // result => 102

    // this is an alternative way of doing the implementation
    let multiplierGenerator numberToMultiply =
        let innerFn x = numberToMultiply * x
        // return the inner function
        innerFn

    let multiplyBy2 = multiplierGenerator 2
    multiplyBy2 3   // result => 6

    let multiplyBy100 = multiplierGenerator 100
    multiplyBy100 2   // result => 200


// --------------------------------
// Part 6: Partial application, or baking in some but not ALL of the parameters
// --------------------------------

module PartialApplication =

    // If code appears more than once, we like to reuse it

    // Say that we start with this:
    do
        let x = 1 + 2 + 5
        let y = 1 + 2 + 10
        printfn "x is %i. y is %i" x y

    // We see that 1 + 2 occur together, so let's group them
    do
        let x = (1 + 2) + 5
        let y = (1 + 2) + 10
        printfn "x is %i. y is %i" x y

    // Finally, we assign 1 + 2 to a special value ("three"), and use that instead
    do
        let three = 1 + 2
        let x = three + 5
        let y = three + 10
        printfn "x is %i. y is %i" x y

    // We can do the same thing with function.
    // If some parameters appear more than once, we can bake them in

    // Say that we start with this:
    do
        printfn "Hello %s" "Alice"
        printfn "Hello %s" "Bob"

    // We see that (printfn "Hello %s") occur together, so let's group them
    do
        (printfn "Hello %s") "Alice"
        (printfn "Hello %s") "Bob"

    // Finally, we assign (printfn "Hello %s") to a special value ("printName"), and use that instead
    do
        let printName = printfn "Hello %s"
        printName "Alice"
        printName "Bob"

        //to use, we must supply the missing parameter
        printName // error: missingParam
        printName "Alice"
        printName "Bob"

// --------------------------------
// Part 7: Operators like + are actually functions too!
// --------------------------------

module OperatorsAreFunctions =

    module Add_V1 =
        // here we are using + as an operator
        let three = 1 + 2
        let four  = 1 + 2

    module Add_V2 =
        // here we are using + as a two parameter function
        let three = (+) 1 2
        let four  = (+) 1 3

        let five  = (+)       1          4
        //          ^function ^parameter ^parameter

    module Add_V3 =
        // here we are grouping "(+) 1" as common code
        let three = ( (+) 1 ) 2
        let four  = ( (+) 1 ) 3

    module Add_V4 =
        // Finally, we can assign "(+) 1" to a function value
        let add1 = (+) 1
        let three = add1 2
        let four  = add1 3

        2 * 3
        (*) 2 3

    module MoreOperatorExamples =

        let add1 = (+) 1
        add1 2   // result => 3

        let multiplyBy2 = (*) 2
        multiplyBy2 3   // result => 6

        let equals3 = (=) 3
        equals3 3   // result => true

        let fiveIsLessThan = (<=) 5   // The argument order is confusing.
                                      // It's short for 5 <= x
        fiveIsLessThan 3   // result => false
        fiveIsLessThan 6   // result => true

// --------------------------------
// Part 8: Function transformers
// --------------------------------

module FunctionTransformers =
    (*
    It's common for functions to have a function as input AND a function as input

    These kinds of functions "transform" the input function somehow.
    *)

    // Demonstration 1:
    // Transform any 'int->int function into a 'string->string function
    let toStrFunction f =
        fun str ->
            // 1. get an int from a string
            let i = System.Int32.Parse str   // NOTE No error handling here.
                                            // We will discuss that later!
            // 2. execute the function and convert the result back into a string
            sprintf "%i" (f i)

    // test
    let strToStrFnA = toStrFunction (fun i -> i + 1)
    strToStrFnA "42"  // "43"

    let strToStrFnB = toStrFunction (fun i -> i * i)
    strToStrFnB "9"  // "81"

    // --------------------------------
    (*
    The most common kind of function transforms start with a
    function for "normal" values and convert it into
    a function that works on more complex values like Option or List
    *)

    // Demonstration 2 -- for Options:
    // Transform any 'a->'b function into a 'a option -> 'b option function
    let toOptionFunction f =
        fun opt ->
            match opt with
            | Some x -> Some (f x)
            | None -> None

    // Test it: transform a "int->int" function into a  "int option -> int option" function
    let optFunction = toOptionFunction (fun i -> i + 1)
    // now test it
    optFunction (Some 42)

    // --------------------------------

    // Demonstration 3 -- for Lists:
    // Transform any 'a->'b function into a 'a list -> 'b list function
    let toListFunction f =
        fun (list: 'a list) ->
            [for x in list do yield (f x)]

    // Test it: transform a "int->int" function into a  "int list -> int list" function
    let listFunction = toListFunction (fun i -> i + 1)
    // now test it
    listFunction [1..5]


    // Test it: Use partial application to define a "int -> string" function
    let hello = sprintf "Hello %i"
    // then transform it into a  "int list -> string list" function
    let helloList = toListFunction hello
    // now test it
    helloList [1..5]



    (*
    PRO TIP
    val toOptionFunction : ('a -> 'b) -> ('a option -> 'b option)
    val toListFunction   : ('a -> 'b) -> ('a list -> 'b list)

    Are exactly the same as the "map" functions!

    Option.map           : ('a -> 'b) -> ('a option -> 'b option)
    List.map             : ('a -> 'b) -> ('a list -> 'b list)
    *)

    // Test it for yourself
    let helloOption = Option.map hello
    helloOption (Some 1)

    let helloList_v2 = List.map hello
    helloList_v2 [1..5]
