// =================================
// This file demonstrates the Coffee maker
// with Result AND Async
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// Load a file with library functions for Result
#load "Result.fsx"


// =========================
// The domain model
// =========================


module Domain =

    type Request =
      | Espresso
      | Cappuccino
      | Latte
      | HotWater

    type SuccessMessage = {
        Message : string
        Request : Request
        }

    type ErrorMessage =
      | NoWater
      | NoCoffee
      | NoMilk

    type MakeCoffee = Request -> AsyncResult<SuccessMessage,ErrorMessage>

// =========================
// Pure implementation code (no I/O)
// =========================

/// the core logic of the coffee maker
module Implementation =
    open Domain

    // represents the internal state of the coffee machine
    type CoffeeMachineState = {
      HasWater : bool
      HasCoffee : bool
      HasMilk : bool
    }

    // If the internal state has water,
    //   return the request
    // otherwise
    //   return ErrorMessage.NoWater
    let checkWaterStatus coffeeMachineState request =
      if coffeeMachineState.HasWater then
        Ok request
      else
        Error ErrorMessage.NoWater

    // If the request is HotWater, return the request
    // If the request is Espresso/Cappuccino/Latte then
    //   check if the internal state has coffee:
    //   * if yes, then return the request
    //   * if no, return ErrorMessage.NoCoffee
    let checkCoffeeStatus coffeeMachineState request =
      match request with
      | HotWater ->
        Ok request
      | Espresso | Cappuccino | Latte ->
        if coffeeMachineState.HasCoffee then
          Ok request
        else
          Error ErrorMessage.NoCoffee

    // If the request is HotWater/Espresso, return the request
    // If the request is Cappuccino/Latte then
    //   check if the internal state has milk:
    //   * if yes, then return the request
    //   * if no, return ErrorMessage.NoMilk
    let checkMilkStatus coffeeMachineState request =
      match request with
      | HotWater | Espresso  ->
        Ok request
      | Cappuccino | Latte ->
        if coffeeMachineState.HasMilk then
          Ok request
        else
          Error ErrorMessage.NoMilk

    // validate the request by checking against the internal state
    // of the machine
    let validateRequest coffeeMachineState request =
      request
      |> checkWaterStatus coffeeMachineState
      |> Result.bind (checkCoffeeStatus coffeeMachineState)
      |> Result.bind (checkMilkStatus coffeeMachineState)


    /// Validate the request and if OK, return the Success message
    /// otherwise return the error
    /// The pure workflow function with no I/O
    let pureMakeCoffee coffeeMachineState request =
        let result = validateRequest coffeeMachineState request
        match result with
        | Ok request ->
            let message = sprintf "%A is ready" request
            Ok {Message=message; Request=request}
        | Error err ->
            Error err


// =========================
// Impure implementation code (with I/O)
// =========================

/// The database to store the machine state
module Database =
    open Implementation

    // the "database" is just a mutable variable in memory :)
    let mutable machineState = { HasWater=true;HasCoffee=true; HasMilk=true }

    /// Load the state from the database
    let loadState() =
        async.Return machineState

    /// Save the state to the database
    let saveState newState =
        machineState <- newState
        async.Return ()


/// final workflow function with I/O
let makeCoffee : Domain.MakeCoffee =
    fun request -> async {
        // IO here
        let! machineState = Database.loadState()

        // pure code
        let result = Implementation.pureMakeCoffee machineState request

        // NOTE: we are not subtracting any water or coffee, so
        // the state doesn't change!
        // Database.saveState machineState

        // final output
        return result
        }



// =============================
// Tests
// =============================

open Domain
open Implementation

// Everything is good
Database.saveState { HasWater=true;HasCoffee=true; HasMilk=true } |> Async.RunSynchronously

makeCoffee Espresso |> Async.RunSynchronously // Ok
makeCoffee Latte    |> Async.RunSynchronously // Ok

// No water
Database.saveState { HasWater=false;HasCoffee=true; HasMilk=true } |> Async.RunSynchronously
makeCoffee HotWater |> Async.RunSynchronously // Error
makeCoffee Espresso |> Async.RunSynchronously // Error

// No coffee
Database.saveState { HasWater=true;HasCoffee=false; HasMilk=true } |> Async.RunSynchronously
makeCoffee HotWater |> Async.RunSynchronously // Ok
makeCoffee Espresso |> Async.RunSynchronously // Error

// No milk
Database.saveState { HasWater=true;HasCoffee=true; HasMilk=false } |> Async.RunSynchronously
makeCoffee HotWater |> Async.RunSynchronously // Ok
makeCoffee Espresso |> Async.RunSynchronously // Ok
makeCoffee Latte    |> Async.RunSynchronously // Error


