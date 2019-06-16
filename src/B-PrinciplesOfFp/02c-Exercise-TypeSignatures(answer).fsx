// --------------
// Q. Make the following functions compile by adding type annotations. Try to use the minimum annotations possible!

let trim (s:string) = s.Trim()

let split ch (s:string) =  s.Split(ch)



// --------------------------
//  Q. For each of the following definitions, say whether it is a simple value or a function, and if a function, what is the function signature?
//  If in doubt, run them to find out!

module ValueOrFunction =

    let testA = 2
    // simple value
    // val testA : int = 2

    let testB x = 2 + x
    // function
    // val testB : x:int -> int

    let testC x = 2.0 + x
    // function
    // val testC : x:float -> float

    let testD = "hello"
    // simple value
    // val testD : string = "hello"

    let testE = printfn "hello"
    // simple value (with no useful value)
    // val testE : unit = ()

    let testF() = 42
    // function (with no useful input)
    // val testF : unit -> int

    let testG() = printfn "hello"
    // function (with no useful input or output)
    // val testG : unit -> unit

    let testH x = String.length x
    // function
    // val testH : x:string -> int

    let testI x = sprintf "%i" x
    // function
    // val testI : x:int -> string

    let testJ x = printfn "%i" x
    // function (with no useful output)
    // val testJ : x:int -> unit

    let testK x =
        printfn "x is %f" x
        x  // return x
    // function
    // val testK : x:float -> float

    let testL (f:int -> string) x = f x
    // val testL : f:(int -> string) -> x:int -> string

    let testM f (x:int) :string = f x
    // val testM : f:(int -> string) -> x:int -> string

    let testN x:string = x 1   // hint: what does :string modify?
    // val testN : x:(int -> string) -> string

    let testO x = 1
    // generic function
    // val testO : x:'a -> int

    let testP x = x 1          // hint: what kind of thing is x?
    // generic function
    // val testP : x:(int -> 'a) -> 'a

    let testQ x y = x
    // generic function
    // val testQ : x:'a -> y:'b -> 'a

    let testR x y z = z
    // val testR : x:'a -> y:'b -> z:'c -> 'c

    let testS x = x=x
    // generic function
    // val testS : x:'a -> bool (with a constraint that type 'a must support equality)


//----------------------------------------------------------
//  Q. For each of the following definitions, predict what the function signature will be. Then run them to find out!

module PredictSignature =

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



// --------------
//  Q. For each of the following signatures, create a function that will be inferred to have that signature.
//  Avoid using explicit type annotations!

module MatchASignature =

    // val sigA = int -> int
    let sigA x = x + 1

    // val sigB = int -> unit
    let sigB x = printfn "%i" x

    // val sigC = int -> string
    let sigC x = sprintf "%i" x

    // val sigD = unit -> string
    let sigD() = "hello"

    // val sigE = string -> string
    let sigE x = sprintf "%s" x
    // NOTE
    //  let sigE x = x
    // does NOT work because the input can be ANY type (as shown by the generic type 'a)

    // val sigF = int -> bool -> float -> string
    let sigF anInt aBool aFloat = sprintf "%i %b %f" anInt aBool aFloat

    // ==========================
    // more complex
    // ==========================

    // val sigG = x:int -> y:int -> int  // 2 different implementation styles, please
    let sigG_v1 x y =
        y + x
    let sigG_v2 x =
        fun y -> y + x

    // val sigH = x:int -> (int -> int)  // Hint: define a nested function
    let sigH x =
        let f y = y + x
        f

    // val sigI = f:(int -> int) -> int
    let sigI f =
        (f 1) + 2

    // val sigJ = x:int -> y:int -> z:int -> int
    let sigJ x y z =
        x + y + z

    // val sigK = f:(int -> int) -> (int -> int)
    let sigK f =
        let inner x = f (x+1) + 2
        inner

    // val sigL = x:int -> f:(int -> int) -> int
    let sigL x f =
        f (x+1) + 2

    // val sigM = f:(int -> int -> int) -> int
    let sigM f =
        (f 1 2) + 3

    //val sigN = f:('a -> int) -> x:'a -> string  // Hint: use sprintf
    let sigN f x =
        let anInt = f x
        sprintf "%i" anInt

    //val sigO = x:int -> ('a -> int)
    let sigO x =
        let innerFn any =
            x+1
        innerFn

    // ==========================
    // Below are four generic signatures.
    // Try to create four functions that are inferred to have these signatures.
    // ==========================

    // val sigW = 'a -> int
    let sigW x = 1

    // val sigX = int -> 'a
    // not possible to implement

    // val sigY = 'a -> 'a
    let sigY x = x

    // val sigZ = 'a -> 'b
    // not possible to implement


