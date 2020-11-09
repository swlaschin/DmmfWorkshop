// =================================
// This file contains the domain code from the "FP Toolkit" slides
//
// =================================

open System

// ========================================
// Combining all the tools
// ========================================

/// Define types and validation for this domain
module Domain =

    type Name = private Name of string
    type EmailAddress = private EmailAddress of string
    type Birthdate = private Birthdate of System.DateTime

    type Customer = {
        Name: Name
        Email: EmailAddress
        Birthdate : Birthdate
    }

    /// Errors just for Validation
    type ValidationError =
        | NameMustNotBeBlank
        | NameMustNotBeLongerThan of int
        | EmailMustNotBeBlank
        | EmailMustHaveAtSign
        | BirthdateMustBeInPast
        | InvalidBirthdate

    /// Errors for the workflow as a whole (not used in this example)
    type WorkflowError =
      // Validation now contains a LIST of errors
      | ValidationErrors of ValidationError list
      // other errors are singles
      | DbError of string
      | SmtpServerError of string

    module Name =
        let create s =
            if String.IsNullOrEmpty(s) then
                Error NameMustNotBeBlank
            elif s.Length > 20 then
                Error (NameMustNotBeLongerThan 20)
            else
                Ok (Name s)

    module EmailAddress =
        let create s =
            if String.IsNullOrEmpty(s) then
                Error EmailMustNotBeBlank
            elif not (s.Contains("@")) then
                Error EmailMustHaveAtSign
            else
                Ok (EmailAddress s)

    module Birthdate =
        let create (str:string) =
            match DateTime.TryParse(str) with
            | true,d ->
                if d < DateTime.Now then
                    Ok (Birthdate d)
                else
                    Error BirthdateMustBeInPast
            | false,_ ->
                Error InvalidBirthdate

