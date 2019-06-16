// ================================================
// Tuples
//
// A tuple is as a pair, a triplet, etc.
//
// ================================================

// Tuples are defined with a multiplication symbol
type IntAndInt = int * int
type IntAndString = int * string

// Tuples are a *single* value, created by combining two (or three) other values
// using a comma.
let myTuple = 1,2         // try running this

// The components of the tuple can be different types
let intAndStringTuple = 1,"hello"         // try running this
let intAndBoolTuple = 1,false             // try running this
let intAndStringAndBoolTuple = 1,"hello",true         // try running this


// pay attention to the output of the interactive window!
// Tuple types have a "*" between each type, as if the types were being "multiplied" together!

//   val myTuple : int * int = (1, 2)
//   val intAndStringTuple : int * string = (1, "hello")
//   val intAndBoolTuple : int * bool = (1, false)
//   val intAndStringAndBoolTuple : int * string * bool = (1, "hello", true)

// Tuples can be deconstructed in the same way that they are constructed:

let tuple1 = 1,"hello"         // try running this
let x1,y1 =  tuple1            // run this to deconstruct the tuple

let tuple2 = 1,false,"hello"     // try running this
let x2,y2,z2 =  tuple2           // run this to deconstruct the tuple

// You can ignore values you don't care about with "_"
let tuple3 = "hello",42         // try running this
let _,theAnswer = tuple3        // run this to deconstruct the tuple

// You can't mix tuples with different sizes and different types,
// as you will get a compiler error!

let tuple4 = 1,false     // try running this
let x4,y4,z4 =  tuple4   // run this to deconstruct the tuple

// If tuples have the same size and types, then equality is defined automatically
let tuple5a = 1,"hello"         
let tuple5b = 1,"hello"         
let isTuple5bEqual = (tuple5a = tuple5b)

let tuple5c = 1,"goodbye"         
let isTuple5cEqual = (tuple5a = tuple5c)

// But again, you can't mix tuples with different sizes and different types,
// as you will get a compiler error!
let tuple5d = 2,42
let isTuple5dEqual = (tuple5a = tuple5d)

let tuple5e = 1,"hello",42         
let isTuple5eEqual = (tuple5a = tuple5e)

// IMPORTANT tuples are one value not two
// Look at the signature of this function:
let addTuple (x,y) = x + y
// it is:
//    val addTuple : x:int * y:int -> int
// Note that it has only *one* parameter!
// The input is of type "int * int"
// The output is of type "int"

