(*
Coffee maker using async

Just for demonstration - this is a bad design (why?)
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

let checkWaterStatus getCoffeeMachineState request =
  async {
    let! coffeeMachineState = getCoffeeMachineState()
    let result =
      if coffeeMachineState.HasWater then
        Ok request
      else
        Error NoWater
    return result
  }

let checkCoffeeStatus getCoffeeMachineState request =
  async {
    let! coffeeMachineState = getCoffeeMachineState()
    let result =
      match request with
      | HotWater ->
        Ok request
      | Espresso | Cappuccino | Latte ->
        if coffeeMachineState.HasCoffee then
          Ok request
        else
          Error NoCoffee
    return result
  }

let checkMilkStatus getCoffeeMachineState request =
  async {
    let! coffeeMachineState = getCoffeeMachineState()
    let result =
      match request with
      | HotWater | Espresso  ->
        Ok request
      | Cappuccino | Latte ->
        if coffeeMachineState.HasMilk then
          Ok request
        else
          Error NoMilk
    return result
  }


let validateRequest getCoffeeMachineState request =
  request
  |> checkWaterStatus getCoffeeMachineState
  |> AsyncResult.bind (checkCoffeeStatus getCoffeeMachineState)
  |> AsyncResult.bind (checkMilkStatus getCoffeeMachineState)

let validateRequest2 getCoffeeMachineState request =
  asyncResult {
    let! request = checkWaterStatus getCoffeeMachineState request
    let! request = checkCoffeeStatus getCoffeeMachineState request
    let! request =checkMilkStatus getCoffeeMachineState request
    return request
    }

let goodState() =
  { HasWater=true;HasCoffee=true; HasMilk=true }
  |> async.Return
Espresso |> validateRequest goodState
Latte |> validateRequest goodState

let noWaterState() =
  { HasWater=false;HasCoffee=true; HasMilk=true }
  |> async.Return
HotWater |> validateRequest noWaterState
Espresso |> validateRequest noWaterState
Latte |> validateRequest noWaterState

let noCoffeeState() =
  { HasWater=true;HasCoffee=false; HasMilk=true }
  |> async.Return
HotWater |> validateRequest noCoffeeState
Espresso |> validateRequest noCoffeeState
Latte |> validateRequest noCoffeeState

let noMilkState() =
  { HasWater=true;HasCoffee=true; HasMilk=false }
  |> async.Return
HotWater |> validateRequest noMilkState
Espresso |> validateRequest noMilkState
Latte |> validateRequest noMilkState
