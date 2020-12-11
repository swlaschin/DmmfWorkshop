// ================================================
// Person domain to be loaded into exercises
// ================================================

open System

/// Define types and validation for this domain
module Domain =

    // String10 must be not empty AND len < 10
    type String10 =  private String10 of string

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
            if String.IsNullOrEmpty(s) then
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
            if String.IsNullOrEmpty(s) then
                Error "Email is null or empty"
            else if s.Contains("@") |> not then
                Error "Email must contain @"
            else
                Ok (Email s)

        let value (Email s) = s



