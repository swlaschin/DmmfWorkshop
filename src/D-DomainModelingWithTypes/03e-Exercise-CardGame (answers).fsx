// ================================================
// Exercise: Model a card game
//
// ================================================

(*
The domain model is:

* A card is a combination of a Suit (Heart, Spade) and a Rank (Two, Three, ... King, Ace)
* A hand is a list of cards
* A deck is a list of cards
* A player has a name and a hand
* A game consists of a deck and list of players
* To deal, remove a card from a shuffled deck
* To pick up a card, add a card to a hand

Exercise:
* Define types to represent the domain
* Optional: implement any functions that are easy!

*)


module CardGame =

    type Suit = Club | Diamond | Spade | Heart

    type Rank = Two | Three | Four | Five | Six | Seven | Eight
                       | Nine | Ten | Jack | Queen | King | Ace

    type Card = { Suit : Suit; Rank: Rank}

    type Hand = Card list
    type Deck = Card list

    type Player = { Name : string; Hand : Hand}
    type Game = { Deck : Deck; Players : Player list}

    type ShuffledDeck = ShuffledDeck of Card list
    type Deal = ShuffledDeck -> Card option * ShuffledDeck
    type PickUp = Card * Hand -> Hand
    type Shuffle  = Deck -> ShuffledDeck


module CardGameImplementation =
    open CardGame

    let pickup : PickUp =
        fun (card,hand) ->
            // the "::" operator prepends the card to the hand
            let newHand = card::hand
            newHand

    let deal : Deal =
        fun (ShuffledDeck deck) ->
            // the "::" pattern deconstructs a non-empty list into head/tail
            match deck with
            | first::rest -> Some first, ShuffledDeck rest
            | [] -> None, ShuffledDeck []

    let shuffle : Shuffle =
        fun deck ->
            // don't worry about how to do proper shuffling
            let random = System.Random()

            deck
            |> List.map (fun card -> random.Next(), card )  // add a random to each card
            |> List.sortBy (fun (rand,card) -> rand)       // sort
            |> List.map (fun (rand,card) -> card)          // remove the random number
            |> ShuffledDeck
