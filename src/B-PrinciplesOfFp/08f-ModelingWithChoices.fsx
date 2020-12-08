//===================================
// Choice types are very useful for modeling
//
// This file shows some examples.
//
// Exercise:
//    look at, execute, and understand all the code in this file
//===================================

(*
IMPORTANT: Choice/Union types are not subclasses!

type PaymentMethod =
| Cash
| Card of CardInfo
| PayPal of EmailAddress

You can think of the choices as more as like different constructors
for the same type.

In an OO language this would be like having constructors like this:

class PaymentMethod {
    static PaymentMethod Cash() {...}
    static PaymentMethod Card(CardInfo cardInfo) {...}
    static PaymentMethod PayPal(EmailAddress emailAddress) {...}
    }

Cash, Card and PayPal are NOT separate types.
So it is NOT like having the subclasses below:

class PaymentMethod {}
sealed class Cash extends PaymentMethod {}
sealed class Card extends PaymentMethod {}
sealed class PayPal extends PaymentMethod {}

*)

// ======================================================
// Modeling temperature as a choice
// ======================================================

// Define a type
type Temp =
    | F of int
    | C of float
 // | K of float  // what happens if you add this new case later?


// Construct some example values
let t1 = F 101
let t2 = C 38.5

// match the two choices
let printTemperature x =
    match x with
    | F fTemp -> sprintf "%iF" fTemp
    | C cTemp -> sprintf "%fC" cTemp

// test the function
printTemperature t1
printTemperature t2


// ======================================================
// Modeling an empty wallet as a choice
// ======================================================

// Define some types
type MoneyAmount = MoneyAmount of float
type Wallet =
    | WithMoney of MoneyAmount
    | EmptyWallet

// Construct some example values
let w1 = WithMoney (MoneyAmount 1.2)
let emptyWallet = EmptyWallet

(*
---------------------------------------
Define a function to print the contents of the wallet
*)

// define a function that matches the two choices
let printWallet wallet =
    match wallet with
    | WithMoney amount -> sprintf "Wallet has %A" amount
    | EmptyWallet -> sprintf "Wallet is empty :("

// test the function
printWallet w1
printWallet emptyWallet

(*
---------------------------------------
Define a function to add money to the wallet
*)

// If the wall
let addMoney amount wallet =
    match wallet with
    | WithMoney (MoneyAmount money)  ->
        let newMoney =  MoneyAmount (money + amount)
        let newWallet =  WithMoney newMoney
        newWallet
    | EmptyWallet ->
        let newWallet =  WithMoney (MoneyAmount amount)
        newWallet

// test the addMoney function
w1 |> addMoney 4.0
emptyWallet |> addMoney 4.0

// this is a bug! How could you fix it?
w1 |> addMoney -4.0



(*
---------------------------------------
A more complicated example -- paying with a Wallet
*)

type PaymentResult =
  | PaidSuccessfully of Wallet // return the remaining wallet
  | NotEnoughMoneyInWallet
  | NoMoneyInWallet

let payWithWallet amount wallet :PaymentResult=
    match wallet with

    | WithMoney (MoneyAmount money)  ->        // do we have any money?
      if money > amount then                   // do we have enough money?
        let remainingMoney =  MoneyAmount (money - amount)
        let newWallet = WithMoney remainingMoney
        PaidSuccessfully newWallet
      else if money = amount then              // do we have exact amount?
        let newWallet = EmptyWallet
        PaidSuccessfully newWallet
      else
        NotEnoughMoneyInWallet
    | EmptyWallet ->
        NoMoneyInWallet

let printPaymentResult result =
  match result with
  | PaidSuccessfully wallet -> printfn "Paid! Remaining in wallet: %A" wallet
  | NotEnoughMoneyInWallet -> printfn "Not enough money in wallet"
  | NoMoneyInWallet -> printfn "No money in wallet"

// test the function
w1 |> payWithWallet 1.1 |> printPaymentResult
w1 |> payWithWallet 1.2 |> printPaymentResult
w1 |> payWithWallet 1.3 |> printPaymentResult
emptyWallet |> payWithWallet 1.3 |> printPaymentResult
emptyWallet |> addMoney 2.0 |> payWithWallet 1.3 |> printPaymentResult


// ============================
// Nested matching
// ============================

// Define some types
type CardType = Visa | Mastercard
type CardInfo = { CardNumber : string; CardType : CardType }
type EmailAddress = EmailAddress of string
type Payment =
    | Card of CardInfo
    | Paypal of EmailAddress

