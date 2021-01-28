// =================================
// Type inference examples
//
// Exercise: Execute each chunk of code in this file and
// and make you understand how it works.
// =================================

// example from the slides
let doSomething f x =
   let y = f (x + 1)
   "hello" + y

// val doSomething : f:(int -> string) -> x:int -> string


// =================================
// Example 1 - type inference
let printName aName =
    printfn "Hello %s" aName

// test
let name = "Alice"
printName name

// val printName : string -> unit

(*
This means that the printName function takes a string as input
and generates a "unit" as output. For now, you can think of the unit type as somewhat like void.
It means that there is no useful output.
*)

let returnHello() =    // () means no input
    "Hello"

// val returnHello : unit -> string


// =================================
// Example 2 - defining a function with an int parameter
//              causes type errors
let printName2 aName =
    printfn "Hello %i" aName

// test -- uncomment below
(*
let name = "Alice"
printName2 name
*)

//    val printName2 : int -> unit
//    error FS0001: The type 'string' is not compatible with [an integer type]




// =================================
// Example 3 - defining a function with two int parameters
let printIntAndString anInt aStr =
    printfn "int=%i str=%s" anInt aStr

// test
printIntAndString 1 "hello"

(*
val printIntAndString : x:int -> y:string -> unit
*)

(*
printIntAndString 1 2
// -----------------^--
// error FS0001: This expression was expected to have type "string"
//               but here has type "int"
*)

// ====================================
// Helping the compiler with "type annotations"
// ====================================

(*
//TODO -- uncomment this to see the error
let toUpper x =
    x.ToUpper()
    // => error FS0072: Lookup on object of indeterminate type
*)

// the same definition but with a type annotation. Now the compiler is happy.
let toUpper (x:string) =
    x.ToUpper()


// Here's how to do type annotations
let aFunction (param1:string) (param2:bool) :string =
//             ^1st param      ^2nd param    ^return type
    // etc
    "" // dummy

(*
let toUpper x :string = ...
*)

// You can have type annotations or not.
// Here's some different ways of writing the same function...

// version 1: no type annotations
let helloInt_v1 anInt =
    sprintf "Hello %i" anInt

// version 2: annotation on parameter only
let helloInt_v2 (anInt:int) =
    sprintf "Hello %i" anInt

// version 3: annotation on return value only
let helloInt_v3 anInt :string =
    sprintf "Hello %i" anInt

// version 4: annotation on parameter and return value
let helloInt_v4 (anInt:int) :string =
    sprintf "Hello %i" anInt

// type annotations are useful when you are getting started,
// or if the compiler complains

// ====================================
// Functions with generic parameters
// ====================================


let returnSameThing x = x

(*
// the type signature is generic, and so F# infers
// a generic 'a rather than a specific type like int or string
val returnSameThing : x:'a -> 'a
*)

(*
// NOTE in C# this would be written as...
T returnSameThing<T>(T x) {
  return x;
  }
*)

// A useful example of generics is to swap a tuple
let swap (x,y) = (y,x)
// val swap : 'a * 'b -> 'b * 'a

// If the values are compared, they must BOTH be the same type
let compare x y = (x=y)
// val compare : 'a -> 'a -> bool


// The unit type is the empty value, like void.
// It is written as ()

// these functions ignore their input, and so the input is inferred to be generic!
let ignoreTheInput x = ()
let ignoreTwoInputs x y = ()
let ignoreThreeInputs x y z = ()

(*
val ignoreTheInput : x:'a -> unit
val ignoreTwoInputs : x:'a -> y:'b -> unit
val ignoreThreeInputs : x:'a -> y:'b -> z:'c -> unit
*)

