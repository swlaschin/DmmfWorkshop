/// =============================================
/// Expression exercises
/// =============================================


//----------------------------------------------------------
//  Q. Define a function that prints "hello" if the input is
//  greater than 0, otherwise it prints "goodbye"
//  * Use if/then/else
//  * Use only ONE call to print!

let printHello1 x =
    let strToPrint =
        if x > 0 then
            "hello"
        else
            "goodbye"
    printfn "%s" strToPrint




//----------------------------------------------------------
//  Q. Define a function that prints "hello" if the input is
//  2 or 4, otherwise it prints "goodbye".
//  * Use pattern matching.
//  * Use only ONE call to print!
let printHello2 x =
    let strToPrint =
        match x with
        | 2 -> "hello"
        | 4 -> "hello"
        | _ -> "goodbye"
    printfn "%s" strToPrint


//----------------------------------------------------------
//  Q. Define a function that prints the name of the colour
//  * Use pattern matching.
//  * Use only ONE call to print!

type Colour = Red | Green | Blue

let printColour colour =
    let strToPrint =
        match colour with
        | Red -> "Red"
        | Green -> "Green"
        | Blue -> "Blue"
    printfn "%s" strToPrint

// Did you need to specify the type of the parameter?

// How many cases did you need to pattern match?

// Now add a new colour and recompile your code. What happens?