(*
Coffee maker
*)

#load "Result.fsx"

type Request =
  | Espresso
  | Cappuccino
  | Latte
  | HotWater

type CoffeeMachineState = {
  HasWater : bool
  HasCoffee : bool
  HasMilk : bool
}

type ErrorMessage =
  | NoWater
  | NoCoffee
  | NoMilk

let checkWaterStatus coffeeMachineState request =
  if coffeeMachineState.HasWater then
    ??
  else
    ??

let checkCoffeeStatus coffeeMachineState request =
  match request with
  | HotWater -> ??
  | Espresso | Cappuccino | Latte ->
    if coffeeMachineState.HasCoffee then
      ??
    else
      ??

let checkMilkStatus coffeeMachineState request =
  match request with
  | HotWater | Espresso  ->
    ??
  | Cappuccino | Latte ->
    ??


let validateRequest coffeeMachineState request =
  request
  |> checkWaterStatus coffeeMachineState
  |> (checkCoffeeStatus coffeeMachineState)
  |> (checkMilkStatus coffeeMachineState)

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
