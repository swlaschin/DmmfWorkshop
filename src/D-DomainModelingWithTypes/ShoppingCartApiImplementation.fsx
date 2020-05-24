// ================================================
// This is a pre-built implementation file for the ShoppingCartApi
// with state and transitions.
//
// You are not expected to build this -- it is used for the ShoppingCartClient example!
// ================================================


// -----------------------------------------------
// Model the domain types that are independent of state
// -----------------------------------------------

module ShoppingCartDomain =

    type CartItem = string     // placeholder for now
    type Payment = float     // placeholder for now


// -----------------------------------------------
// Model the state machine with a type and transitions
// This is the "API" for the state machine
// -----------------------------------------------

module ShoppingCartApi =
    open ShoppingCartDomain

    type CartContents = CartItem list  // placeholder for now

    // Create types to represent the data stored for each state
    type ActiveCartData = CartContents
    type PaidCartData = CartContents * Payment

    // Create a "state" type that represent the union of all the states
    type ShoppingCart =
        | EmptyCartState
        | ActiveCartState of ActiveCartData
        | PaidCartState of PaidCartData


    // Next, define the transitions using types

    /// "initCart" creates a new cart given the first CartItem
    type InitCart = CartItem -> ShoppingCart

    /// "addToActive" creates a new state from active data and a new CartItem
    type AddToActive = CartItem -> ActiveCartData -> ShoppingCart

    /// "pay" creates a new state from active data and a Payment
    type Pay = Payment -> ActiveCartData -> ShoppingCart

    /// "removeFromActive" creates a new state from active data after removing an CartItem
    type RemoveFromActive = CartItem -> ActiveCartData -> ShoppingCart


    // --------------------------------------------
    // Implementation of the API
    // --------------------------------------------

    /// "initCart" creates a new cart when adding the first item
    let initCart : InitCart =
        fun itemToAdd ->
            let activeData = [itemToAdd]
            ActiveCartState activeData

    /// "addToActive" creates a new state from active data and a new item
    let addToActive : AddToActive =
        fun itemToAdd activeCartData ->
            let newActiveData = itemToAdd::activeCartData
            ActiveCartState newActiveData

    /// "pay" creates a new state from active data and a payment amount
    let pay : Pay =
        fun payment activeCartData ->
            let paidData = activeCartData, payment
            PaidCartState paidData

    // you'll need this helper for removeItem transition
    let private removeItemFromContents (productToRemove:CartItem) (cart:CartContents) :CartContents =
        cart |> List.filter (fun prod -> prod <> productToRemove)

    /// "removeFromActive" creates a new state from active data after removing an item
    let removeFromActive : RemoveFromActive =
        fun itemToRemove activeCartData ->
            let newContents = removeItemFromContents itemToRemove activeCartData
            // removeItem is tricky -- you need to test the card contents after removal to
            // find out what the new state is!
            match newContents with
            | [] -> EmptyCartState
            | smallerData -> ActiveCartState smallerData
