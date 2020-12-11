// ================================================
// FSM Exercise: Modeling package delivery transitions
//
// See Shipments transition diagram.png
//
// This file implements the state transitions. Requires the domain file answers
//
// ================================================

// -----------------------------------------------
// Model the domain types that are independent of state
// -----------------------------------------------

module DeliveryDomain =

    type Package = string // placeholder for now
    type TruckId = int
    type DeliveryTimestamp = System.DateTime
    type Signature = string

// -----------------------------------------------
// Model the state machine with a type and transitions
// This is the "API" for the state machine
// -----------------------------------------------

module DeliveryApi =
    open DeliveryDomain

    /// Information about a package in an Undelivered state
    type UndeliveredData = {
        Package : Package
        }

    /// Information about a package in an OutForDelivery state
    type OutForDeliveryData = {
        Package : Package
        TruckId : TruckId
        AttemptedAt : DeliveryTimestamp
        }

    /// Information about a package in a Delivered state
    type DeliveredData = {
        Package : Package
        Signature : Signature
        DeliveredAt : DeliveryTimestamp
        }

    /// Information about a package in an FailedDelivery state
    type FailedDeliveryData = {
        Package : Package
        FailedAt : DeliveryTimestamp
        }

    // Create a "state" type that represents the union of all the states
    type ShipmentState =
        | UndeliveredState of UndeliveredData
        | OutForDeliveryState of OutForDeliveryData
        | DeliveredState of DeliveredData
        | FailedDeliveryState of FailedDeliveryData

    // Next, define the transitions using types but
    // don't worry about implementing them right now

    /// "SendOutForDelivery" transitions to OutForDeliveryState given a UndeliveredData and TruckId
    type SendOutForDelivery = UndeliveredData -> TruckId -> ShipmentState

    /// "SignedFor" transitions to DeliveredState given OutForDelivery data and a Signature
    type SignedFor = OutForDeliveryData -> Signature -> ShipmentState

    /// "AddressNotFound" transitions to FailedDeliveryState given OutForDelivery data
    type AddressNotFound = OutForDeliveryData -> ShipmentState

    /// "Redeliver" transitions to OutForDeliveryState given FailedDelivery data and TruckId
    type Redeliver = FailedDeliveryData -> TruckId -> ShipmentState


    // =========================================
    // Implementation of the transition functions
    // =========================================


    /// Send a package out for delivery and transition to OutForDeliveryState
    let sendOutForDelivery : SendOutForDelivery =
        fun undelivered truckId ->
            // create data associated with new state
            let attemptedAt = System.DateTime.UtcNow
            let outForDeliveryData : OutForDeliveryData = {
                Package = undelivered.Package
                TruckId = truckId
                AttemptedAt = attemptedAt
                }
            // return the new state
            OutForDeliveryState outForDeliveryData

    /// When an OutForDelivery succeeds, transition to DeliveredState
    let signedFor : SignedFor =
        fun outForDelivery signature ->
            // create data associated with new state
            let deliveredData : DeliveredData = {
                Package = outForDelivery.Package
                Signature = signature
                DeliveredAt = outForDelivery.AttemptedAt
                }
            // return the new state
            DeliveredState deliveredData

    /// When an OutForDelivery fails, return a new FailedDeliveryData
    let addressNotFound : AddressNotFound =
        fun outForDelivery ->
            // create data associated with new state
            let failedDeliveryData : FailedDeliveryData = {
                Package =  outForDelivery.Package
                FailedAt = outForDelivery.AttemptedAt
                }
            // return the new state
            FailedDeliveryState failedDeliveryData

    /// Send a package out for redelivery
    let redeliver : Redeliver =
        fun failedDelivery truckId ->
            // create data associated with new state
            let attemptedAt = System.DateTime.UtcNow
            let outForDeliveryData : OutForDeliveryData = {
                Package = failedDelivery.Package
                TruckId = truckId
                AttemptedAt = attemptedAt
                }
            // return the new state
            OutForDeliveryState outForDeliveryData
