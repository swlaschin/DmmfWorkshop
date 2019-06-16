(*
The syntax of F# is different from C-style languages in TWO key ways:

1) Curly braces are not used to delimit blocks of code. Instead, indentation is used (like Python).
2) Whitespace is used to separate parameters rather than commas.

Other tips:

* "let" is used instead of "var"
* "let" is also used for defining functions
* "type" is used instead of "class", "enum", etc
* In non-parameter usage, commas are replaced by semicolons in most places. 
*)


// ================================================
// Comments
// ================================================


// single line comments use a double slash
(* multi line comments use (* . . . *) pair

-end of multi line comment- *)



// ================================================
// Syntax for simple values
// ================================================

// The "let" keyword defines an (immutable) value
// Think of "let" being used everywhere you would use "var" in C#

// try running this
let myInt = 5
let myFloat = 3.14
let myString = "hello"           //note that no types needed
let myBool = true

// pay attention to the output of the interactive window!
// val myInt : int = 5
// val myFloat : float = 3.14
// val myString : string = "hello"
// val myBool : bool = true
//
// the format is always "val" [name] ":" [type] "=" [value]


// now try running both these lines at once -- what happens?
let y=2
y=3

// the answer is that "=" operator in a "let" is not assignment but "binding" -- connecting a name with a value.
// the first time, in "let y=2", y is unknown and is "bound" to "2"
// the second time, in "y=3", y is known and is compared to "3", giving the answer "false"




// ================================================
// Printing 
// ================================================

// The printf/printfn functions are similar to the
// Console.Write/WriteLine functions in C#.

// Try running this:
printfn "Printing an int %i, a float %f, a bool %b" 1 2.0 true

// Try running this:
printfn "A string %s, and an F# native structure like a list %A" "hello" [1;2;3;4]

// There are also sprintf/sprintfn functions for formatting data
// into a string, similar to String.Format in C#.

// Try running this:
let msg = sprintf "The message is %s" "hello"


// ================================================
// Part 2 - Welcome to the world of F# compiler errors
// ================================================

// F# has a lot more compiler errors than you might be used to.
// It's important that you don't get frustrated with them, so it helps to see the common ones!

// IMPORTANT ints and floats are not compatible. Also bytes, shorts, etc.
let addIntAndFloat = add 1 1.0

// "int" is a cast that can be used to fix this
let addIntAndFloat2 = add 1 (int 1.0)

// "float" is a another cast 
let addIntAndFloat3 = (float 1) + 1.0

// "string" is a another cast 
let addIntAndString = (string 1) + "hello"


// Will this function compile? If not, then why not? 
// Can you fix it so that it does compile?

//let function2a x = 
//    printfn "x is %f" x
//    x + 1

// will this function compile? If not, then why not? 
// Can you fix it so that it does compile?
//let function2b x y = 
//    printfn "x is %s" x
//    printfn "y is %i" y
//    x + y


// Indentation errors are another common error


(*
// uncomment and fix this code to make it compile
let functionWithIndentationError x = 
    let y = 1
     let z = 2    
    x + y + z
*)

// Forgetting to have a return value is also a common error.

(*
// uncomment and fix this code to make it compile
let functionWithNoReturn x = 
    let y = 1
    let z = 2
*)


// ================================================
// Type annotations
// ================================================

// When dealing with OO code, such as the .NET libraries,
// F# often cannot know the type it is dealing with, you will need to help it out.

// Run this function definition. It fails with "Lookup on object of indeterminate type"
// because the compiler does not know what type "s" is!
(*
let getLength s = s.Length
*)

// You can add "type annotations" in the form "(param:type)"
// to help the compiler 
//
// IMPORTANT: The type annotations are "backwards" compared to C#. 
// Rather than "string name", the parameter is declared as "name:string"

let getStrLength (s:string) = s.Length

// Now that the compiler knows that "s" is a string, it can look up the "Length" method and know that it returns an int.
// So the output of the interactive window says:
//   val getStrLength : s:string -> int


// You can't use OO-style polymorphism in functional programming.

// Functions with different types must have different names!
// So a function that works on arrays has to be named differently.
let getArrayLength (s:int[]) = s.Length

// The output of the interactive window now says
//    val getArrayLength : s:int[] -> int

// To annotate the *return* type of the function, rather than a parameter,
// put the annotation after all the parameter, and not in parentheses, like this:

let addTwoFloats f1 f2 :float = f1 + f2

// The return type can affect the types that are inferred for the parameters.
// What types are "s1" and "s2" going to be now?
let addTwoStrings s1 s2 :string = s1 + s2


// Exercise -- create a wrapper function for StreamReader. 
// What is the minimum annotation you need?
(*
let openFile path = new System.IO.StreamReader(path)
*)









// ================================================
// Everything is an expression
// ================================================

// There are no "statements" in F#. Everything is an expression that can be assigned to a value.

let printExpression = (printfn "hello")

let functionExpression = (fun x -> x+1)

let ifExpression = if true then 1 else 2

let tryCatchExpression x = 
    try
        100/x
    with
    | ex -> 0


// For expressions that have multiple branches, all branches must return the *same* type

// In this example, the "then" branch returns a int but the else branch returns a string,
// which causes a compiler error.
let badIfExpression = 
    if true then 
        1 
    else 
        "hello"

// In this example, the "when" branch returns a unit but the main branch returns a int
// which causes a compiler error.
let badTryCatchExpression x = 
    try
        100/x
    with
    | ex -> printfn "error"



// ================================================
// Pattern Matching 
// ================================================

// Match..with.. is a supercharged case/switch statement.
let stringPatternMatch =
   let x = "a"
   match x with
    | "a" -> printfn "x is a"
    | "b" -> printfn "x is b"
    | _ -> printfn "x is something else"   // underscore matches anything

let intPatternMatch =
   let x = 1
   match x with
    | 1 -> printfn "x is 1"
    | 2 -> printfn "x is 2"
    | _ -> printfn "x is something else"   // underscore matches anything

// each case looks a bit like a lambda, with an arrow after the pattern is matched
//  | [choice] -> action

let tuplePatternMatch =
   let x = (1,2)
   match x with
    | 1,1 -> printfn "x is 1,1"
    | 2,_ -> printfn "the first part of x is 2"
    | _,2 -> printfn "the second part of x is 2"
    | _ -> printfn "x is something else"   // underscore matches anything


// Exercise - what happens if you leave off the "_" case in "intPatternMatch" above?

// Exercise  - what happens if you put the "_" case FIRST in the list in "intPatternMatch" above?



// ================================================
// Organizing code with modules
// ================================================

// to keep a group of types and functions together
// you can put them in a "module"

module MyFirstModule =
   
    type MyType = MyType of string
    let myFunction() = 42


module MySecondModule =

    // to access code in another module, use a fully qualified name
    let result = MyFirstModule.myFunction() 

    // this works for .NET library as well
    let path = System.IO.Path.Combine("a","b")


module MyThirdModule =

    // to bring code from another module  into scope, use "open"
    open MyFirstModule  
    let result = myFunction() 

    // of
    open System.IO
    let path = Path.Combine("a","b")

