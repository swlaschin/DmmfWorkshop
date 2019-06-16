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
  | ??   // name not blank
  | ?? of int  // name not longer than
  | ??   // email not longer than


let nameNotBlank input =
  if input.Name = "" then
    Error ??
  else
    Ok input

let name50 input =
  if input.Name.Length > 50 then
    Error (?? 50)
  else
    Ok input

let emailNotBlank input =
  if input.Email = "" then
    Error ??
  else
    Ok input


let validateRequest input =
  input
  |> nameNotBlank
  |> Result.bind name50
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

let badRequest1 = {
  UserId=0
  Name= ""
  Email="abc@example.com"
}
badRequest1 |> validateRequest

let unsendableRequest = {
  UserId=0
  Name= "Alice"
  Email="ABC@example.COM"
}
unsendableRequest |> validateRequest

// ------------------------
// Add another step
// ------------------------

// trim spaces and lowercase
let canonicalizeEmail input =
   { input with Email = input.Email.Trim().ToLower() }

let canonicalizeEmailR =
  Result.map canonicalizeEmail

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

let tee f result =
  f result
  result

let updateDbR =
  Result.map (tee updateDb)

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

let catch exceptionThrowingFunction handler oneTrackInput =
    try
        Ok (exceptionThrowingFunction oneTrackInput)
    with
    | ex ->
        Error (handler ex)

let catchR exceptionThrowingFunction handler twoTrackInput =
    let catch' = catch exceptionThrowingFunction handler
    twoTrackInput
    |> Result.bind catch'


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

// obviously the real ones would use resource files!

let translateError_EN err =
    match err with
    | ?? ->
        "Name must not be blank"
    | ?? i ->
        sprintf "Name must not be longer than %i chars" i
    | ?? ->
        "Email must not be blank"
    | SmtpServerError msg ->
        sprintf "SmtpServerError [%s]" msg

let translateError_FR err =
    match err with
    | ?? ->
        "Nom ne doit pas être vide"
    | ?? i ->
        sprintf "Nom ne doit pas être plus long que %i caractères" i
    | ?? ->
        "Email doit pas être vide"
    | SmtpServerError msg ->
        sprintf "SmtpServerError [%s]" msg

// ------------------------
// return the response
// ------------------------


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


