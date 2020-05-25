// ================================================
// Exercise:
// Use Result.mapError so that functions with different error
// types can be chained together
// ================================================

// Some data to validate
type Input = {
   Name : string
   Email : string
}

type ValidationError =
    | NameIsBlank
    | EmailIsBlank

type DatabaseError =
    | CustomerNotFound


// ----------------------------------
// Some functions to chain together
// ----------------------------------

let checkNameNotBlank input =
  if input.Name = "" then
    Error NameIsBlank
  else
    Ok input

let checkEmailNotBlank input =
  if input.Email = "" then
    Error EmailIsBlank
  else
    Ok input

let updateDatabase input =
  if input.Name = "BadCustomer" then
    Error CustomerNotFound
  else
    Ok input

// ==============================
// Exercise: chain these functions together
// ==============================

// ----------------------------------
// define a common type with both errors

type WorkflowError =
| ValidationCase of ValidationError
| DatabaseCase of DatabaseError


// ----------------------------------
// build the workflow

let workflow input =

    // redefine the functions to use the common error type
    let checkNameNotBlank' input =
        input
        |> checkNameNotBlank
        |> Result.mapError ValidationCase

    let checkEmailNotBlank' input =
        input
        |> checkEmailNotBlank
        |> Result.mapError ValidationCase

    let updateDatabase' input =
        input
        |> updateDatabase
        |> Result.mapError DatabaseCase

    // now they can be chained
    input
    |> checkNameNotBlank'
    |> Result.bind checkEmailNotBlank'
    |> Result.bind updateDatabase'


// ----------------------------------
// test the workflow

let goodInput = {Name="Scott";Email="x@example.com"}
workflow goodInput

let blankName = {Name="";Email="x@example.com"}
workflow blankName

let blankEmail = {Name="Scott";Email=""}
workflow blankEmail

let badCustomer = {Name="BadCustomer";Email="x@example.com"}
workflow badCustomer



// ----------------------------------
// Alternative approach

let workflow_v2 input =

    // redefine the functions to use the common error type
    let checkNameNotBlank' = checkNameNotBlank >> Result.mapError ValidationCase
    let checkEmailNotBlank' = checkEmailNotBlank >> Result.mapError ValidationCase
    let updateDatabase' = updateDatabase >> Result.mapError DatabaseCase

    // now they can be chained
    input
    |> checkNameNotBlank'
    |> Result.bind checkEmailNotBlank'
    |> Result.bind updateDatabase'

