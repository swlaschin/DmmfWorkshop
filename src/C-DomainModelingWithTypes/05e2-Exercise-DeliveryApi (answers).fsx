// ================================================
// Exercise: Design the API of the Delivery state machine
//
// See 04e0-Exercise-Delivery(diagram).png
// ================================================


(*
Exercise: write some client code that uses the DeliveryAPI

Rule: "You can't put a package on a truck if it is already out for delivery"
Rule: "You can't sign for a package that is already delivered"

States are:
* EmptyCartState
* ActiveCartState
* PaidCartState
*)


(*
Exercise 1: create types that model states in the package delivery system

Rule: "You can't put a package on a truck if it is already out for delivery"
Rule: "You can't sign for a package that is already delivered"

States are:
* UndeliveredState (with Package)
* OutForDeliveryState (with Package, TruckId and AttemptedAt timestamp)
* DeliveredState (with Package, Signature and DeliveredAt timestamp)
* FailedDeliveryState (with Package and FailedAt timestamp)

Exercise 2: create types that model state TRANSITIONS in the package delivery system
* this is up to you!

*)

open System

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
