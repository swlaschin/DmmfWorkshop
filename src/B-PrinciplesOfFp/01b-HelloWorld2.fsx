// ======================================
// The Basics of F#
// ======================================


// ======================================
// Common literal values
// ======================================

"hello"  // string
42       // int
3.141    // float
3.0M     // decimal
true     // bool
[1;2;3]  // list (immutable)
()       // unit (like void)

// a one line comment
(* a multi line comment *)
//    /* not a multi line comment */

// ======================================
// Playing around with printfn
// ======================================

// C# equivalent to printfn is Console.WriteLine
// Java: System.Out
// Kotlin: printLn

printfn "%s" "hello"  // %s string
printfn "%i" 42       // %i int

printfn "%f" 3.15   // %f float
printfn "%g" 3.15   // %g float
printfn "%0.1f" 3.15   //with formatting
printfn "%0.9f" 3.15   //with formatting

printfn "%b" false    // %b bool
printfn "%A" [1..3]   // %A anything
printfn "%s is %i years old" "Alice" 42

// define a function that prints squares
// up to N
let printSquares n =
   for i in [1..n] do
      let sq = i*i
      printfn "%i" sq

// call it with 5
printSquares 5

// type inference!
// val printSquares : n:int -> unit

// ======================================
// sprintf is like printfn except that it returns a string
// ======================================

// C# equivalent to sprintf is String.Format

let x = sprintf "%i" 42     // x = "42"
let y = sprintf "%f" 3.15   // y = "3.150000"


