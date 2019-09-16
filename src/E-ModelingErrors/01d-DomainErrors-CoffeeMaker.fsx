
type CoffeeChoice =
    | Espresso
    | ??

type SuccessNotification = string

type CoffeeMakingError =
    | ??

type MakeCoffee =
  CoffeeChoice -> Result<SuccessNotification,CoffeeMakingError>

