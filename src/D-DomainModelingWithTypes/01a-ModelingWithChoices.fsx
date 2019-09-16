// ======================================================
// Modeling temperature as a choice
// ======================================================

type Temp =
    | F of int
    | C of float

let printTemperature temp =
    match temp with
    | F fTemp -> sprintf "%iF" fTemp
    | C cTemp -> sprintf "%fC" cTemp


let t1 = F 101
let t2 = C 38.5

printTemperature t1
printTemperature t2


// ======================================================
// Modeling an empty wallet as a choice
// ======================================================

type Currency = Currency of float
type Wallet =
    | Amount of Currency
    | NoMoney

let printWallet wallet =
    match wallet with
    | Amount curr -> sprintf "%A" curr
    | NoMoney -> sprintf "No Money"

// test
let w1 = Amount (Currency 1.2)
let w2 = NoMoney


printWallet w1
printWallet w2

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

let printOption opt =
    match opt with
    | Some data ->
        sprintf "Some %i" data
    | None ->
        "None"


// test
let someInt = Some 1
let noInt = None


printOption someInt
printOption noInt




