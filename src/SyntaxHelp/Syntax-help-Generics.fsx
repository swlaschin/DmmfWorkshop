

// ================================================
// Part 8. Special Types - Automatic generalization
// ================================================

// What is the type for this?
let same x = x

// It works for *any* type, so F# compiler *automatically* generalizes it to a generic type.
// Generic types have a letter and a tick in front, like 'a and 'b.
//
// The C# equivalent would be 
//    T Same<T>(T x) { return x; }

// So what is the type for?
let makeTuple x y = (x,y)

// And what is the type for this?
List.length


// Exercise 8a - predict the type of this function
let example8a x = (x,x)

// Exercise 8b - predict the type of this function
let example8b x = [x]

// Exercise 8c - predict the type of this function
let example8c x y = (y,x)

// Exercise 8d - predict the type of this function
let example8d x y = (y,x+1)

// Exercise 8e - predict the type of this function
let example8e f x = 
    f x

// Exercise 8f - predict the type of this function
let example8f f x = 
    x |> f  |> f
