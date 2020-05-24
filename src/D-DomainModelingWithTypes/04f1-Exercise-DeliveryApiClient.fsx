// ================================================
// Exercise: Implement a client of the DeliveryAPI
//
// ================================================

(*
Exercise: write some client code that uses the DeliveryAPI

Rule: "You can't put a package on a truck if it is already out for delivery"
Rule: "You can't sign for a package that is already delivered"

States are:
* UndeliveredState (with Package)
* OutForDeliveryState (with Package, TruckId and AttemptedAt timestamp)
* DeliveredState (with Package, Signature and DeliveredAt timestamp)
* FailedDeliveryState (with Package and FailedAt timestamp)

*)

open System

#load "DeliveryApiImplementation.fsx"
open DeliveryApiImplementation
open DeliveryApiImplementation.DeliveryDomain
open DeliveryApiImplementation.DeliveryApi

#load "DeliveryApiImplementation.fsx"
open DeliveryApiImplementation
open DeliveryApiImplementation.DeliveryDomain
open DeliveryApiImplementation.DeliveryApi

// ================================================
// Now write some client code that uses this API
// ================================================

module DeliveryClient =

    let putShipmentOnTruck (truckId:TruckId) state =
        // Logic is:
        // if Undelivered, use "sendOutForDelivery"
        // if Failed, use "redeliver"
        // in all other cases, print a warning and return the original state
        match state with
        | UndeliveredState data ->
            DeliveryApi.sendOutForDelivery data truckId
        | OutForDeliveryState _ ->
            printfn "package already out"
            ??
        | DeliveredState _ -> ??


    let markAsDelivered (signature:Signature) state =
        // Logic is:
        // if OutForDelivery, use "signedFor"
        // in all other cases, print a warning and return the original state
        match state with
        | UndeliveredState _  -> ??
        | OutForDeliveryState data -> ??

    let markAddressNotFound state =
        // Logic is:
        // if OutForDelivery, use "addressNotFound"
        // in all other cases, print a warning and return the original state
        match state with
        | UndeliveredState _  ->
            printfn "package not out"
            state // return original state
        | OutForDeliveryState data -> ??



// ================================================
// Now write some test code
// ================================================

open DeliveryClient

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
