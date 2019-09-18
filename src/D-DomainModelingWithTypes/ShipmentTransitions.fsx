// ================================================
// FSM Exercise: Modeling package delivery transitions
//
// See Shipments transition diagram.png
//
// This file implements the state transitions. Requires the domain file answers
//
// ================================================



// uncomment this to edit this file standalone
//#load "04d3 - FSM Exercise 3 - Shipments domain (answers).fsx"
open ``04d3 - FSM Exercise 3 - Shipments domain (answers)``

module ShipmentTransitions =
    open ShipmentsDomain

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
