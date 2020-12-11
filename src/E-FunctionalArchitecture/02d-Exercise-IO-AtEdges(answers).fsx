// =================================
// Exercise: Reimplement code to move IO to the edges
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

#load "IoExample.fsx"
open IoExample
open IoExample.Domain


// -------------------------
// A impure/non-deterministic implementation
// with async I/O mixed in with business logic
// -------------------------


module ImpureImplementation =

    let updateCustomer (newCustomer:Domain.Customer) = async {

        let! existingCustomer = CustomerDatabase.readCustomer newCustomer.Id
            // use let! when there is a return value in the Async

        // check for changes
        if (existingCustomer.Name <> newCustomer.Name) ||
           (existingCustomer.EmailAddress <> newCustomer.EmailAddress) then
            // store updated customer
            do! CustomerDatabase.updateCustomer newCustomer
                // use do! when there is no return value in the Async

        // send verification email if email changed
        if (existingCustomer.EmailAddress <> newCustomer.EmailAddress) then
            let emailMessage : EmailServer.EmailMessage = {
                EmailAddress = newCustomer.EmailAddress
                EmailBody = "Please verify your new email"
                }
            do! EmailServer.sendMessage emailMessage
        }

// -------------------------
// A completely pure/deterministic implementation
// with no async or I/O. This will be easy to test!
// -------------------------

module PureImplementation =

    // Exercise: Reimplement the code above to move all IO to the edges

    // TIP: define a structure to represent the three results:
    //  1. no change
    //  2. update customer
    //  3. update customer and also send email
    type WorkflowResult =
        | NoChange
        | CustomerUpdated of Domain.Customer * EmailServer.EmailMessage option
        // NOTE I used an option here for the email message,
        // but I could have defined a three-choice type instead
        // | NoChange
        // | CustomerUpdated of Domain.Customer
        // | CustomerUpdatedWithNewEmail of Domain.Customer * EmailServer.EmailMessage

    // alternative design:
    // have a list all the things that can need to happen in the IO
    type Decision =
        | UpdateCustomer of Domain.Customer
        | SendEmail of EmailServer.EmailMessage

    type Decisions = Decision list


    // Pure business logic -- decisions only -- no I/O
    let updateCustomer (newCustomer:Domain.Customer) (existingCustomer:Domain.Customer) :WorkflowResult =

        // 1. decide whether a verification email should be sent
        let emailMessageOption =
            if (existingCustomer.EmailAddress <> newCustomer.EmailAddress) then
                let emailMessage : EmailServer.EmailMessage = {
                    EmailAddress = newCustomer.EmailAddress
                    EmailBody = "Please verify your new email"
                    }
                Some emailMessage
            else
                None

        // 2. decide whether the database should be updated
        if (existingCustomer.Name <> newCustomer.Name) ||
           (existingCustomer.EmailAddress <> newCustomer.EmailAddress) then
            // signal that the customer should be updated
            CustomerUpdated (newCustomer,emailMessageOption)
        else
            // signal that there was no change
            NoChange


// -------------------------
// test the pure code
// -------------------------

/// this would be replaced with a proper test framework
module TestFramework =

    let expectAreEqual testName expected actual =
        if expected = actual then
            printfn "Test '%s': Passed" testName
        else
            let reason = sprintf "Expected=%A. Actual=%A" expected actual
            printfn "Test '%s': Failed '%s" testName reason

    let failed testName reason =
        printfn "Test '%s': Failed '%s" testName reason


/// My tests for the pure code
module MyTests =

    open PureImplementation

    let expectSameResultCase testName case expectedCust result =

        match case,result  with
        | "NoChange",NoChange ->
            () // passed

        | "CustomerUpdatedWithNoEmail",CustomerUpdated (custToUpdate,None) ->
            TestFramework.expectAreEqual testName expectedCust custToUpdate

        | "CustomerUpdatedWithEmail",CustomerUpdated (custToUpdate,Some emailToSend) ->
            TestFramework.expectAreEqual testName expectedCust custToUpdate
            // can't compare the email message because we dont wnat's in it, but we can check that
            // it contains the email address
            TestFramework.expectAreEqual testName expectedCust.EmailAddress emailToSend.EmailAddress

        | _ ->
            // all other cases are a mismatch
            let reason = sprintf "Expected case=%A. Actual result=%A" case result
            TestFramework.failed testName reason

    let existingCustomer =
        {Id=CustomerId 1; EmailAddress=EmailAddress "x@example.com"; Name="Alice"}
    let customerWithChangedNameOnly =
        {Id=CustomerId 1; EmailAddress=EmailAddress "x@example.com"; Name="Bob"}
    let customerWithChangedNameAndEmail =
        {Id=CustomerId 1; EmailAddress=EmailAddress "z@example.com"; Name="Bob"}

    let test1() =
        let result = PureImplementation.updateCustomer existingCustomer existingCustomer
        expectSameResultCase "When no change to customer expect no customer to update" "NoChange" existingCustomer result

    let test2() =
        let result = PureImplementation.updateCustomer customerWithChangedNameOnly existingCustomer
        expectSameResultCase "When only name changed expect a customer to update" "CustomerUpdatedWithNoEmail" customerWithChangedNameOnly result

    let test3() =
        let result = PureImplementation.updateCustomer customerWithChangedNameAndEmail existingCustomer
        expectSameResultCase "When name and email changed expect a customer to update and email to send" "CustomerUpdatedWithEmail" customerWithChangedNameAndEmail result

// run the tests!
(*
MyTests.test1()
MyTests.test2()
MyTests.test3()
*)




// -------------------------
// Impure "Shell"/API layer
// -------------------------

module PureImplementation_Shell =
    open PureImplementation

    // Does IO, then calls the pure business logic
    let updateCustomer (newCustomer:Domain.Customer) = async {
        // impure
        let! existingCustomer = CustomerDatabase.readCustomer newCustomer.Id

        // pure business logic
        let result = PureImplementation.updateCustomer existingCustomer newCustomer

        // impure
        match result with
        | NoChange ->
            ()  // do nothing

        | CustomerUpdated (newCustomer,None) ->
            do! CustomerDatabase.updateCustomer newCustomer

        | CustomerUpdated (newCustomer,Some emailMessage) ->
            do! CustomerDatabase.updateCustomer newCustomer
            do! EmailServer.sendMessage emailMessage

        }