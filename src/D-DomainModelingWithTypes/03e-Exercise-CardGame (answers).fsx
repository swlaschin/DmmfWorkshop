// ================================================
// Exercise: Model a card game
//
// ================================================

(*
The domain model is:

* A card is
    * a combination of a Suit (Heart, Spade) and a Rank (Two, Three, ... King, Ace)
    * OR a Joker
* A hand is a list of cards
* A deck is a list of cards
* A shuffled deck is also a list of cards, but different from a normal deck
* A player has a name and a hand
* A game consists of a deck and list of players

Actions:
* To deal, remove a card from a shuffled deck and put it on the table
* To pick up a card, add a card to a hand
* To shuffle, start with a normal deck and create a shuffled deck from it

Exercise:
* Define types to represent the domain
* Implement any functions that you can (look at the answer file if you get stuck)

*)


module CardGame =
    type Suit = Club | Diamond | Spade | Heart

    type Rank = Two | Three | Four | Five | Six | Seven | Eight
                | Nine | Ten | Jack | Queen | King | Ace

    type NonJokerCard = { Suit : Suit; Rank: Rank}
    type Card =
        | NonJokerCard of NonJokerCard
        | Joker

    // we should probably use wrapper types for these three types to stop them getting mixed up
    type Hand = Hand of Card list
    type Deck = Deck of Card list
    type ShuffledDeck = ShuffledDeck of Card list

    type Player = { Name : string; Hand : Hand}
    type Game = { Deck : Deck; Players : Player list}
    // Alternative design for Game using a tuple
    //type Game =  Deck * Player list


    type Deal = ShuffledDeck -> Card option * ShuffledDeck   // what if the deck is empty?

    type PickUp = Card * Hand -> Hand
        // Question: Is it worth creating a special type for "Card * Hand" ?
        // Answer: Is it a useful concept in the domain? If so, then yes.
        //         In this case, probably not.

    type Shuffle  = Deck -> ShuffledDeck

    (*
    // Question: How do you document the rules of the game using types?
    // Answer: You can't -- you can only document the key concepts.
    //         For example, the algorithm used to shuffle cards or to score hands.
    /          Just add a comment such as "See algorithm.doc for details"
    *)


    (*
    // Question: How do you model extra behavior,
    //           such as calculating the scoring of a Hand?
    //
    //           In OO you would add methods to the Card and Hand
    // Answer: In FP we would do a "transform" to a new kind of thing,
    //         such as a CardScore type
    *)

    /// Aces High rule is used in Poker,
    /// or must be agreed at the beginning
    type AreAcesHigh = bool

    // For example Ace=13 Two=2
    type CardScore = int // constraint 1 - 13

    type ScoreCard = Card * AreAcesHigh -> CardScore
    type ScoreHand = Hand * AreAcesHigh -> CardScore

    // An alternative approach is to pass a "scoring" function
    // that has the AreAcesHigh flag baked in.
    type ScoreHand_v2 = Hand * (Card -> CardScore) -> CardScore

// =====================================
// Helper functions
// =====================================

module Util =

    // helper function for shuffling implementation
    let fisherYatesShuffle aList =
        let n = List.length aList
        let rand = System.Random()

        // convert to a mutable array! This an example of when mutation is OK because it's hidden
        let a = aList |> List.toArray

        // Fisher-Yates shuffle
        for i in [0..n-1] do
            // pick j, random integer such that i ≤ j < n
            let j = rand.Next(i,n)
            // exchange
            let x = a.[j]
            a.[j] <- a.[i]
            a.[i] <- x

        // convert back to an immutable list
        List.ofArray a

// =====================================
// Card Game Implementation
// =====================================


module CardGameImplementation =
    open CardGame

    let pickup : PickUp =
        fun (card,Hand cardList) ->   // extract the cardList from the hand in the parameter
            // the "::" operator prepends the card to the hand
            let newCardList = card::cardList
            Hand newCardList

    let deal : Deal =
        fun (ShuffledDeck cardList) ->
            // the "::" pattern deconstructs a non-empty list into head/tail
            match cardList with
            | first::rest -> Some first, ShuffledDeck rest
            | [] -> None, ShuffledDeck []

    let shuffle : Shuffle =
        fun (Deck cardList) ->
            let shuffledCards = Util.fisherYatesShuffle cardList
            // and wrap
            ShuffledDeck shuffledCards

    let scoreCard : ScoreCard =
        fun (card,areAcesHigh) ->
            match card with
            | Joker -> 0
            | NonJokerCard card ->
                match card.Rank with
                | Ace -> if areAcesHigh then 11 else 1
                // In blackjack, all face cards (Jack, Queen, King) count as 10
                | King
                | Queen
                | Jack
                | Ten -> 10    // this is an example of combining many choices in one branch
                | Nine -> 9
                | Eight -> 8
                | Seven -> 7
                | Six -> 6
                | Five -> 5
                | Four -> 4
                | Three -> 3
                | Two -> 2

    let scoreHand : ScoreHand =
        fun (Hand cardList,areAcesHigh) ->
            // use List.map to apply a function to each element of a list
            // use List.sum to add up a list
            cardList
            |> List.map (fun card -> scoreCard(card,areAcesHigh))
            |> List.sum


    let scoreHand_v2 : ScoreHand_v2 =
        fun (Hand cardList,scoringFunction) ->
            cardList
            |> List.map scoringFunction
            |> List.sum





