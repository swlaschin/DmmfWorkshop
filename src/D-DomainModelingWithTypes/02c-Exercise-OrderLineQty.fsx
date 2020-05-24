// =================================
// Exercise: OrderLineQty
//
// Given the definition of OrderLineQty below,
// write functions that increments and decrements it.
// =================================


/// I have defined the "library" code for OrderLineQty here
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


// ---------------------------------------
// Exercise starts here
// ---------------------------------------

// bring the ConstrainedTypes module into scope
open ConstrainedTypes

// Exercise: Write a function that adds one to an OrderLineQty
let increment (olq:OrderLineQty) =
    let i1 = OrderLineQty.value olq
    let i2 = i1 + 1
    ???

// Exercise: Write a function that subtracts one from an OrderLineQty
let decrement (olq:OrderLineQty) =
    ???



// ==================
// test your code
// ==================

// increment the smallest and largest values
increment OrderLineQty.minValue
increment OrderLineQty.maxValue

// decrement the smallest and largetst values
decrement OrderLineQty.minValue
decrement OrderLineQty.maxValue


// If you want to get rid of the optional value,
// you can use Option.defaultValue to get
// a default value in the None case

increment OrderLineQty.minValue
|> Option.defaultValue OrderLineQty.minValue

increment OrderLineQty.maxValue
|> Option.defaultValue OrderLineQty.maxValue

decrement OrderLineQty.minValue
|> Option.defaultValue OrderLineQty.minValue

decrement OrderLineQty.maxValue
|> Option.defaultValue OrderLineQty.maxValue

