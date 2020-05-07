// ================================================
// List exercises
// ================================================


//----------------------------------------------------------
//  Q.  Use List.reduce to convert a list of strings to
// a single comma separated string

["a"; "b"; "c"] |> List.reduce (fun x y -> x + ", " + y)

//----------------------------------------------------------
//  Q.  Use List.filter find strings that contain a "h"

let contains value (input:string) =
    input.Contains(value)

let stringsThatContainh list =
    // list |> List.filter (fun e -> e |> contains "h")
    list |> List.filter (contains "h")  // same as above


// test
["alice"; "bob"; "hello"; "hi"]
|> stringsThatContainh


//----------------------------------------------------------
//  Q.  Follow up: Use List.filter find strings that
// contain ANY string (passed in as parameter)

let stringsThatContain char list =
    list |> List.filter (contains char)


// test
["alice"; "bob"; "hello"; "hi"]
|> stringsThatContain "o"

//----------------------------------------------------------
//  Q.  Follow up: Use List.filter find strings that
// contain TWO strings
// TIP: Use && to combine boolean values.
let stringsThatContain2 char1 char2 list =
    list |> List.filter (fun e ->
        (e |> contains char1)
        && (e |> contains char2)
    )

// test
["alice"; "bob"; "hello"; "hi"]
|> stringsThatContain2 "h" "o"


//----------------------------------------------------------
//  Q. Advanced! Create a function that combines two predicate functions
// to create a new predicate function, and use that to
// create a filter on three chars

type Predicate<'a> = 'a -> bool

let combine (pred1:Predicate<'a>) (pred2:Predicate<'a>) : Predicate<'a> =
    fun x ->
        (pred1 x) && (pred2 x)

let stringsThatContain3 char1 char2 char3 list =
    let pred1 = combine (contains char1) (contains char2)
    let pred2 = combine pred1 (contains char3)
    list |> List.filter pred2

// test
["alice"; "bob"; "hello"; "hi"]
|> stringsThatContain3 "h" "o" "e"

//----------------------------------------------------------
//  Q. Advanced! Using List.reduce,
// create a filter on list of  chars
// TIP: Use List.map to convert each char to a predicate
// TIP: Use List.reduce to combine a list

let stringsThatContainAny listOfChars list =
    let preds = listOfChars |> List.map contains
    let pred = preds |> List.reduce combine
    list |> List.filter pred

// test
["alice"; "bob"; "hello"; "hi"]
|> stringsThatContainAny ["h";"e"]



