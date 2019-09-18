// ================================================
// FSM Exercise: Modeling e-commerce shopping cart transitions
//
// See Shopping cart transition diagram.png
//
// ================================================

(*
Exercise: create types that model an e-commerce shopping cart

Rule: "You can't remove an item from an empty cart"
Rule: "You can't change a paid cart"
Rule: "You can't pay for a cart twice"

States are:
* EmptyCartState
* ActiveCartState
* PaidCartState
*)

module ShoppingCartDomain =

    // define an "undefined" type for use when we don't know what type to use
    type undefined = exn

    // 1) Start with the domain types that are independent of state

    type Product = string     // placeholder for now
    type CartContents = Product list  // placeholder for now
    type Payment = float     // placeholder for now

    // 2) Create a type to represent the data stored for each type

    type EmptyCartData = undefined  //what data to store?
    type ActiveCartData = undefined  //what data to store?
    type PaidCartData = undefined //what data to store?

    // 3) Create a type that represent the choice of all the states

    type ShoppingCart =
        | EmptyCartState
        | ActiveCartState of undefined // what goes here?
        | PaidCartState of undefined // what goes here?


