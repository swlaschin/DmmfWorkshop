(*
Railway oriented programming -- with custom error type
*)

#load "Result.fsx"

type Request = {
    UserId: int
    Name: string
    Email: string
}

type ErrorMessage =
  | NameMustNotBeBlank
  | NameMustNotBeLongerThan of int
  | EmailMustNotBeBlank
  | SmtpServerError of string

/// A validation function to check the name is not blank
let nameNotBlank input =
  if input.Name = "" then
    Error NameMustNotBeBlank
  else
    Ok input

/// A validation function to check the name is not too long
let nameIsLessThan50 input =
  if input.Name.Length > 50 then
    Error (NameMustNotBeLongerThan 50)
  else
    Ok input

/// A validation function to check the email is not blank
let emailNotBlank input =
  if input.Email = "" then
    Error EmailMustNotBeBlank
  else
    Ok input

/// Chain the three validation functions in series
/// to make an overall "validation" function
let validateRequest input =
  input
  |> nameNotBlank
  |> Result.bind nameIsLessThan50
  |> Result.bind emailNotBlank


// -------------------------------
// test data
// -------------------------------

let goodRequest = {
  UserId=0
  Name= "Alice"
  Email="ABC@gmail.COM"
}
goodRequest |> validateRequest

/// This is a bad request because the name is blank
let badRequest1 = {
  UserId=0
  Name= ""
  Email="abc@example.com"
}
badRequest1 |> validateRequest

/// This is a valid request but the email contains
/// "example.com" and will be rejected by the email server
let unsendableRequest = {
  UserId=0
  Name= "Alice"
  Email="ABC@example.COM"
}
unsendableRequest |> validateRequest

// ------------------------
// Add another step
// ------------------------

/// trim spaces and make the email lowercase
let canonicalizeEmail input =
   { input with Email = input.Email.Trim().ToLower() }

/// Convert the canonicalizeEmail (one-track function)
/// into a two-track function using "map"
let canonicalizeEmailR input =
  input |> Result.map canonicalizeEmail

// test so far
goodRequest
|> validateRequest
|> canonicalizeEmailR

// ------------------------
// Update the database
// ------------------------

let updateDb (request:Request) =
    // do something
    // return nothing at all
    printfn "Database updated with userId=%i email=%s" request.UserId request.Email
    ()

/// Utility function to convert a dead-end function
/// into a one-track function
let tee f result =
  f result
  result

/// Convert the updateDb into a one-track function using "tee"
/// then turn *that* into a two-track function using "map"
let updateDbR input =
  input |> Result.map (tee updateDb)

// test so far
goodRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR


// ------------------------
// Send an email
// ------------------------

let sendEmail (request:Request) =
    if request.Email.EndsWith("example.com") then
        failwithf "Can't send email to %s" request.Email
    else
        printfn "Sending email=%s" request.Email
        request // return request for processing by next step

/// Utility function to convert a one-track function
/// into a "points/switch" function
let catch exceptionThrowingFunction handler oneTrackInput =
    try
        Ok (exceptionThrowingFunction oneTrackInput)
    with
    | ex ->
        Error (handler ex)

/// Utility function to convert a one-track function
/// into a "points/switch" function and then into a
/// two-track function.
let catchR exceptionThrowingFunction handler twoTrackInput =
    // catch' is a points/switch function
    let catch' = catch exceptionThrowingFunction handler
    // use "bind" to convert it to two track
    twoTrackInput
    |> Result.bind catch'

/// Convert "sendEmail" into a two-track function
let sendEmailR twoTrackInput =
    // convert SMTP exceptions to our list
    let handler (ex:exn) = SmtpServerError ex.Message
    catchR sendEmail handler twoTrackInput

// test so far
goodRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR

unsendableRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR

// ------------------------
// log the errors
// ------------------------

/// Log both sides of a two-track input and
/// then return the original input
/// (for the next function in the pipeline to use)
let loggerR twoTrackInput =
    match twoTrackInput with
    | Ok (req:Request) ->
        printfn "LOG INFO Name=%s EMail=%s" req.Name req.Email
    | Error err ->
        printfn "LOG ERROR %A" err
    twoTrackInput

// test so far
goodRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR
|> loggerR

// ------------------------
// message converter examples
// ------------------------

/// Translate an error type into an EN string
let translateError_EN err =
    // obviously the real implementation would use resource files
    // or similar
    match err with
    | NameMustNotBeBlank ->
        "Name must not be blank"
    | NameMustNotBeLongerThan i ->
        sprintf "Name must not be longer than %i chars" i
    | EmailMustNotBeBlank ->
        "Email must not be blank"
    | SmtpServerError msg ->
        sprintf "SmtpServerError [%s]" msg

/// Translate an error type into an FR string
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

// ------------------------
// return the response
// ------------------------

/// Collapse the two-track pipeline into a single value (a string)
let returnMessageR translator result =
    match result with
    | Ok obj ->
        sprintf "200 %A" obj
    | Error msg ->
        let errStr = translator msg
        sprintf "400 %s" errStr


// test so far
goodRequest
|> validateRequest
|> canonicalizeEmailR
|> updateDbR
|> sendEmailR
|> loggerR
|> returnMessageR translateError_EN


// ------------------------
// final code
// ------------------------

let updateCustomerR request =
  request
  |> validateRequest
  |> canonicalizeEmailR
  |> updateDbR
  |> sendEmailR
  |> loggerR
  |> returnMessageR translateError_FR


// test
goodRequest |> updateCustomerR

badRequest1 |> updateCustomerR

unsendableRequest |> updateCustomerR


