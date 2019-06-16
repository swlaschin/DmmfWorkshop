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


    // Implement a function that (maybe) creates a VerifiedEmail
    // from an unverified EmailAddress if the hash is "OK"
    let createVerifiedEmail (hash:string) (email:EmailAddress) :VerifiedEmail option =
        if hash = "OK" then
            // then what
        else
            // else what

// ================================================
// Now write some client code that uses this API
// ================================================

module EmailClient =
    open EmailDomain

    // Create a "verifyContactInfo" function that transitions
    // from Unverified state to Verified state if
    // (a) the initial state is Unverified and
    // (b) the hash matches
    // If this is not met, leave the current state alone.
    let verifyContactInfo (hash:string) (email:EmailContactInfo)  :EmailContactInfo =
        match email with
        | Unverified emailAddress ->
             let verifiedOrNone = ???
             match verifiedOrNone with
             | Some verifiedEmail ->
                printfn "the email was verified"
                ???
             | None ->
                printfn "the email was not verified"
                // return original state
                ???
        | Verified _ ->
            printfn "the email is already verified"
            // return original state
            ???

    // Create a "sendVerificationMessage" function that just prints
    // "VerificationMessage sent" to the console.
    // Rule: "You can't send a verification message to a verified email"
    let sendVerificationMessage (email:EmailContactInfo) =
        match email with
        // what

    // Create a "sendPasswordResetMessage" function that just prints
    // "PasswordReset sent" to the console.
    // Rule: "You can't send a password reset message to a unverified email "
    let sendPasswordResetMessage (email:EmailContactInfo) =
        match email with
        // what


// ================================================
// Now write some test code
// ================================================

open EmailDomain
open EmailClient

let email = EmailAddress "x@example.com"
let unverified = Unverified email

unverified |> sendVerificationMessage


let verifiedOk =
    let hash = "OK"
    unverified |> verifyContactInfo hash

verifiedOk |> sendPasswordResetMessage


// errors
verifiedOk |> sendVerificationMessage
unverified |> sendPasswordResetMessage

