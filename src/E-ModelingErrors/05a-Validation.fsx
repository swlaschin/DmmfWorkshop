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

[<CLIMutableAttribute>]  // This is needed for JSON, so ignore this for now
type RequestDto = {
    UserId: int
    Name: string
    Email: string
}


//===========================================
// Validation logic
//===========================================

open Domain

/// Convert a dto into a Domain.Request,
/// and do validation at the same time
let dtoToRequest (dto:RequestDto) :Validation<Domain.Request,ValidationError> =

    // a "constructor" function
    let createRequest userId name email =
       {UserId=userId; Name= name; Email=email }

    // the validated components
    let userIdOrError =
        dto.UserId
        |> Domain.UserId.create  // a Result with a single error
        |> Validation.ofResult   // converts to list of errors
    let nameOrError =
        dto.Name
        |> Domain.Name.create    // a Result with a single error
        |> Validation.ofResult   // converts to list of errors
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

let badRequestDto : RequestDto  = {
  UserId = 0
  Name = ""
  Email = ""
}

// TEST: try the good and bad DTOs
goodRequestDto |> dtoToRequest
badRequestDto |> dtoToRequest


// -------------------------------
// Combining DTOs and JSON
// -------------------------------

// .NET Standard
#r "../../lib/Newtonsoft.Json.dll"
let serializeJson = Newtonsoft.Json.JsonConvert.SerializeObject
let deserializeJson<'a> str = Newtonsoft.Json.JsonConvert.DeserializeObject<'a> str

// .NET Core
// #r "System.Text.Json"
// open System.Text.Json
// let serializeJson = JsonSerializer.Serialize
// let deserializeJson<'a> (str:string) = JsonSerializer.Deserialize<'a>(str)


(*
// For JSON to work, CLIMutableAttribute is needed on the DTO

[<CLIMutableAttribute>]
type RequestDto = {
    UserId: int
    Name: string
    Email: string
}
*)

/// Combine JSON and validation in one step
let jsonToRequest json =
    json
    |> deserializeJson
    |> dtoToRequest

// some good JSON
let goodJson  = """{"UserId":1,"Name":"Alice","Email":"ABC@gmail.COM"}"""

// some invalid JSON
let badJson  = """{"UserId":1,"Name":"","Email":""}"""

// TEST: try the good and bad JSON
let goodDomainObj = goodJson |> jsonToRequest
let badDomainObj = badJson |> jsonToRequest


// -------------------------------
// To build a complete pipeline, we need one more step
// to convert the Result to a JSON string
// -------------------------------

let returnHttpResponse (validation:Validation<_,ValidationError>) =
    // helper functions -- a real library would have these built in
    let httpOk s = "200 " + s
    let httpBadRequest s = "400 " + s

    match validation with
    // if the validation was OK, return OK
    | Ok data ->
        serializeJson data
        |>  httpOk

    // if the validation had errors, return BadRequest
    | Error errors ->
        errors
        |> List.map string  // convert thee error type to string for serialization
        |> serializeJson
        |> httpBadRequest

/// Define a dummy workflow that returns a record
let myWorkflow (request:Domain.Request) =
    printfn "Workflow being processed"
    {| WorkflowId = 42 |}

// try to deserialize some bad JSON to a domain object
// and return the validation error
let webWorkflow json =
    json
    // 1. convert the json into a domain object
    |> jsonToRequest
    // 2. we now have a valid domain object to pass
    // in to our workflow
    |> Result.map myWorkflow
    // return the HTTP response
    |> returnHttpResponse

// TEST: try the good and bad JSON
goodJson |> webWorkflow
badJson |> webWorkflow