// ================================================
// Exercise: Implement a client of the ShoppingCartApi
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

// ================================================
// Load the Domain and API (implemented in a separate file)
// ================================================

#load "ShoppingCartApiImplementation.fsx"
open ShoppingCartApiImplementation
open ShoppingCartApiImplementation.ShoppingCartDomain
open ShoppingCartApiImplementation.ShoppingCartApi

// ================================================
// Now write some client code that uses this API
// ================================================
module ShoppingCartClient =

    // "clientAddItem" changes the cart state after adding an item
    // function signature should be
    //     CartItem -> ShoppingCart-> ShoppingCart
    let clientAddItem (newItem:CartItem) (cart:ShoppingCart)  :ShoppingCart =
        match cart with
        | EmptyCartState ->
            printfn "Adding item %s to empty cart" newItem
            ShoppingCartApi.initCart newItem
        | ActiveCartState data ->
            printfn "Adding item %s to active cart" newItem
            ShoppingCartApi.addToActive newItem data
        // | paid -> what here?

    // "clientPayForCart " changes the cart state after paying
    // function signature should be
    //     Payment -> ShoppingCart-> ShoppingCart
    let clientPayForCart (payment:Payment) (cart:ShoppingCart)  :ShoppingCart =
        match cart with
        | EmptyCartState ->
            printfn "Can't pay for empty cart"
            cart // return original cart
        // | active -> return new state
        // | paid ->


    // "clientRemoveItem " changes the cart state after removing an item
    // function signature should be
    //     CartItem -> ShoppingCart-> ShoppingCart
    let clientRemoveItem (itemToRemove:CartItem) (cart:ShoppingCart)  :ShoppingCart =
        match cart with
        | EmptyCartState ->
            printfn "Can't remove item from empty cart"
            cart
        | ActiveCartState -> ??


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

let cart1a = cart2 |> clientRemoveItem item2
printfn "Is cart1 == cart1a? %b" (cart1 = cart1a)
let cart0a = cart1a |> clientRemoveItem item1
printfn "Is cart0 == cart0a? %b" (cart0 = cart0a)

// errors
clientAddItem item2 cart3

clientPayForCart 20.00 cart0
clientPayForCart 20.00 cart3

cart0 |> clientRemoveItem item1
cart3 |> clientRemoveItem item1