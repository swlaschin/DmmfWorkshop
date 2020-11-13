﻿// ================================================
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
    {
    first = String10.value person.Name.First
    last = String10.value person.Name.Last
    age = Age.value person.Age
    email = Email.value person.Email
    }

/// Exercise: Create a person from a DTO.
/// This might fail if the DTO is invalid
let fromDto (personDto:PersonDto) :Validation<Person,string> =
    let firstOrError =
        personDto.first
        |> String10.create "first name"
        |> Validation.ofResult
    let lastOrError =
        personDto.last
        |> String10.create "last name"
        |> Validation.ofResult
    let ageOrError =
        personDto.age
        |> Age.create
        |> Validation.ofResult
    let emailOrError =
        personDto.email
        |> Email.create
        |> Validation.ofResult

    let nameOrError =
        (Validation.lift2 createName) firstOrError lastOrError
    let personOrError =
        (Validation.lift3 createPerson) nameOrError ageOrError emailOrError

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

// check that validation happens
goodDto |> fromDto
badDto |> fromDto

// convert to domain object and then convert back to a DTO
match goodDto |> fromDto with
| Ok domainObj -> toDto domainObj
| Error _ -> failwith "should not happen"
