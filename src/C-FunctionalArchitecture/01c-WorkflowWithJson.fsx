// =================================
// This file demonstrates how to do a complete workflow with JSON
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

// Load a file with library functions for Result
#load "Result.fsx"

//===============================================
// The Domain -- this is typically in a separate file
//===============================================

/// Define types and validation for an Email Service
module EmailServiceDomain =

    type UserId = private UserId of int
    type EmailAddress = private EmailAddress of string
    type EmailBody = private EmailBody of string

    /// A request to send an email
    type Request = {
        UserId: UserId
        FromAddress: EmailAddress
        ToAddress: EmailAddress
        EmailBody: EmailBody
    }

    /// The response from sending an email
    type Response = {
        ResponseId: int
        EmailMessage: string
    }

    /// Errors just for Validation
    type ValidationError =
        | UserIdMustBePositive
        | EmailAddressMustNotBeBlank of fieldName:string
        | EmailAddressMustHaveAtSign of fieldName:string
        | EmailBodyMustNotBeBlank
        | EmailBodyMustNotBeLongerThan of int

    /// Errors for the workflow as a whole (not used in this example)
    type WorkflowError =
      // Validation now contains a LIST of errors
      | ValidationErrors of ValidationError list
      // other errors are single errors
      | DbError of string
      | SmtpServerError of string

    module UserId =
        let create id =
            if id > 0 then
                Ok (UserId id)
            else
                Error UserIdMustBePositive

    module EmailAddress =
        let create fieldName str =
            if String.IsNullOrEmpty(str) then
                Error (EmailAddressMustNotBeBlank fieldName)
            elif not (str.Contains("@")) then
                Error (EmailAddressMustHaveAtSign fieldName)
            else
                Ok (EmailAddress str)

        let value (EmailAddress str) =
            str

    module EmailBody =
        let create str =
            if String.IsNullOrEmpty(str) then
                Error EmailBodyMustNotBeBlank
            elif str.Length > 20 then
                Error (EmailBodyMustNotBeLongerThan 20)
            else
                Ok (EmailBody str)

        let value (EmailBody str) =
            str



//===============================================
// Implementation of the workflow -- this is typically in a separate file
//===============================================

module EmailServiceImplementation =
    open EmailServiceDomain

    /// Define a workflow -- this is completely pure. No I/O
    let sendAMessage (request:Request) :Result<Response,WorkflowError> =
        let fromAddress = EmailAddress.value request.FromAddress
        let toAddress = EmailAddress.value request.ToAddress
        let body = EmailBody.value request.EmailBody

        if toAddress.Contains("example.com") then
            // bad request
            let errorMsg = sprintf "Can't send email to %s" toAddress
            Error (SmtpServerError errorMsg)
        else
            // sending email goes here
            let emailMessage = sprintf "From: %s\nTo: %s\nSubject: Test message\n\n%s" fromAddress toAddress body
            let result = {ResponseId=42; EmailMessage=emailMessage}
            Ok result

//===============================================
// Implementation of the DTOs -- this is typically in a separate file
//===============================================

module EmailServiceDto =
    open EmailServiceDomain

    [<CLIMutableAttribute>]  // This is needed for JSON, so ignore this for now
    type RequestDto = {
        UserId: int
        FromAddress: string
        ToAddress: string
        Body: string
    }

    /// Convert a dto into a Domain.Request,
    /// and do validation at the same time
    let dtoToRequest (dto:RequestDto) :Validation<Request,ValidationError> =

        // a "constructor" function
        let createRequest userId fromAddress toAddress emailBody =
           {UserId=userId; FromAddress=fromAddress; ToAddress=toAddress; EmailBody=emailBody}

        // the validated components
        let userIdOrError =
            dto.UserId
            |> UserId.create         // a Result with a single error
            |> Validation.ofResult   // convert to list of errors
        let fromAddressOrError =
            dto.FromAddress
            |> EmailAddress.create "fromAddress"
            |> Validation.ofResult
        let toAddressOrError =
            dto.ToAddress
            |> EmailAddress.create "toAddress"
            |> Validation.ofResult
        let emailBodyOrError =
            dto.Body
            |> EmailBody.create      // a Result with a single error
            |> Validation.ofResult   // convert to list of errors

        // use the "lift4" function (because there are four parameters)
        let requestOrError =
            (Validation.lift4 createRequest) userIdOrError fromAddressOrError toAddressOrError emailBodyOrError

        requestOrError



