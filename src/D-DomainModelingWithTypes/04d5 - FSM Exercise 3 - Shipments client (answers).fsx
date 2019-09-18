// ================================================
// FSM Exercise: Modeling package delivery transitions
//
// See Shipments transition diagram.png
///
// ================================================

(*
Exercise: create types that model package delivery transitions

Rule: "You can't put a package on a truck if it is already out for delivery"
Rule: "You can't sign for a package that is already delivered"

States are:
* UndeliveredState
* OutForDeliveryState
* DeliveredState
* FailedDeliveryState
*)

open System

#load "04d3 - FSM Exercise 3 - Shipments domain (answers).fsx"
open ``04d3 - FSM Exercise 3 - Shipments domain (answers)``
open ShipmentsDomain

// ================================================
// Define the API (implemented in a separate file)
// ================================================

#load "ShipmentTransitions.fsx"
open ShipmentTransitions

module ShipmentApi =
    let sendOutForFirstDelivery undelivedData truckId =
        ShipmentTransitions.sendOutForFirstDelivery undelivedData truckId

    let sendOutForRedelivery failedDeliveryData truckId =
        ShipmentTransitions.sendOutForRedelivery failedDeliveryData truckId

    let addressNotFound outForDeliveryData =
        ShipmentTransitions.addressNotFound outForDeliveryData

    let signedFor outForDeliveryData signature =
        ShipmentTransitions.signedFor outForDeliveryData signature

// ================================================
// Now write some client code that uses this API
// ================================================

module ShipmentsClient =
    open ShipmentsDomain

    let putShipmentOnTruck (truckId:TruckId) state =
        // Logic is:
        // if Undelivered, use "sendOutForFirstDelivery" and put the result in the OutForDeliveryState case
        // if Failed, use "sendOutForRedelivery" and put the result in the OutForDeliveryState case
        // in all other cases, print a warning and return the original state
        match state with
        | UndeliveredState data ->
            let newData = ShipmentApi.sendOutForFirstDelivery data truckId
            // create a new state from that data
            OutForDeliveryState newData
        | OutForDeliveryState _ ->
            printfn "package already out"
            // return original state
            state
        | DeliveredState _ ->
            printfn "package already delivered"
            // return original state
            state
        | FailedDeliveryState data ->
            let newData = ShipmentApi.sendOutForRedelivery data truckId
            OutForDeliveryState newData

    let markAsDelivered (signature:Signature) state =
        // Logic is:
        // if OutForDelivery, use "signedFor" and put the result in the DeliveredState case
        // in all other cases, print a warning and return the original state
        match state with
        | UndeliveredState _  ->
            printfn "package not out"
            state  // return original state
        | OutForDeliveryState data ->
            let newData = ShipmentApi.signedFor data signature
            DeliveredState newData
        | DeliveredState data ->
            printfn "package already delivered"
            state // return original state
        | FailedDeliveryState _ ->
            printfn "package not out"
            state  // return original state

    let markAddressNotFound state =
        // Logic is:
        // if OutForDelivery, use "addressNotFound" and put the result in the FailedDeliveryState case
        // in all other cases, print a warning and return the original state
        match state with
        | UndeliveredState _  ->
            printfn "package not out"
            state // return original state
        | OutForDeliveryState data ->
            let newData = ShipmentApi.addressNotFound data
            FailedDeliveryState newData
        | DeliveredState _ ->
            printfn "package already delivered"
            state // return original state
        | FailedDeliveryState _ ->
            printfn "package not out"
            state  // return original state



// ================================================
// Now write some test code
// ================================================

open ShipmentsClient

let package = "My Package"
let newShipment =
    let data : UndeliveredData = {Package = package}
    UndeliveredState data

let truckId = 123
let outForDelivery =
    newShipment |> putShipmentOnTruck truckId

let signature = "Scott"
let delivered =
    outForDelivery |> markAsDelivered signature

let failedDelivery =
    outForDelivery |> markAddressNotFound

// errors when using the wrong state
delivered |> markAsDelivered signature
delivered |> putShipmentOnTruck truckId
outForDelivery |> putShipmentOnTruck truckId
