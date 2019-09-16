// ================================================
// FSM Exercise: A simple 2-state transition for Unverified and Verified email
//
// See "Email transition diagram.png"
//
// ================================================

// Here are the EmailAddress types from the slide

module EmailDomain =

    type EmailAddress =
        EmailAddress of string

    type VerifiedEmail =
        VerifiedEmail of EmailAddress

    type EmailContactInfo =
        | Unverified of EmailAddress
        | Verified of VerifiedEmail


    // Create a function that (maybe) creates a VerifiedEmail
    let verify (hash:string) (email:EmailAddress) :VerifiedEmail option =
        if hash="OK" then
            Some (VerifiedEmail email)
        else
            None

// ================================================
// Now write some client code that uses this API
// ================================================

module EmailClient =
    open EmailDomain

    // Create a "verifyContactInfo" function that transitions from Unverified state to Verified state
    let verifyContactInfo (hash:string) (email:EmailContactInfo)  =
        match email with
        | Unverified emailAddress ->
             let verifiedOrNone = verify hash emailAddress
             match verifiedOrNone with
             | Some verifiedEmail ->
                // transition to verified state
                printfn "the email was verified"
                Verified verifiedEmail,"Success"
             | None ->
                printfn "the email was not verified"
                // return original state
                email,"Failure"
        | Verified _ ->
            printfn "the email is already verified"
            // return original state
            email,"Already verified"

    // Create a "sendVerificationMessage" function
    // Rule: "You can't send a verification message to a verified email"
    let sendVerificationMessage (email:EmailContactInfo) =
        match email with
        | Unverified emailAddress ->
             printfn "Sending verification email to %A" emailAddress
        | Verified _ ->
             printfn "The email is already verified"

    // Create a "sendPasswordResetMessage " function
    // Rule: "You can't send a password reset message to a unverified email "
    let sendPasswordResetMessage (email:EmailContactInfo) =
        match email with
        | Unverified emailAddress ->
             printfn "Can't send reset email to unverified %A" emailAddress
        | Verified verifiedEmailAddress ->
             printfn "Sending reset email to %A" verifiedEmailAddress


// ================================================
// Now write some test code
// ================================================

open EmailDomain
open EmailClient

let email = EmailAddress "x@example.com"
let unverified = Unverified email

unverified |> sendVerificationMessage


let verifiedOk,status =
    let hash = "OK"
    unverified |> verifyContactInfo hash

verifiedOk |> sendPasswordResetMessage


// errors
verifiedOk |> sendVerificationMessage
unverified |> sendPasswordResetMessage

