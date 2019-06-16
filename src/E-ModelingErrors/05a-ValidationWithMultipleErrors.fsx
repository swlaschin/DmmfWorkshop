(*

Railway oriented programming -- with error type
*)

#load "Result.fsx"

type Request = {
    UserId: int
    Name: string
    Email: string
}

type ValidationError =
  | UserIdMustBePositive
  | NameMustNotBeBlank
  | NameMustNotBeLongerThan of int
  | EmailMustNotBeBlank

type ErrorMessage =
  | ValidationError of ValidationError list
  | SmtpServerError of string


let nameNotBlank name =
  if name= "" then
    Error NameMustNotBeBlank
  else
    Ok name

let name50 name =
  if String.length name > 50 then
    Error (NameMustNotBeLongerThan 50)
  else
    Ok name

let emailNotBlank email =
  if email = "" then
    Error EmailMustNotBeBlank
  else
    Ok email

/// Validate the UserId -- this always works
let validateUserId id =
    if id > 0 then
        Ok id
    else
        Error UserIdMustBePositive
    |> Validation.ofResult

/// Validate the Name -- this might fail
let validateName name =
    name
    |> nameNotBlank
    |> Result.bind name50
    |> Validation.ofResult

/// Validate the Email -- this might fail
let validateEmail email =
    email
    |> emailNotBlank
    |> Validation.ofResult

let (<!>) = Validation.map
let (<*>) = Validation.apply

let validateRequest req =
    let createRequest userId name email =
       {UserId=userId; Name= name; Email=email }

    createRequest
    <!> (validateUserId req.UserId)
    <*> (validateName req.Name)
    <*> (validateEmail req.Email)
    |> Result.mapError ValidationError


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
  Email=""
}
badRequest1 |> validateRequest




