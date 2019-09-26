(*
Given the definition of OrderLineQty below,
write functions that increments and decrements it.
*)

module ConstrainedTypes =
    type OrderLineQty = private OrderLineQty of int

    module OrderLineQty =
        /// Public function to create a OrderLineQty.
        /// I.e. wrap an int in a OrderLineQty (if possible!).
        /// Used like a "factory method" "constructor" etc
        let create qty =
            if qty < 1 then
                None
            else if qty > 99 then
                None
            else
                Some (OrderLineQty qty)

        let maxValue =
            OrderLineQty 99

        let minValue =
            OrderLineQty 1

        /// Public function to get the data out of a OrderLineQty
        /// An "unwrapper" or "deconstructor" function
        let value olQty =
            match olQty with
            | OrderLineQty qty -> qty

        // short version of the code above
        (*
        let value (OrderLineQty qty) =
            qty
        *)


// same as "using ConstrainedTypes"
open ConstrainedTypes

// Write a function that adds one to an OrderLineQty
let increment (olq:OrderLineQty) =
    let i1 = OrderLineQty.value olq
    let i2 = i1 + 1
    OrderLineQty.create i2

// val increment :
//   olq:OrderLineQty -> OrderLineQty option

// Write a function that subtracts one from an OrderLineQty
let decrement (olq:OrderLineQty) =
    let i1 = OrderLineQty.value olq
    let i2 = i1 - 1
    OrderLineQty.create i2

// val decrement :
//   olq:OrderLineQty -> OrderLineQty option

// ==================
// test
// ==================

increment OrderLineQty.minValue
increment OrderLineQty.maxValue

decrement OrderLineQty.minValue
decrement OrderLineQty.maxValue


// for a default value in the None case
// use Option.defaultValue

increment OrderLineQty.minValue
|> Option.defaultValue OrderLineQty.minValue

increment OrderLineQty.maxValue
|> Option.defaultValue OrderLineQty.maxValue

decrement OrderLineQty.minValue
|> Option.defaultValue OrderLineQty.minValue

decrement OrderLineQty.maxValue
|> Option.defaultValue OrderLineQty.maxValue

