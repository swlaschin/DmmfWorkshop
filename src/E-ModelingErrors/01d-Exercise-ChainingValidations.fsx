// ================================================
// Exercise:
// Some validation functions that should be chained together
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
// Exercise -- chain these functions together
//
// This might be hard unless you have
// seen this done before.
//
// I will be showing a MUCH easier way shortly!
//-------------------------------------


let validateInput input =
    input
    |> checkNameNotBlank
    |> ??  // checkName50
    |> ??  // checkEmailNotBlank
    |> ??  // checkEmailMustHaveAtSign

    // add some more validations if you like

// -------------------------------
// test that the validation works
// -------------------------------

let goodInput = {Name="Scott";Email="x@example.com"}
validateInput goodInput

let blankName = {Name="";Email="x@example.com"}
validateInput blankName

let blankEmail = {Name="Scott";Email=""}
validateInput blankEmail