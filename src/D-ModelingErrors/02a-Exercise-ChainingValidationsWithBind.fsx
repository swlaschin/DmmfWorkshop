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
    |> ??
    |> ??
    |> ??


// -------------------------------
// test that the validation works
// -------------------------------

let goodInput = {Name="Scott";Email="x@example.com"}
validateInput goodInput

let blankName = {Name="";Email="x@example.com"}
validateInput blankName

let blankEmail = {Name="Scott";Email=""}
validateInput blankEmail


//-------------------------------------
// Exercise: add some more validation functions
//-------------------------------------
// examples:
// * email contains @ symbol  -- use Email.Contains("@")
// * email length < 50        -- use Email.Length

