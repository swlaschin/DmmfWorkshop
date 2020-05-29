// ================================================
// Exercise: Design the API of the ShoppingCart state machine
//
// See 04c0-Exercise-ShoppingCart(diagram).png
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

    // Create a "state" type that represents the union of all the states
    type ShoppingCart =
        | EmptyCartState
        | ActiveCartState of ActiveCartData
        | PaidCartState of PaidCartData

    // Next, define the transitions using types but
    // don't worry about implementing them right now

    /// "initCart" creates a new cart given the first CartItem
    type InitCart = CartItem -> ShoppingCart

    /// "addToActive" creates a new state from ActiveCartData and a new CartItem
    type AddToActive = ActiveCartData * CartItem -> ShoppingCart

    /// "pay" creates a new state from ActiveCartData and a Payment
    type Pay = ActiveCartData * Payment -> ShoppingCart

    /// "removeFromActive" creates a new state from ActiveCartData after removing an CartItem
    type RemoveFromActive = ActiveCartData * CartItem -> ShoppingCart



//-------------------------------
// Notes on State transition design
//-------------------------------

(*
These designs use the form:

    Substate * ExtraInfo -> WholeState
e.g
    ActiveCartData * Payment -> ShoppingCart
    ActiveCartData * CartItem -> ShoppingCart


Why not use this following form instead?

    WholeState * ExtraInfo -> WholeState

e.g
    ShoppingCart * Payment -> ShoppingCart
    ShoppingCart * CartItem -> ShoppingCart

The answer is that the first style never has any
unhandled cases -- it always succeeds.

In the second style, we have unhandled cases to consider:
E.g. "what is the cart is not in the ActiveCart state and we try to pay?"

In the first case, the *client* makes these decisions.
In the second case, the *server* makes these decisions.

If we do want to use the second style, we should return an error, rather
than just handling it silently, like this:

    type CartError =
        | CantPayForPaidCart
        | CantRemovedFromEmptyCart

    type AddToActive = ShoppingCart * CartItem -> Result<ShoppingCart,CartError>


We will talk more about error handling in the last session.
*)



