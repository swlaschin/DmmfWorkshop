// ================================================
// FSM Exercise: A simple 2-state transition for Unverified and Verified email
//
// See "Email transition diagram.png"
//
// ================================================
 
// Here are the EmailAddress types from the slide

type EmailAddress =
    EmailAddress of string

type VerifiedEmail =
    VerifiedEmail of EmailAddress

type EmailContactInfo =
    | Unverified of EmailAddress
    | Verified of VerifiedEmail


// Create a function that (maybe) creates a VerifiedEmail
let verify (hash:string) (email:EmailAddress) :VerifiedEmail option =
    if hash = "OK" then
        // then what
    else
        // else what
    // remember, this function returns a new state

// ================================================
// Now write some client code that uses this API
// ================================================

// Create a "verifyContactInfo" function that transitions from Unverified state to Verified state
let verifyContactInfo (hash:string) (email:EmailContactInfo)  :EmailContactInfo =
    match email with
    // what

// Create a "sendVerificationMessage" function
// Rule: "You can't send a verification message to a verified email"
let sendVerificationMessage (email:EmailContactInfo) =
    match email with
    // what

// Create a "sendPasswordResetMessage " function
// Rule: "You can't send a password reset message to a unverified email "
let sendPasswordResetMessage (email:EmailContactInfo) =
    match email with
    // what

