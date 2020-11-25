// =================================
// This file is part of a complete app example.
// Part 1. The domain
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

//===============================================
// The Domain 
//===============================================

/// Define types and validation for an Email Service
module EmailServiceDomain =

    // ------------------------
    // Common constrained types
    // ------------------------

    type UserId = private UserId of int
    type EmailAddress = private EmailAddress of string
    type EmailBody = private EmailBody of string


    // ------------------------
    // Input and output for a workflow
    // ------------------------

    /// A request to send an email
    type Request = {
        UserId: UserId
        FromAddress: EmailAddress
        ToAddress: EmailAddress
        EmailBody: EmailBody
    }

    /// The response from sending an email
    type Response = {
        ResponseId: int
        EmailMessage: string
    }

    /// Errors just for Validation
    type ValidationError =
        | UserIdMustBePositive
        | EmailAddressMustNotBeBlank of fieldName:string
        | EmailAddressMustHaveAtSign of fieldName:string
        | EmailBodyMustNotBeBlank
        | EmailBodyMustNotBeLongerThan of int

    /// Errors for the workflow as a whole (not used in this example)
    type WorkflowError =
      // Validation now contains a LIST of errors
      | ValidationErrors of ValidationError list
      // other errors are single errors
      | DbError of string
      | SmtpServerError of string

    // ------------------------
    // The workflows in the domain (only one here!)
    // ------------------------

    /// Send an email message, and get a response or an error
    type SendAMessage = Request -> Result<Response,WorkflowError>

    // -------------------------------------
    // Support modules for constrained types in the domain
    // -------------------------------------
    module UserId =
        let create id =
            if id > 0 then
                Ok (UserId id)
            else
                Error UserIdMustBePositive

    module EmailAddress =
        let create fieldName str =
            if String.IsNullOrEmpty(str) then
                Error (EmailAddressMustNotBeBlank fieldName)
            elif not (str.Contains("@")) then
                Error (EmailAddressMustHaveAtSign fieldName)
            else
                Ok (EmailAddress str)

        let value (EmailAddress str) =
            str

    module EmailBody =
        let create str =
            if String.IsNullOrEmpty(str) then
                Error EmailBodyMustNotBeBlank
            elif str.Length > 20 then
                Error (EmailBodyMustNotBeLongerThan 20)
            else
                Ok (EmailBody str)

        let value (EmailBody str) =
            str

