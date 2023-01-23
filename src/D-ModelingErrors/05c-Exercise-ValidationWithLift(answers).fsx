// ================================================
// Exercise:
// Combine validation functions using map
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
    type String10 = private String10 of string

    // Age must be between 0..130
    type Age = private Age of int

    // Email must be not empty and contain an @ symbol
    type Email = private Email of string

    type Name = {
        First: String10
        Last: String10
        }

    type Person = {
        Name: Name
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

    // Exercise: fix the compiler error by using Validation.map2
    (*
    let nameOrError =
        createName firstOrError lastOrError
    *)
    let nameOrError =
        (Validation.map2 createName) firstOrError lastOrError

    nameOrError // return


// -------------------------------
// test that the validation works for Person
// -------------------------------

/// Create a constructor for Person
let createPerson name age email :Person =
    {Name=name; Age=age; Email=email}


// Exercise: create a function that takes all the components and builds a Person or error
// given all the primitive values
let createPersonOrError (first:string) (last:string) (age:int) (email:string) : Validation<Person,string> =

    // fix this bad implementation
    (*
    let nameOrError =
        createName first last
    *)
    let nameOrError =
        createNameOrError first last
    let ageOrError =
        Age.create age
        |> Validation.ofResult
    let emailOrError =
        Email.create email
        |> Validation.ofResult

    (*
    let personOrError =
        nameOrError ageOrError emailOrError
    *)
    let personOrError =
        (Validation.map3 createPerson) nameOrError ageOrError emailOrError

    personOrError // return


// test your new function!
createPersonOrError "Albert" "Smith" 42 "a@example.com"  // good
createPersonOrError "Jean-Claude" "" -1 "example.com"    // errors
