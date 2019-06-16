//----------------------------------------------------------
//  Q. Write a `sayGreeting` function that takes two parameters: `greeting` and `name`.
//  Then create a `hello` function from it using partial application, where the greeting set to `"Hello"`.
//  Also create a `goodbye` function from it using partial application, where the greeting set to `"Goodbye"`.

let sayGreeting greeting aName =
    sprintf "%s %s" greeting aName

let hello = sayGreeting "Hello"
let goodbye = sayGreeting "Goodbye"

//----------------------------------------------------------
//  Q. Define a `sayGreeting_v2` _value_ (not a function) that is equivalent to the `sayGreeting` function. Hint: use a lambda.

let sayGreeting_v2 =
    fun greeting aName -> sprintf "%s %s" greeting aName

//----------------------------------------------------------
//  Q. Define a `sayGreeting_v3` function where the `greeting` parameter is a parameterless function (e.g. unit as input)

let sayGreeting_v3 greetingFn aName =
    let greeting = greetingFn()
    sprintf "%s %s" greeting aName

//----------------------------------------------------------
//  Q. Define a `sayGreeting_v4` function where the `greeting` parameter is a function that depends on the name

let sayGreeting_v4 greetingFn aName =
    let greeting = greetingFn aName
    sprintf "%s %s" greeting aName

//----------------------------------------------------------
//  Q. The function to filter a list is the `List.filter` function. It takes two parameters, the first is a predictate (returning bool) and the second is a list.
//
//  * Define a function `isLessThanFive` with signature `int->bool`
//  * Create a new function `filterLessThanFive` by partially applying this to the `List.filter` function.
//  * Test your function with the input list `[1..10]`

let isLessThanFive i = (i < 5)
let filterLessThanFive = List.filter isLessThanFive

filterLessThanFive [1..10] // [1; 2; 3; 4]


//----------------------------------------------------------
//  Q. For each of the following definitions, predict what the function signature will be. Then run them to find out!

let testA f = (f 1) + 2
// val testA : f:(int -> int) -> int

let testB f = sprintf "%i" (f 1)
// val testB : f:(int -> int) -> string

let testC f = f() + 2
// val testC : f:(unit -> int) -> int

let testD x = fun y -> y * x
// val testD : x:int -> y:int -> int

let testE x y = y * x
// val testE : x:int -> y:int -> int

let testF = fun x y -> y * x
// val testF : x:int -> y:int -> int

let testG f x = (f 1) + x
// val testG : f:(int -> int) -> x:int -> int

let testH f x =
    if f x then
        sprintf "%i is dood" x
    else
        sprintf "%i is bad" x
// val testH : f:(int -> bool) -> x:int -> string

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
// val testI : f:(int -> int) -> (string -> string)

let testJ f =
    let swappedFn y x =
        f x y
    // return the function with params swapped
    swappedFn
// val testJ : f:('a -> 'b -> 'c) -> ('b -> 'a -> 'c)

let testK log f =
    let loggedFn input =
        log input
        let output = f input
        log output
        output // return output
    // return the logged function
    loggedFn
// val testK : log:('a -> unit) -> f:('a -> 'a) -> ('a -> 'a)

let testL f g x =
    // apply f to x, then apply g to the result
    g (f x)
// val testL : f:('a -> 'b) -> g:('b -> 'c) -> x:'a -> 'c


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
let sigA_v1 x y =
    y + x
let sigA_v2 x =
    fun y -> y + x

// val sigB = x:int -> (int -> int)  // Hint: define a nested function
let sigB x =
    let f y = y + x
    f

// val sigC = f:(int -> int) -> int
let sigC f =
    (f 1) + 2

// val sigD = x:int -> y:int -> z:int -> int
let sigD x y z =
    x + y + z

// val sigE = f:(int -> int) -> (int -> int)
let sigE f =
    let inner x = f (x+1) + 2
    inner

// val sigF = x:int -> f:(int -> int) -> int
let sigF x f =
    f (x+1) + 2

// val sigG = f:(int -> int -> int) -> int
let sigG f =
    (f 1 2) + 3

//val sigH = f:('a -> int) -> x:'a -> string  // Hint: use sprintf
let sigH f x =
    let anInt = f x
    sprintf "%i" anInt

//val sigI = x:int -> ('a -> int)
let sigI x =
    let innerFn any =
        x+1
    innerFn
