(*
Given the data type below,
write a function that adds two OrderLineQtys
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

// Write a function that adds two OrderLineQtys

let addOrderQty oq1 oq2 =
    ???

// test
let oq10 = 10 |> OrderLineQty.create |> Option.get
// NOTE: never use Option.get in production!

let oq60 = 60 |> OrderLineQty.create |> Option.get

addOrderQty oq10 oq10
addOrderQty oq60 oq60