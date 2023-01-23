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
        | CustomerUpdated of Domain.Customer
        | CustomerUpdatedWithNewEmail of Domain.Customer * EmailServer.EmailMessage

    // Alternative design #1
    // We could have used an option here for the email message,
    // and so there would be two choice instead
    (*
    type WorkflowResult =
       | NoChange
       | CustomerUpdated of Domain.Customer * EmailServer.EmailMessage option
    *)

    // Alternative design #2
    // We could return a list all the things that can need to happen in the IO
    (*
    type Decision =
        | UpdateCustomer of Domain.Customer
        | SendEmail of EmailServer.EmailMessage

    type WorkflowResult = Decision list
    *)

    // Pure business logic -- decisions only -- no I/O
    let updateCustomer (newCustomer:Domain.Customer) (existingCustomer:Domain.Customer) :WorkflowResult =

        // 1. decide whether the database should be updated
        let updateCustomer =
            (existingCustomer.Name <> newCustomer.Name) ||
            (existingCustomer.EmailAddress <> newCustomer.EmailAddress)

        // 2. decide whether a verification email should be sent
        let emailMessageOption =
            if (existingCustomer.EmailAddress <> newCustomer.EmailAddress) then
                let emailMessage : EmailServer.EmailMessage = {
                    EmailAddress = newCustomer.EmailAddress
                    EmailBody = "Please verify your new email"
                    }
                Some emailMessage
            else
                None

        // 3. return the result
        if not updateCustomer then
            // signal that there was no change
            NoChange
        else
            match emailMessageOption with
            | None ->
                // signal that the customer should be updated
                CustomerUpdated newCustomer
            | Some emailMsg ->
                // signal that the customer should be updated and the email should be sent
                CustomerUpdatedWithNewEmail (newCustomer, emailMsg)


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
        | NoChange -> ()
        | _ -> Expecto.Api.failtest "Expected NoChange"

    let test2 = testCase "Changed name only" <| fun () ->
        let newCustomer =
            {Id=CustomerId 1; EmailAddress=EmailAddress "x@example.com"; Name="Bob"}
        let result = PureCore.updateCustomer newCustomer existingCustomer
        match result with
        | CustomerUpdated _ -> ()
        | _ -> Expecto.Api.failtest "Expected CustomerUpdated"

    let test3 = testCase "Changed name and email" <| fun () ->
        let newCustomer =
            {Id=CustomerId 1; EmailAddress=EmailAddress "z@example.com"; Name="Bob"}
        let result = PureCore.updateCustomer newCustomer existingCustomer
        match result with
        | CustomerUpdatedWithNewEmail _ -> ()
        | _ -> Expecto.Api.failtest "Expected CustomerUpdatedWithNewEmail"

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
        let result = PureCore.updateCustomer existingCustomer newCustomer

        // impure
        match result with
        | NoChange ->
            ()  // do nothing

        | CustomerUpdated newCustomer ->
            do! CustomerDatabase.updateCustomer newCustomer

        | CustomerUpdatedWithNewEmail (newCustomer,emailMessage) ->
            do! CustomerDatabase.updateCustomer newCustomer
            do! EmailServer.sendMessage emailMessage

        }