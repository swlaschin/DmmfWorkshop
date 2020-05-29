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
type undefined = exn

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
        let strToDayOfWeek s = notImplemented()

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
        // Should it have a different name?
        let strToInt s = notImplemented()

// test the function
IntUtil.ExceptionBasedDesign.strToInt "123"     // good
IntUtil.ExceptionBasedDesign.strToInt "hello"   // exception :(

IntUtil.ExtendedOutputDesign.strToInt "123"     // good
IntUtil.ExtendedOutputDesign.strToInt "hello"   // good

// -----------------------------------
// 3. Create a function that gets the first item in a list
// -----------------------------------

module ListUtil =

    module ExceptionBasedDesign =
        let firstItem aList =
            match aList with
            | first::rest -> first
            | [] -> failwith "list does not have a first item"

    module ExtendedOutputDesign =
        // Exercise: Convert this function to be total
        //           by extending the output.
        // Should it have a different name?
        let tryFirstItem aList = notImplemented()

// test the function
ListUtil.ExceptionBasedDesign.firstItem [1;2;3]           // good
ListUtil.ExceptionBasedDesign.firstItem ([]:int list)     // exception :(

ListUtil.ExtendedOutputDesign.tryFirstItem [1;2;3]        // good
ListUtil.ExtendedOutputDesign.tryFirstItem ([]:int list)  // good

// NOTE: the []:int list is just to get around an issue working interactively!
// it is not normally needed

// -----------------------------------
// 4. HARDER Create a function that gets the first item in a list
// by constraining the input
// -----------------------------------

type ConstrainedList<'a> = undefined

// Convert a normal list to a ConstrainedList
// This is exactly the same idea as NonZeroInteger -- the client
// is responsible for validating the input before passing it
// as a parameter
let toConstrainedList aList = notImplemented()

// Exercise: define a new type for aConstrainedList
//           so that "firstItem" always works
let firstItem (aConstrainedList:ConstrainedList<'a>) =
    notImplemented()


// test
let showConstrainedInputResult aList =
    let constrainedListOpt = toConstrainedList aList
    match constrainedListOpt with
    | Some constrainedList ->
        firstItem constrainedList
        |> printfn "The first item is %A"
    | None ->
        printfn "Input is not valid"


// test the function
showConstrainedInputResult [1;2;3]  // good
showConstrainedInputResult []       // bad