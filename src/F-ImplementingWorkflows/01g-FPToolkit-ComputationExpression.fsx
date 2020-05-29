// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System


// ========================================
// Computation expressions
// ========================================

// ===================================
// CE for Option
// ===================================

module OptionComputationExpression =

    // Define the computation expression
    type OptionBuilder() =
        member this.Return(x) = Some x
        member this.Bind(x,f) = Option.bind f x

    let option = OptionBuilder()


    // Some Option-returning functions
    let increment x = Some (x + 1)
    let double x = Some (x * 2)
    let square x = Some (x * x)

    // implement a pipeline using bind
    let pipelineWithBind x =
        x
        |> increment
        |> Option.bind double
        |> Option.bind square

    // implement a pipeline using CE
    let pipelineWithCE x =
        option {
            let! y = increment x
            let! w = double y
            let! z = square w
            return z
        }

    // tests
    pipelineWithBind 1
    pipelineWithCE 1


    // downside of CE -- each result must be named
    // upside of CE -- results can be used NOT in a pipeline
    let addThreeThingsWithCE x =
        option {
            let! y = increment x
            let! w = double 2
            let! z = square 3
            return y + w + z
        }

    addThreeThingsWithCE 1


// ===================================
// CE for Result
// ===================================

module ResultComputationExpression =

    // Define the computation expression
    type ResultBuilder() =
        member this.Return(x) = Ok x
        member this.Bind(x,f) = Result.bind f x

    let result = ResultBuilder()

    // Some Result-returning functions
    let increment x = Ok (x + 1)
    let double x = Ok (x * 2)
    let square x = Ok (x * x)

    // implement a pipeline using bind
    let pipelineWithBind x : Result<_,string> =
        x
        |> increment
        |> Result.bind double
        |> Result.bind square

    // implement a pipeline using CE
    let pipelineWithCE x : Result<_,string> =
        result {
            let! y = increment x
            let! w = double y
            let! z = square w
            return z
        }

    // tests
    pipelineWithBind 1
    pipelineWithCE 1


    // downside of CE -- each result must be named
    // upside of CE -- results can be used NOT in a pipeline
    let addThreeThingsWithCE x : Result<_,string> =
        result {
            let! y = increment x
            let! w = double 2
            let! z = square 3
            return y + w + z
        }

    addThreeThingsWithCE 1

// ===================================
// CE for List
// ===================================

module ListComputationExpression =

    // Define the computation expression
    type ListBuilder() =
        member this.Return(x) = [x]
        member this.Bind(x,f) = List.collect f x

    let list = ListBuilder()


    // Some List-returning functions
    let increment x = [x + 1; x + 2]
    let double x = [ x * 2; x * 3]
    let square x = [ x * x; x * x * x]

    // implement a pipeline using bind
    let pipelineWithBind x =
        x
        |> increment
        |> List.collect double
        |> List.collect square

    // implement a pipeline using CE
    let pipelineWithCE x =
        list {
            let! y = increment x
            let! w = double y
            let! z = square w
            return z
        }

    // tests
    pipelineWithBind 1
    pipelineWithCE 1


    // downside of CE -- each result must be named
    // upside of CE -- results can be used NOT in a pipeline
    let addThreeThingsWithCE x =
        list {
            let! y = increment x
            let! w = double 2
            let! z = square 3
            return y + w + z
        }


    addThreeThingsWithCE 1

// ===================================
// CE for Async
// ===================================

module AsyncComputationExpression =

    // helper
    let asyncBind f x = async.Bind(x,f)

    // Some List-returning functions
    let increment x = async {return x + 1}
    let double x = async {return x * 2}
    let square x = async {return x * x}

    // implement a pipeline using bind
    let pipelineWithBind x =
        x
        |> increment
        |> asyncBind double
        |> asyncBind square

    // implement a pipeline using CE
    let pipelineWithCE x =
        async {
            let! y = increment x
            let! w = double y
            let! z = square w
            return z
        }

    // tests
    pipelineWithBind 1 |> Async.RunSynchronously
    pipelineWithCE 1 |> Async.RunSynchronously


    // downside of CE -- each result must be named
    // upside of CE -- results can be used NOT in a pipeline
    let addThreeThingsWithCE x =
        async {
            let! y = increment x
            let! w = double 2
            let! z = square 3
            return y + w + z
        }


    addThreeThingsWithCE 1 |> Async.RunSynchronously

