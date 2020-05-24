// =================================
// This file demonstrates how to define and construct constrained types
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System

module ConstrainedTypes =

    /// Define a wrapper type with a *private* constructor.
    /// Only code in the same module can use this constructor now.
    type String10 = private String10 of string

    /// Define a helper module for String10 that
    /// has access to the private constructor
    module String10 =

        /// Expose a public "factory" function
        /// to construct a value, or return an error
        let create str =
            if String.IsNullOrEmpty(str) then
                None
            else if str.Length > 10 then
                None
            else
                Some (String10 str)

        /// Expose a public function
        /// to extract the wrapped value
        let value (String10 str) = str


    /// Define a wrapper type with a *private* constructor.
    /// Only code in the same module can use this constructor now.
    type EmailAddress = private EmailAddress of string

    /// Define a helper module for EmailAddress that
    /// has access to the private constructor
    module EmailAddress =

        /// Expose a public "factory" function
        /// to construct a value, or return an error
        let create str =
            if String.IsNullOrEmpty(str) then
                None
            else if str.Contains("@") |> not then
                None
            else
                Some (EmailAddress str)

        /// Expose a public function
        /// to extract the wrapped value
        let value (EmailAddress str) = str

open ConstrainedTypes

//TODO uncomment to see the compiler error
// let compileError = String10 "1234567890"

// create using the exposed constructor
let validString10 = String10.create("1234567890")
let invalidString10 = String10.create("12345678901")

// create using the exposed constructor
let validEmail = EmailAddress.create("a@example.com")
let invalidEmail = EmailAddress.create("example.com")


// compare two ways of constraining a value
// 1. using a type
// 2. using a validation attribute
type Contact = {
    // 1. putting the validation in the type
    Email: EmailAddress

    // 2. putting the validation in a property attribute
    //[Validation(EMailAddress)]
    Email2: string

    }
