// ================================================
// Exercise: Convert non-total functions to total functions
//
// ================================================

// -----------------------------------
(*
Quiz:

Which of these are total functions
and which are partial functions?

// get first element of a list
firstElement : int list -> int  // PARTIAL. List could be empty
// a better design is to "try" to get the first element
tryFirstElement : int list -> int option  // TOTAL

// get number of elements in a list
elementCount : int list -> int  // TOTAL. Works for all lists

// convert a str to an int
strToInt : string -> int       // PARTIAL. String could be "" or "zzz" etc

// format an int into a string
intToStr : int -> string       // TOTAL. Works for all ints

// convert a 32-bit int into a 16-bit int
longToInt : int32 -> int16    // PARTIAL. Input could be larger than max int16

// convert a float into an int (assuming truncation of decimals)
floatToInt: float -> int      // PARTIAL. NaN is a float with no int equivalent

*)


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
        let strToDayOfWeek str =
            match str with
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
        let strToDayOfWeek str =
            match str with
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

        let strToInt (str:string) =
            match System.Int32.TryParse str with
            // the Int32.TryParse method returns a tuple
            | (true,i) -> i
            | (false,_) -> failwith "input is not an int"


    module ExtendedOutputDesign =
        // Exercise: Convert this function to be total
        //           by extending the output.
        // Reuse the "match System.Int32.TryParse" code
        // but return something different
        //
        // Question: Why is it not called "strToInt"
        // like the original function?
        let tryStrToInt (str:string) =
            match System.Int32.TryParse str with
            // the Int32.TryParse method returns a tuple
            | (true,i) -> Some i
            | (false,_) -> None

        let tryStrToInt_v2 (str:string) =
            try
                // the Int32.Parse function is not total
                // and throws 3 different exceptions.
                // Read the docs to figure out which ones :(
                Some (System.Int32.Parse str)
            with
            | :? System.FormatException
            | :? System.OverflowException
            | :? System.NullReferenceException ->
                None



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


