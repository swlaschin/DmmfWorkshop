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
ResultWithOption.validateInput {Name="Scott"; Email=""}  // None

//--------------------------------------------------
// Using a Result with a string to indicate an error.
// Helpful but "stringly-typed" (not statically typed!)
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
   // Error "Name must not be blank"
ResultWithString.validateInput {Name="Scott"; Email=""}
   // Error "Email must not be blank"


//--------------------------------------------------
// Using a Result with a special choice type
// to indicate an error.
// Helpful and properly type checked (unlike strings).
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
// Error EmailMustNotBeBlank

//========================================================
// Question: How can you keep the errors for each subsystem separate?
//========================================================

module CompoundErrors =

    // errors for validation subsystem
    type ValidationError =
        | NameMustNotBeBlank
        | EmailMustNotBeBlank

    // errors for DB subsystem
    type DbError =
        | UserNotFound
        | DuplicateKey
        | AuthenticationError

    // This type combines all the errors for all subsystems
    type WorkflowError =
        | Validation of ValidationError
        | Db of DbError


    // An alternative is to "collapse" the subsystem errors to something simpler
    //type WorkflowError_v2 =
    //    | Validation of ValidationError
    //    | SystemError of string // convert detailed error to a simpler one

    // how to handle nested error types
    let handleError err =
        match err with
        | Validation vError ->
            match vError with
            | NameMustNotBeBlank -> ()
            | EmailMustNotBeBlank -> ()
        | Db dbError -> 
            match dbError with
            | UserNotFound -> ()
            | DuplicateKey -> ()
            | AuthenticationError -> ()
