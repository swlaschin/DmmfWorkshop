
// --------------------------
//  Q. For each of the following definitions,
//  say whether it is a simple value or a function,
//  and if a function, what is the function signature?
//  If in doubt, run them to find out!

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

// --------------
//  Q. For each of the following signatures, create a function that will be inferred to have that signature.
//  Avoid using explicit type annotations!

// val sigA = int -> int

// val sigB = int -> unit

// val sigC = int -> string

// val sigD = unit -> string

// val sigE = string -> string

// val sigF = int -> bool -> float -> string


// --------------
// Q. Make the following functions compile by adding type annotations.
// Try to use the minimum annotations possible!

let trim s = s.Trim()        // s is string

let split ch s =  s.Split(ch)   // s is string, ch is a char


// --------------
// Q. Below are four generic signatures.
// Try to create four functions that are inferred to have these signatures.

// val sigW = 'a -> int

// val sigX = int -> 'a

// val sigY = 'a -> 'a

// val sigZ = 'a -> 'b
