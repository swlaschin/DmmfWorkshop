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
        /// An "unwrapper" or "deconstructor" or "property" function
        let value (OrderLineQty qty) =
            qty


// same as "using ConstrainedTypes"
open ConstrainedTypes

type OrderLine = {
    Qty: OrderLineQty
    }

// Write a function that adds two OrderLineQtys
let addOrderQty oq1 oq2 =
    let v1 = OrderLineQty.value oq1
    let v2 = OrderLineQty.value oq2
    let v3 = v1 + v2
    OrderLineQty.create v3


// val addOrderQty :
//   oq1:OrderLineQty -> oq2:OrderLineQty -> OrderLineQty option

// test
let oq10 = 10 |> OrderLineQty.create |> Option.get
// NOTE: never use Option.get in production!

let oq60 = 60 |> OrderLineQty.create |> Option.get

addOrderQty oq10 oq10
addOrderQty oq60 oq60

