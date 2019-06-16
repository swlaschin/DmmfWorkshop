open System

//----------------------------------------------------------
//  Q. Create a `NonZeroInteger`  type that can only contain non-zero integers

module ConstrainedTypes =

    type NonZeroInteger = private NonZeroInteger of int

    module NonZeroInteger =
        /// Public constructor
        let create i =
            if i = 0 then
                Some ???
            else
                None

        /// Return the value
        let value (NonZeroInteger i) =
            ??

open ConstrainedTypes

// test
let badNZI = NonZeroInteger.create 0
let goodNZI = NonZeroInteger.create 1


//----------------------------------------------------------
//  Q. Create a `AllDigitsString` type that can only contain digit chars

open System

module ConstrainedTypes2 =

    type AllDigitsString = ???

    module AllDigitsString =
        /// Public constructor
        let create (s:string) =
            let isAllDigits = s |> Seq.forall Char.IsDigit
            if isAllDigits then
                ???
            else
                ???

        /// Return the value
        let value (???) = ???


// test
open ConstrainedTypes2

let x2 = AllDigitsString.create "123"
let value = x2 |> Option.get  // dont do this except for testing!

let x3 = AllDigitsString.create "abc"
