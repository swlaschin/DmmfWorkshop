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

*)



// ----------------------------------------
// Helper module
// ----------------------------------------
module StringTypes =

    type String1 = String1 of string
    type String50 = String50 of string

// ----------------------------------------
// Main domain code
// ----------------------------------------

open StringTypes

type EmailAddress =
    EmailAddress of string

type VerifiedEmail =
    VerifiedEmail of EmailAddress

type EmailContactInfo =
    | Unverified of EmailAddress
    | Verified of VerifiedEmail

type PostalContactInfo = {
    address1: string
    address2: string
    address3: string
    address4: string
    country: string
    }

type ContactInfo =
    | EmailOnly of EmailContactInfo
    | AddrOnly of PostalContactInfo
    | EmailAndAddr of EmailContactInfo * PostalContactInfo

type PersonalName = {
    FirstName: String50
    MiddleInitial: String1 option
    LastName: String50 }

type Contact = {
    Name: PersonalName
    ContactInfo : ContactInfo  }

