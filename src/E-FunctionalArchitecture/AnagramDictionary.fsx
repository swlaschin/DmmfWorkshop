type WordDictionary =
    private WordDictionary of string[]

module WordDictionary =

    /// load a WordDictionary from a text file
    let load fileName =
        fileName
        |> System.IO.File.ReadAllLines
        // remove blank lines
        |> Array.filter (System.String.IsNullOrWhiteSpace >> not)
        // canonicalize
        |> Array.map (fun s -> s.ToLower())
        // sort so that we can do binary search later
        |> Array.sort
        // wrap in wrapper type
        |> WordDictionary

    /// return a sequence of random words from a WordDictionary
    let randomWordGenerator minLength maxLength (WordDictionary words) =
        // create a random number generator
        let gen = System.Random()

        // generate a sequence of random words between minLength & maxLength
        let randomWords =
            Seq.initInfinite (fun _ ->
                let index = gen.Next(0,words.Length)
                words.[index]
                )
            |> Seq.filter (fun w -> w.Length >= minLength && w.Length <= maxLength)

        // return the sequence
        randomWords

    /// return true if a word is in the WordDictionary
    let isWord (WordDictionary words) (word:string) =

        // canonicalize
        let word = word.ToLower()

        // linear search
        // words |> Array.contains word

        // binary search
        let index = System.Array.BinarySearch(words,word)
        index >= 0  // true if non-negative index found



(*
// some tests
let filename = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"WordListTest.txt")
// let filename = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"WordList10000.txt")

let wd = WordDictionary.load filename

"cat" |> WordDictionary.isWord wd
"cap" |> WordDictionary.isWord wd
"caz" |> WordDictionary.isWord wd

let gen = WordDictionary.randomWordGenerator 3 6 wd
gen |> Seq.take 10 |> Seq.toList

*)



module Anagram =

    // return a lazy sequence of all random shuffles of a word
    let rec permutations word =
        let n = String.length word
        let rand = System.Random()

        // Fisher-Yates shuffle
        let shuffle() =
            let a = word |> Seq.toArray
            for i in [0..n-1] do
                // pick j, random integer such that i ≤ j < n
                let j = rand.Next(i,n)
                // exchange
                let x = a.[j]
                a.[j] <- a.[i]
                a.[i] <- x

            // convert back to string
            a |> System.String

        // return an infinite sequence. The first shuffle might be in
        // the dictionary, so the client may need to retrieve another.
        Seq.initInfinite (fun _ -> shuffle() )


    // permutations ""
    // permutations "c"
    // permutations "ca"
    // permutations "cat"
    // permutations "cata"
    // permutations "catapult"
    // permutations "catamorphism"


    /// Given an element and a list
    /// return a new list without the specified element.
    /// If not found, return None
    let private withoutElement anElement aList =

        /// Given an element and a list, and other elements previously skipped,
        /// return a new list without the specified element.
        /// If not found, return None
        let rec loop aList skipped =
            match aList with
            | [] -> None
            | head::tail when anElement = head ->
                // matched, so create a new list from the skipped and the remaining
                // and return it
                let skipped' = List.rev skipped
                Some (skipped' @ tail)
            | head::tail  ->
                // no match, so prepend head to the skipped and recurse
                let skipped' = head :: skipped
                loop tail skipped'

        loop aList []

    // return true if two words are anagrams of each other
    let rec isAnagram (word1:string) (word2:string) =

        /// Given two lists, return true if they have the same contents
        /// regardless of order
        let rec isPermutationOf list1 list2 =
            match list1 with
            | [] ->
                List.isEmpty list2 // if both empty, true
            | h1::t1 ->
                match list2 |> withoutElement h1  with
                | None ->
                    false
                | Some t2 ->
                    isPermutationOf t1 t2

        // use the helper function
        isPermutationOf (List.ofSeq word1) (List.ofSeq word2)

    // isAnagram "" ""
    // isAnagram "c" "c"
    // isAnagram "c" "d"
    // isAnagram "cat" "tac"
    // isAnagram "cat" "taca"

