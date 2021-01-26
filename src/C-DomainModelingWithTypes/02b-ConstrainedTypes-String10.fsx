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

    // Question: Is this overkill? Does everything have to be wrapped?
    // Question: When to use public vs.private constructors?
    // Answer: See "02g-ConstrainedTypes_FrequentQuestions.fsx"

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

        // Alternative implementations to extract the wrapped value

        // remember that wrapping and unwrapping are symmetrical!
        // let str10 = (String10 str)  // wrap
        // let (String10 str) = str10  // unwrap

        let value_v2 (str10:String10) =
            let (String10 str) = str10  // unwrap
            str
            // the unwrapping (String10 str) is normally done directly in the parameter list though


open ConstrainedTypes

//TODO uncomment to see the compiler error
// let compileError = String10 "1234567890"

// create using the exposed constructor
let validString10Opt = String10.create "1234567890"
let invalidString10Opt = String10.create "12345678901"

let printWrappedValue emailOpt =
    // If we want to get the inner value out, we have to pattern match
    // the option
    match emailOpt with
    | Some string10 ->
        let inner = String10.value string10
        printfn "The String10 is valid and the wrapped value is %s" inner
    | None ->
        printfn "The value is None"

printWrappedValue validString10Opt
printWrappedValue invalidString10Opt





// --------------------------------------------
// EXAMPLE: Defining a workflow that uses constrained types as parameter
// --------------------------------------------
module CoreImplementation =

    // define a dummy workflow
    let mainWorkflow (str10:String10) =
        // values are immutable and not null
        // so no defensive programming is needed.

        // Check to see if the str10 null? NOT NEEDED
        // Uncomment below to see the compiler error
        // if str10 = null then failwith "null exception"

        // Check to see if the str10.Length <= 10? NOT NEEDED
        // Uncomment below to see the check, but it will never fail.
        // if (String10.value str10).Length > 10 then failwith "invalid string"

        () // do nothing


// --------------------------------------------
// Example: Calling a workflow which needs constrained types
// --------------------------------------------

module WebServiceExample =

    (* ------- uncomment to see compiler error below

    // the main public API that wraps the workflow
    let myApi_v1 input =

        // create a value from the input (eg JSON)
        let string10Option = String10.create input

        // If you try to call the workflow without checking if it is valid
        // you will get a compile-time error
        CoreImplementation.mainWorkflow string10Option
    ------- *)

    // the main public API that wraps the workflow
    let myApi_v2 input =

        // ----------------------------------
        // Validation at the edges
        // ----------------------------------
        // create a value from the input (eg JSON)
        let string10Option = String10.create input
        // lots of other validations here

        match string10Option with
        | None ->
            // the input is not valid, so return an error
            "400 BadRequest"
        | Some string10 ->
            // otherwise the input IS valid, so call the workflow
            CoreImplementation.mainWorkflow string10
            "200 OK"



// test
WebServiceExample.myApi_v2 "1234567890"
WebServiceExample.myApi_v2 "12345678901"


// --------------------------------------------
// Compare two ways of constraining a value
// 1. using a type
// 2. using a validation attribute
// See "02g-ConstrainedTypes_FrequentQuestions.fsx"
// --------------------------------------------

type Contact = {
    // 1. putting the validation in the type
    FirstName: String10

    // 2. putting the validation in a property attribute
    //[Validation(MaxLen(10))]
    FirstName_v2: string

    }
