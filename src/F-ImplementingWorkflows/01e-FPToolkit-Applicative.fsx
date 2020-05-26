// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// Tool #5 - applicative
// ========================================

module OptionApplicative =

    let lift2 f opt1 opt2 =
        match (opt1,opt2) with
        | Some x, Some y -> Some (f x y)
        | _ -> None

    let lift3 f opt1 opt2 opt3 =
        match (opt1,opt2,opt3) with
        | Some x, Some y, Some z -> Some (f x y z)
        | _ -> None

    let add x y = x + y

    (lift2 add) (Some 1) (Some 2)


module ResultApplicative =

    let lift2 f r1 r2 =
        match (r1,r2) with
        | Ok x, Ok y -> Ok (f x y)
        | Error e1, Ok _ -> Error e1
        | Ok _, Error e2 -> Error e2
        | Error e1, Error e2 -> Error (e1 @ e2)

    let add x y = x + y

    let result1 : Result<_,string list> = (lift2 add) (Ok 1) (Ok 2)
    let result2 = (lift2 add) (Error ["bad"]) (Ok 2)
    let result3 = (lift2 add) (Error ["bad"]) (Error ["oops"])


module ListApplicative =

    let add x y = x + y

    // one kind of list applicative  
    let listApply1 f list1 list2 = 
        List.map2 f list1 list2 

    // another kind of list applicative  
    let listApply2 f list1 list2 = 
        [ for x in list1 do
          for y in list2 do
             yield f x y
        ]

    let result1 = (listApply1 add) [1;2;3] [10;100;1000]
    let result2 = (listApply2 add) [1;2;3] [10;100;1000]
