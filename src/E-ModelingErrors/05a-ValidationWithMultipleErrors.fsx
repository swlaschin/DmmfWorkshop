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
  | DbError of string
  | SmtpServerError of string


let nameNotBlank name : Validation<string,ValidationError> =
  if name= "" then
    Error [NameMustNotBeBlank]
  else
    Ok name

let name50 name : Validation<string,ValidationError> =
  if String.length name > 50 then
    Error [NameMustNotBeLongerThan 50]
  else
    Ok name

let emailNotBlank email : Validation<string,ValidationError> =
  if email = "" then
    Error [EmailMustNotBeBlank]
  else
    Ok email

/// Validate the UserId -- this always works
let validateUserId id : Validation<int,ValidationError> =
    if id > 0 then
        Ok id
    else
        Error [UserIdMustBePositive]

/// Validate the Name -- this might fail
let validateName name =
    name
    |> nameNotBlank
    |> Validation.bind name50

/// Validate the Email -- this might fail
let validateEmail email =
    email
    |> emailNotBlank

let validateRequest req : Result<Request,ErrorMessage> =

    // a "constructor" function
    let createRequest userId name email =
       {UserId=userId; Name= name; Email=email }

    // the validated components
    let userIdOrError  = validateUserId req.UserId
    let nameOrError = validateName req.Name
    let emailOrError = validateEmail req.Email

    // uncomment to see this this fail...
    //createRequest userIdOrError nameOrError emailOrError

    // option1 -- use the special operators
    let ( <!> ) = Validation.map
    let ( <*> ) = Validation.apply
    let requestOrError =
        createRequest <!> userIdOrError <*> nameOrError <*> emailOrError

    // NOTE: option1 is equivalent to this ugly code
    let requestOrError2 =
        Validation.apply (Validation.apply (Validation.map createRequest userIdOrError) nameOrError) emailOrError

    // option2 -- use the "lift3" function
    // (because there are three parameters)
    let requestOrError3 = (Validation.lift3 createRequest) userIdOrError nameOrError emailOrError

    // convert back into a normal Result
    requestOrError |> Result.mapError ValidationError


// -------------------------------
// test data
// -------------------------------

let goodRequest = {
  UserId=1
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




