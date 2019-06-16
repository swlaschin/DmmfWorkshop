(*
1) create a constrained type for Age (age must be between 0 and 130)
2) create a constrained type for Email where it must contain an @ sign
3) reuse the PersonalName from the previous example
4) create a type Person with
    * property Name of type PersonalName
    * property Age of type Age
    * property Email of type Email
4) create a type PersonDto with same properties as primitive types (string,int)
   (This will be used for XML or JSON serialization)
5) create a function "toDto" that converts a Person into a DTO
6) create a function "fromDto" that converts a DTO into Person
*)

open System

#load "Result.fsx"

// Here I'm using modules to group related functions together

module ConstrainedTypes =

    type String10 =  private String10 of string

    module String10 =

        let create s =
            if String.IsNullOrEmpty(s) then
                Error "String10 is null or empty"
            else if (s.Length > 10) then
                Error "String10 is too long"
            else
                Ok (String10 s)

        let value (String10 s) = s

    type Age = private Age of int

    module Age =

        let create i =
            if i < 0 then
                Error "Age too small"
            else if i > 130 then
                Error "Age too big"
            else
                Ok (Age i)

        let value (Age s) = s

    type Email = private Email of string

    module Email =

        let create s =
            if String.IsNullOrEmpty(s) then
                Error "Email is null or empty"
            else if s.Contains("@") |> not then
                Error "Email must contain @"
            else
                Ok (Email s)

        let value (Email s) = s


open ConstrainedTypes

/// This is a domain type
type PersonalName = {
    First: String10
    Last: String10
    }

/// This is a domain type
type Person = {
    Name: PersonalName
    Age: Age
    Email: Email
    }

/// This is what will be serialized as JSON
type PersonDto = {
    first: string
    last: string
    age: int
    email: string
    }

/// Create a constructor to be used with partial application
let createName first last :PersonalName =
    {PersonalName.First=first; Last=last}

/// Create a constructor to be used with partial application
let createPerson name age email :Person =
    {Person.Name=name; Age=age; Email=email}

/// convert a person into a DTO -- this always succeeds
let toDto person =
    {
    first = String10.value person.Name.First
    last = String10.value person.Name.Last
    age = Age.value person.Age
    email = Email.value person.Email
    }

let (<!>) = Validation.map
let (<*>) = Validation.apply

/// create a person from a DTO -- this might fail
let fromDto personDto =
    let firstR =
        personDto.first
        |> String10.create
        |> Validation.ofResult
    let lastR =
        personDto.last
        |> String10.create
        |> Validation.ofResult
    let ageR =
        personDto.age
        |> Age.create
        |> Validation.ofResult
    let emailR =
        personDto.email
        |> Email.create
        |> Validation.ofResult

    let nameR = createName <!> firstR <*> lastR
    let person = createPerson <!> nameR <*> ageR <*>emailR

    person // return


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

