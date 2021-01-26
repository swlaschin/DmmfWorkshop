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
    type EmailAddress = private EmailAddress of string

    // Question: Is this overkill? Does everything have to be wrapped?
    // Question: When to use public vs.private constructors?
    // Answer: See "02g-ConstrainedTypes_FrequentQuestions.fsx"

    /// Define a helper module for EmailAddress that
    /// has access to the private constructor
    module EmailAddress =

        /// Expose a public "factory" function
        /// to construct a value, or return an error
        let create str =
            if String.IsNullOrEmpty(str) then
                None
            else if not (str.Contains("@")) then
                None
            else
                Some (EmailAddress str)

        /// Expose a public function
        /// to extract the wrapped value
        let value (EmailAddress str) = str

        // Alternative implementations to extract the wrapped value

        // remember that wrapping and unwrapping are symmetrical!
        // let emailAddress = (EmailAddress emailStr)  // wrap
        // let (EmailAddress emailStr) = emailAddress  // unwrap

        let value_v2 (emailAddress:EmailAddress) =
            let (EmailAddress str) = emailAddress // unwrap
            str
            // the unwrapping (EmailAddress str) is normally done directly in the parameter list though

open ConstrainedTypes

//TODO uncomment to see the compiler error
// let compileError = EmailAddress "a@example.com"

// create using the exposed constructor
let validEmailOpt = EmailAddress.create "a@example.com"
let invalidEmailOpt = EmailAddress.create "example.com"

let printWrappedValue emailOpt =
    // If we want to get the inner value out, we have to pattern match
    // the option
    match emailOpt with
    | Some email ->
        let inner = EmailAddress.value email
        printfn "The EmailAddress is valid and the wrapped value is %s" inner
    | None ->
        printfn "The value is None"

printWrappedValue validEmailOpt
printWrappedValue invalidEmailOpt

// --------------------------------------------
// EXAMPLE: Defining a workflow that uses constrained types as parameter
// --------------------------------------------
module CoreImplementation =

    // define a workflow that needs an email address parameter
    let mainWorkflow (emailAddress:EmailAddress) =
        // values are immutable and not null
        // so no defensive programming is needed.

        // Check to see if the emailAddress is null? NOT NEEDED
        // Uncomment below to see the compiler error
        // if emailAddress = null then failwith "null exception"

        // Check to see if the emailAddress contains "@"? NOT NEEDED
        // Uncomment below to see the check, but it will never fail.
        // if not ((EmailAddress.value emailAddress).Contains("@")) then failwith "invalid email"

        () // do nothing


// --------------------------------------------
// Example: Calling a workflow which needs constrained types
// --------------------------------------------

module WebServiceExample =

    (* ------- uncomment to see compiler error below

    // the main public API that wraps the workflow
    let myApi_v1 input =

        // create a value from the input (eg JSON)
        let emailAddressOption = EmailAddress.create input

        // If you try to call the workflow without checking if it is valid
        // you will get a compile-time error
        CoreImplementation.mainWorkflow emailAddressOption
    ------- *)

    // the main public API that wraps the workflow
    let myApi_v2 input =

        // ----------------------------------
        // Validation at the edges
        // ----------------------------------
        // create a value from the input (eg JSON)
        let emailAddressOption = EmailAddress.create input
        // lots of other validations here

        match emailAddressOption with
        | None ->
            // the input is not valid, so return an error
            "400 BadRequest"
        | Some emailAddress ->
            // otherwise the input IS valid, so call the workflow
            CoreImplementation.mainWorkflow emailAddress
            "200 OK"



// test
WebServiceExample.myApi_v2 "example.com"
WebServiceExample.myApi_v2 "x@example.com"


// --------------------------------------------
// Compare two ways of constraining a value
// 1. using a type
// 2. using a validation attribute
// See "02g-ConstrainedTypes_FrequentQuestions.fsx"
// --------------------------------------------

type Contact = {
    // 1. putting the validation in the type
    Email: EmailAddress

    // 2. putting the validation in a property attribute
    //[Validation(EmailAddress)]
    Email_v2: string

    }
