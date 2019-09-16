open System

let notImplemented = failwith "not implemented"

//----------------------------------------------------------
//  Q. Create a `NonZeroInteger`  type that can only contain non-zero integers

module ConstrainedTypes =

    type NonZeroInteger = private NonZeroInteger of int

    module NonZeroInteger =
        /// Public constructor
        let create i =
            //what goes here?
            notImplemented()

        /// Return the value
        let value ??  =
            //what goes here?
            ??

open ConstrainedTypes

// test
let nonZeroOpt0 = NonZeroInteger.create 0
let nonZeroOpt1 = NonZeroInteger.create 1


//----------------------------------------------------------
//  Q. Create a `ZipCode`  type that can only contain 5 digit chars

open System

module ConstrainedTypes2 =

    type ZipCode = private ZipCode of string

    module ZipCode =
        /// Public constructor
        let create (s:string) =
            let isAllDigits = s |> Seq.forall Char.IsDigit
            if (s.Length = 5) && isAllDigits then
                Some (ZipCode s)
            else
                None

        /// Return the value
        let value (ZipCode str) = str



// test
open ConstrainedTypes2

let zip1Option = ZipCode.create "12345"
let zip1 = zip1Option |> Option.get  // dont do this except for testing!

let zip2Option = ZipCode.create "abc"

let zip3Option = ZipCode.create "123456"
