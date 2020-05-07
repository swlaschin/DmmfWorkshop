 s
(*
//----------------------------------------------------------

1) Define a type to represent RomanDigit I, V, X
   (keep it simple and just use these three for now.
   Do NOT represent a digit by a string!
*)

type RomanDigit = ??

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
    ??

/// Convert a list of digits to a integer
let romanNumberToInt (romanDigits:RomanNumber) =
    ??

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
* Wrap the list in the RomanNumber type

Suggested helper functions:
   a) a function that converts a char to a RomanDigit
      You will need a wildcard case when matching. Use
         | _ -> failwith "should not happen"

   b) a wrapper for `String.ToUpper` that is suitable for piping
   c) a wrapper for `String.Replace` that is suitable for piping

*)

let toUpper (s:string) =
    s.ToUpper()

let charToRomanDigit ch :RomanDigit =
    // pattern match on the char
    ??

let strToRomanNumber (str:string) :RomanNumber =
    // toUpper
    // then replace "IV" with "IIII"
    // then convert to roman digits
    // TIP: a str is a sequence of characters, so use Seq.map
    // TIP: to convert a seq to a list, use Seq.toList
    ??

// test
strToRomanNumber "XXI"
strToRomanNumber "IV"


// chain them together
"XXI" |> strToRomanNumber |> romanNumberToInt
