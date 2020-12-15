// =================================
// Exercise: Reimplement the code from the previous impure example
// by moving the IO to the edges and making it pure.
// =================================


(*
===========================================
Use-case: Update the name and email in a user account
===========================================

1. Retrieve the existingCustomer from the database
2. Compare the newCustomer data to the existingCustomer
3a. If there are no changes, do nothing
3b. If either the name or email has changed, update the database
4. If the email has changed, also send a verification email to the new address.

*)

// the domain and infrastructure code for this example
#load "IoExample.fsx"
open IoExample
open IoExample.Domain

//========================================
// A completely pure/deterministic implementation
// with no async or I/O. This will be easy to test!
//========================================

module PureCore =

    // Exercise: Reimplement the code from the previous impure example
    // by moving the IO to the edges and making it pure.

    // TIP: define a structure to represent the three results:
    //  1. no change
    //  2. update customer
    //  3. update customer and also send email
    type WorkflowResult =
        | NoChange
        | ??

    // Pure business logic -- decisions only -- no I/O
    let updateCustomer (newCustomer:Domain.Customer) (existingCustomer:Domain.Customer) : WorkflowResult =
        // 1. decide whether a verification email should be sent
        // 2. decide whether the database should be updated

        // Exercise -- implement this logic without any IO
        ???



//========================================
// test the pure code
//========================================

// the test framework
#load "Expecto.fsx"
open Expecto

module MyTests =
    open PureCore

    let existingCustomer =
        {Id=CustomerId 1; EmailAddress=EmailAddress "x@example.com"; Name="Alice"}

    let test1 = testCase "No change" <| fun () ->
        let newCustomer = existingCustomer // no change
        let result = PureCore.updateCustomer newCustomer existingCustomer
        match result with
        | no change -> ()
        | _ -> Expecto.Api.failtest "Expected No Change"

    let test2 = testCase "Changed name only" <| fun () ->
        let newCustomer =
            {Id=CustomerId 1; EmailAddress=EmailAddress "x@example.com"; Name="Bob"}
        let result = PureCore.updateCustomer newCustomer existingCustomer
        match result with
        | ??? _ -> ()
        | _ -> Expecto.Api.failtest "Expected customer updated"

    let test3 = testCase "Changed name and email" <| fun () ->
        let newCustomer =
            {Id=CustomerId 1; EmailAddress=EmailAddress "z@example.com"; Name="Bob"}
        let result = PureCore.updateCustomer newCustomer existingCustomer
        match result with
        | ?? _ -> ()
        | _ -> Expecto.Api.failtest "Expected customer updated and email sent"

    let allTests = testList "updateCustomer" [test1; test2; test3]

// run the tests!
Expecto.Api.runTest MyTests.test1
Expecto.Api.runTest MyTests.test2
Expecto.Api.runTest MyTests.test3
Expecto.Api.runTest MyTests.allTests

//========================================
// Impure "Shell"/API layer
//========================================


module Shell =

    open PureCore

    // Does IO, then calls the pure business logic
    let updateCustomer (newCustomer:Domain.Customer) = async {

        // impure
        let! existingCustomer = CustomerDatabase.readCustomer newCustomer.Id

        // pure business logic
        let result = PureCore.updateCustomer ??

        // impure
        match result with
        | NoChange ->
            ()  // do nothing
        | ?? ->
           // Exercise -- implement the remaining IO

        }