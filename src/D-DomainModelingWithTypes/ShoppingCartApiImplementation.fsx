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

    type CartItem = string   // placeholder for now
    type Payment = float     // placeholder for now


// -----------------------------------------------
// Model the state machine with a type and transitions
// This is the "API" for the state machine
// -----------------------------------------------

module ShoppingCartApi =
    open ShoppingCartDomain

    // Create types to represent the data stored for each state
    type ActiveCartData =
        ActiveCartData of CartItem list

    type PaidCartData = {
        Contents: CartItem list
        Payment : Payment
        }

    // Create a "state" type that represent the union of all the states
    type ShoppingCart =
        | EmptyCartState
        | ActiveCartState of ActiveCartData
        | PaidCartState of PaidCartData


    // Next, define the transitions using types

    /// "initCart" creates a new cart given the first CartItem
    type InitCart =
        CartItem -> ShoppingCart

    /// "addToActive" creates a new state from active data and a new CartItem
    type AddToActive =
        CartItem * ActiveCartData -> ShoppingCart

    /// "pay" creates a new state from active data and a Payment
    type Pay =
        Payment * ActiveCartData -> ShoppingCart

    /// "removeFromActive" creates a new state from active data after removing an CartItem
    type RemoveFromActive =
        CartItem * ActiveCartData -> ShoppingCart


    // --------------------------------------------
    // Implementation of the API
    // --------------------------------------------

    /// "initCart" creates a new cart when adding the first item
    let initCart : InitCart =
        fun itemToAdd ->
            let cartItems = [itemToAdd]  // new list with item in it
            let activeData = ActiveCartData cartItems
            ActiveCartState activeData    // wrap the list with the State

    /// "addToActive" creates a new state from active data and a new item
    let addToActive : AddToActive =
        fun (itemToAdd,ActiveCartData cartItems) ->
            // "::" means prepend to list
            let newCartItems = itemToAdd::cartItems
            let newActiveData = ActiveCartData newCartItems
            ActiveCartState newActiveData

    /// "pay" creates a new state from active data and a payment amount
    let pay : Pay =
        fun (payment,ActiveCartData cartItems) ->
            let paidData = {Contents = cartItems; Payment = payment}
            PaidCartState paidData

    // you'll need this helper for removeItem transition
    let private removeItemFromContents (productToRemove:CartItem) (cartItems:CartItem list) :CartItem list =
        cartItems |> List.filter (fun prod -> prod <> productToRemove)

    /// "removeFromActive" creates a new state from active data after removing an item
    let removeFromActive : RemoveFromActive =
        fun (itemToRemove,ActiveCartData cartItems) ->
            let newCartItems = removeItemFromContents itemToRemove cartItems
            // removeItem is tricky -- you need to test the card contents after removal to
            // find out what the new state is!
            if newCartItems.IsEmpty then
                EmptyCartState
            else
                let newCartData = ActiveCartData newCartItems
                ActiveCartState newCartData


