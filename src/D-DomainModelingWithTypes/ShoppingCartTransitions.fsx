// ================================================
// FSM Exercise: Modeling e-commerce shopping cart transitions
//
// This file implements the state transitions. Requires the domain file answers
//
// ================================================

// uncomment this to edit this file standalone
//#load "04c3 - FSM Exercise 2 - Shopping cart domain (answers).fsx"
open ``04c3 - FSM Exercise 2 - Shopping cart domain (answers)``
open ShoppingCartDomain

module ShoppingCartTransitions =
    open ShoppingCartDomain

    // "initCart" creates a new cart when adding the first item
    // The function signature should be
    //     Product -> ShoppingCart

    let initCart (itemToAdd:Product) :ShoppingCart =
        let activeData = [itemToAdd]
        ActiveCartState activeData

    // "addToActive" creates a new state from active data and a new item
    // function signature should be
    //     Product -> ActiveCartData -> ShoppingCart

    let addToActive (itemToAdd:Product) (activeCartData:ActiveCartData) :ShoppingCart =
        let newActiveData = itemToAdd::activeCartData
        ActiveCartState newActiveData

    // "pay" creates a new state from active data and a payment amount
    // function signature should be
    //     Payment -> ActiveCartData -> ShoppingCart

    let pay (payment:Payment) (activeCartData:ActiveCartData)  :ShoppingCart =
        let paidData = activeCartData, payment
        PaidCartState paidData

    // "removeFromActive" creates a new state from active data after removing an item
    // function signature should be
    //     Product -> ActiveCartData -> ShoppingCart

    // removeItem is tricky -- you need to test the card contents after removal to find out what the new state is!

    // you'll need this helper for removeItem transition
    let removeItemFromContents (productToRemove:Product) (cart:CartContents) :CartContents =
        cart |> List.filter (fun prod -> prod <> productToRemove)

    let removeFromActive (itemToRemove:Product) (activeCartData:ActiveCartData)  :ShoppingCart =
        let newContents = removeItemFromContents itemToRemove activeCartData
        match newContents with
        | [] -> EmptyCartState
        | smallerData -> ActiveCartState smallerData
