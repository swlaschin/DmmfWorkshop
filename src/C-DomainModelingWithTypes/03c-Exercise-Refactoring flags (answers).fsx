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
* OR Guest (without an id)
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
module Customer_After_v1 =

    type CustomerName = CustomerName of string
    type RegistrationId  = RegistrationId of int

    type Customer =
        | Guest of CustomerName
        | RegisteredCustomer of CustomerName * RegistrationId
                                // this could also be a custom record type
                                // (see below)

// Alternative design with custom record type for RegisteredCustomer
module Customer_After_v2 =

    type CustomerName = CustomerName of string
    type RegistrationId  = RegistrationId of int

    type RegisteredCustomer = {
        Name: CustomerName
        RegistrationId : RegistrationId
        }

    type Customer =
        | Guest of CustomerName
        | RegisteredCustomer of RegisteredCustomer

    // I can now treat RegisteredCustomer as a type
    // with special behavior (similar to VerifiedEmail)
    type HandleRegisteredOnly = RegisteredCustomer -> unit

// Alternative design with common data refactored out
module Customer_After_v3 =

    type CustomerName = CustomerName of string
    type RegistrationId  = RegistrationId of int

    // extract out the common data (the name)
    type RegistrationStatus =
        | Guest
        | Registered of RegistrationId

    type Customer = {
        Name: CustomerName  // common to both choices
        RegistrationStatus : RegistrationStatus
        }

(*
Exercise 3b

An internet connection is either connected or disconnected.
The original design uses a flag to tell the two cases apart

Illegal states are possible!
* The IsConnected can be true but ConnectionHandle is not assigned.
* The IsConnected can be false but ConnectionHandle is assigned.

Your task: Redesign this type into two states: Connected and Disconnected
In your redesign, make sure that the illegal states cannot happen. :)

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

An Order is either New OR Paid or Completed.
* If it is New, it has an Id and a list of items
* If it is Paid, the Amount and PaidDate are also set.
* If it is Completed, the ShippedDate is also set.

Question: What are the illegal states?

Your task: redesign this type into three states.
Can you guess what the states are from the enum?
How does the refactored version help improve the documentation?

*)

// contains the original code
module Order_Before =
    type OrderStateEnum = New | Paid | Shipped
    type Order =
       {
       OrderId: int
       Items : string list
       OrderState: OrderStateEnum
       PaidAmount: float option
       PaidDate: System.DateTime option
       ShippedDate: System.DateTime option
       }

// contains the redesigned code (but see an alternative design below)
module Order_After_v1 =

    type Item = string // can make more complicated later
    type OrderId = OrderId of int
    type Amount = float
    type Date = System.DateTime

    // In this design, each state is represented by a type
    // Each state has a copy of all the data -- there is no referencing data from other states

    type NewOrder = {
        OrderId : OrderId
        Items : Item list
        }

    type PaidOrder = {
        OrderId : OrderId
        Items : Item list
        PaidAmount : Amount
        PaidDate : Date
        }

    type CompletedOrder = {
        OrderId : OrderId
        Items : Item list
        PaidAmount : Amount
        PaidDate : Date
        ShippedDate : Date
        }

    type Order =
        | New of NewOrder
        | Paid of PaidOrder
        | Completed of CompletedOrder


// A second alternative
module Order_After_v2 =

    type Item = string // can make more complicated later
    type OrderId = OrderId of int
    type Amount = float
    type Date = System.DateTime

    // In this design, each state is represented by a type
    // But each state contains data from the previous state

    type OrderContent = {
        OrderId : OrderId
        Items : Item list
        }

    type PaidOrder = {
        OrderContent : OrderContent
        PaidAmount : Amount
        PaidDate : Date
        }

    type CompletedOrder = {
        PaidOrder : PaidOrder
        ShippedDate : Date
        }

    type Order =
        | New of OrderContent
        | Paid of PaidOrder
        | Completed of CompletedOrder

(*
What are the pros and cons of each design?
*)

// ====================================
(*
Question:

All states have a OrderId and Items? Can that be refactored out and put in a "superclass"?

Answer:

Yes, absolutely. The third alternative design below does that.


The good news is that all three designs are equivalent and so any can be used.
Which is better? It depends on the domain language? Do people say
    A PaidOrder OR CompletedOrder
or do they say
    An Order with Status=Paid OR Status=Completed
*)

// ====================================

// third alternative design
module Order_After_v3 =

    type Item = string // can make more complicated later
    type OrderId = OrderId of int
    type Amount = float
    type Date = System.DateTime

    type PaidOrderInfo = {
        PaidAmount : Amount
        PaidDate : Date
        }

    type CompletedOrderInfo = {
        PaidAmount : Amount
        PaidDate : Date
        ShippedDate : Date
        }

    type OrderStatus =
        | New                       // no extra info needed now
        | Paid of PaidOrderInfo
        | Completed of CompletedOrderInfo

    type Order = {
        Id: OrderId
        Items : Item list
        Status: OrderStatus
        }

// ====================================

(*
Question:

You used "type Amount = float" above rather than creating a new record type.
When does it make sense to use a type alias rather than a separate type?

Answer:

For initial sketching of a domain, an alias is fine.
If you need more behavior or constraints later, it is easy to change over
later on as you refine and refactor.
*)

// ====================================

(*
Question:

Should we use more specific types such as
    type PaidAmount = ...
    type PaidDate = ...
rather than the more generic
    type Amount = ...
    type Date = ...

Answer:

It depends on whether PaidAmount and PaidDate have special behavior
different from Amount and Date.

In this case I don't this there is. But some dates (like say, a DeliveryDate)
might have special constraints such as being on a weekday or something.

WARNING: Beware of mixing up "policy" (which changes) with constraints (which never change)
* An EmailAddress MUST have an @ sign.
* A DeliveryDate being on a weekday is a policy and might easily change later.

*)

// ====================================

(*
Question:

In the Connection_After example, you used a tuple in the choice type:

    type Connection =
        | Connected of ConnectionHandle * ConnectionStartedUtc

but in the Order_After example you created a record "NewOrder"

    type NewOrder = {
        OrderId : OrderId
        Items : string list
        }

rather than using a tuple in the choice type like this:

    type Order =
        | New of OrderId * Items

When does it make sense to use a tuple rather than a separate type?

Answer:

It depends! For the data associated with a choice, it is sometimes easier
to use a tuple.

But if it will be exposed as API, or might need to change, you might want
to use a record.

You don't have to get it right on the first try -- it is easy to change over
from one style to another later on as you refine and refactor.

*)

// ====================================


