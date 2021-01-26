// =================================
// This file contains the code used in the slides so far
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


module ExamplesFromSlides_1 =

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
    //let areEqual1 = (value1a=value1b) //TODO uncomment to see an error


    type CustomerId = CustomerId of int
    type OrderId = OrderId of int

    let value2a = CustomerId 123
    let value2b = OrderId 123
    //let areEqual2 = (value2a=value2b)   //TODO uncomment to see an error


// ================================================
// (from slides) Constructing optional values
// ================================================
module ExamplesFromSlides_2 =

    //-----------------------------------
    // EmailAddress 
    //-----------------------------------

    type EmailAddress = EmailAddress of string

    let createEmailAddress (s:string) =
        if s.Contains("@") then
            Some (EmailAddress s)
        else
            None
    // val createEmailAddress : s:string -> EmailAddress option

    //-----------------------------------
    // String50 
    //-----------------------------------

    type String50 = String50 of string

    let createString50 (s:string) =
        if s.Length <= 50 then
            Some (String50 s)
        else
            None
    // val createString50 : s:string -> String50 option

    //-----------------------------------
    // OrderLineQty 
    //-----------------------------------

    type OrderLineQty = OrderLineQty of int

    let createOrderLineQty qty =
        if qty >0 && qty <= 99 then
            Some (OrderLineQty qty)
        else
            None
    // val createOrderLineQty : qty:int -> OrderLineQty option


module TestExamplesFromSlides =
    open ExamplesFromSlides_2

    let goodEmail = createEmailAddress "good@example.com"
    let badEmail = createEmailAddress "bad"

