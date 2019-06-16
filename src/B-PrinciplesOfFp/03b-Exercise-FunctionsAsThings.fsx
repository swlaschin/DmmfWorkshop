//----------------------------------------------------------
//  Q. Write a `sayGreeting` function that takes two parameters:
//  `greeting` and `name`.
//
// Then create a `hello` function from it using
// partial application, where the greeting set to `"Hello"`.
//
// Also create a `goodbye` function from it using
// partial application, where the greeting set to `"Goodbye"`.


//----------------------------------------------------------
// Q. Define a `sayGreeting_v2` _value_ (not a function)
// that is equivalent to the `sayGreeting` function.
// Hint: use a lambda.

//----------------------------------------------------------
// Q. Define a `sayGreeting_v3` function where the
// `greeting` parameter is a parameterless function
// (e.g. unit as input)

//----------------------------------------------------------
//  Q. Define a `sayGreeting_v4` function where the
// `greeting` parameter is a function that depends on the name

//----------------------------------------------------------
// Q. The function to filter a list is the `List.filter`
// function. It takes two parameters, the first is a
// predictate (returning bool) and the second is a list.
//
//  * Define a function `isLessThanFive` with signature `int->bool`
//  * Create a new function `filterLessThanFive` by partially applying this to the `List.filter` function.
//  * Test your function with the input list `[1..10]`


//----------------------------------------------------------
// Q. For each of the following definitions, predict
// what the function signature will be. Then run them to find out!

let testA f = (f 1) + 2

let testB f = sprintf "%i" (f 1)

let testC f = f() + 2

let testD x = fun y -> y * x

let testE x y = y * x

let testF = fun x y -> y * x

let testG f x = (f 1) + x

let testH f x =
    if f x then
        sprintf "%i is dood" x
    else
        sprintf "%i is bad" x

// NOTE: `int x` converts any value x to an int
let testI f =
    // transform the function by messing with its inputs
    // and outputs
    let transformedFn (strInput:string) =
        let intInput = int strInput
        let intOutput = f intInput
        sprintf "%i" intOutput
    // return the transformed function
    transformedFn

let testJ f =
    let swappedFn y x =
        f x y
    // return the function with params swapped
    swappedFn

let testK log f =
    let loggedFn input =
        log input
        let output = f input
        log output
        output // return output
    // return the logged function
    loggedFn

let testL f g x =
    // apply f to x, then apply g to the result
    g (f x)


// use testI
let add1 x = x + 1
let strAdd1 = testI add1
strAdd1 "20"  // string "21"
strAdd1 "42"  // string "43"

//----------------------------------------------------------
//  Q. For each of the following signatures, create a function that has that signature.
//  Avoid using explicit type annotations!

//  TIP: If you get an `'a` in your function signature, that means that the parameter can be any type (like generic `<T>` in C#).
//  You will need do something to force it to be an int!

//  TIP: A nested function, when returned, is shown with parentheses like this: `(int->int)`.
//  Returning a lambda will not show parentheses.

// val sigA = x:int -> y:int -> int  // 2 different implementation styles, please

// val sigB = x:int -> (int -> int)  // Hint: define a nested function

// val sigC = f:(int -> int) -> int

// val sigD = x:int -> y:int -> z:int -> int

// val sigE = f:(int -> int) -> (int -> int)

// val sigF = x:int -> f:(int -> int) -> int

// val sigG = f:(int -> int -> int) -> int

//val sigH = f:('a -> int) -> x:'a -> string  // Hint: use sprintf

//val sigI = x:int -> ('a -> int)
