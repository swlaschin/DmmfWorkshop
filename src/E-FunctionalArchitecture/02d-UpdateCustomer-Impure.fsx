// =================================
// Exercise: Reimplement this code to move IO to the edges
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
// An example of an impure/non-deterministic implementation
// with async I/O mixed in with business logic
//========================================


let updateCustomer (newCustomer:Domain.Customer) = async {

    // await
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

