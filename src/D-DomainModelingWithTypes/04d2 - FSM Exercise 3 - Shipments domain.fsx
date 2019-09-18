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

module ShipmentDomain =

    // define an "undefined" type for use when we don't know what type to use
    type undefined = exn

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
        // add TruckId, AttemptedAt fields
        PreviousAttempts : DeliveryAttempt list
        }

    /// Information about a package in an FailedDelivery state
    type FailedDeliveryData = {
        Package : Package
        PreviousAttempts :  DeliveryAttempt list
        }

    /// Information about a package in a Delivered state
    type DeliveredData = {
        Package : Package
        // add Signature and DeliveredAt fields
        }

    // =========================================
    // 3) Create a type that represent the choice of all the states

    type Shipment =
        | UndeliveredState of UndeliveredData
        | OutForDeliveryState of OutForDeliveryData
        | DeliveredState of undefined // what goes here?
        | FailedDeliveryState of undefined // what goes here?

