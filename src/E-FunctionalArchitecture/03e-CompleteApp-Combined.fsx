// =================================
// This file is the complete app example, combined into one single file
// containing the domain, the implementation, the dtos, and the main API
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

//===============================================
// The Domain (normally a separate file)
//===============================================

/// Define types and validation for an Email Service
module EmailServiceDomain =

    // ------------------------
    // Common constrained types
    // ------------------------

    type UserId = private UserId of int
    type EmailAddress = private EmailAddress of string
    type EmailBody = private EmailBody of string


    // ------------------------
    // Input and output for a workflow
    // ------------------------

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

    // ------------------------
    // The workflows in the domain (only one here!)
    // ------------------------

    /// Send an email message, and get a response or an error
    type SendAMessage = Request -> Result<Response,WorkflowError>

    // -------------------------------------
    // Support modules for constrained types in the domain
    // -------------------------------------
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
// Implementation of the workflow(s) defined in the domain
//===============================================

module EmailServiceImplementation =
    open EmailServiceDomain

    /// Implement the core workflow defined in the domain
    /// Note that this is completely pure. There is no I/O
    let sendAMessage : SendAMessage =
        fun request ->
            let fromAddress = EmailAddress.value request.FromAddress
            let toAddress = EmailAddress.value request.ToAddress
            let body = EmailBody.value request.EmailBody

            if toAddress.Contains("example.com") then
                // bad request
                let errorMsg = sprintf "Can't send email to %s" toAddress
                Error (SmtpServerError errorMsg)
            else
                // create the email to send here but don't actually send it yet
                let emailMessage = sprintf "From: %s\nTo: %s\nSubject: Test message\n\n%s" fromAddress toAddress body
                let result = {ResponseId=42; EmailMessage=emailMessage}
                Ok result

//===============================================
// Implementation of the DTOs
//===============================================

// Load a file with library functions for Result
#load "Result.fsx"

module Dto =
    open EmailServiceDomain

    [<CLIMutableAttribute>]  // This is needed for JSON, so ignore this for now
    type RequestDto = {
        UserId: int
        FromAddress: string
        ToAddress: string
        Body: string
    }

    module RequestDto =

        /// Convert a dto into a Domain.Request,
        /// and do validation at the same time
        let toDomain (dto:RequestDto) :Validation<Request,ValidationError> =

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
// Some interactive tests
//===========================================

module TestDtos =
    open EmailServiceDomain
    open Dto

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
    goodRequestDto |> RequestDto.toDomain
    badRequestDto |> RequestDto.toDomain
    invalidEmailDto |> RequestDto.toDomain


// -------------------------------
// Working with DTOs and JSON
// -------------------------------


// use the .NET Standard JSON library
#r "../../lib/Newtonsoft.Json.dll"
let serializeJson = Newtonsoft.Json.JsonConvert.SerializeObject
let deserializeJson<'a> str = Newtonsoft.Json.JsonConvert.DeserializeObject<'a> str

// uncomment to use the .NET Core JSON library
// #r "System.Text.Json"
// open System.Text.Json
// let serializeJson = JsonSerializer.Serialize
// let deserializeJson<'a> (str:string) = JsonSerializer.Deserialize<'a>(str)

/// Combine JSON and validation in one step
let jsonToRequest json =
    json
    |> deserializeJson
    |> Dto.RequestDto.toDomain


// uncomment this block for testing
// or just highlight and run inside the comment!
(*

// some good JSON
let goodJson  = """{"UserId":1,"FromAddress":"abc@gmail.com","ToAddress":"xyz@gmail.com","Body":"Hello",}"""

// some invalid JSON
let badJson  = """{"UserId":0,"FromAddress":"","ToAddress":"gmail.com","Body":"",}"""
let invalidEmailJson  = """{"UserId":2,"FromAddress":"abc@example.com","ToAddress":"xyz@example.com","Body":"Hello",}"""

// TEST: try the good and bad JSON
let goodDomainObj = goodJson |> jsonToRequest
let badDomainObj = badJson |> jsonToRequest
let invalidEmailObj = invalidEmailJson |> jsonToRequest
*)

//===============================================
// Infrastructure code: SMTP, databases, file system, etc
//===============================================


module SmtpService =

    /// Actually send a message to the email server
    let sendEmail (emailMessage:string) =
        printfn "sending message %s" emailMessage

//===============================================
// Top level code (e.g "Program", "Shell", "API") that combines
// * the implementation
// * serialization
// * other I/O and infrastructure
//===============================================

module EmailServiceApi =
    open EmailServiceDomain

    // helper function to convert Error type into a HTTP Response code
    let returnHttpResponse (result:Result<_,WorkflowError>) =
        // helper functions -- a real library would have these built in
        let makeHttpOk s = "200 OK\n\n" + s
        let makeHttpBadRequest s = "400 Bad Request\n\n" + s
        let makeHttpServerError s = "500 Internal Server Error\n\n" + s

        match result with
        // if the validation was OK, return OK
        | Ok data ->
            serializeJson data
            |>  makeHttpOk

        // if the validation had errors, return BadRequest
        | Error (WorkflowError.ValidationErrors errors) ->
            errors
            |> List.map string  // convert the error type to string for serialization
            |> serializeJson
            |> makeHttpBadRequest

        | Error (WorkflowError.SmtpServerError error) ->
            error
            |> makeHttpServerError

        | Error (WorkflowError.DbError error) ->
            error
            |> makeHttpServerError

    /// Actually send a message to the email server
    let contactSmtpServer (workflowResponse:EmailServiceDomain.Response) =
        SmtpService.sendEmail workflowResponse.EmailMessage
        workflowResponse

    // --------------------------------------
    // Top level public API for the workflows
    // --------------------------------------

    /// 1. convert json to a domain object
    /// 2. Call the Implementation workflow
    /// 3. Convert the result back to an HTTP response
    let sendAMessage json =
        printfn "sendAMessage with input %s" json

        json
        // 1. convert the json into a domain object
        |> jsonToRequest

        // 2. wrap any validation errors into a WorkflowError.ValidationErrors case
        |> Result.mapError (fun errs -> EmailServiceDomain.WorkflowError.ValidationErrors errs)

        // =========== Pure code starts ============

        // 3. we now have a valid domain object to pass
        // in to our workflow
        |> Result.bind EmailServiceImplementation.sendAMessage

        // =========== Pure code ends ============

        // 4. Contact the SMTP server, if OK
        |> Result.map contactSmtpServer

        // 5. return a HTTP response based on the result
        |> returnHttpResponse

// uncomment this block for testing
// or just highlight and run inside the comment!
(*

// TEST: try the good and bad JSON
goodJson |> EmailServiceApi.sendAMessage
badJson |> EmailServiceApi.sendAMessage
invalidEmailJson |> EmailServiceApi.sendAMessage
*)
