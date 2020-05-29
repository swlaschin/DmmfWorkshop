// =============================================
// Convert a number to Roman numerals
// =============================================

(*

Use the "tally" system:

* start with n "I"
* replace "IIIII" with "V"
* replace "VV" with "X"
* replace "XXXXX"  with "L"
* replace "LL" with "C"
* replace "CCCCC"  with "D"
* replace "DD" with "M"
*)


// =============================================
// Exercise I:
//
// Implement this logic using a piping model.
// Use the code below as a starting point
// =============================================

/// Helper to convert the built-in .NET library method
/// to a pipeable function
let replace (oldValue:string) (newValue:string) (inputStr:string) =
    inputStr.Replace(oldValue, newValue)

let toRomanNumerals number =
    let replace_IIIII_V str = replace "IIIII" "V" str
    let replace_VV_X str = replace "VV" "X" str
    let replace_XXXXX_L str = replace "XXXXX" "L" str
    let replace_LL_C str = replace "LL" "C" str
    let replace_CCCCC_D str = replace "CCCCC" "D" str
    let replace_DD_M str = replace "DD" "M" str

    String.replicate number "I"
    |> replace_IIIII_V
    |> replace_VV_X
    |> replace_XXXXX_L
    |> replace_LL_C
    |> replace_CCCCC_D
    |> replace_DD_M

// test it
toRomanNumerals 12
toRomanNumerals 14
toRomanNumerals 1947

// =============================================
// The replace function can also be used "inline".
// To do this, pass the first two parameters explicitly,
// and the last parameter will be passed implicitly via the pipe
//
// The advantage of this approach is that you don't need to
// define all the helper functions.
//
// Exercise II:
// * Rewrite the code to use "replace" directly, without helper functions
//
// =============================================


// Inline version
let toRomanNumerals_v2 number =
    String.replicate number "I"
    |> replace "IIIII" "V"
    |> replace "VV" "X"
    |> replace "XXXXX" "L"
    |> replace "LL" "C"
    |> replace "CCCCC" "D"
    |> replace "DD" "M"

// test it
toRomanNumerals_v2 12
toRomanNumerals_v2 14
toRomanNumerals_v2 1947



// ======================================
// What about the special forms IV,IX,XC etc?
//
// Exercise III:
// * Add these as additional transforms at the end of the pipe
//   Just replace "IIII" with "IV", etc
// ======================================

let toRomanNumerals_v3 number =
    String.replicate number "I"
    |> replace "IIIII" "V"
    |> replace "VV" "X"
    |> replace "XXXXX" "L"
    |> replace "LL" "C"
    |> replace "CCCCC" "D"
    |> replace "DD" "M"
    // TIP: additional special forms should be
    // done highest to lowest
    |> replace "DCCCC" "CM"
    |> replace "CCCC" "CD"
    |> replace "LXXXX" "XC"
    |> replace "XXXX" "XL"
    |> replace "VIIII" "IX"
    |> replace "IIII" "IV"

// test it
toRomanNumerals_v3 4
toRomanNumerals_v3 14
toRomanNumerals_v3 19


// ======================================
// What about logging?
//
// Exercise IV:
// * Add a logging step at the end of the pipeline
// ======================================

let toRomanNumerals_v4 number =
    let logger output =
        printfn "The input was %i and the output was %s" number output
        output  // return the output

    String.replicate number "I"
    |> replace "IIIII" "V"
    |> replace "VV" "X"
    |> replace "XXXXX" "L"
    |> replace "LL" "C"
    |> replace "CCCCC" "D"
    |> replace "DD" "M"
    // TIP: additional special forms should be
    // done highest to lowest
    |> replace "DCCCC" "CM"
    |> replace "CCCC" "CD"
    |> replace "LXXXX" "XC"
    |> replace "XXXX" "XL"
    |> replace "VIIII" "IX"
    |> replace "IIII" "IV"
    |> logger




// test it
toRomanNumerals_v4 4
toRomanNumerals_v4 14
toRomanNumerals_v4 19
