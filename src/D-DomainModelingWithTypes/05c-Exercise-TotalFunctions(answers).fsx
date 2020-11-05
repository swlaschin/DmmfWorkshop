// ================================================
// Exercise: Convert non-total functions to total functions
//
// ================================================

// -----------------------------------
// 1. Create a function that converts a string (e.g "Sunday") into an DayOfWeek type
// -----------------------------------

(*
1) Define a DayOfWeek type with a case for each day
2) Define a strToDayOfWeek function  which is total

TIP - you can pattern match on strings!

match x with
| "a" ->
| "b" ->
| etc
| _ ->  // wildcard matches anything

*)

let notImplemented() = failwith "not implemented"

module Calendar =
    type DayOfWeek = Sun | Mon | Tue | Wed | Thu | Fri | Sat

    module ExceptionBasedDesign =
        let strToDayOfWeek s =
            match s with
            | "Sunday" | "Sun" -> Sun
            | "Monday" | "Mon" -> Mon
            | "Tuesday" | "Tue" -> Tue
            | "Wednesday" | "Wed" -> Wed
            | "Thursday" | "Thu" -> Thu
            | "Friday" | "Fri" -> Fri
            | "Saturday" | "Sat" -> Sat
            | _ -> failwith "input is not a DayOfWeek"

    module ExtendedOutputDesign =
        // Exercise: convert the function to be total
        //           by extending the output.
        let strToDayOfWeek s =
            match s with
            | "Sunday" | "Sun" -> Some Sun
            | "Monday" | "Mon" -> Some Mon
            | "Tuesday" | "Tue" -> Some Tue
            | "Wednesday" | "Wed" -> Some Wed
            | "Thursday" | "Thu" -> Some Thu
            | "Friday" | "Fri" -> Some Fri
            | "Saturday" | "Sat" -> Some Sat
            | _ -> None

// test the function
Calendar.ExceptionBasedDesign.strToDayOfWeek "Sunday"  // good
Calendar.ExceptionBasedDesign.strToDayOfWeek "Sun"     // good
Calendar.ExceptionBasedDesign.strToDayOfWeek "April"   // exception :(

// test the function
Calendar.ExtendedOutputDesign.strToDayOfWeek "Sunday"  // good
Calendar.ExtendedOutputDesign.strToDayOfWeek "Sun"     // good
Calendar.ExtendedOutputDesign.strToDayOfWeek "April"   // good

// -----------------------------------
// 2. Create a function that converts a string into an int
// -----------------------------------

module IntUtil =

    module ExceptionBasedDesign =

        let strToInt (s:string) =
            match System.Int32.TryParse s with
            | true, i -> i
            | false, _ -> failwith "input is not an int"

    module ExtendedOutputDesign =
        // Exercise: Convert this function to be total
        //           by extending the output.
        // Reuse the "match System.Int32.TryParse" code
        // but return something different
        //
        // Question: Should it have a different name?
        let tryStrToInt (s:string) =
            match System.Int32.TryParse s with
            | true, i -> Some i
            | false, _ -> None

// test the function
IntUtil.ExceptionBasedDesign.strToInt "123"     // good
IntUtil.ExceptionBasedDesign.strToInt "hello"   // exception :(

IntUtil.ExtendedOutputDesign.tryStrToInt "123"     // good
IntUtil.ExtendedOutputDesign.tryStrToInt "hello"   // good

// -----------------------------------
// 3. Create a function that gets the first item in a list
// -----------------------------------

module ListUtil =
    // In F#, lists are implemented as linked lists, NOT as arrays/vectors

    // to pattern match:
    // for empty list, use []
    // for non-empty list, use firstItem :: rest
    //    where "rest" is another linked list
    //
    // we normally work on the first element only,
    // not the nth one

    module ExceptionBasedDesign =
        let firstItem aList =
            match aList with
            | [] ->
                failwith "list does not have a first item"
            | first::rest ->
                first

    module ExtendedOutputDesign =
        // Exercise: Convert this function to be total
        //           by extending the output.
        // Should it have a different name?
        let tryFirstItem aList =
            match aList with
            | first::rest -> Some first
            | [] -> None

let emptyList :int list = []
// NOTE: the :int list is just to get around an issue working interactively!
// it is not normally needed

// test the function
ListUtil.ExceptionBasedDesign.firstItem [1;2;3]         // good
ListUtil.ExceptionBasedDesign.firstItem (emptyList)     // exception :(

ListUtil.ExtendedOutputDesign.tryFirstItem [1;2;3]      // good
ListUtil.ExtendedOutputDesign.tryFirstItem (emptyList)  // no exception!


