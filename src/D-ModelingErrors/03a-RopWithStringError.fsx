// =================================
// This file demonstrates "railway oriented programming"
// where the error type is a string
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

// IMPORTANT - if you get errors such as
//   Could not load type 'ErrorMessage' from assembly 'FSI-ASSEMBLY,
// then try:
// 1. Reset the FSI interactive
// 2. Load code in small chunks

// Load a file with library functions for Result
#load "Result.fsx"

// A library of utility functions for railway oriented programming
// which are not specific to this workflow and could be reused.
#load "RopUtil.fsx"


// Some data to validate
type Request = {
    UserId: int
    Name: string
    Email: string
}

//===========================================
// Step 1 of the pipeline: validation
//===========================================

let nameNotBlank input =
  if input.Name = "" then
    Error "Name must not be blank"
  else
    Ok input

let name50 input =
  if input.Name.Length > 50 then
    Error "Name must not be longer than 50 chars"
  else
    Ok input

let emailNotBlank input =
  if input.Email = "" then
    Error "Email must not be blank"
  else
    Ok input


/// Combine all the smaller validation functions into one big one
let validateRequest input =
  input
  |> nameNotBlank
  |> Result.bind name50
  |> Result.bind emailNotBlank

(*
NOTE: If we wanted to use the ideas from domain modeling,
we could define a NEW type to represent a ValidatedRequest
and return that as the output of the validation.
*)
(*
type ValidatedRequest = ValidatedRequest of Request

let validateRequest input  =
    input
    |> nameNotBlank
    |> Result.bind name50
    |> Result.bind emailNotBlank
    |> Result.map ValidatedRequest   // mark as Validated
*)

// -------------------------------
// test the "validateRequest" step interactively
// before implementing the next step

let goodRequest = {
  UserId=0
  Name= "Alice"
  Email="   ABC@gmail.COM   "   // note: this has spaces and some uppercase
}
goodRequest |> validateRequest

let badRequest = {
  UserId=0
  Name= ""
  Email="abc@example.com"
}
badRequest |> validateRequest

let unsendableRequest = {
  UserId=0
  Name= "Alice"
  Email="ABC@example.COM"
}
unsendableRequest |> validateRequest


//===========================================
// Step 2 of the pipeline: lowercasing the email
//===========================================

// trim spaces and lowercase
let canonicalizeEmail (input:Request) =
   { input with Email = input.Email.Trim().ToLower() }


// -------------------------------
// test the "canonicalize" step interactively
// before implementing the next step

goodRequest
|> validateRequest
|> Result.map canonicalizeEmail

//-----------------------------------------
// Enhance this step by making a two-track version of canonicalizeEmail
// called "canonicalizeEmailR"
//
// This means we can hide the Result.map and the pipeline looks nicer :)


let canonicalizeEmailR twoTrackInput =
  twoTrackInput |> Result.map canonicalizeEmail


// test "canonicalizeEmailR"
goodRequest
|> validateRequest
|> canonicalizeEmailR


//===========================================
// Step 3 of the pipeline: Update the database
//===========================================


let updateDb (request:Request) =
    // do something
    // return nothing at all
    printfn "Database updated with userId=%i email=%s" request.UserId request.Email
    ()


//-----------------------------------------
// Enhance this step by making a two-track version of updateDb
// called "updateDbR"

let updateDbR twoTrackInput =
  twoTrackInput
  |> Result.map (RopUtil.tee updateDb)

// -------------------------------
// test the "updateDbR" step interactively
// before implementing the next step

goodRequest   // also try badRequest and unsendableRequest here
|> validateRequest
|> canonicalizeEmailR
|> updateDbR


//===========================================
// Step 4 of the pipeline: Send an email
//===========================================

let sendEmail (request:Request) =
    if request.Email.EndsWith("example.com") then
        failwithf "Can't send email to %s" request.Email
    else
        printfn "Sending email=%s" request.Email
        request // return request for processing by next step

(*
// this code will throw an exception :(
unsendableRequest
|> validateRequest
|> canonicalizeEmailR
|> Result.map sendEmail
*)

// The fix is to convert the exception-throwing code
// into Result-returning code
let sendEmailR twoTrackInput =
    // define a handler for exceptions
    let exnConverter (ex:exn) = ex.Message
    // convert the exception-throwing "sendEmail"
    // into something useful.
    RopUtil.catchR sendEmail exnConverter twoTrackInput


// -------------------------------
// test the "sendEmailR" step interactively
// before implementing the next step

goodRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR

unsendableRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR   // unsendableRequest fails here

//===========================================
// Step 5 of the pipeline: Log the errors
//===========================================

let loggerR twoTrackInput =
    match twoTrackInput with
    | Ok (req:Request) ->
        printfn "LOG INFO Name=%s EMail=%s" req.Name req.Email
    | Error err ->
        printfn "LOG ERROR %s" err
    twoTrackInput // return same input for use in the next step of the pipeline

// -------------------------------
// test the "loggerR" step interactively
// before implementing the next step

goodRequest   // also try badRequest and unsendableRequest here
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR
|> loggerR

//===========================================
// Last step of the pipeline: return the response
//===========================================

let returnMessageR result =
    match result with
    | Ok obj ->
        sprintf "200 %A" obj
    | Error msg ->
        sprintf "400 %s" msg

// -------------------------------
// test the "returnMessageR" step interactively

goodRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR
|> loggerR
|> returnMessageR


//===========================================
// Finally, build a bigger function that runs the whole pipeline
//===========================================

let updateCustomerR input =
    // See how tidy the pipeline is :)
    input
    |> validateRequest
    |> canonicalizeEmailR
    |> updateDbR
    |> sendEmailR
    |> loggerR
    |> returnMessageR


// -------------------------------
// test the entire pipeline with different inputs

goodRequest
|> updateCustomerR

badRequest
|> updateCustomerR

unsendableRequest
|> updateCustomerR



(*
DEMO: Working with database transactions

let updateDbWithTransaction f (request:Request) =
    // do something
    // return nothing at all
    printfn "Start Database transaction with userId=%i email=%s" request.UserId request.Email
    match (f request) with
    | Ok data ->
        printfn "Commit Database Transaction"
        Ok data
    | Error e ->
        printfn "Abort Database Transaction"
        Error e

let updateDbWithTransactionR f r =
    Result.bind (updateDbWithTransaction f) r

unsendableRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbWithTransactionR (fun req ->
    Ok req
    |> sendEmailR
    |> loggerR
    )
|> returnMessageR
*)