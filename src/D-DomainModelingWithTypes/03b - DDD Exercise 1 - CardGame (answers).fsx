// ================================================
// DDD Exercise: Model a card game
//
// ================================================

(*
A card is a combination of a Suit (Heart, Spade) and a Rank (Two, Three, ... King, Ace)

A hand is a list of cards

A deck  is a list of cards

A player has a name and a hand

A game consists of a deck and list of players

*)

module CardGame =

    type Suit = Club | Diamond | Spade | Heart

    type Rank = Two | Three | Four | Five | Six | Seven | Eight
                       | Nine | Ten | Jack | Queen | King | Ace

    type Card = Suit * Rank

    type Hand = Card list
    type Deck = Card list

    type Player = {Name : string; Hand : Hand}
    type Game = {Deck : Deck; Players : Player list}

