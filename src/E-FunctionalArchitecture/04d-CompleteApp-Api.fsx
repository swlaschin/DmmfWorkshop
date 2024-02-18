// =================================
// This file is part of a complete app example.
// Part 4. The top level code (API) that brings it all together
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

// Load the Domain, implementation and DTO
#load "04c-CompleteApp-Dto.fsx"
open ``04a-CompleteApp-Domain``
open ``04b-CompleteApp-Implementation``
open ``04c-CompleteApp-Dto``



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
let goodJson  = """{"UserId":1,"FromAddress":"abc@gmail.com","ToAddress":"xyz@gmail.com","Body":"Hello"}"""

// some invalid JSON
let badJson  = """{"UserId":0,"FromAddress":"","ToAddress":"gmail.com","Body":""}"""
let invalidEmailJson  = """{"UserId":2,"FromAddress":"abc@example.com","ToAddress":"xyz@example.com","Body":"Hello"}"""

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
