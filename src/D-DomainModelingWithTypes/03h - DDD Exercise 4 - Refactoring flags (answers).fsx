// ================================================
// DDD Exercise: Refactoring designs to use states
// ================================================

(*
Much code has implicit states that you can recognize by
fields called "IsSomething", or nullable date

This is a sign that states transitions are present
but not being modelled properly.
*)

(*
Exercise 3a

An ecommerce site has customers
* if they have registered, they have a name and an ID
* if they have not registered, they just have a name
The original design uses a flag to tell the two cases apart

Your task: redesign this type into two states:
* RegisteredCustomer (with an id)
* OR GuestCustomer (without an id)
Also, replace "int" and "string" with words from the domain

*)

// contains the original code
module Customer_Before =

    type Customer =
        {
        CustomerName: string
        // redesign to rid of this bool
        IsGuest: bool
        // redesign to rid of this option
        RegistrationId: int option
        }

// contains the redesigned code
module Customer_After =

    type CustomerName = CustomerName of string
    type RegistrationId  = RegistrationId of int

    type Customer =
        | Guest of CustomerName
        | RegisteredCustomer of CustomerName * RegistrationId
                                // this could also be a custom record type

(*
Exercise 3b

An internet connection is either connected or disconnected.
The original design uses a flag to tell the two cases apart

Your task: Redesign this type into two states: Connected and Disconnected
Also, replace "int" and "string" with words from the domain

*)

// contains the original code
module Connection_Before =

    type Connection =
       {
       IsConnected: bool
       ConnectionStartedUtc: System.DateTime option
       ConnectionHandle: int
       ReasonForDisconnection: string
       }

// contains the redesigned code
module Connection_After =

    type ConnectionHandle = ConnectionHandle of int
    // NOTE: I'm using an alias rather than a new type.
    // It helps to document the code but doesn't force any constraint.
    type ConnectionStartedUtc = System.DateTime
    // NOTE: Another alias
    type ReasonForDisconnection = string

    type Connection =
        | Connected of ConnectionHandle * ConnectionStartedUtc
        | Disconnected of ReasonForDisconnection

(*
// Exercise 3c

An Order is either Paid or Unpaid.

Your task: redesign this type into two states.
Can you guess what the states are from the flags?
How does the refactored version help improve the documentation?

*)

// contains the original code
module Order_Before =

    type Order =
       {
       OrderId: int
       IsPaid: bool
       PaidAmount: float option
       PaidDate: System.DateTime option
       }

// contains the redesigned code
module Order_After =

    type OrderId = OrderId of int
    type PaidAmount = float
    type PaidDate = System.DateTime

    type Order =
        | Unpaid of OrderId
        | Paid of OrderId * PaidAmount * PaidDate
                 // this is a tuple. Should it be a new record type?

(*
Questions for discussion:

* When does it make sense to use a type alias rather than a separate type?

* When does it make sense to define a new type rather than use a tuple?

*)