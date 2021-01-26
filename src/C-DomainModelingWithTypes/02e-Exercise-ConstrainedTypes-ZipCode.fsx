// =================================
// Exercise: ConstrainedTypes
//
// Given the definition of the constrained types below,
// write functions that implement the constructor and property getter.
// =================================

open System

// a useful helper function
let notImplemented() = failwith "not implemented"

//----------------------------------------------------------
//  Exercise: Create a `ZipCode`  type that can only contain 5 digit chars
//----------------------------------------------------------

open System

module ConstrainedTypes =

    /// Must be 5 chars, all digits
    type ZipCode = private ZipCode of string

    module ZipCode =

        /// TODO Public constructor
        let create (s:string) =

            // local helper function
            let is5Digits (s:string) =
                let isAllDigits = s |> Seq.forall Char.IsDigit
                (s.Length = 5) && isAllDigits

            // construct the ZipCode
            ??

        /// TODO Return the value
        let value ?? = ??



// --------------------------------
// test ZipCode
// --------------------------------

open ConstrainedTypes

let printWrappedValue zipCodeOpt =
    match zipCodeOpt with
    | Some zipCode ->
        let inner = ZipCode.value zipCode
        printfn "The ZipCode is valid and the wrapped value is %s" inner
    | None ->
        printfn "The value is None"

let zip1Option = ZipCode.create "12345"
printWrappedValue zip1Option

let zip2Option = ZipCode.create "abc"
printWrappedValue zip2Option

let zip3Option = ZipCode.create "123456"
printWrappedValue zip3Option
