/// =============================================
/// Convert a number to Roman
/// =============================================

(*

Use the "tally" system:

* start with n "I"
* replace "IIIII" with "V"
* replace "VV" with "X"
* replace "XXXXX"  with "L"
* replace "LL" with "C"
* replace "CCCCC"  with "D"
* replace "DD" with "M"

Challenge, write this using a piping model with partial application.
Use the code below as a starting point

For extra points, handle IV, IX, XC, etc.

*)

/// Convert the built-in .NET library method
/// to a pipeable function
/// (automatic currying)
let replace oldValue newValue (inputStr:string) =
    inputStr.Replace( oldValue=oldValue, newValue=newValue)


// uncomment this code to start
let toRomanNumerals number =
    let replace_IIIII_V = replace ???
    let replace_VV_X = replace ???

    String.replicate number "I"
    |> ???
    |> ???
    |> ???


// test it
toRomanNumerals 12
toRomanNumerals 14
toRomanNumerals 1947


// Inline version
let toRomanNumerals_v2 number =
    String.replicate number "I"
    |> ???
    |> ???


// test it
toRomanNumerals_v2 12
toRomanNumerals_v2 14
toRomanNumerals_v2 1947

