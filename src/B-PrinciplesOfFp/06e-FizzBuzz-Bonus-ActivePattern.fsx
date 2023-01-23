// =======================================
// Active patterns version
// =======================================

// F# also supports "active patterns" where you can pattern match
// on functions. This workshop is not really about F# in detail,
// but it's a nice feature to encapsulate logic cleanly

let (|DivisibleBy|_|) divisor n =
    if (n % divisor) = 0 then
        Some DivisibleBy
    else
        None

// Here's what FizzBuzz looks like using active patterns
let simpleFizzBuzz_v2 n =
    match n with
    | DivisibleBy 3 & DivisibleBy 5 ->
        "FizzBuzz"
    | DivisibleBy 3 ->
        "Fizz"
    | DivisibleBy 5 ->
        "Buzz"
    | _ ->
        string n



