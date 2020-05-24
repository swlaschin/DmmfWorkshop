// =================================
// This file contains the code used in the slides so far
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


module StringTypes =

    type String1 = String1 of string
    type String50 = String50 of string

    let createString1 (s:string) =
        if s <> null && (s.Length <= 1)
            then Some (String50 s)
            else None

    let createString50 (s:string) =
        if s <> null && s.Length <= 50
            then Some (String50 s)
            else None

module DomainTypes =

    open StringTypes

    type EmailAddress =
        EmailAddress of string

    type VerifiedEmail =
       VerifiedEmail of EmailAddress

    type VerificationHash = string
    type VerificationService = (EmailAddress *  VerificationHash) -> VerifiedEmail option

    type EmailContactInfo =
      | Unverified of EmailAddress
      | Verified of VerifiedEmail


    type PersonalName = {
      FirstName: String50
      MiddleInitial: String1 option
      LastName: String50 }

    type Contact = {
      Name: PersonalName
      Email: EmailContactInfo }

// ================================================
// (from slides) The new domain with address info added as well
// ================================================

module DomainTypes_WithAddress =

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

