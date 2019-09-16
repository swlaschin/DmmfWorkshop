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
* Empty
* ActiveCartData
* PaidCartData
*)


module ShoppingCartDomain =

    // 1) Start with the domain types that are independent of state

    type Product = string     // placeholder for now
    type CartContents = Product list  // placeholder for now
    type Payment = float     // placeholder for now

    // 2) Create a type to represent the data stored for each type

    // type EmptyCartData = not needed
    type ActiveCartData = CartContents
    type PaidCartData = CartContents * Payment

    // 3) Create a type that represent the choice of all the states

    type ShoppingCart =
        | EmptyCartState
        | ActiveCartState of ActiveCartData
        | PaidCartState of PaidCartData

    // 4) Create transition functions that transition from one state to another

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
        | EmptyCartState ->
            printfn "Adding item %s to empty cart" newItem
            initCart newItem
        | ActiveCartState data ->
            printfn "Adding item %s to active cart" newItem
            addToActive newItem data
        | PaidCartState data ->
            printfn "Can't modify paid cart"
            cart // return original cart

    // "clientPayForCart " changes the cart state after paying
    // function signature should be
    //     Payment -> ShoppingCart-> ShoppingCart

    let clientPayForCart (payment:Payment) (cart:ShoppingCart)  :ShoppingCart =
        match cart with
        | EmptyCartState ->
            printfn "Can't pay for empty cart"
            cart // return original cart
        | ActiveCartState data ->
            printfn "Paying %g for active cart" payment
            pay payment data
        | PaidCartState data ->
            printfn "Cart already paid for"
            cart // return original cart


// ================================================
// Now write some test code
// ================================================

open ShoppingCartDomain
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
