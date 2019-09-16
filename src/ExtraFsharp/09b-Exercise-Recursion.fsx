/// =============================================
/// Recursion exercises
/// =============================================


//----------------------------------------------------------
//  Q. Count down to 0 and then count back up to 3.

let rec countdownThenCountUp  =  // what parameters are needed?
    ??

// test
countdownThenCountUp 3   // any other parameters needed?



//----------------------------------------------------------
// Q. Using list pattern matching, sum the items in a list
// Hide any extra parameters by creating an inner "loop" function

let sum inputList =

    // inner function
    let rec loop aList ?? = // any other parameters needed?
        ??

    loop inputList ?? // any other parameters needed?

// test
[1..10] |> sum