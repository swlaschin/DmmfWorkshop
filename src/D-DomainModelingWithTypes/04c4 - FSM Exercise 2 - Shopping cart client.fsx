// ================================================
// FSM Exercise: Modeling e-commerce shopping cart transitions
//
// See Shopping cart transition diagram.png
//
// ================================================

(*
Exercise: write some client code that uses the shopping cart API

Rule: "You can't remove an item from an empty cart"
Rule: "You can't change a paid cart"
Rule: "You can't pay for a cart twice"

States are:
* EmptyCartState
* ActiveCartState
* PaidCartState
*)

#load "04c3 - FSM Exercise 2 - Shopping cart domain (answers).fsx"
open ``04c3 - FSM Exercise 2 - Shopping cart domain (answers)``
open ShoppingCartDomain

// ================================================
// Define the API (implemented in a separate file)
// ================================================

#load "ShoppingCartTransitions.fsx"
open ShoppingCartTransitions

module ShoppingCartApi =

    let initCart firstItem =
        ShoppingCartTransitions.initCart firstItem

    let addToActive itemToAdd activeCartData =
        ShoppingCartTransitions.addToActive itemToAdd activeCartData

    let removeFromActive itemToRemove activeCartData =
        ShoppingCartTransitions.removeFromActive itemToRemove activeCartData

    let pay activeCartData =
        ShoppingCartTransitions.pay activeCartData


// ================================================
// Now write some client code that uses this API
// ================================================
module ShoppingCartClient =

    open ShoppingCartDomain

    // "clientAddItem" changes the cart state after adding an item
    // function signature should be
    //     Product -> ShoppingCart-> ShoppingCart

    let clientAddItem (newItem:Product) (cart:ShoppingCart)  :ShoppingCart =
        match cart with
        // | empty ->
            let new cart contents = what??
            return what new state

        // | active ->
            let new cart contents = what??
            return what new state

        // | paid ->

    // "clientPayForCart " changes the cart state after paying
    // function signature should be
    //     Payment -> ShoppingCart-> ShoppingCart

    let clientPayForCart (payment:Payment) (cart:ShoppingCart)  :ShoppingCart =
        match cart with
        // | empty ->
        // | active -> return new state
        // | paid ->




// ================================================
// Now write some test code
// ================================================

open ShoppingCartClient

let item1 = "Book"
let item2 = "Dvd"
let item3 = "Headphones"

let cart0 = EmptyCartState
let cart1 = clientAddItem item1 cart0
let cart2 = clientAddItem item2 cart1
let cart3 = clientPayForCart 20.00 cart2

// errors
clientAddItem item2 cart3
clientPayForCart 20.00 cart0
clientPayForCart 20.00 cart3
