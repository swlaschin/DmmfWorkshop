
// ================================================
// Syntax for Functions 
// ================================================

// The "let" keyword also defines a named function.
// Note that no parentheses are used!
// Try running this:
let square x = x * x          

// pay attention to the output of the interactive window!
// val square : x:int -> int     
//
// format is always "val" [name] ":" [type] 
// where type is something like "int -> int" with an arrow in it.
//  "int -> int" means the function takes an int as input and returns an int as output

// Try running this:
square 3                      // Now run the function. Again, no parens.

// pay attention to the output of the interactive window!
// val it : int = 9
// This result is NOT a function

// A two parameter function is defined in a similar way
// Try running this:
let add x y = x + y           // don't use add (x,y)! It means something
                              // completely different.

// pay attention to the output of the interactive window!
// val add : x:int -> y:int -> int
//  "int -> int -> int" means the function takes an int as input and another int as input and returns an int as output

// Try running this:
add 2 3                       // Now run the function.



// to define a multiline function, just use indents. No semicolons needed.
// Try running this:
let evens list =
   let isEven x = x%2 = 0     // Define "isEven" as a sub function
   List.filter isEven list    // List.filter is a library function
                              // with two parameters: a boolean function
                              // and a list to work on

// pay attention to the output of the interactive window!
// val evens : list:int list -> int list
//  "int list -> int list " means the function takes a list of ints as input and returns a list of ints as output

// Try running this:
evens oneToFive               // Now run the function


// Important: a function is not run unless you pass a parameter.
// otherwise it is just a value, like an int or string

// Run the line below and pay attention to the output of the interactive window.
// Do you get a simple value?
square 

// Run the line below and pay attention to the output of the interactive window
// Do you get a simple value?
square 3



// ================================================
// Returning from a function
// ================================================

// IMPORTANT: In F# there is no "return" keyword. A function always
// returns the value of the last expression used.
let doSomething y =
   printfn "y is %i" y
   2+2

// Anything other than the last line must NOT return a value
// you must use "ignore" in these cases

// Try running this:
let functionWithoutIgnore x = 
    let y = 1
    2 + 2   // error. "This expression should have type 'unit', but has type 'int'"
    x + y

// Try running this:
let functionWithIgnore x = 
    let y = 1
    ignore (2 + 2)  // ok
    x + y


// Question - why does this function compile without using ignore?
let functionWithPrint x = 
    let y = 1
    printfn "%i" (2 + 2)
    x + y


// Predict what the signature of the following function will be. Then run it to find out!
// Tip: what is the type of the input? What is the type of the output?
let functionSig1 x = 
    x + 1

//let functionSig2 x = 
//    printfn "x is %f" x
//    x
//


// ================================================
// Part 4 - Introducting "piping"
// ================================================

// You can use parentheses in the normal way to clarify precedence. In this example,
// do "add1" first, then do "times2" on the result.
let add1ThenMultiply2 y =
   let add1 x = x + 1
   let times2 x = x * 2
   times2 (add1 y)   // do "add1" first, then do "times2" 

// test
// add1ThenMultiply2 4

// BUT in F# it is more idiomatic to "pipe" the output of one operation to the next using "|>"
// Piping data around is very common in F#, similar to UNIX pipes.
let add1ThenMultiply2Piped y =
   let add1 x = x + 1
   let times2 x = x * 2
   y |> add1 |> times2   // y is passed to add1 and the output of that passed to times2   

// test
// add1ThenMultiply2Piped 4

// another example
let someMorePiping =
   let add1 x = x + 1
   let times2 x = x * 2
   let square x = x * x
   let dividedBy2 x = x / 2

   4
   |> add1 
   |> times2   
   |> square
   |> dividedBy2 




// Existing .NET library methods can be wrapped to make them suitable for piping

let replace (oldStr:string) newStr (str:string) =
    str.Replace(oldStr,newStr)
     
let startsWith (pattern:string) (str:string) =
    str.StartsWith(pattern)

// with these in place, we can do nice things like this:
"hello" |> replace "h" "j"

"hello" |> replace "h" "j" |> startsWith "jell" 

// -----------------------
// IMPORTANT - if there is more than one parameter, the piped parameter is always the LAST one!

// so
"hello" |> replace "h" "j" 
// is the same as 
replace "h" "j" "hello"


// Another example:
let add x y = x+y 

add 4 5
// is the same as 
5 |> add 4 


// Exercise: Think of a number function
//   think of a number
//   add 1
//   square it
//   subtract 1
//   divide by the number you first thought of
//   subtract the number you first thought of
//   the answer is 2!
//
// Challenge, write this using a piping model.
//   use the code below as a starting point 

let thinkOfANumber numberYouThoughtOf =
    let squareIt x = x * x
    let add1 x = x + 1
    let subtract1 x = x - 1
    
    numberYouThoughtOf
    |> add1
    |> squareIt
    |> subtract1 
    // |> what comes here?

// Exercise: given this function:
let subtract a b = a - b

// predict the result of    1 |> subtract 2   
// predict the result of    2 |> subtract 1   

// REMEMBER if there is more than one parameter, the piped parameter is always the LAST one!
// Should you rename the function to make it more sensible when used with piping?


// ================================================
// Part 9 - Lambdas
// ================================================

// A "lambda" is an "anonymous function" or "inline function".

// In C# it is written with a double arrow like this:
//      aValue => aValue + 1

// In F# it is written with a "fun" keyword and a *single" arrow like this:
//      fun aValue -> aValue + 1

// A simple example
4 |> (fun x -> x + 1)    // try running this

// lambdas are often used with list functions
[1..10] |> List.map (fun x -> x*x)   // try running this

// lambdas can be assigned to values and used like a normal function

// "add1" version 1
let add1 x = x+1
4 |> add1

// "add1" version 2
4 |> fun x -> x+1

// "add1" version 3
let add1_lambda = fun x -> x+1
4 |> add1_lambda

// "map square" version 1
let square x = x*x
[1..10] |> List.map square 

// "map square" version 2
[1..10] |> List.map (fun x -> x*x)

// "map square" version 3
let square_lambda = fun x -> x*x
[1..10] |> List.map square_lambda


// Exercise - what is the difference between these three definitions?
let add_v1 = 
    fun x y -> x + y

let add_v2 x = 
    fun y -> x + y

let add_v3 x y = 
    x + y

// try running each one of them 
add_v1 2 3
add_v2 2 3
add_v3 2 3