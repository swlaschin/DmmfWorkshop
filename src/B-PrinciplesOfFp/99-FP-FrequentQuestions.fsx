// ====================================
// Common questions about functional programming
// ====================================

// helper functions for examples
type undefined = exn
let notImplemented() = failwith "not implemented"


// ====================================
// Question: Can you overload a function with different types?
// Answer: No, because it breaks type inference.
//         You need to give them different names (or put them in different namespaces/modules)
// ====================================

module OverloadExamples =

    module Int =
        let add1 x = x + 1

    module Float =
        let add1 x = x + 1.0

    // you can define a choice type, but it is a bit of hack
    type Both =
        | I of int
        | F of float

    module Both =
        let add1 x =
            match x with
            | I i -> I (i + 1)
            | F f -> F (f + 1.0)

    (*
    F# does support OO code as well, and that DOES support overloading
    *)

    type NumberOperations() =
        static member Add1(x:int) = x + 1
        static member Add1(x:float) = x + 1.0

    // test
    NumberOperations.Add1 2
    NumberOperations.Add1 2.0

// ====================================
// Question: Can you have more than one constructor for a type (overloading again)
// Answer: Not in the FP-style, but you can have "factory" functions
// ====================================


module ConstructorExamples =

    // BOTH fields must always be used
    type MyRecord = {
        a : int
        b : string
        }


    module MyRecord =
        // a number of "factory" functions
        let createMyRecord1 () = {a=0; b=""}
        let createMyRecord2 i  = {a=i; b=""}
        let createMyRecord3 s  = {a=0; b=s}

    (*
    F# does support OO code as well, and that DOES support overloading multiple constructors
    *)


// ====================================
// Question: How can you emulate inheritance and subclassing in FP?
// Answer: Use composition
// ====================================

module InheritanceExampleOO =

    type BaseClass(name) =
        member this.Name = name

        abstract member Calculation: unit -> int
        default this.Calculation() = 2

        member this.PrintNameAndCalculation() =
            printfn "Name=%s. Calculation=%i" this.Name (this.Calculation())

    type SubClass(name, size) =
        inherit BaseClass(name)
        member this.Size = size
        override this.Calculation() = size * 3

    let bc = BaseClass("Alice")
    bc.PrintNameAndCalculation()

    let sc = SubClass("Bob", 7)
    sc.PrintNameAndCalculation()


module InheritanceExampleFP =

    module Util =
        let printNameAndCalculation name calculation =
            let result = calculation()
            printfn "Name=%s. CalculationResult=%i" name result

    type BaseClass = {
        Name: string
        }

    module BaseClass =
        let calculation() = 2
        let printNameAndCalculation (bc:BaseClass) =
            Util.printNameAndCalculation bc.Name calculation

    type NotASubClass = {
        Name : string
        Size : int
        }

    module NotASubClass =
        let calculation (sc:NotASubClass)  = sc.Size * 3
        let printNameAndCalculation (sc:NotASubClass) =
            let calculation'() = calculation sc  // change calculation into a form the utility function can use
            Util.printNameAndCalculation sc.Name calculation'

    let bc : BaseClass = {Name="Alice"}
    bc |> BaseClass.printNameAndCalculation

    let sc : NotASubClass = {Name="Bob"; Size=7}
    sc |> NotASubClass.printNameAndCalculation


// ====================================
// Question: How do you do dependency injection?
// Answer:
//   1. Keep the IO away from the core code (see the Functional Architecture project)
//      and then you won't need it
//   2. If you really do need a dependency, pass in a function or an interface directly,
//      but you can partially apply the dependency so that other people won't see it
// ====================================

type Payment = undefined
type SaveResult = undefined
type DbConnection = string

// lets say you need a DbConnection
type SavePayment = DbConnection -> Payment -> SaveResult

// your original function
let savePayment : SavePayment =
    fun dbConnection payment ->
        notImplemented()

// but you can "bake in" the connection like this, and then pass around this new function
let savePayment2 payment =  savePayment "myConnection" payment
// val savePayment2 : payment:Payment -> SaveResult

// the DbConnection is now hidden!


// ====================================
// Question: How do the SOLID principles apply to FP?
// Answer: Very well!
// ====================================

(*
SOLID =
   Single Responsibility Principle
   Open Closed Principle
   Liskov Substitution Principle
   Interface Segregation Principle
   Dependency Inversion Principle

"Single Responsibility Principle"
Each function does only one thing. You cannot get any more "Single Responsibility" than that!

"Open Closed Principle"
Data types are closed for modification. But anyone can write a new function that uses them.

"Liskov Substitution Principle"
There is no inheritance. On the other hand, function types are excellent interfaces.
For example, given the function type Int->Int, then ANY function with those types will work.
If you want constrain the implementation to a specific behavior,
then use the "decorator" pattern to extend a specific, correct implemention, or in FP terms,
just compose onto it.

"Interface Segregation Principle"
Function types are interfaces with one method. You cannot get any more segregated than that!

"Dependency Inversion Principle"
This is a general design principle that is not OO specific.
In FP-style code, we always pass dependencies in explicitly, typically as functions (see example above)


*)


