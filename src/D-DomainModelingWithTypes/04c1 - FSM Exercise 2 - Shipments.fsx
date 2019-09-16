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
* FailedDelivery
*)

open System


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
    // anything else??
    }

/// Information about a package in an FailedDelivery state
type FailedDeliveryData = {
    Package : Package
    PreviousAttempts :  DeliveryAttempt list
    }

/// Information about a package in a Delivered state
type DeliveredData = {
    Package : Package
    // anything else??
    }

// =========================================
// 3) Create a type that represent the choice of all the states

type Shipment =
    | UndeliveredState of UndeliveredData
    | OutForDeliveryState of OutForDeliveryData
    | DeliveredState of exn
    | FailedDeliveryState of exn

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
    // add implementation
    failwith "not implemented"

/// When an OutForDelivery succeeds, return a new DeliveredData
let signedFor (outForDelivery:OutForDeliveryData) (signature:Signature) :DeliveredData =
    // add implementation
    failwith "not implemented"

/// Send a package out for delivery the after a failure and return a OutForDeliveryData
let sendOutForRedelivery (failedDelivery:FailedDeliveryData) (truckId:TruckId)  :OutForDeliveryData =
    // add implementation
    failwith "not implemented"

// ================================================
// Now write some client code that uses this API
// ================================================

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
    | UndeliveredState data ->
        failwith "not implemented"

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