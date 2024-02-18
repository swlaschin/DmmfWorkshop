// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise: Fix the compiler errors by using the toolkit
// =================================

open System

// Load a file with library functions for Result
#load "Result.fsx"
#load "ToolkitExampleDomain.fsx"
open ToolkitExampleDomain
open ToolkitExampleDomain.Domain

// ========================================
// Combining all the tools
// ========================================

type Url = System.Uri
type Json = string

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

// Download a URL into a JSON file
type DownloadFile = Url -> Async<Result<Json,WorkflowError>>

// Decode the JSON into the Customer DTO
type DecodeCustomerDto = Json -> Result<CustomerDto,WorkflowError>

// Convert the DTO into a valid customer
type CreateValidCustomer = CustomerDto -> Result<Customer,ValidationError list>

// Store the customer in a database
type StoreCustomerInDb = Customer -> Async<Result<unit,WorkflowError>>

//===========================================
// Download
//===========================================

// A dummy implementation of DownloadFile
let downloadFile : DownloadFile =
    fun url -> async {
        return (Ok "")
        }

//===========================================
// Decode JSON to DTO
//===========================================

// A dummy implementation of DecodeCustomerDto
let decodeCustomerDto : DecodeCustomerDto =
    fun json ->
        Ok {Name=""; Email=""; Birthdate=""}


//===========================================
// CreateValidCustomer
//===========================================

// An implementation of CreateValidCustomer
let createValidCustomer : CreateValidCustomer =
    fun dto ->

        // a "constructor" function
        let createCustomer name email bdate :Domain.Customer =
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

        // Exercise: create the customer by using the "createCustomer" function
        // Tip: use the "map3" function on createCustomer (because there are three parameters)
        let customerOrError =
            createCustomer nameOrError emailOrError bdateOrError

        customerOrError


//===========================================
// StoreCustomer
//===========================================

// A dummy implementation of StoreCustomer
let storeCustomerInDb : StoreCustomerInDb =
    fun customer -> async {
        return Ok ()
        }

//===========================================
// Workflow
//===========================================

/// Convert a jsonOrError into a customerOrError
let processCustomerDto (jsonOrError:Result<Json,WorkflowError>) =

    // (helper function for pipeline below)
    // Create a valid customer from the DTO
    // and then convert the list of validation errors
    // into the "ValidationErrors" case WorkflowError
    let createValidCustomer' customerDto =
        customerDto
        |> createValidCustomer
        |> Result.mapError WorkflowError.ValidationErrors

    // Exercise: Eliminate the compiler errors
    // by using Result.map or Result.bind as needed
    jsonOrError             // NOTE:this is two-track input
    |> decodeCustomerDto    // Json -> DTO
    |> createValidCustomer' // DTO -> Customer

// type signature is:
// val processCustomerDto : jsonOrError:Result<Json,WorkflowError> -> Result<Customer,WorkflowError>


// Exercise: Eliminate the compiler errors
// by using one of Async.map/Async.bind/AsyncResult.map/AsyncResult.bind
let downloadAndStoreCustomer url =
    url
    |> downloadFile        // download -> Json
    |> processCustomerDto  // Json -> Customer
    |> storeCustomerInDb   // Customer -> database

// type signature is:
// val downloadAndStoreCustomer : url:Url -> AsyncResult<unit,WorkflowError>


