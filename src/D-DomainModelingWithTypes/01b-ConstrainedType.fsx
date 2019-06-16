open System

module ConstrainedTypes =

    type String10 = private String10 of string

    module String10 =
    /// Public constructor
        let create str =
            if String.IsNullOrEmpty(str) then
                None
            else if str.Length > 10 then
                None
            else
                Some (String10 str)

        /// Property
        let value (String10 str) = str


    type EmailAddress = private EmailAddress of string

    module EmailAddress =
        /// Public constructor
        let create str =
            if String.IsNullOrEmpty(str) then
                None
            else if str.Contains("@") |> not then
                None
            else
                Some (EmailAddress str)

        /// Property
        let value (EmailAddress str) = str

open ConstrainedTypes

let compileError = String10 "1234567890"

let valid = String10.create("1234567890")
let invalid = String10.create("12345678901")

let validEmail = EmailAddress.create("a@example.com")
let invalidEmail = EmailAddress.create("example.com")
