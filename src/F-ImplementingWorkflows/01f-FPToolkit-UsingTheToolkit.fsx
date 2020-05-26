// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

// Load a file with library functions for Result
#load "Result.fsx"

// ========================================
// Combining all the tools
// ========================================

type Url = System.Uri
type Json = string

/// Define types and validation for this domain
module Domain =

    type Name = private Name of string
    type EmailAddress = private EmailAddress of string
    type Birthdate = private Birthdate of System.DateTime

    type Customer = {
        Name: Name
        Email: EmailAddress
        Birthdate : Birthdate
    }

    /// Errors just for Validation
    type ValidationError =
        | NameMustNotBeBlank
        | NameMustNotBeLongerThan of int
        | EmailMustNotBeBlank
        | EmailMustHaveAtSign
        | BirthdateMustBeInPast
        | InvalidBirthdate

    /// Errors for the workflow as a whole (not used in this example)
    type WorkflowError =
      // Validation now contains a LIST of errors
      | ValidationErrors of ValidationError list
      // other errors are singles
      | DbError of string
      | SmtpServerError of string

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

    module Birthdate =
        let create (str:string) =
            match DateTime.TryParse(str) with
            | true,d ->
                if d < DateTime.Now then
                    Ok (Birthdate d)
                else
                    Error BirthdateMustBeInPast
            | false,_ ->
                Error InvalidBirthdate

//===========================================
// A DTO to validate
//===========================================

type CustomerDto = {
    Name: string
    Email: string
    Birthdate : string
}


//===========================================
// Workflow steps to compose using the toolkit
//===========================================

open Domain

// Download a URL into a JSON file
type DownloadFile = Url -> Async<Result<Json,WorkflowError>>

// Decode the JSON into the Customer DTO
type DecodeCustomerDto = Json -> Result<CustomerDto,WorkflowError>

// Convert the DTO into a valid customer
type CreateValidCustomer = CustomerDto -> Result<Customer,ValidationError list>

// Store the customer in a database
type StoreCustomer = Customer -> Async<Result<unit,WorkflowError>>

//===========================================
// Download
//===========================================

let downloadFile : DownloadFile =
    fun url -> async {
        return (Ok "")
        }

//===========================================
// Decode JSON to DTO
//===========================================

let decodeCustomerDto : DecodeCustomerDto =
    fun json ->
        Ok {Name=""; Email=""; Birthdate=""}


//===========================================
// CreateValidCustomer
//===========================================

let createValidCustomer : CreateValidCustomer =
    fun dto ->

        // a "constructor" function
        let createCustomer name email bdate =
           {Name= name; Email=email; Birthdate=bdate }

        // the validated components
        let nameOrError =
            dto.Name
            |> Domain.Name.create
            |> Validation.ofResult
        let emailOrError =
            dto.Email
            |> Domain.EmailAddress.create
            |> Validation.ofResult
        let bdateOrError =
            dto.Birthdate
            |> Domain.Birthdate.create
            |> Validation.ofResult

        // use the "lift3" function (because there are three parameters)
        let customerOrError =
            (Validation.lift3 createCustomer) nameOrError emailOrError bdateOrError

        customerOrError


//===========================================
// StoreCustomer
//===========================================

let storeCustomer : StoreCustomer =
    fun customer -> async {
        return Ok ()
        }

//===========================================
// Workflow
//===========================================

let processCustomerDto jsonOrError =
    let createValidCustomer' =
        createValidCustomer >> Result.mapError WorkflowError.ValidationErrors

    jsonOrError
    |> Result.bind decodeCustomerDto
    |> Result.bind createValidCustomer'

let downloadAndStoreCustomer url =
    url
    |> downloadFile
    |> Async.map processCustomerDto
    |> AsyncResult.bind storeCustomer

