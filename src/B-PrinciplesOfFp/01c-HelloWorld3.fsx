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
// Modules
// ====================================

/// this is a module, used to group code together
module MyModule =

    let add2 x =
        x + 2   // no "return" keyword. Last expression is returned

// access the code in a module with the name
MyModule.add2 40  // Result => 42

// alternatively, bring the entire module 
// into scope with "open"
open MyModule  // "open" is same as "using" in C#
add2 40

