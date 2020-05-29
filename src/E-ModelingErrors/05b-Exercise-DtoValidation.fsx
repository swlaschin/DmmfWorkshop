// ================================================
// Exercise:
// Use Validation as so that functions with different error
// types can be chained together
// ================================================

(*

1) create a type Person with
* property Name of type PersonalName
* property Age of type Age
* property Email of type Email
2) create a constrained type for Age (age must be between 0 and 130)
3) create a constrained type for Email where it must contain an @ sign

4) create a function "toDto" that converts a Person into a DTO
5) create a function "fromDto" that converts a DTO into Person

*)

open System

// Load a file with library functions for Result
#load "Result.fsx"

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

    // Exercise:
    //  create a type Person with
    //  * property Name of type PersonalName
    //  * property Age of type Age
    //  * property Email of type Email
    type Person = {
        ??
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
        // Exercise:
        // Add constructor for Age (age must be between 0 and 130)
        let create i =
            if i < 0 then
                Error "Age too small"
            else if i > 130 then
                ??
            else
                ??

        let value (Age s) = s


    module Email =
        // Exercise:
        // Add constructor for Email
        let create s =
            if String.IsNullOrEmpty(s) then
                ??
            else if s.Contains("@") |> not then
                Error "Email must contain @"
            else
                ??

        let value (Email s) = s



//===========================================
// A DTO to validate
//===========================================


/// This is what will be serialized as JSON
type PersonDto = {
    first: string
    last: string
    age: int
    email: string
    }

//===========================================
// Validation logic
//===========================================

open Domain

/// Create a constructor for PersonalName
let createName first last :PersonalName =
    {First=first; Last=last}

/// Create a constructor for Person
let createPerson name age email :Person =
    {Name=name; Age=age; Email=email}

/// Convert a person into a DTO for outputing as JSON.
/// This always succeeds.
let toDto (person:Person) :PersonDto =
    // Exercise ) create a function "toDto" that converts a Person into a DTO
    {
    first = String10.value person.Name.First
    ??
    }

/// Create a person from a DTO.
/// This might fail if the DTO is invalid
let fromDto (personDto:PersonDto) :Validation<Person,string> =
    // Exercise: create a function "fromDto" that converts a DTO into Person
    let firstOrError =
        personDto.first
        |> String10.create "first name"
        |> Validation.ofResult
    let lastOrError = ??
    let ageOrError = ??
    let emailOrError = ??

    let nameOrError = ??
    let personOrError = ??

    personOrError // return


// -------------------------------
// test data
// -------------------------------

let goodDto = {
    first = "Alice"
    last = "Adams"
    age = 1
    email = "x@example.com"
    }

let badDto = {
    first = ""
    last = "Adams"
    age = 1000
    email = "xexample.com"
    }

goodDto |> fromDto
badDto |> fromDto

