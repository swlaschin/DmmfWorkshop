// ================================================
// DDD Exercise: Model a Contact management system
//
// ================================================

(*
REQUIREMENTS

The Contact management system stores Contacts

A Contact has
* a personal name
* an optional email address
* an optional postal address
* Rule: a contact must have an email or a postal address

A Personal Name consists of a first name, middle initial, last name
* Rule: the first name and last name are required
* Rule: the middle initial is optional
* Rule: the first name and last name must not be more than 50 chars
* Rule: the middle initial is exactly 1 char, if present

A postal address consists of four address fields plus a country

Rule: An Email Address can be verified or unverified
Rule: A verified email must only be created by a verification service

*)



// ----------------------------------------
// Constrained Types
// ----------------------------------------
module ConstrainedTypes =
    open System

    type String1 = private String1 of string
    type String50 = private String50 of string

    type EmailAddress = private EmailAddress of string

    // common code for all the constructors
    let private createCtor validator ctor str =
        if validator str then
            None
        else
            Some (ctor str)

    // helper function
    let private isNotEmpty str = not (String.IsNullOrEmpty(str))

    module String1 =
        let create str =
            let validator str = isNotEmpty str && str.Length <= 1
            createCtor validator String1 str
        let value (String1 str) = str

    module String50 =
        let create str =
            let validator str = isNotEmpty str && str.Length <= 50
            createCtor validator String50 str
        let value (String50 str) = str

    module EmailAddress =
        let create str =
            let validator str = isNotEmpty str && str.Contains("@")
            createCtor validator EmailAddress str
        let value (EmailAddress str) = str

// ----------------------------------------
// Verification Service
// ----------------------------------------
module VerificationService =
    open ConstrainedTypes

    type VerifiedEmail = private VerifiedEmail of EmailAddress
    type VerificationHash = string
    type VerificationService = (EmailAddress *  VerificationHash) -> VerifiedEmail option

    // implementation of verification service
    let verificationService : VerificationService =
        fun (emailAddress,hash) ->
            if hash = "OK" then
                Some (VerifiedEmail emailAddress)
            else
                None

    module VerifiedEmail =
        let value (VerifiedEmail email) = email

// ----------------------------------------
// Main domain
// ----------------------------------------
module ContactDomain =

    open ConstrainedTypes
    open VerificationService

    type EmailContactInfo =
        | Unverified of EmailAddress
        | Verified of VerifiedEmail

    type PostalContactInfo = {
        address1: String50
        address2: String50
        address3: String50
        address4: String50
        country: String50
        }

    type ContactInfo =
        | EmailOnly of EmailContactInfo
        | AddrOnly of PostalContactInfo
        | EmailAndAddr of EmailContactInfo * PostalContactInfo

    type PersonalName = {
        FirstName: String50
        MiddleInitial: String1 option
        LastName: String50
        }

    type Contact = {
        Name: PersonalName
        ContactInfo : ContactInfo
        }

    type SendPasswordReset = VerifiedEmail -> unit

module ContactImplementation =
    open ConstrainedTypes
    open VerificationService
    open ContactDomain

    let sendPasswordReset : SendPasswordReset =
        fun verifiedEmail ->
            let emailAddressStr = verifiedEmail |> VerifiedEmail.value |> EmailAddress.value
            printfn "Sending password reset to %s" emailAddressStr