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

type Option<'a> =
    | Some of 'a
    | None
let printOption opt =
    match opt with
    | Some dataAssociatedWithThisCase ->
        sprintf "Some %i" dataAssociatedWithThisCase
    | None -> "None"


// test
let someInt = Some 1
let noInt = None


printOption someInt
printOption noInt





let actual2 = printOption None
let expected2 = "None";
actual2 = expected2



