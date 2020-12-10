// ================================================
// Exercise:
// Combine validation functions using lift
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


// -------------------------------
// test that the validation works for PersonalName
// -------------------------------

open Domain

/// Create a constructor for PersonalName
let createName (first:String10) (last:String10) :PersonalName =
    {First=first; Last=last}


let goodName =
    let firstOrError =
        String10.create "firstName" "123456789" // less than 10 -- good!
        |> Validation.ofResult
    let lastOrError =
        String10.create "lastName"  "123456789" // less than 10 -- good!
        |> Validation.ofResult
    // Exercise: fix the compiler error by using Validation.lift2
    let nameOrError =
        (Validation.lift2 createName) firstOrError lastOrError
    nameOrError // return

let badName =
    let firstOrError =
        String10.create "FirstName" "12345678901" // more than 10 -- bad!
        |> Validation.ofResult
    let lastOrError =
        String10.create "LastName"  "12345678901" // more than 10 -- bad!
        |> Validation.ofResult
    // Exercise: fix the compiler error by using Validation.lift2
    let nameOrError =
        (Validation.lift2 createName) firstOrError lastOrError
    nameOrError // return

// -------------------------------
// test that the validation works for Person
// -------------------------------

/// Create a constructor for Person
let createPerson name age email :Person =
    {Name=name; Age=age; Email=email}

let goodPerson =
    let nameOrError =
        let firstOrError =
            String10.create "firstName" "123456789"
            |> Validation.ofResult
        let lastOrError =
            String10.create "lastName"  "123456789"
            |> Validation.ofResult
        // Exercise: fix the compiler error by using Validation.lift2
        (Validation.lift2 createName) firstOrError lastOrError

    let ageOrError =
        Age.create 42
        |> Validation.ofResult
    let emailOrError =
        Email.create "test@example.com"
        |> Validation.ofResult

    // Exercise: fix the compiler error by using Validation.lift3
    let personOrError = 
        (Validation.lift3 createPerson) nameOrError ageOrError emailOrError
    
    personOrError // return


let badPerson =
    let nameOrError =
        // Exercise: fix all the compiler errors
        let firstOrError =
            String10.create "firstName" "12345678901"
            |> Validation.ofResult  //  ADD Validation.ofResult
        let lastOrError =
            String10.create "lastName"  "12345678901"
            |> Validation.ofResult  //  ADD Validation.ofResult
        // ADD Validation.lift2
        (Validation.lift2 createName) firstOrError lastOrError

    let ageOrError =
        Age.create 999
        |> Validation.ofResult
    let emailOrError =
        Email.create "example.com"
        |> Validation.ofResult

    // Exercise: fix the compiler error by using Validation.lift3
    let personOrError = 
        (Validation.lift3 createPerson) nameOrError ageOrError emailOrError

    personOrError // return

// Exercise: create a function that takes all the components and builds a Person or error
// given all the primitive values
let createPersonOrError (first:string) (last:string) (age:int) (email:string) : Validation<Person,string> =

    // fix this bad implementation
    // let name = createName first last
    // createPerson name age email

    let nameOrError =
        let firstOrError =
            String10.create "firstName" first
            |> Validation.ofResult
        let lastOrError =
            String10.create "lastName" last
            |> Validation.ofResult
        (Validation.lift2 createName) firstOrError lastOrError

    let ageOrError =
        Age.create age
        |> Validation.ofResult
    let emailOrError =
        Email.create email
        |> Validation.ofResult

    let personOrError = 
        (Validation.lift3 createPerson) nameOrError ageOrError emailOrError

    personOrError // return
