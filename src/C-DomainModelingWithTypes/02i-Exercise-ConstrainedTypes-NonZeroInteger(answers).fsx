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
// Exercise: Create a `NonZeroInteger`  type that can only contain non-zero integers
//----------------------------------------------------------

// This could be stored in an external file and loaded
// as a script using something like this:
// #load "ConstrainedTypes.fsx"

module ConstrainedTypes =

    /// Must be <> 0
    type NonZeroInteger = private NonZeroInteger of int

    module NonZeroInteger =

        // TODO: Implement public constructor
        let create i =
            if i = 0 then
                None
            else
                Some (NonZeroInteger i)

        // TODO: Return the value
        let value (NonZeroInteger i) = i

// --------------------------------
// test NonZeroInteger
// --------------------------------

open ConstrainedTypes

// test
// NonZeroInteger 1 // uncomment for error
let nonZeroOpt0 = NonZeroInteger.create 0
let nonZeroOpt1 = NonZeroInteger.create 1

