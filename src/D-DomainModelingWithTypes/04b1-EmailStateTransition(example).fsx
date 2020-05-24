// =================================
// This file demonstrates how to define EmailAddress state machine
//
// See 04b0-EmailStateTransition(diagram).png
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

(*
All the state transitions are going to work the same way.

1) Start with the domain types that are independent of state
2) Create the "API"
  2a) Create a type to represent the data stored for each type
  2b) Create a type that represents the union of all the states
  2c) Create transition functions that transition from one state type to another
     These functions take as input:
       * the data associated with a state
       * and maybe some extra data
     These functions output:
       * a new state 
       * or maybe a state OPTION if the transition might not work

3) Clients then write functions using the state union type and the "API"

*)

// Let's see this in action using the email transitions as our example

// -----------------------------------------------
// Model the domain types that are independent of state
// -----------------------------------------------
module EmailDomain =

    type EmailAddress =
        EmailAddress of string

module VerificationService =
    open EmailDomain

    type VerifiedEmail =
        VerifiedEmail of EmailAddress

    let verify email hash =
        if hash="OK" then
            Some (VerifiedEmail email)
        else
            None

// -----------------------------------------------
// Model the state machine with a type and transitions
// This is the "API" for the state machine
// -----------------------------------------------

module EmailApi =
    open EmailDomain
    open VerificationService

    // Create a type that represent the choice of all the states
    type EmailAddressState =
        | UnverifiedState of EmailAddress
        | VerifiedState of VerifiedEmail


    // Define the initial state
    let initialState emailAddress =
        UnverifiedState emailAddress

    // Create transition functions that transition from one *individual* substate to
    // a new state as a whole
    //
    // Eg in this case, we transition from EmailAddress->EmailAddressState
    // not from EmailAddressState->EmailAddressState
    //
    // Leave it to the client to handle invalid state transitions
    let transitionToVerified hash emailAddress =
        match VerificationService.verify emailAddress hash with
        | Some verifiedEmail ->
            // new state
            Some (VerifiedState verifiedEmail)
        | None ->
            // stay in same state
            printfn "Transition failed"
            None



// -----------------------------------------------
// The client can then use the API to access the current state
// and to transition to new states
// -----------------------------------------------

module EmailApiClient =
    open EmailDomain
    open VerificationService
    open EmailApi

    // Rule: "You can't send a verification message to a verified email"
    let sendVerificationMessage state =
        match state with
        | UnverifiedState email ->
            printfn "Sending verification message to %A" email
        | VerifiedState _ ->
            printfn "Won't send message because already verified"

    // Rule: "You can't send a password reset message to a unverified email "
    let sendPasswordResetMessage state =
        match state with
        | UnverifiedState email ->
            printfn "Won't send message because not verified."
        | VerifiedState (VerifiedEmail email) ->
            printfn "Sending password reset message to %A" email

    // Try to transition into the verified state"
    let verify hash state =
        match state with
        | UnverifiedState email ->
            match EmailApi.transitionToVerified hash email with
            | Some newState ->
                newState
            | None ->
                printfn "Can't transition. Leaving state untouched"
                state
        | VerifiedState (VerifiedEmail email) ->
            printfn "Already verified. Ignoring"
            state

// -----------------------------------------------
// Example usage
// -----------------------------------------------

open EmailDomain

// initial state
let emailAddress = EmailAddress "abc@example.com"
let unverifiedEmailState = EmailApi.initialState emailAddress

// successful transition
let goodHash = "OK"
let verifiedEmailState = EmailApiClient.verify goodHash unverifiedEmailState

// failed transition
let badhash = "BAD"
let stillUnverifiedEmailState = EmailApiClient.verify badhash unverifiedEmailState

// failed transition
let stillVerifiedEmailState = EmailApiClient.verify goodHash verifiedEmailState


// send a message
EmailApiClient.sendVerificationMessage unverifiedEmailState
EmailApiClient.sendVerificationMessage verifiedEmailState

// send a message
EmailApiClient.sendPasswordResetMessage unverifiedEmailState
EmailApiClient.sendPasswordResetMessage verifiedEmailState

