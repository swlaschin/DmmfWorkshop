// =================================
// Type inference
// =================================

// example from the slides
let doSomething f x =
   let y = f (x + 1)
   "hello" + y

// val doSomething : f:(int -> string) -> x:int -> string


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
let toUpper x =
    x.ToUpper()
    // => error FS0072: Lookup on object of indeterminate type
*)

let toUpper (x:string) =
    x.ToUpper()

let aFunction (param1:string) (param2:bool) :string =
    // etc
    "" // dummy

(*
let toUpper x :string = ...
*)

// no type annotations
let helloInt_v1 anInt =
    sprintf "Hello %i" anInt

// annotation on parameter only
let helloInt_v2 (anInt:int) =
    sprintf "Hello %i" anInt

// annotation on return value only
let helloInt_v3 anInt :string =
    sprintf "Hello %i" anInt

// annotation on parameter and return value
let helloInt_v4 (anInt:int) :string =
    sprintf "Hello %i" anInt

// ====================================
// Functions with generic parameters
// ====================================

let returnSameThing x = x

(*
T returnSameThing<T>(T x) {
  return X
  }
*)
(*
val returnSameThing : x:'a -> 'a
*)

let ignoreTheInput x = ()
let ignoreTwoInputs x y = ()
let ignoreThreeInputs x y z = ()

(*
val ignoreTheInput : x:'a -> unit
val ignoreTwoInputs : x:'a -> y:'b -> unit
val ignoreThreeInputs : x:'a -> y:'b -> z:'c -> unit
*)