//===========================================
// Validation logic
//===========================================

open EmailServiceDomain
open EmailServiceDto

// -------------------------------
// test data
// -------------------------------

let goodRequestDto : RequestDto = {
    UserId = 1
    FromAddress = "abc@gmail.com"
    ToAddress = "xyz@gmail.com"
    Body = "Hello"
    }

let badRequestDto : RequestDto  = {
    UserId = 0
    FromAddress = "gmail.com"
    ToAddress = ""
    Body = ""
    }

let invalidEmailDto : RequestDto  = {
    UserId = 2
    FromAddress = "abc@example.com"
    ToAddress = "xyz@example.com"
    Body = "Hello"
    }

// TEST: try the good and bad DTOs
goodRequestDto |> dtoToRequest
badRequestDto |> dtoToRequest
invalidEmailDto |> dtoToRequest


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


/// Combine JSON and validation in one step
let jsonToRequest json =
    json
    |> deserializeJson
    |> dtoToRequest

// some good JSON
let goodJson  = """{"UserId":1,"FromAddress":"abc@gmail.com","ToAddress":"xyz@gmail.com","Body":"Hello",}"""

// some invalid JSON
let badJson  = """{"UserId":0,"FromAddress":"","ToAddress":"gmail.com","Body":"",}"""

let invalidEmailJson  = """{"UserId":2,"FromAddress":"abc@example.com","ToAddress":"xyz@example.com","Body":"Hello",}"""

// TEST: try the good and bad JSON
let goodDomainObj = goodJson |> jsonToRequest
let badDomainObj = badJson |> jsonToRequest
let invalidEmailObj = invalidEmailJson |> jsonToRequest

// -------------------------------
// To build a complete pipeline, we need one more step
// to convert the Result to a JSON string
// -------------------------------
module EmailServiceApi =

    // helper function to convert Error type into a HTTP Response code
    let returnHttpResponse (result:Result<_,WorkflowError>) =
        // helper functions -- a real library would have these built in
        let makeHttpOk s = "200 " + s
        let makeHttpBadRequest s = "400 " + s
        let makeHttpServerError s = "500 " + s

        match result with
        // if the validation was OK, return OK
        | Ok data ->
            serializeJson data
            |>  makeHttpOk

        // if the validation had errors, return BadRequest
        | Error (WorkflowError.ValidationErrors errors) ->
            errors
            |> List.map string  // convert thee error type to string for serialization
            |> serializeJson
            |> makeHttpBadRequest

        | Error (WorkflowError.SmtpServerError error) ->
            error
            |> makeHttpServerError

        | Error (WorkflowError.DbError error) ->
            error
            |> makeHttpServerError

    /// Define a workflow that returns a response
    /// This wraps the pure function with logging, I/O, etc
    let handleResponse (workflowResponse:EmailServiceDomain.Response) =
        printfn "sending message %s" workflowResponse.EmailMessage
        workflowResponse

    // try to deserialize some bad JSON to a domain object
    // and return the validation error
    let sendAMessage json =
        printfn "sendAMessage with input %s" json

        json
        // 1. convert the json into a domain object
        |> jsonToRequest

        // 2. wrap any validation errors into a WorkflowError.ValidationErrors case
        |> Result.mapError (fun errs -> WorkflowError.ValidationErrors errs)

        // =========== Pure code ============
        // 3. we now have a valid domain object to pass
        // in to our workflow
        |> Result.bind EmailServiceImplementation.sendAMessage
        // =========== Pure code ============

        // 4. Contact the SMTP server, if OK
        |> Result.map handleResponse

        // 5. return a HTTP response based on the result
        |> returnHttpResponse

// TEST: try the good and bad JSON
goodJson |> EmailServiceApi.sendAMessage
badJson |> EmailServiceApi.sendAMessage
invalidEmailJson |> EmailServiceApi.sendAMessage