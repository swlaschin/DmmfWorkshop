// =================================
// Exercise: "railway oriented programming"
// where the error type is a choice type
//
// Exercise:
//    Convert this pipeline to use a custom Error type
//    rather than strings
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
// Define the error type here.
// TIP: Add to it incrementally as you work through
// the pipeline, rather than defining every error up front.
//===========================================

type ErrorMessage =
  | NameMustNotBeBlank
  | NameMustNotBeLongerThan of int
  | EmailMustNotBeBlank
  | SmtpServerError of string


//===========================================
// Step 1 of the pipeline: validation
//===========================================

let nameNotBlank input =
  if input.Name = "" then
    Error NameMustNotBeBlank
  else
    Ok input

let name50 input =
  if input.Name.Length > 50 then
    Error (NameMustNotBeLongerThan 50)
  else
    Ok input

let emailNotBlank input =
  if input.Email = "" then
    Error EmailMustNotBeBlank
  else
    Ok input

/// Combine all the smaller validation functions into one big one
let validateRequest input =
  input
  |> nameNotBlank
  |> Result.bind name50
  |> Result.bind emailNotBlank


// -------------------------------
// test the "validateRequest" step interactively
// before implementing the next step

let goodRequest = {
  UserId=0
  Name= "Alice"
  Email="ABC@gmail.COM"
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
let canonicalizeEmail input =
   { input with Email = input.Email.Trim().ToLower() }

let canonicalizeEmailR twoTrackInput =  // value restriction error fixed!!!
  twoTrackInput |> Result.map canonicalizeEmail

// -------------------------------
// test the "canonicalize" step interactively
// before implementing the next step

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

let updateDbR twoTrackInput =
  twoTrackInput |> Result.map (RopUtil.tee updateDb)

// -------------------------------
// test the "updateDbR" step interactively
// before implementing the next step

goodRequest    // also try badRequest and unsendableRequest here
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

let sendEmailR twoTrackInput =
    // convert SMTP exceptions to our list
    let handler (ex:exn) = SmtpServerError ex.Message
    RopUtil.catchR sendEmail handler twoTrackInput

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
|> sendEmailR           // unsendableRequest fails here

//===========================================
// Step 5 of the pipeline: Log the errors
//===========================================

let loggerR twoTrackInput =
    match twoTrackInput with
    | Ok (req:Request) ->
        printfn "LOG INFO Name=%s EMail=%s" req.Name req.Email
    | Error err ->
        printfn "LOG ERROR %A" err
    twoTrackInput   // return same input for use in the next step of the pipeline

// -------------------------------
// test the "loggerR" step interactively
// before implementing the next step

goodRequest     // also try badRequest and unsendableRequest here
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR
|> loggerR

//===========================================
// Translator from error type to string
//===========================================

// obviously the real ones would use resource files!

let translateError_EN err =
    match err with
    | NameMustNotBeBlank ->
        "Name must not be blank"
    | NameMustNotBeLongerThan i ->
        sprintf "Name must not be longer than %i chars" i
    | EmailMustNotBeBlank ->
        "Email must not be blank"
    | SmtpServerError msg ->
        sprintf "SmtpServerError [%s]" msg

let translateError_FR err =
    match err with
    | NameMustNotBeBlank ->
        "Nom ne doit pas être vide"
    | NameMustNotBeLongerThan i ->
        sprintf "Nom ne doit pas être plus long que %i caractères" i
    | EmailMustNotBeBlank ->
        "Email doit pas être vide"
    | SmtpServerError msg ->
        sprintf "SmtpServerError [%s]" msg

//===========================================
// Last step of the pipeline: return the response
//===========================================

let returnMessageR translator result =
    match result with
    | Ok obj ->
        sprintf "200 %A" obj
    | Error msg ->
        let errStr = translator msg
        sprintf "400 %s" errStr


// -------------------------------
// test the "returnMessageR" step interactively
goodRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR
|> loggerR
|> returnMessageR translateError_EN


//===========================================
// Finally, build a bigger function that runs the whole pipeline
//===========================================

let updateCustomerR request =
  request
  |> validateRequest
  |> canonicalizeEmailR
  |> updateDbR
  |> sendEmailR
  |> loggerR
  |> returnMessageR translateError_EN
  // or change to this
  // |> returnMessageR translateError_FR


// -------------------------------
// test the entire pipeline with different inputs

goodRequest |> updateCustomerR

badRequest |> updateCustomerR

unsendableRequest |> updateCustomerR


