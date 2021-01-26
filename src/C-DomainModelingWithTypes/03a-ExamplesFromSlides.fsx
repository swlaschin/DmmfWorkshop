// =================================
// This file contains the code used in the slides so far
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


module DomainTypes =
    
    type EmailAddress =
        EmailAddress of string

    type VerifiedEmail =
       VerifiedEmail of EmailAddress

    type VerificationHash = string
    type VerificationService =
        (EmailAddress *  VerificationHash) -> VerifiedEmail option

    type EmailContactInfo =
      | Unverified of EmailAddress
      | Verified of VerifiedEmail

