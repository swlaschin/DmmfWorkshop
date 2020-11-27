module rec FAQ
// ====================================
// Common questions about domain modeling
// ====================================


// ====================================
// Using Tuples vs. using Records
// ====================================

// option 1 - BAD
type Name1 = string * string * string

// option 2 - Better
type Name2 = { First: string; Last: string }

// option 3 - Depends on whether they have special domain logic
type Name3 = FirstName * LastName * EmailAddress
type FirstName = string // not really a proper domain concept?
type LastName = string
type EmailAddress = string // a proper domain concept with extra validation




// ====================================
// How to return a choice
// ====================================
module ChoiceExample =
    type ErrorMessage =
        | CantGiveCash
        | ComeBackLater

    type MyFunctionResult =
        | Success of string
        | Failure of ErrorMessage

    type MyFunction = string -> MyFunctionResult




// ====================================
// How to model "value objects" vs "Entities"
// ====================================

module EntityExample =

    // A value object has no identity.
    // If the data inside it changes, it is a different object.
    type CustomerName = {
        First: string
        Last: string
    }

    let n1 = {First="A"; Last="Smith"}
    let n2 = {First="A"; Last="Patel"}  // this is a different name
    n1 = n2  // not equal

    // An entity has an identity that is preserved
    // even if the data inside it changes.
    [<NoEquality;NoComparison>]
    type Customer = {
        Id : int
        Name : CustomerName
        }

    let c1a = {Id = 1; Name = n1}
    let c1b = {Id = 1; Name = n2}   // this is the same customer. with a different name

    // how should they compare equal?
    // * by id?
    // * by all data?

    // my answer is not to allow them to be compared at all!
    c1a = c1b            //error

    // but you can still compare parts of them
    c1a.Id = c1b.Id      //true
    c1a.Name = c1b.Name  //false



// ====================================
// How to model "void"
// ====================================

// C# style for no input
(*
int DoSomething() { return 42; }
*)

// F# style for no input
(*
type DoSomething = unit -> int
let doSomething() = 42
*)

// C# style for no output
(*
void DoSomething(int x) { }
*)

// F# style for no output
(*
type DoSomething = int -> unit
let doSomething x = ()  // "Nothing" or "None"
*)

// ====================================
// How to model a simple state machine
// ====================================

module StateMachine =
    // change state of door
    type DoorState = OpenDoor | ClosedDoor
    type OpenTheDoor = DoorState -> DoorState
    type CloseTheDoor = DoorState -> DoorState


// ====================================
// How to model interfaces
// ====================================

// C# for an interface with one method
(*
interface IOpenDoor {
   DoorState Open(DoorState doorState);
   }
*)
// or
(*
Func<DoorState,DoorState>
*)

// F# for an interface
(*
type OpenDoor = DoorState -> DoorState
*)




