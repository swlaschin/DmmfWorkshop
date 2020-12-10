// ================================================
// Exercise: Convert a domain object to and from a DTO object
// ================================================

(*

There is a type Person with
* property Name of type PersonalName
* property Age of type Age
* property Email of type Email
See the file "PersonDomain.fsx" for definitions

Exercise:
1) create a function "toDto" that converts a Person into a DTO
2) create a function "fromDto" that converts a DTO into Person

*)

open System

// Load a file with library functions for Result
#load "Result.fsx"

// Load a file with definition of the domain
#load "PersonDomain.fsx"
open PersonDomain.Domain


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

/// Create a constructor for PersonalName
let createName first last :PersonalName =
    {First=first; Last=last}

/// Create a constructor for Person
let createPerson name age email :Person =
    {Name=name; Age=age; Email=email}

/// Exercise: Convert a person into a DTO for outputing as JSON.
/// This always succeeds.
let toDto (person:Person) :PersonDto =
    // Exercise: create a function "toDto" that converts a Person into a DTO
    {
    first = String10.value person.Name.First
    ??
    }

/// Exercise: Create a person from a DTO.
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
// test these functions with some example data
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

// check that validation happens when creating a domain object from a DTO
goodDto |> fromDto
badDto |> fromDto


// roundtrip a DTO to a domain object and back
let roundTrip dto =
    let domainObjOrError = fromDto dto
    match  domainObjOrError with
    | Ok domainObj -> Ok (toDto domainObj)
    | Error e -> Error e

// check the round trip logic
goodDto |> roundTrip
badDto |> roundTrip

