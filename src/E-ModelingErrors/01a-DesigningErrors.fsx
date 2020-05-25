// =================================
// This file demonstrates how to define different kinds of errors
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

// Some data to validate
type Input = {
   Name : string
   Email : string
}

//--------------------------------------------------
// Using an Option to indicate an error.
// Not very helpful!
//--------------------------------------------------

module ResultWithOption =

    let validateInput input =
       if input.Name = "" then
          None
       else if input.Email = "" then
          None
       else
          Some input  // happy path

    // function signature is
    // val validateInput : input:Input -> Input option


// test
ResultWithOption.validateInput {Name="Scott"; Email="scott@example.com"}
ResultWithOption.validateInput {Name=""; Email="scott@example.com"}
ResultWithOption.validateInput {Name="Scott"; Email=""}

//--------------------------------------------------
// Using a Result with a string to indicate an error.
// Helpful but stringly-typed.
//--------------------------------------------------

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


//--------------------------------------------------
// Using a Result with a special choice type
// to indicate an error.
// Helpful and type checked.
//--------------------------------------------------

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
