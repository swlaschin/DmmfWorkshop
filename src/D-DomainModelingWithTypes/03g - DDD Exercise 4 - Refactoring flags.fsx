// ================================================
// DDD Exercise: Refactoring designs to use states
// ================================================

(*
Much code has implicit states that you can recognize by fields called "IsSomething", or nullable date

This is a sign that states transitions are present but not being modelled properly.
*)

// Exercise 3a - redesign this type into two states: RegisteredCustomer (with an id) and GuestCustomer (without an id)
type Customer_Before =
    {
    CustomerName: string
    IsGuest: bool
    RegistrationId: int option
    }

type Customer_After = ??


// Exercise 3b - redesign this type into two states: Connected and Disconnected
type Connection_Before =
   {
   IsConnected: bool
   ConnectionStartedUtc: System.DateTime option
   ConnectionHandle: int
   ReasonForDisconnection: string
   }

type Connection__After = ??


// Exercise 3c - redesign this type into two states -- can you guess what the states
// are from the flags -- how does the refactored version help improve the documentation?
type Order_Before =
   {
   OrderId: int
   IsPaid: bool
   PaidAmount: float option
   PaidDate: System.DateTime option
   }

type Order__After = ??

