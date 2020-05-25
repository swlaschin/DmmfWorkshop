// =================================
// This file demonstrates how to do validation
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

// Load a file with library functions for Result
#load "Result.fsx"

/// Define types and validation for this domain
module Domain =

    type UserId = private UserId of int

    type Name = private Name of string

    type EmailAddress = private EmailAddress of string

    type Request = {
        UserId: UserId
        Name: Name
        Email: EmailAddress
    }

    /// Errors just for Validation
    type ValidationError =
      | UserIdMustBePositive
      | NameMustNotBeBlank
      | NameMustNotBeLongerThan of int
      | EmailMustNotBeBlank
      | EmailMustHaveAtSign

    /// Errors for the workflow as a whole (not used in this example)
    type WorkflowError =
      // Validation now contains a LIST of errors
      | ValidationErrors of ValidationError list
      // other errors are singles
      | DbError of string
      | SmtpServerError of string

    module UserId =
        let create id =
            if id > 0 then
                Ok (UserId id)
            else
                Error UserIdMustBePositive

    module Name =
        let create s =
            if String.IsNullOrEmpty(s) then
                Error NameMustNotBeBlank
            elif s.Length > 20 then
                Error (NameMustNotBeLongerThan 20)
            else
                Ok (Name s)

    module EmailAddress =
        let create s =
            if String.IsNullOrEmpty(s) then
                Error EmailMustNotBeBlank
            elif not (s.Contains("@")) then
                Error EmailMustHaveAtSign
            else
                Ok (EmailAddress s)

//===========================================
// A DTO to validate
//===========================================

type RequestDto = {
    UserId: int
    Name: string
    Email: string
}


//===========================================
// Validation logic
//===========================================

open Domain

let validateRequest (dto:RequestDto) :Validation<Domain.Request,ValidationError> =

    // a "constructor" function
    let createRequest userId name email =
       {UserId=userId; Name= name; Email=email }

    // the validated components
    let userIdOrError =
        dto.UserId
        |> Domain.UserId.create
        |> Validation.ofResult // converts to list of errors
    let nameOrError =
        dto.Name
        |> Domain.Name.create
        |> Validation.ofResult
    let emailOrError =
        dto.Email
        |> Domain.EmailAddress.create
        |> Validation.ofResult

    // use the "lift3" function (because there are three parameters)
    let requestOrError =
        (Validation.lift3 createRequest) userIdOrError nameOrError emailOrError

    requestOrError


// -------------------------------
// test data
// -------------------------------

let goodRequestDto : RequestDto = {
    UserId = 1
    Name = "Alice"
    Email = "ABC@gmail.COM"
    }
goodRequestDto |> validateRequest

let badRequestDto1 : RequestDto  = {
  UserId = 0
  Name = ""
  Email = ""
}
badRequestDto1 |> validateRequest


// -------------------------------
// Combining DTOs and JSON
// -------------------------------

#r "System.Text.Json"
open System.Text.Json


(*
// For JSON to work, go back and CLIMutableAttribute to the DTO

[<CLIMutableAttribute>]
type RequestDto = {
    UserId: int
    Name: string
    Email: string
}
*)

let goodJson  = """{"UserId":1,"Name":"Alice","Email":"ABC@gmail.COM"}"""
let goodDomainObj =
    goodJson
    |> JsonSerializer.Deserialize
    |> validateRequest

let badJson  = """{"UserId":1,"Name":"","Email":""}"""
let badDomainObj =
    badJson
    |> JsonSerializer.Deserialize
    |> validateRequest
