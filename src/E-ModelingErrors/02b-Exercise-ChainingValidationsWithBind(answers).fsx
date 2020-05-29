// ================================================
// Exercise:
// Chain validation functions using bind
// ================================================

// Some data to validate
type Input = {
   Name : string
   Email : string
}

//-------------------------------------
// Here are the validation functions
//-------------------------------------

let checkNameNotBlank input =
  if input.Name = "" then
    Error "Name must not be blank"
  else
    Ok input

let checkName50 input =
  if input.Name.Length > 50 then
    Error "Name must not be longer than 50 chars"
  else
    Ok input

let checkEmailNotBlank input =
  if input.Email = "" then
    Error "Email must not be blank"
  else
    Ok input

let checkEmailMustHaveAtSign input =
  if not (input.Email.Contains("@")) then
    Error "Email must have @ sign"
  else
    Ok input

//-------------------------------------
// Here is the custom bind function for Result
//-------------------------------------

/// NOTE: This is the same as the built-in Result.bind function.
let resultBind nextFunction result =

    match result with
    // if OK, apply the next function to the input
    | Ok input -> nextFunction input
    // otherwise leave alone
    | Error err -> Error err

//-------------------------------------
// Exercise -- chain these functions together using resultBind (or Result.bind)
//-------------------------------------

let validateInput input =
    input
    |> checkNameNotBlank
    |> resultBind checkName50
    |> resultBind checkEmailNotBlank
    |> Result.bind checkEmailMustHaveAtSign  // Can also use built-in Result.bind

    // add some more validations if you like

// The "bind" function has many different names in different contexts
// E.g "flatMap"
// For example, we could also call it "andThen" to make it read more easily
let andThen = Result.bind

// and here is the same code written using "andThen"
let validateInput_v2 input =
    input
    |> checkNameNotBlank
    |> andThen checkName50
    |> andThen checkEmailNotBlank
    |> andThen checkEmailMustHaveAtSign

// -------------------------------
// test that the validation works
// -------------------------------

let goodInput = {Name="Scott";Email="x@example.com"}
validateInput goodInput

let blankName = {Name="";Email="x@example.com"}
validateInput blankName

let blankEmail = {Name="Scott";Email=""}
validateInput blankEmail