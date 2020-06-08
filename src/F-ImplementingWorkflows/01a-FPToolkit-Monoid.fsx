// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// FP Toolkit: Monoids
// ========================================

type Monoid<'a> = {
    Combine : 'a -> 'a -> 'a
    Identity: 'a
    }

// library functions for Monoids
module Monoid =
    let reduce (monoid:Monoid<'a>) list =
        let mutable result = monoid.Identity
        for item in list do
            result <- monoid.Combine result item
        result


// examples of monoid implementation

let intAdd = {Combine = (+); Identity = 0 }
let intMultiply = {Combine = (*); Identity = 1 }
let stringConcat = {Combine = (+); Identity = "" }
let listConcat = {Combine = (@); Identity = [] }

[1..10] |> Monoid.reduce intAdd
[1..10] |> Monoid.reduce intMultiply
["a"; "b"; "c"] |> Monoid.reduce stringConcat
[ [1;2]; [3;4]; [5;6]] |> Monoid.reduce listConcat

// =================================
// An example of a custom Monoid
// for OrderLines
// =================================

// A domain type representing just the
// aggregatable data from an order line
type OrderLineStats = {
    Qty:int
    Total:float
    }

// library of functions for OrderLineStats
module OrderLineStats =

    let combine line1 line2 =
       let newQty = line1.Qty + line2.Qty
       let newTotal = line1.Total + line2.Total
       {Qty=newQty; Total=newTotal}

    let identity = {Qty=0; Total=0.0}

    /// An implementation of the Monoid interface
    /// for OrderLineStats
    let monoid = {Combine=combine; Identity=identity }


// Example of usage
let orderLineStats = [
    {Qty=2; Total=19.98}
    {Qty=1; Total= 1.99}
    {Qty=3; Total= 3.99} ]

orderLineStats |> Monoid.reduce OrderLineStats.monoid

// =================================
// An example of a using Map and Result
// to sum up OrderLines
// =================================

// a domain type with non-aggregatable fields
type OrderLine = {
    OrderId:string   // not aggregatable
    ProductId:string // not aggregatable
    Qty:int          // aggregatable
    Total:float      // aggregatable
    }

module OrderLine =

    // the "map" function
    let toStats (orderLine:OrderLine) : OrderLineStats  =
        {
            Qty=orderLine.Qty
            Total=orderLine.Total
        }

// Example of usage
let orderLines = [
   {OrderId="O1"; ProductId="P42"; Qty=2; Total=19.98}
   {OrderId="O2"; ProductId="P46"; Qty=1; Total= 1.99}
   {OrderId="O3"; ProductId="P49"; Qty=3; Total= 3.99} ]

orderLines
|> List.map OrderLine.toStats            // map
|> Monoid.reduce OrderLineStats.monoid   // reduce


