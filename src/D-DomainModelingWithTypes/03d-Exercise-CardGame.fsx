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

    type Suit = ??
    type Rank = ??
    type Card = ??

    type Hand = ??
    type Deck = ??

    type Player = ??
    type Game = ??

    type Deal = ??
    type PickUp = ??
    type Shuffle = ??

    (*
    // Sidebar: How do you add extra behavior,
    // such as calculating the scoring of a Hand?
    //
    // In OO you would add methods to the Card and Hand
    // In FP we would do a "transform" to a new kind of thing

    /// Aces High rule is used in Poker,
    /// or must be agreed at the beginning
    type AreAcesHigh = bool

    // For example Ace=13 Two=2
    type CardScore = int // constraint 1 - 13

    type Score = Card * AreAcesHigh -> CardScore
    type ScoreHand = Hand * AreAcesHigh -> CardScore

    *)


module CardGameImplementation =
    open CardGame

    // optionally implement some functions here

    (*
    let pickup : PickUp =
        fun (card,hand) ->
            // the "::" operator prepends the card to the hand
            let newHand = card::hand
            newHand
    *)

    (*
    let deal : Deal =
        fun (ShuffledDeck deck) ->
            // the "::" pattern deconstructs a non-empty list into head/tail
            match deck with
            | first::rest -> first, ShuffledDeck rest
            | [] -> // what goes here?
    *)


    (*
    let shuffle : Shuffle =
        fun deck ->
            // don't worry about how to do proper shuffling
            let random = System.Random()

            deck
            |> List.map (fun card -> random.Next(), card )  // add a random to each card
            |> List.sortBy (fun (rand,card) -> rand)        // sort
            |> List.map ??                                  // remove the random number
            |> ??
    *)