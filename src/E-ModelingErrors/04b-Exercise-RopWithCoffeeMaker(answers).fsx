(*
Coffee maker
*)

#load "Result.fsx"

/// The request from the user
type Request =
  | Espresso
  | Cappuccino
  | Latte
  | HotWater

/// The state of the coffee machine
type CoffeeMachineState = {
  HasWater : bool
  HasCoffee : bool
  HasMilk : bool
}

/// All possible errors
type ErrorMessage =
  | NoWater
  | NoCoffee
  | NoMilk

/// If there is enough water, return OK of request.
/// If not, return an error
let checkWaterStatus coffeeMachineState request :Result<Request,ErrorMessage> =
  if coffeeMachineState.HasWater then
    Ok request
  else
    Error NoWater

/// If there is enough coffee, return OK of request.
/// If not, return an error.
let checkCoffeeStatus coffeeMachineState request :Result<Request,ErrorMessage> =
  match request with
  | HotWater ->
    Ok request
  | Espresso | Cappuccino | Latte ->
    if coffeeMachineState.HasCoffee then
      Ok request
    else
      Error NoCoffee

/// If there is enough coffee, return OK of request.
/// If not, return an error.
let checkMilkStatus coffeeMachineState request :Result<Request,ErrorMessage> =
  match request with
  | HotWater | Espresso  ->
    Ok request
  | Cappuccino | Latte ->
    if coffeeMachineState.HasMilk then
      Ok request
    else
      Error NoMilk

/// Combine all the validations.
/// If they are all good, return OK of request.
/// If not, return an error.
let validateRequest coffeeMachineState request :Result<Request,ErrorMessage> =
  request
  |> checkWaterStatus coffeeMachineState
  |> Result.bind (checkCoffeeStatus coffeeMachineState)
  |> Result.bind (checkMilkStatus coffeeMachineState)


// =============================
// Tests
// =============================


let goodState = { HasWater=true;HasCoffee=true; HasMilk=true }
Espresso |> validateRequest goodState
Latte |> validateRequest goodState

let noWaterState = { HasWater=false;HasCoffee=true; HasMilk=true }
HotWater |> validateRequest noWaterState
Espresso |> validateRequest noWaterState
Latte |> validateRequest noWaterState

let noCoffeeState = { HasWater=true;HasCoffee=false; HasMilk=true }
HotWater |> validateRequest noCoffeeState
Espresso |> validateRequest noCoffeeState
Latte |> validateRequest noCoffeeState

let noMilkState = { HasWater=true;HasCoffee=true; HasMilk=false }
HotWater |> validateRequest noMilkState
Espresso |> validateRequest noMilkState
Latte |> validateRequest noMilkState
