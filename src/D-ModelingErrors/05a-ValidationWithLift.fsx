// ================================================
// Example: Combine validation functions using map
// ================================================


type Validation<'Success,'Failure> =
    Result<'Success,'Failure list>

module Validation =

    let ofResult r =
        match r with
        | Ok x -> Ok x
        | Error e -> Error [e]

    let map2 f x1V x2V =
        match (x1V,x2V) with
        | Ok x1, Ok x2 -> Ok (f x1 x2)
        | Error e1, Ok _ -> Error e1
        | Ok _, Error e2 -> Error e2
        | Error e1, Error e2  -> Error (List.concat [e1;e2])

    let map3 f x1V x2V x3V =
        match (x1V,x2V,x3V) with
        | Ok x1, Ok x2, Ok x3 -> Ok (f x1 x2 x3)
        | Error e1, Ok _, Ok _ -> Error e1
        | Ok _, Error e2, Ok _ -> Error e2
        | Ok _, Ok _, Error e3 -> Error e3
        | Error e1, Error e2, Ok _ -> Error (List.concat [e1;e2])
        | Ok _, Error e2, Error e3-> Error (List.concat [e2;e3])
        | Error e1, Ok _, Error e3-> Error (List.concat [e1;e3])
        | Error e1, Error e2, Error e3  -> Error (List.concat [e1;e2;e3])

// =============================================
// The domain
// =============================================

/// Define types for this domain
module Domain =

    // String10 must be not empty AND len < 10
    type String10 =  private String10 of string

    type Name = {
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
// test that the validation works for Name
// -------------------------------

open Domain

/// Create a constructor for Name
let createName (first:String10) (last:String10) :Name =
    {First=first; Last=last}

let createNameOrError strFirst strLast =
    let firstOrError =
        strFirst
        |> String10.create "FirstName"
        |> Validation.ofResult
    let lastOrError =
        strLast
        |> String10.create "LastName"
        |> Validation.ofResult

    let nameOrError =
        (Validation.map2 createName) firstOrError lastOrError

    nameOrError // return

let goodName =
    // input from user
    let strFirst = "Albert" // less than 10 -- good!
    let strLast = "Smith"   // less than 10 -- good!
    createNameOrError strFirst strLast

let badName =
    // input from user
    let strFirst = "Jean-Claude"   // more than 10 -- bad!
    let strLast = ""               // empty -- bad!
    createNameOrError strFirst strLast