// Construct some example values
let visaPayment = Card {CardNumber="123"; CardType=Visa}
let mcPayment = Card {CardNumber="123"; CardType=Mastercard}
let paypalPayment = Paypal (EmailAddress "me@example.com")

// ---------------------------------------
// basic matching
let printPayment_v1 payment =
  match payment with
  | Card cardInfo ->
    // extract the fields of cardInfo with dots: .CardNumber .CardType
    printfn "Paid by Card: number: %s type: %A" cardInfo.CardNumber cardInfo.CardType
  | Paypal emailAddress ->
    // extract the inner value of the EmailAddress
    let (EmailAddress e) = emailAddress
    printfn "Paid by Paypal: email: %s " e

// ---------------------------------------
// Refactored to extract cardNumber/cardType/EmailAddress
// directly in the match
let printPayment_v2 payment =
  match payment with
  | Card {CardNumber=cardNumber; CardType=cardType} ->
    // the fields of the CardInfo are already extracted!
    printfn "Paid by Card: number: %s type: %A" cardNumber cardType
  | Paypal (EmailAddress e) ->
    // the inner value of the EmailAddress is already extracted!
    printfn "Paid by Paypal: email: %s " e

// ---------------------------------------
// We can also match on inner fields too.
// Here we have DIFFERENT matches for Visa vs Mastercard
let printPayment_v3 payment =
  match payment with
  | Card {CardNumber=cardNumber; CardType=Visa} ->
    printfn "Paid by Visa: number: %s" cardNumber
  | Card {CardNumber=cardNumber; CardType=Mastercard} ->
    printfn "Paid by Mastercard: number: %s" cardNumber
  | Paypal (EmailAddress e) ->
    printfn "Paid by Paypal: email: %s " e


// test
printPayment_v1 visaPayment
printPayment_v1 mcPayment
printPayment_v1 paypalPayment

// test
printPayment_v2 visaPayment
printPayment_v2 mcPayment
printPayment_v2 paypalPayment

// test
printPayment_v3 visaPayment
printPayment_v3 mcPayment
printPayment_v3 paypalPayment


// ======================================================
// Modeling optional values as a choice
// ======================================================

(*
/// this is the same definition as the built-in type
type Option<'a> =
    | Some of 'a
    | None
*)
// IMPORTANT: Reset the F# interactive session after defining!

// Construct some examples of Option
let someInt = Some 1
let noInt = None

// match the two choices
let printOption opt =
    match opt with
    | Some data ->
        sprintf "Some %A" data
    | None ->
        "None"

// test the matching function
printOption someInt
printOption noInt


// Working with wrapped data by unwrapping it and then re-wrapping it is painful.
// The "optionMap" function defined below allows a function to be applied to wrapped data
// without unwrapping it in the top level code.
//
// NOTE: This is a re-implementation of the built-in function Option.map
let optionMap f opt =
    match opt with
    | Some data ->
        Some (f data)
    | None ->
        None

someInt |> optionMap (fun x -> x + 42) |> printOption
noInt |> optionMap (fun x -> x + 42) |> printOption


// ======================================================
// Modeling success/failure values as a choice
// ======================================================

(*
/// this is the same definition as the built-in type
type Result<'a,'b> =
    | Ok of 'a
    | Error of 'b
*)
// IMPORTANT: Reset the F# interactive session after defining!

// Construct some examples of Option
let success = Ok 1
let error = Error "something bad happened"

// match the two choices
let printResult result =
    match result with
    | Ok successValue ->
        sprintf "Success %A" successValue
    | Error failureValue ->
        sprintf "Failure %A" failureValue

// test the matching function
printResult success
printResult error

// Working with wrapped data by unwrapping it and then re-wrapping it is painful.
// The "resultMap" function defined below allows a function to be applied to wrapped data
// without unwrapping it in the top level code.
//
// NOTE: This is a re-implementation of the built-in function Result.map
let resultMap f result =
    match result with
    | Ok data ->
        Ok (f data)
    | Error e ->
        Error e

success |> resultMap (fun x -> x + 42) |> printResult
error |> resultMap (fun x -> x + 42) |> printResult


// ======================================================
// Modeling recursive data with a choice
// ======================================================

type StringList =
    | Empty
    | NonEmpty of string * StringList

type DirectoryItem =
    | SubDir of Directory
    | File of string
and Directory = DirectoryItem list

