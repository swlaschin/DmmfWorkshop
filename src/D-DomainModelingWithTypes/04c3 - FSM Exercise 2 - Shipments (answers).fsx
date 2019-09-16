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
* Undelivered
* OutForDelivery
* Delivered
*)

open System

module ShipmentsDomain =

    // =========================================
    // 1) Start with the domain types that are independent of state

    type Package = string // placeholder for now
    type TruckId = int
    type DeliveryDate = DateTime
    type Signature = string

    // =========================================
    // 2) Create types to represent the data stored for each state

    /// Information about a package in an Undelivered state
    type UndeliveredData = {
        Package : Package
        }

    /// Tracks a single attempt to deliver a package
    type DeliveryAttempt = {
        Package : Package
        AttemptedAt : DeliveryDate
        }

    /// Information about a package in an OutForDelivery state
    type OutForDeliveryData = {
        Package : Package
        TruckId : TruckId
        AttemptedAt : DeliveryDate
        PreviousAttempts : DeliveryAttempt list
        }

    /// Information about a package in a Delivered state
    type DeliveredData = {
        Package : Package
        Signature : Signature
        DeliveredAt : DeliveryDate
        }

    /// Information about a package in an FailedDelivery state
    type FailedDeliveryData = {
        Package : Package
        PreviousAttempts :  DeliveryAttempt list
        }

    // =========================================
    // 3) Create a type that represent the choice of all the states

    type Shipment =
        | UndeliveredState of UndeliveredData
        | OutForDeliveryState of OutForDeliveryData
        | DeliveredState of DeliveredData
        | FailedDeliveryState of FailedDeliveryData

    // =========================================
    // 4) Create transition functions that transition from one state type to another


    /// Send a package out for delivery the first time and return a OutForDeliveryData
    let sendOutForFirstDelivery (undelivered:UndeliveredData) (truckId:TruckId)  :OutForDeliveryData =
        // create data associated with new state
        let attemptedAt = System.DateTime.UtcNow
        let outForDeliveryData : OutForDeliveryData = {
            Package = undelivered.Package
            TruckId = truckId
            AttemptedAt = attemptedAt
            PreviousAttempts = []
            }
        // return the data
        outForDeliveryData

    /// When an OutForDelivery fails, return a new FailedDeliveryData
    let addressNotFound (outForDelivery:OutForDeliveryData) :FailedDeliveryData =
        // create data associated with new state
        let previousAttempt : DeliveryAttempt = {
            Package = outForDelivery.Package
            AttemptedAt = outForDelivery.AttemptedAt
            }
        let previousAttempts = previousAttempt :: outForDelivery.PreviousAttempts
        let failedDeliveryData : FailedDeliveryData = {
            Package =  outForDelivery.Package
            PreviousAttempts = previousAttempts
            }
        // return the data
        failedDeliveryData

    /// When an OutForDelivery succeeds, return a new DeliveredData
    let signedFor (outForDelivery:OutForDeliveryData) (signature:Signature) :DeliveredData =
        let deliveredData : DeliveredData = {
            Package = outForDelivery.Package
            Signature = signature
            DeliveredAt = outForDelivery.AttemptedAt
            }
        deliveredData


    /// Send a package out for delivery the after a failure and return a OutForDeliveryData
    let sendOutForRedelivery (failedDelivery:FailedDeliveryData) (truckId:TruckId)  :OutForDeliveryData =
        // create data associated with new state
        let attemptedAt = System.DateTime.UtcNow
        let outForDeliveryData : OutForDeliveryData = {
            Package = failedDelivery.Package
            TruckId = truckId
            AttemptedAt = attemptedAt
            PreviousAttempts = failedDelivery.PreviousAttempts
            }
        // return the data
        outForDeliveryData

// ================================================
// Now write some client code that uses this API
// ================================================

module ShipmentsClient =
    open ShipmentsDomain

    let putShipmentOnTruck (truckId:TruckId) state =
        match state with
        | UndeliveredState data ->
            let newData = sendOutForFirstDelivery data truckId
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
            let newData = sendOutForRedelivery data truckId
            OutForDeliveryState newData

    let markAsDelivered (signature:Signature) state =
        match state with
        | UndeliveredState _  ->
            printfn "package not out"
            state  // return original state
        | OutForDeliveryState data ->
            let newData = signedFor data signature
            DeliveredState newData
        | DeliveredState data ->
            printfn "package already delivered"
            state // return original state
        | FailedDeliveryState _ ->
            printfn "package not out"
            state  // return original state

    let markAddressNotFound state =
        match state with
        | UndeliveredState _  ->
            printfn "package not out"
            state // return original state
        | OutForDeliveryState data ->
            let newData = addressNotFound data
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

open ShipmentsDomain
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

// errors when using the wrong state
delivered |> markAsDelivered signature
delivered |> putShipmentOnTruck truckId

outForDelivery |> markAddressNotFound