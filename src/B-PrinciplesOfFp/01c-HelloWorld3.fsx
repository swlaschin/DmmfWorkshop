// ====================================
// F# IS DIFFERENT
// ====================================

// ====================================
// F# does not allow implicit casting
// ====================================

1 + 1.5
1 + int 1.5
float 1 + 1.5

1 + "2"
1 + int "2"
string 1 + "2"

// ====================================
// mutability
// ====================================

let x = 10
x = 11        // this is wrong
// x <- 11    // this is correct

// ====================================
// F# gotchas
// ====================================

// Equality	is "=" not "=="
// 1==2   // C-style syntax
// 1=2    // F# syntax

// Inequality is "<>" not "!="
// 1 != 2     // C-style syntax
// 1 <> 2     // F# syntax

// Negation	is "not" not "!"
// !(1==2)    // C-style syntax
// not (1=2)  // F# syntax

// First assignment is "let"
// var x = 1;    // C# syntax
// let x = 1     // F# syntax

// Mutation is "<-"
// x = 2;    // C-style syntax
// x <- 2    // F# syntax

// Function parameter separator is space not comma
// int f(int x,int y) {...}   // C-style syntax to define a function
// let f x y = ...            // F# syntax to define a function

// f(x,y);   // C-style syntax to call a function
// f x y     // F# syntax to call a function

// List separator is semicolon not comma
// [ 1; 2; 3 ]              // F# syntax for a new list
// { name="Scott"; age=27}  // F# syntax for a new record

// Tuples use a comma!
// let x = (2,3)   // construct
// let (y,z) = x   // deconstruct

// Colon is normally something to do with types
// val x:int = 1
//      ^---type
// type MyRecord = {x:int}
//                   ^---type

// Curly braces	are NOT used for blocks
let x =
   let y = 1
   y + 1

// Curly braces	ARE used for records
// { name: string; age:int} // F# syntax for a record definition
// { name="Scott"; age=27}  // F# syntax for a record constructor


// ====================================
// Modules
// ====================================

/// this is a module, used to group code together
module MyModule =

    let add2 x =
        x + 2   // no "return" keyword. Last expression is returned

    let add3 x =
        x + 3   // no "return" keyword. Last expression is returned

// access the code in a module with the name
MyModule.add2 40  // Result => 42

// Without the module prefix, the functions are not in scope
add2 40   // error

// But you can bring the entire module
// into scope with "open"
open MyModule  // "open" is same as "using" in C#
add2 40
add3 30

