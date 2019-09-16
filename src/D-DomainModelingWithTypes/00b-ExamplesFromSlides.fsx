module ExamplesFromSlides =

    // ================================================
    // (from slides) Using options to model missing data
    // ================================================

    type PersonalName =
        {
        FirstName: string
        MiddleInitial: string option
        LastName: string
        }


    // ================================================
    // (from slides) Using single choice types to keep types distinct
    // ================================================

    (*
    Is an EmailAddress just a string?
    Is a CustomerId just a int?
    Use single choice types to keep them distinct
    *)

    type EmailAddress = EmailAddress of string
    type PhoneNumber = PhoneNumber of string

    let value1a = EmailAddress "123"
    let value1b = PhoneNumber "123"
    let areEqual1 = (value1a=value1b)


    type CustomerId = CustomerId of int
    type OrderId = OrderId of int

    let value2a = CustomerId 123
    let value2b = OrderId 123
    let areEqual2 = (value2a=value2b)


    // ================================================
    // (from slides) Constructing optional values
    // ================================================

    open System.Text.RegularExpressions

    let createEmailAddress (s:string) =
        if Regex.IsMatch(s,@"^\S+@\S+\.\S+$")
            then Some (EmailAddress s)
            else None
    // val createEmailAddress : s:string -> EmailAddress option

    type String50 = String50 of string

    let createString50 (s:string) =
        if s.Length <= 50
            then Some (String50 s)
            else None
    // val createString50 : s:string -> String50 option


    type OrderLineQty = OrderLineQty of int

    let createOrderLineQty qty =
        if qty >0 && qty <= 99
            then Some (OrderLineQty qty)
            else None
    // val createOrderLineQty : qty:int -> OrderLineQty option


module TestExamplesFromSlides =
    open ExamplesFromSlides

    let goodEmail = createEmailAddress "good@example.com"
    let badEmail = createEmailAddress "bad"

// ================================================
// (from slides) The new domain!
// ================================================


module StringTypes =

    type String1 = String1 of string
    type String50 = String50 of string

    let createString1 (s:string) =
        if (s.Length <= 1)
            then Some (String50 s)
            else None

    let createString50 (s:string) =
        if s.Length <= 50
            then Some (String50 s)
            else None

module DomainTypes =

    open StringTypes

    type EmailAddress =
        EmailAddress of string

    type VerifiedEmail =
       VerifiedEmail of EmailAddress

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
    let verifiedEmail = verifiedEmailOpt.Value // don't do this in real code!
    VerifiedState verifiedEmail

sendVerificationMessage unverifiedEmailState
sendVerificationMessage verifiedEmailState

sendPasswordResetMessage unverifiedEmailState
sendPasswordResetMessage verifiedEmailState

