﻿// =============================================
// Convert a number to Roman numerals
// =============================================

(*

Use the "tally" system:

* Start with N copies of "I".
  Eg. The number 8 becomes "IIIIIIII"
* Replace "IIIII" with "V"
* Replace "VV" with "X"
* Replace "XXXXX"  with "L"
* Replace "LL" with "C"
* Replace "CCCCC"  with "D"
* Replace "DD" with "M"

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
    let replace_VV_X str = replace ???

    String.replicate number "I"
    |> ???
    |> ???
    |> ???


// test it
toRomanNumerals 12
toRomanNumerals 14
toRomanNumerals 1947

// test it on all the numbers up to 30
[1..30] |> List.map toRomanNumerals |> String.concat ","


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
    |> ???


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

(*
let toRomanNumerals_v3 number =
   // add IV
   // add IX
   // TIP: additional special forms should be
   // done highest to lowest
*)

(*
// test it
toRomanNumerals_v3 4
toRomanNumerals_v3 14
toRomanNumerals_v3 19
toRomanNumerals_v3 1947
toRomanNumerals_v3 1999
*)


// ======================================
// What about logging?
//
// Exercise IV:
// * Add a logging step at the end of the pipeline
// ======================================

let toRomanNumerals_v4 number =
    // helper function
    let logger output =
        // print "The input was %i and the output was %s"
        // anything else?

    String.replicate number "I"
    |> replace "IIIII" "V"
    |> ???
    |> ???
    // last step
    |> logger
