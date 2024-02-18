// ================================================
// Example: Combine validation functions using map2/map3 etc
// ================================================

// Load a file with library functions for Validation
#load "Validation.fsx"  // TIP evaluate this import separately before evaluating the rest of the code below

// =============================================
// The domain
// =============================================

/// Define types for this domain
module Domain =

    // String10 must be not empty AND len < 10
    type String10 =  private String10 of string

    type PersonalName = {
        First: String10
        Last: String10
        }

    module String10 =
        // pass in a field name so that we know which field had the error
        let create fieldName s =
            if System.String.IsNullOrEmpty(s) then
                Error (sprintf "%s is null or empty" fieldName)
            else if (s.Length > 10) then
                Error (sprintf "%s is too long" fieldName)
            else
                Ok (String10 s)

        let value (String10 s) = s



// -------------------------------
// test that the validation works for PersonalName
// -------------------------------

open Domain

/// Create a constructor for PersonalName
let createName (first:String10) (last:String10) :PersonalName =
    {First=first; Last=last}

let validateName inputFirst inputLast =

    let firstOrError =
        inputFirst
        |> String10.create "FirstName"
        |> Validation.ofResult
    let lastOrError =
        inputLast
        |> String10.create "LastName"
        |> Validation.ofResult

    let personOrError =
        (Validation.map2 createName) firstOrError lastOrError

    personOrError // return

// ========================
// Test validation
// ========================

let goodName =
    // input from user
    let inputFirst = "Albert" // less than 10 -- good!
    let inputLast = "Smith"   // less than 10 -- good!
    validateName inputFirst inputLast

let badName =
    // input from user
    let inputFirst = "Jean-Claude"   // more than 10 -- bad!
    let inputLast = ""               // empty -- bad!
    validateName inputFirst inputLast

