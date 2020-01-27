
// ================================================
// Worked example: Modeling email state transition
//
// See email_transition_diagram.png
//
// ================================================

(*
All the state transitions are going to work the same way.

1) Start with the domain types that are independent of state
2) Create the "API"
  2a) Create a type to represent the data stored for each type
  2b) Create a type that represent the choice of all the states
  2c Create transition functions that transition from one state type to another
     These functions take as input:
       * a state type
       * and maybe some extra data
     These functions output:
       * a new state type
       * or maybe a state OPTION if the transition might not work

3) Clients then write functions using the state union type and the "API"

*)

// Let's see this in action using the email transitions as our example

// 1) Start with the domain types that are independent of state
module EmailDomainTypes =

    type EmailAddress =
        EmailAddress of string

// 2) Create the "API"
module EmailApi =
    open EmailDomainTypes

    // 2a) Create a type to represent the data stored for each type
    type VerifiedEmail =
        VerifiedEmail of EmailAddress

    // 2b) Create a type that represent the choice of all the states
    type EmailContactInfo =
        | UnverifiedState of EmailAddress
        | VerifiedState of VerifiedEmail


    // 2c) Create transition functions that transition from one *individual* state to the next
    module EmailVerificationService =

        let verify email hash =
            if hash="OK" then
                Some (VerifiedEmail email)
            else
                None

// 3) Clients write functions using the state union type
module EmailApiClient =
    open EmailDomainTypes
    open EmailApi

    // Rule: "You can't send a verification message to a verified email"
    let sendVerificationMessage state =
        match state with
        | UnverifiedState email ->
            printfn "Sending verification message to %A" email
        | VerifiedState _ ->
            printfn "Already verified"

    // Rule: "You can't send a password reset message to a unverified email "
    let sendPasswordResetMessage state =
        match state with
        | UnverifiedState email ->
            printfn "Not verified. Can't send"
        | VerifiedState (VerifiedEmail email) ->
            printfn "Sending password reset message to %A" email

// Examples
open EmailDomainTypes
open EmailApi
open EmailApiClient

let emailAddress = EmailAddress "abc@example.com"

let unverifiedEmailState =
    UnverifiedState emailAddress

let verifiedEmailState =
    let hash = "OK"
    let verifiedEmailOpt = EmailVerificationService.verify emailAddress hash
    let verifiedEmail = verifiedEmailOpt.Value // WARNING don't extract the value like this in production code! 
                                               // It's ok for interactive exploration, but in production code
                                               // always do pattern matching on Options.
                                               
    VerifiedState verifiedEmail

sendVerificationMessage unverifiedEmailState
sendVerificationMessage verifiedEmailState

sendPasswordResetMessage unverifiedEmailState
sendPasswordResetMessage verifiedEmailState

