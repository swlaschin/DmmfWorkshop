/// =============================================
/// Stack based calculator
/// =============================================

(*

Write a stack based calculator

I have included functions for "pop" "push" and "dup", EMPTY amd PRINT

1) Write functions for pushing 1,2,3 on the stack -- call them ONE, TWO, THREE
2) Write functions for adding and multiplying the top two items on the stack -- call them ADD, TIMES
3) Write functions for doubling and squaring the top item on the stack -- call them DOUBLE and SQUARE - use composition to create them

I should then be able to run this code

EMPTY
|> THREE
|> TWO
|> ONE
|> ADD
|> PRINT
|> TIMES
|> PRINT
|> DOUBLE
|> PRINT
|> SQUARE
|> PRINT

Should print
    Top=3
    Top=9
    Top=18
    Top=324

TIP:

(x,y) is a tuple

to unpack a tuple
let x,y = myTuple

*)

type Stack = Stack of int list

/// return a new stack with the new value on top
let push x (Stack xs) =
    Stack (x::xs)

/// return top value and new stack as a tuple
let pop (Stack s) =
    match s with
    | [] -> failwith "Stack underflow"
    | x::xs -> (x, Stack xs)  // return top value and new stack as a tuple

/// duplicate the top value
let dup (Stack s) =
    match s with
    | [] -> Stack []
    | x::xs -> Stack (x::x::xs)

/// define an empty stack
let EMPTY = Stack []

/// print the top element and return the original stack
let PRINT (Stack s) =
    match s with
    | [] -> printfn "(empty)"
    | x::xs -> printfn "Top=%i" x
    (Stack s) // return the original stack


/// Add the top two elements
let ADD stack =
    // pop the top of the stack
    ??
    // pop the top of the stack again
    ??
    // add the two numbers
    ??
    // push the result back on the stack
    ??

    stack

/// Multiply the top two elements
let TIMES stack =
    //??
    stack

/// Push 1 on the stack
let ONE stack =
    //??
    stack

/// Push 2 on the stack
let TWO stack =
    //??
    stack

/// Push 3 on the stack
let THREE stack =
    //??
    stack

/// Push 4 on the stack
let FOUR stack =
    //??
    stack


/// Double the top element
let DOUBLE =
    // Implement by composing functions we've already defined.
    ??

/// Square the top element
let SQUARE =
    // Implement by composing functions we've already defined.
    ??

// test
EMPTY
|> THREE
|> TWO
|> ONE
|> ADD
|> PRINT
|> TIMES
|> PRINT
|> DOUBLE
|> PRINT
|> SQUARE
|> PRINT
