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