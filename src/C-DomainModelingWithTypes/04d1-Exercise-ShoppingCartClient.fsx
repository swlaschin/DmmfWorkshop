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

// "#load" keyword brings in an external file
// The # is not a comment  :)
#load "ShoppingCartApiImplementation.fsx"
open ShoppingCartApiImplementation
open ShoppingCartApiImplementation.ShoppingCartDomain
open ShoppingCartApiImplementation.ShoppingCartApi

// ================================================
// Now write some client code that uses this API
// ================================================
module ShoppingCartClient =

    (*
    // reminder how to wrap and unwrap if you need to...
    // to wrap
    let activeCartData = (ActiveCartData items)
    // to unwrap
    let (ActiveCartData items) = activeCartData
    *)

    // "clientAddItem" changes the cart state after adding an item
    // function signature should be
    //     CartItem -> ShoppingCart-> ShoppingCart
    let clientAddItem (newItem:CartItem) (cart:ShoppingCart)  :ShoppingCart =
        match cart with
        | EmptyCartState ->
            printfn "Adding item %s to empty cart" newItem
            ShoppingCartApi.initCart newItem
        | ActiveCartState activeCartData ->
            printfn "Adding item %s to active cart" newItem
            ShoppingCartApi.addToActive (newItem,activeCartData)
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
        //| ActiveCartState activeCartData -> ??


// ================================================
// Now write some test code
// ================================================

open ShoppingCartClient

// define some items
let item1 = "Book"
let item2 = "Dvd"
let item3 = "Headphones"

// create some different carts
let emptyCart = EmptyCartState
let activeCart1 = clientAddItem item1 emptyCart
let activeCart2 = clientAddItem item2 activeCart1
let paidCart = clientPayForCart 20.00 activeCart2

// check how the errors are handled
clientAddItem item2 paidCart
clientPayForCart 20.00 emptyCart
clientPayForCart 20.00 paidCart

emptyCart |> clientRemoveItem item1
paidCart |> clientRemoveItem item1

// structural equality!
let activeCart3 = activeCart2 |> clientRemoveItem item2
printfn "Is activeCart1 == activeCart3? %b" (activeCart1 = activeCart3)
let activeCart4 = activeCart3 |> clientRemoveItem item1
printfn "Is emptyCart == activeCart4? %b" (emptyCart = activeCart4)

