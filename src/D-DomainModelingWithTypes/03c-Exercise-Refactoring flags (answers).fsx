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

// contains the redesigned code (but see an alternative design below)
module Order_After_v1 =

    type OrderId = OrderId of int
    type Amount = float
    type Date = System.DateTime

    type PaidOrder = {
        OrderId : OrderId
        PaidAmount : Amount
        PaidDate : Date
        }

    type Order =
        | Unpaid of OrderId
        | Paid of PaidOrder

// ====================================
// Question: Both Unpaid and Paid have a OrderId? Can that be refactored out?
// Answer: Yes, absolutely. The alternative design below does that.
//         The good news is that both designs are equivalent and so either can be used.
//         Which is better? It depends on the domain language? Do people say
//           PaidOrder OR UnpaidOrder
//         or do they say
//           An Order with PaidStatus OR UnpaidStatus
// ====================================

// alternative design
module Order_After_v2 =

    type OrderId = OrderId of int
    type Amount = float
    type Date = System.DateTime

    type OrderStatus =
        | Paid of Amount * Date
        | Unpaid

    type Order = {
        Id: OrderId
        Status: OrderStatus
        }

// ====================================
// Question: You used "type Amount = float" above rather than creating a new record type.
//           When does it make sense to use a type alias rather than a separate type?
// Answer: For initial sketching of a domain, an alias is fine.
//         If you need more behavior or constraints later, it is easy to change over
//         later on as you refine and refactor.
// ====================================

// ====================================
// Question: Should we use more specific types such as
//              type PaidAmount = ...
//              type PaidDate = ...
//           rather than the more generic
//              type Amount = ...
//              type Date = ...
// Answer: It depends on whether PaidAmount and PaidDate have special behavior different from
//         Amount and Date?
//         In this case I don't this there is. But some dates (like say, a DeliveryDate)
//         might have special constraints such as being on a weekday or something.
//
// WARNING: Beware of mixing up "policy" (which changes) with constraints (which never change)
// * An EmailAddress MUST have an @ sign.
// * A DeliveryDate being on a weekday is a policy and might easily change later.
// ====================================

// ====================================
// Question: You used "Amount * Date" above rather than creating a new record type.
//           When does it make sense to use a tuple rather than a separate type?
// Answer: It depends! For the data associated with a choice, it is sometimes easier.
//         But it will be exposed as API, or might need to change, you might want to use a record.
//         You don't have to get it right on the first try -- it is easy to change over from one style
//         to another later on as you refine and refactor.
// ====================================


