// =================================
// This file demonstrates how to define different kinds of errors
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

type Input = {
   Name : string
   Email : string
}

module ResultWithString =

    let validateInput input =
       if input.Name = "" then
          Error "Name must not be blank"
       else if input.Email = "" then
          Error "Email must not be blank"
       else
          Ok input  // happy path

    // function signature is
    // val validateInput : input:Input -> Result<Input,string>


// test
ResultWithString.validateInput {Name="Scott"; Email="scott@example.com"}
ResultWithString.validateInput {Name=""; Email="scott@example.com"}
ResultWithString.validateInput {Name="Scott"; Email=""}


//============================================
// The same validation function implemented
// using a special ValidationError type rather
// than strings
//============================================


module ResultWithErrorType =

    type ValidationError =
        | NameMustNotBeBlank
        | EmailMustNotBeBlank

    let validateInput input =
       if input.Name = "" then
          Error NameMustNotBeBlank
       else if input.Email = "" then
          Error EmailMustNotBeBlank
       else
          Ok input  // happy path

    // function signature is
    // validateInput : input:Input -> Result<Input,ValidationError>

// test
ResultWithErrorType.validateInput {Name="Scott"; Email="scott@example.com"}
ResultWithErrorType.validateInput {Name=""; Email="scott@example.com"}
ResultWithErrorType.validateInput {Name="Scott"; Email=""}
