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

An illegal state is possible! The registered flag can be true
but there is no ID assigned.

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
        IsRegistered: bool
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

Illegal states are possible!
* The IsConnected can be true but ConnectionStartedUtc is not assigned.
* The IsConnected can be false but ConnectionHandle is assigned.

Your task: Redesign this type into two states: Connected and Disconnected
Also, replace "int" and "string" with words from the domain

*)

// contains the original code
module Connection_Before =

    type Connection =
       {
       IsConnected: bool
       ConnectionStartedUtc: System.DateTime option
       ConnectionHandle: int option
       ReasonForDisconnection: string option
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
If it is paid, the Amount and PaidDate are set.

Question: What are the illegal states?

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

    type PaidOrder = {
        OrderId : OrderId
        PaidAmount : PaidAmount
        PaidDate : PaidDate
        }

    type Order =
        | Unpaid of OrderId
        | Paid of PaidOrder



(*
Questions for discussion:

* When does it make sense to use a type alias rather than a separate type?

* When does it make sense to define a new type rather than use a tuple?

*)