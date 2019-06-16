// ================================================
// FSM Exercise: Modeling package delivery transitions
//
// See Shipments transition diagram.png
///
// ================================================

(*
Exercise: create types that model package delivery transitions

This is much more complex! You might just want to look at the answers!


Rule: "You can't put a package on a truck if it is already out for delivery"
Rule: "You can't sign for a package that is already delivered"

States are:
* Undelivered
* OutForDelivery
* Delivered
*)

open System

module ShipmentsDomain =


    // 1) Start with the domain types that are independent of state

    type Package = string // placeholder for now
    type TruckId = int
    type SentUtc = SentUtc of DateTime  // Q: is it worth trying to keep the times distinct?
    type DeliveredUtc = DeliveredUtc of DateTime
    type Signature = string

    // 2) Create types to represent the data stored for each state

    type DeliveryAttempt = {
        Package: Package
        SentUtc: SentUtc
        }
    type UndeliveredData = {
        UndeliveredPackage: Package
        PreviousAttempts: DeliveryAttempt list
        }
    type OutForDeliveryData = {
        Attempt: DeliveryAttempt
        TruckId : TruckId
        PreviousAttempts: DeliveryAttempt list
        }
    type DeliveredData = {
        DeliveredPackage : Package
        Signature : Signature
        DeliveredUtc : DeliveredUtc
        }

    // 3) Create a type that represent the choice of all the states

    type Shipment =
        | what?
        | what?
        | what?

    // 4) Create transition functions that transition from one state type to another

    let sendOutForDelivery (package:Package) (anythingElse:whatType) :whatReturnType =
        let sentUtc = SentUtc System.DateTime.UtcNow
        let newAttempt = {Package=undeliveredData.UndeliveredPackage; SentUtc=sentUtc}
        // create a new state
        let outForDeliveryData = {
            Attempt = ???
            TruckId = truckId
            PreviousAttempts = ???
            }
        OutForDeliveryState ???

    let addressNotFound (outForDelivery:whatInputType) :whatReturnType =
        ??

    let signedFor (outForDeliveryData:OutForDeliveryData) (signature:Signature) :Shipment =
        ??

// ================================================
// Now write some client code that uses this API
// ================================================

module ShipmentsClient =
    open ShipmentsDomain

    let putShipmentOnTruck extraData state =
        match state with
        // | undelivered -> change state
        // | outfordelivery -> ignore?
        // | delivered -> ignore?


    let markAsDelivered (signature:Signature) state =
        // | undelivered -> ignore?
        // | outfordelivery -> change state
        // | delivered -> ignore?


// ================================================
// Now write some code that exercises the domain and client
// ================================================

open ShipmentsDomain
open ShipmentsClient

let package = "My Package"
let newShipment = UndeliveredState {UndeliveredPackage = package; PreviousAttempts =[] }

let truckId = 123
let outForDelivery =
    newShipment |> putShipmentOnTruck truckId

let signature = "Scott"
let delivered =
    outForDelivery |> markAsDelivered signature

// errors when using the wrong state
delivered |> markAsDelivered signature
delivered |> putShipmentOnTruck truckId