// --------------
// Q. Make the following functions compile by adding type annotations.
// Try to use the minimum annotations possible!

let trim s = s.Trim()        // s is string

let split ch s =  s.Split(ch)   // s is string, ch is a char

(*
type names vs instance values

type name | instance
-----------------------
unit      | ()
bool      | true, false
int       | 42
*)


// --------------------------
//  Q. For each of the following definitions,
//  say whether it is a simple value or a function,
//  and if a function, what is the function signature?
//  If in doubt, run them to find out!

module ValueOrFunction =

    let testA = 2

    let testB x = 2 + x

    let testC x = 2.0 + x

    let testD = "hello"

    let testE = printfn "hello"

    let testF () = 42

    let testG () = printfn "hello"

    let testH x = String.length x

    let testI x = sprintf "%i" x

    let testJ x = printfn "%i" x

    let testK x =
        printfn "x is %f" x
        x  // return x

    let testL (f:int -> string) x = f x


    let testM f (x:int) :string = f x

    let testN x :string = x 1   // hint: what does :string modify?

    let testO x = 1

    let testP x = x 1          // hint: what kind of thing is x?

    let testQ x y = x

    let testR x y z = z

    let testS x = x=x


//----------------------------------------------------------
// Q. For each of the following definitions, predict
// what the function signature will be. Then run them to find out!

module PredictSignature =

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

module MatchASignature =

    // val sigA = int -> int

    // val sigB = int -> unit

    // val sigC = int -> string

    // val sigD = unit -> string

    // val sigE = string -> string

    // val sigF = int -> bool -> float -> string

    // ==========================
    // more complex
    // ==========================

    // val sigG = x:int -> y:int -> int  // 2 different implementation styles, please

    // val sigH = x:int -> (int -> int)  // Hint: define a nested function

    // val sigI = f:(int -> int) -> int

    // val sigJ = x:int -> y:int -> z:int -> int

    // val sigK = f:(int -> int) -> (int -> int)

    // val sigL = x:int -> f:(int -> int) -> int

    // val sigM = f:(int -> int -> int) -> int

    // val sigN = f:('a -> int) -> x:'a -> string  // Hint: use sprintf

    // val sigO = x:int -> ('a -> int)

    // ==========================
    // Below are four generic signatures.
    // Try to create four functions that are inferred to have these signatures.
    // ==========================

    // val sigW = 'a -> int

    // val sigX = int -> 'a

    // val sigY = 'a -> 'a

    // val sigZ = 'a -> 'b



    ()