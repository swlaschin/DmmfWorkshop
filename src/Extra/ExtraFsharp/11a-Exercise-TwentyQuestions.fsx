// ================================================
// Twenty Questions
// ================================================

(*
Write an program that plays twenty questions

There is a list of data items, where each item is a list of terms
starting with categories and ending at an answer. E.g.

["Plant"; "Red"; "Rose"]
["Plant"; "Yellow"; "Daffodil"]
["Animal"; "Big"; "Grey"; "Elephant"]
["Animal"; "Big"; "White"; "Polar Bear"]
["Animal"; "Small"; "Grey"; "Mouse"]
["Animal"; "Small"; "Black"; "Rat"]

The algorithm is:
* Pick the first of the leftmost category ("Plant")
  and ask "is it a plant?"
* If yes, filter to the items that have that category:
    ["Plant"; "Red"; "Rose"]
    ["Plant"; "Yellow"; "Daffodil"]
  and then remove the first item in the list from each one:
    ["Red"; "Rose"]
    ["Yellow"; "Daffodil"]
  when there is only one data row left, you can make a final guess
    "it must be a Rose!"
* If the answer is no, remove all the items that have that category,
  (eg remove all plants) and repeat.

*)


let initialDatabase = [
    ["Plant"; "Red"; "Rose"]
    ["Plant"; "Yellow"; "Daffodil"]
    ["Animal"; "Big"; "Grey"; "Elephant"]
    ["Animal"; "Big"; "White"; "Polar Bear"]
    ["Animal"; "Small"; "Grey"; "Mouse"]
    ["Animal"; "Small"; "Black"; "Rat"]
]

let rec getAnswer() =
    printfn "Please enter y or n"
    let str = System.Console.ReadLine()
    match str with
    | "y" | "Y" -> true
    | "n" | "N" -> false
    | _ ->
        printfn "Try again"
        getAnswer()

let makeNewDatabaseIfCorrect category database =
    database
    |> ??


let makeNewDatabaseIfNotCorrect category database =
    database
    |> ??

let rec makeAGuess remainingDatabase =
    match remainingDatabase with
    | [] ->
        printfn "The database is empty. This should never happen!"
    | [item] ->
        // last one
        let name = ??
        printfn "It is a %s!" name
    | item::rest ->
        // guess using the first one
        let category = ??
        printfn "Is it a %s?" category
        let correct = getAnswer()
        if correct then
            remainingDatabase
            |> makeNewDatabaseIfCorrect category
            // recurse
            |> makeAGuess
        else
            remainingDatabase
            |> makeNewDatabaseIfNotCorrect category
            // recurse
            |> makeAGuess

let start() =
    printfn "Starting"
    makeAGuess initialDatabase

// start()