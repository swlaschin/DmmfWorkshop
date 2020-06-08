// ================================================
// Exercise: Add error types to the examples below
//
// ================================================

/// helper type
type undefined = exn

//-------------------------------------
// Exercise 1.
// Add an error type to TicTacToe
//-------------------------------------

module TicTacToe =

    type Player = X | O

    type Square = {
      Row: int
      Col: int
    }

    type MoveInformation = {
      Player : Player
      Square: Square
    }

    type MoveSuccess =
      | Draw
      | Winner of Player
      | KeepPlaying

    // Exercise:
    // Document the possible errors that can happen
    // when making a move
    type MoveError =
      | PlayedSameSquare of Square
      | PlayedTwiceInARow of Player
      | PlayedWhenGameIsOver of Player

    // In the main workflow , use a Result as the output
    type PlayMove = MoveInformation -> Result<MoveSuccess,MoveError>


//-------------------------------------
// Exercise 2.
// Add an error type to AtmCashMachine
//-------------------------------------

module AtmCashMachine =

    type Card = Card of string
    type PIN = PIN of string
    type AmountToWithdraw = AmountToWithdraw of int

    type WithdrawalSuccess = {
        TransactionNumber : string
        Date : System.DateTime
        }

    // Exercise:
    // Document the possible errors that can happen
    // when withdrawing money
    type WithdrawalError =
        | CardNotAcceptedHere
        | PinNotValid
        | CantWithdrawThatMuchMoney
        | MachineOutOfOrder

    // In the main workflow , use a Result as the output
    type WithdrawMoney =
        Card * PIN * AmountToWithdraw -> Result<WithdrawalSuccess,WithdrawalError>



//-------------------------------------
// Exercise 3.
// Add an error type to CoffeeMaker
//-------------------------------------

module CoffeeMaker =

    type CoffeeChoice =
        | Espresso
        | Cappuccino
        | HotWater
        | HotMilk

    /// Tell the user that their coffee is ready
    type SuccessNotification = {
        Message : string
        CoffeeChoice : CoffeeChoice
        }

    // Exercise:
    // Document the possible errors that can happen
    // when making coffee

    /// Tell the user that their coffee was not made
    type CoffeeMakingError =
        | NoCoffeeAvailable
        | NoMilkAvailable
        | NoWaterAvailable

    // In the main workflow , use a Result as the output
    type MakeCoffee =
        CoffeeChoice -> Result<SuccessNotification,CoffeeMakingError>

