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

module ImpureImplementation =

    let updateCustomer (newCustomer:Domain.Customer) =

        let existingCustomer = CustomerDatabase.readCustomer newCustomer.Id

        // check for changes
        if (existingCustomer.Name <> newCustomer.Name) ||
           (existingCustomer.EmailAddress <> newCustomer.EmailAddress) then
            // store updated customer
            CustomerDatabase.updateCustomer newCustomer

        // send verification email if email changed
        if (existingCustomer.EmailAddress <> newCustomer.EmailAddress) then
            let emailMessage : EmailServer.EmailMessage = {
                EmailAddress = newCustomer.EmailAddress
                EmailBody = "Please verify your new email"
                }
            EmailServer.sendMessage emailMessage

module PureImplementation =

    // Exercise: Reimplement the code above to move all IO to the edges

    // TIP: define a structure to represent the three results:
    //  1. no change
    //  2. update customer
    //  3. update customer and also send email
    type WorkflowResult =
        | NoChange
        | CustomerUpdated of Domain.Customer * EmailServer.EmailMessage option
        // NOTE I used an option here for the email,
        // but I could defined a three-choice type instead

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

module PureImplementation_Shell =
    open PureImplementation

    // Does IO, then calls the pure business logic
    let updateCustomer (newCustomer:Domain.Customer) =

        // impure
        let existingCustomer = CustomerDatabase.readCustomer newCustomer.Id

        // pure business logic
        let result = PureImplementation.updateCustomer existingCustomer newCustomer

        // impure
        match result with
        | NoChange ->
            ()  // do nothing

        | CustomerUpdated (newCustomer,None) ->
            CustomerDatabase.updateCustomer newCustomer

        | CustomerUpdated (newCustomer,Some emailMessage) ->
            CustomerDatabase.updateCustomer newCustomer
            EmailServer.sendMessage emailMessage