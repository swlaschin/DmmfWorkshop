// =================================
// Exercise: OrderLineQty
//
// Given the definition of OrderLineQty below,
// write functions that increments and decrements it.
// =================================


/// I have defined the "library" code for OrderLineQty here
module ConstrainedTypes =

    // I'm using a module to create a "namespace".
    // Since this is a submodule inside a bigger file
    // I need to use "=" and indent everything inside the module.

    /// Must be between >= 1 and <= 100
    type OrderLineQty = private OrderLineQty of int

    module OrderLineQty =
        /// Public function to create a OrderLineQty.
        /// I.e. wrap an int in a OrderLineQty (if possible!).
        /// Used like a "factory method" "constructor" etc
        let create qty =
            // IMPORTANT! In F# if/then/else is an *expression*
            // not a statement, so each branch must return something
            // and all branches must return the same type
            // The nearest equivalent in C-like languages
            // is the ternary "if"
            //    if x ? a : b
            if qty < 1 then
                None
            else if qty > 100 then
                None
            else
                Some (OrderLineQty qty)

        let maxValue =
            OrderLineQty 100

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

// We're now back in the main file,
// so we need to bring the ConstrainedTypes module
// into scope
open ConstrainedTypes

// Exercise: Write a function that adds one to an OrderLineQty
let increment (olq:OrderLineQty) =
    let i1 = OrderLineQty.value olq   // i1 is an int
    let i2 = i1 + 1                   // i2 is an int
    OrderLineQty.create i2            // return an OrderLineQty here

// This is what the type signature looks like.
// Note that it MUST return an optional OrderLineQty!
// val increment :
//   olq:OrderLineQty -> OrderLineQty option

// Exercise: Write a function that subtracts one from an OrderLineQty
let decrement (olq:OrderLineQty) =
    let i1 = OrderLineQty.value olq
    let i2 = i1 - 1
    OrderLineQty.create i2

// This is what the type signature looks like.
// Note that it MUST return an option!
// val decrement :
//   olq:OrderLineQty -> OrderLineQty option

// ----------------------
// test your code
// ----------------------

// increment the smallest and largest values
increment OrderLineQty.minValue
increment OrderLineQty.maxValue

// decrement the smallest and largest values
decrement OrderLineQty.minValue
decrement OrderLineQty.maxValue


// =========================================
// Adding defaults
// =========================================

(*
What happens if you use the increment button on the website
and you go above 100?

Should you remove the item from the shopping cart?

Probably not. Instead, you want to "max out" at 100

You can do this using a "defaultValue" function.
It will leave a "Some" alone but it will
replace a "None" and with another value.

*)

let defaultValue aValue anOption =
    match anOption with
    | Some x -> x      // if Some, return the wrapped value
    | None -> aValue   // if None, return the default value

// usage examples
Some 1 |> Option.defaultValue 42    // 1
None   |> Option.defaultValue 42    // 42


// Example using the increment function above
// These now return a normal OrderLineQty instead of an optional one.
increment OrderLineQty.minValue        // this is a "Some"
|> defaultValue OrderLineQty.maxValue  // so the defaultValue is not used


increment OrderLineQty.maxValue        // this is a "None"
|> defaultValue OrderLineQty.maxValue  // so the defaultValue IS used


// =========================================
// Exercise: Implement a different "increment" function
// =========================================

// Exercise: Write a function that adds one to an OrderLineQty
// If it goes over OrderLineQty.maxValue then return maxValue
let increment_v2 (olq:OrderLineQty) =
    let i1 = OrderLineQty.value olq
    let i2 = i1 + 1
    OrderLineQty.create i2
    |> defaultValue OrderLineQty.maxValue

// This is what the type signature looks like.
// Note that it does NOT return an option now!
// val increment_v2 :
//   olq:OrderLineQty -> OrderLineQty

// test it to make sure it works and stays at 100
increment_v2 OrderLineQty.maxValue
