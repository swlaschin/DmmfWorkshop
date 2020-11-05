// ================================================
// Exercise: Design a NonEmptyList data structure
//
// ================================================

// -----------------------------------
// If the input is constrained,
// you can ALWAYS get the first item in a list!
// (this is a "constraining the input" example)
// -----------------------------------

let notImplemented() = failwith "not implemented"
type undefined = exn

// Exercise: define a type "NonEmptyList"
//           so that "firstItem" always works
type NonEmptyList<'a> = undefined

/// Helper functions for NonEmptyList
module NonEmptyList =

    /// Returns the first item in a NonEmptyList
    let first (aNonEmptyList:NonEmptyList<'a>) =
        // Exercise: implement this
        notImplemented()

    /// Convert a normal list to a NonEmptyList
    let fromList aList =
        // This is exactly the same idea as NonZeroInteger -- the client
        // is responsible for validating the input before passing it
        // as a parameter to a function that needs a non-empty list

        // Exercise: implement this
        match aList with
        // input is an empty list
        | [] ->
            notImplemented()
        // input is a non-empty list
        | first::rest ->
            notImplemented()



    /// Returns a tuple of the first item
    /// and the rest of the list (as an NonEmptyList option)
    let split (aNonEmptyList:NonEmptyList<'a>) =
        // Exercise: implement this
        let first : 'a = notImplemented()
        let rest : NonEmptyList<'a> option = notImplemented()
        (first, rest)



// some code that uses NonEmptyList
let showFirstItem aList =
    let nonEmptyListOpt = NonEmptyList.fromList aList
    match nonEmptyListOpt with
    | Some nonEmptyList ->
        NonEmptyList.first nonEmptyList // always succeeds
        |> printfn "The first item is %A"
    | None ->
        printfn "Input is not valid"


// test the function
showFirstItem [1;2;3]  // good
showFirstItem []       // bad


// -----------------------------------
// Use NonEmptyList to make sure
// you can ALWAYS deal a card from a deck
// -----------------------------------


module CardGame =
    type Suit = Heart | Spade | Club
    type Rank = Ace | King | Queen
    type Card = Suit * Rank
    type ShuffledDeck = ShuffledDeck of NonEmptyList<Card>
    type Deal = ShuffledDeck -> Card * ShuffledDeck option

    // the implementation of Deal
    let deal : Deal =
        fun (ShuffledDeck cardList) ->
            let first,rest = ??
            let newDeckOpt = rest |> Option.map (fun list -> ShuffledDeck list)
            first, newDeckOpt
