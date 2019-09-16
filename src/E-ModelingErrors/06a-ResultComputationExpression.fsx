open System

type ResultBuilder() =
    member this.Return(x) = Ok x
    member this.Bind(x,f) = Result.bind f x

let result = ResultBuilder()


// ==============================

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

// ==============================


type PersonalName = {
    First: String10
    Last: String10
    }

type Person = {
    Name: PersonalName
    Age: Age
    Email: Email
    }

type PersonDto = {
    first: string
    last: string
    age: int
    email: string
    }


let fromDto personDto =
    result {
    let! first = String10.create personDto.first
    let! last = String10.create personDto.last
    let! age = Age.create personDto.age
    let! email = Email.create personDto.email
    let person = {
        Name = {First=first; Last=last}
        Age = age
        Email = email
        }
    return person
    }


// ==============================

let goodDto = {
    first = "Alice"
    last = "Adams"
    age = 1
    email = "x@example.com"
    }

let badDto = {
    first = ""
    last = "Adams"
    age = 1
    email = "x@example.com"
    }

goodDto |> fromDto
badDto |> fromDto
