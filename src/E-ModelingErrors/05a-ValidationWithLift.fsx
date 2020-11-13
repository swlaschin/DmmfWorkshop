// ================================================
// Example: Combine validation functions using lift
// ================================================

// Load a file with library functions for Result
#load "Result.fsx"

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
let createName first last :PersonalName =
    {First=first; Last=last}


let goodName =
    let firstOrError =
        String10.create "firstName" "123456789" // less than 10 -- good!
        |> Validation.ofResult
    let lastOrError =
        String10.create "lastName"  "123456789" // less than 10 -- good!
        |> Validation.ofResult
    // Exercise: fix the compiler error by using Validation.lift2
    (Validation.lift2 createName) firstOrError lastOrError


let badName =
    let firstOrError =
        String10.create "FirstName" "12345678901" // more than 10 -- bad!
        |> Validation.ofResult
    let lastOrError =
        String10.create "LastName"  "12345678901" // more than 10 -- bad!
        |> Validation.ofResult
    // Exercise: fix the compiler error by using Validation.lift2
    (Validation.lift2 createName) firstOrError lastOrError

