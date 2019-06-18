module Layer2Fs.CsApi

open Layer2Fs.DomainTypes
open System

let CreateCashPayment() = Cash
let CreateChequePayment checkNo = Cheque checkNo
let CreateCardPayment (cardType,cardNo) = Card (cardType,cardNo)

let ProcessPayment(paymentMethod,(fnCash:Action),(fnCheque:Action<int>),(fnCard:Action<CardType,CardNumber>)) =
    match paymentMethod with
    | Cash ->  fnCash.Invoke()
    | Cheque checkNo -> fnCheque.Invoke(checkNo)
    | Card (cardType,cardNo) -> fnCard.Invoke(cardType,cardNo)


