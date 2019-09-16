(*
//----------------------------------------------------------

1) Define a type to represent RomanDigit "I", "V", "X"
   (keep it simple and just use these three for now.
   Do NOT represent a digit by a string!
*)

type RomanDigit = I | V | X

(*
//----------------------------------------------------------

2) Define a RomanNumber type that is a list of RomanDigits

*)

type RomanNumber = RomanDigit list

(*
//----------------------------------------------------------

3) Define a romanNumberToInt function that converts a
   RomanNumber to an int

   Suggested approach:
   a) create a function that converts each RomanDigit to an int
   b) Use List.map to convert the entire list
   c) Use List.sum to sum a list

*)


/// Convert a single digit to a integer
let romanDigitToInt digit =
    // use pattern matching
    match digit with
    | I -> 1
    | V -> 5
    | X -> 10

/// Convert a list of digits to a integer
let romanNumberToInt (romanDigits:RomanNumber)  =
    let ints = romanDigits |> List.map romanDigitToInt
    let sum = ints |> List.sum
    sum

// test
let romanNumber = [X; V; I]
romanNumberToInt romanNumber


(*
//----------------------------------------------------------

4) Define a strToRomanNumber function that converts a
   string to a RomanNumber. (Assume the string only contains
   valid characters)


The algorithm will be:

* uppercase the input
* make the following string replacements:
  - "IV" with "IIII"
  - "IX" with "VIIII"
* Use Seq.map to convert each char to a RomanDigit
* Convert the Seq to a list using Seq.toList

Suggested helper functions:
   a) a function that converts a char to a RomanDigit
      You will need a wildcard case when matching. Use
         | _ -> failwith "should not happen"

   b) a wrapper for `String.ToUpper` that is suitable for piping
   c) a wrapper for `String.Replace` that is suitable for piping

*)

let toUpper (s:string) =
    s.ToUpper()

let replace (find:string) repl (s:string) =
    s.Replace(find,repl)

let charToRomanDigit ch :RomanDigit =
    match ch with
    | 'I' -> I
    | 'V' -> V
    | 'X' -> X
    | _ -> failwith "should not happen"

let strToRomanNumber (str:string) :RomanNumber =
    // toUpper
    // then replace "IV" with "IIII"
    // then convert to roman digits
    // TIP: a str is a sequence of characters, so use Seq.map
    // TIP: to convert a seq to a list, use Seq.toList

    str
    |> toUpper
    |> replace "IV" "IIII"
    |> replace "IX" "VIIII"
    |> Seq.map charToRomanDigit // map each char to a digit
    |> Seq.toList


// test
strToRomanNumber "XXI"
strToRomanNumber "IV"

// chain them together
"XXI" |> strToRomanNumber |> romanNumberToInt
