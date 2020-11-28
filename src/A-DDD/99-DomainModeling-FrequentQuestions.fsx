module rec FAQ
// ====================================
// Common questions about domain modeling
// ====================================


// ====================================
// Question: Using Tuples vs. using Records, which is better?
// Answer: It depends!
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
// Question: How to return X OR Y from a function?
// Answer: Use a choice type
// ====================================

module ChoiceExample =
    (*
    Let's say the domain model is:

      UseCashMachine:
         input: Card AND Pin
         output: Money OR CantGiveCash OR ComeBackLater

    *)
    type Card = string
    type Pin = string
    type Money = int

    type ErrorMessage =
        | CantGiveCash
        | ComeBackLater

    // Model Success OR Failure
    type CashMachineResult =
        | Success of Money
        | Failure of ErrorMessage

    // outpure of function is a choice
    type UseCashMachine = Card * Pin -> CashMachineResult




// ====================================
// Question: How to model DDD "value objects" vs "Entities"?
// Answer:
// ====================================

module EntityExample =
    (*
    Definition: A "value object" has no identity.
    If the data inside it changes, it is a different object.

    Definition: An "entity" has an identity that is preserved
    even if the data inside it changes.
    *)

    // This is a value object, if the data changes we say it becomes a "different name"
    type CustomerName = {
        First: string
        Last: string
    }

    // this is the default in F#, you can test it for yourself
    let n1 = {First="A"; Last="Smith"}
    let n2 = {First="A"; Last="Patel"}  // this is a different name
    n1 = n2  // not equal

    // This is an entity, if the data changes it is still the "same customer"
    // So how should we compare two Customer objects?
    // Option 1: Compare by all data? No, then it's not an entity
    // Option 2: Compare by ID only? This would require overriding the "Equals" and "GetHashCode" methods.
    //           I don't like this because it changes the default behavior and creates hidden behavior
    //           (I would have to read the documentation or look inside the class -- that's bad!)
    // Option 3: Don't allow comparison at all! I like this approach best. You can still compare the ids if you like.
    [<NoEquality;NoComparison>]
    type Customer = {
        Id : int
        Name : CustomerName
        }

    let c1a = {Id = 1; Name = n1}
    let c1b = {Id = 1; Name = n2}   // this is the same customer. with a different name

    // my answer is not to allow them to be compared at all!
    c1a = c1b            //error

    // but you can still compare parts of them
    c1a.Id = c1b.Id      //true
    c1a.Name = c1b.Name  //false



// ====================================
// Question: How do you model "void"?
// Answer: Using the "unit" type
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
// Question: How do you model a simple state machine?
// Answer: With choices rather than subclasses
// ====================================

module StateMachine =
    // the state of door (two states)
    type DoorState = OpenDoor | ClosedDoor

    // actions that change the state of the door
    type OpenTheDoor = DoorState -> DoorState
    type CloseTheDoor = DoorState -> DoorState


// ====================================
// Question: How do you model interfaces?
// Answer 1: In general, we try to avoid interfaces and just use individual functions
//           A function is just an interface with one method!
//           Normally in any client, you only need one or two methods from the interface,
//           So just pass them in directly. This stops interface bloat!
// ====================================

(*
Let's say some client code needs to be able to open and close a door

Interface approach

    interface IDoor {
        DoorState Open(DoorState doorState);
        DoorState Close(DoorState doorState);
        // often there are many other methods that you don't need for
        // a particular workflow :(
    }

    client is given a IDoor interface


An interface with one method can be replaced by a function

Here's the C# for an interface with one method

    interface IOpenDoor {
       DoorState Open(DoorState doorState);
       }

or as a function:

    Func<DoorState,DoorState>

In a functional approach, a client is given just the TWO functions it needs,
one to open the door and one to close it. See below
*)

type undefined = exn

module InterfaceAsFunctions =

    type Door = undefined // something!

    type OpenDoor = Door -> Door
    type CloseDoor = Door -> Door

    type DoorUser = (*current*) Door * (*actions*) OpenDoor * CloseDoor -> (*new*) Door



// ====================================
// Question: How do you model interfaces when you have a LOT of functions you need to pass in?
// Answer: In that case, DO use an interface! Or a record of functions
// ====================================

module GroupingFunctionsInAnInterface =

    type Door = undefined // something!

    // use an interface to group a set of functions
    type IDoorActions =
        abstract OpenDoor : Door -> Door
        abstract CloseDoor : Door -> Door

    type DoorUser = (*current*) Door * (*actions*) IDoorActions -> (*new*) Door

    // use a record to group a set of functions
    type DoorActions = {
        OpenDoor : Door -> Door
        CloseDoor : Door -> Door
        }

    type DoorUser_v2 = (*current*) Door * (*actions*) DoorActions -> (*new*) Door

