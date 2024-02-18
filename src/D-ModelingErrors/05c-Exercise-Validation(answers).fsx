// ================================================
// Exercise:
// Combine validation functions using map
// ================================================

// Load a file with library functions for Validation
#load "Validation.fsx"  // TIP evaluate this import separately before evaluating the rest of the code below

// =============================================
// The domain
// =============================================

/// Define types for this domain
module Domain =

    // String10 must be not empty AND len < 10
    type String10 = private String10 of string

    // Age must be between 0..130
    type Age = private Age of int

    // Email must be not empty and contain an @ symbol
    type Email = private Email of string

    type PersonalName = {
        First: String10
        Last: String10
        }

    type Person = {
        Name: PersonalName
        Age: Age
        Email: Email
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


    module Age =
        let create i =
            if i < 0 then
                Error "Age too small"
            else if i > 130 then
                Error "Age too big"
            else
                Ok (Age i)

        let value (Age s) = s


    module Email =
        let create s =
            if System.String.IsNullOrEmpty(s) then
                Error "Email is null or empty"
            else if s.Contains("@") |> not then
                Error "Email must contain @"
            else
                Ok (Email s)

        let value (Email s) = s


// =======================================
// Create a validation function for PersonalName
// =======================================

open Domain

/// Create a constructor for PersonalName
let createName (first:String10) (last:String10) :PersonalName =
    {First=first; Last=last}

let createNameOrError inputFirst inputLast =
    let firstOrError =
        String10.create "firstName" inputFirst
        |> Validation.ofResult
    let lastOrError =
        String10.create "lastName"  inputLast
        |> Validation.ofResult
    // Exercise: fix the compiler error by using Validation.map2
    let nameOrError =
        (Validation.map2 createName) firstOrError lastOrError
    nameOrError // return

// -------------------------------
// test that the validation for PersonalName works
// -------------------------------

let goodName =
    createNameOrError "Albert" "Smith"

let badName =
    createNameOrError "Jean-Claude" ""


// =======================================
// Create a validation function for Person
// =======================================

/// Create a constructor for Person
let createPerson name age email :Person =
    {Name=name; Age=age; Email=email}

let createPersonOrError inputFirst inputLast inputAge inputEmail =

    // Exercise: fix the error
    let nameOrError = createNameOrError inputFirst inputLast

    let ageOrError =
        Age.create inputAge
        // Exercise: fix the compiler error by converting the Result to a Validation
        |> Validation.ofResult

    let emailOrError =
        Email.create inputEmail
        |> Validation.ofResult

    // Exercise: fix the compiler error by using Validation.map3
    let personOrError =
        (Validation.map3 createPerson) nameOrError ageOrError emailOrError

    personOrError // return

// -------------------------------
// test that the validation for Person works
// -------------------------------

let goodPerson =
    createPersonOrError "Alice" "Adams" 42 "me@example.com"

let badPerson =
    createPersonOrError "Jean-Claude" "" 999 "example.com"

