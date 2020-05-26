// =================================
// This file contains the code from the "FP Toolkit" slides
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================


// ========================================
// Tool #1 - Monoids
// ========================================

type Monoid<'a> = {
    Combine : 'a -> 'a -> 'a
    Identity: 'a
    }

let reduce (monoid:Monoid<'a>) list =
    let mutable result = monoid.Identity
    for item in list do
        result <- monoid.Combine result item
    result
    
let intAdd = {Combine = (+); Identity = 0 }
let intMultiply = {Combine = (*); Identity = 1 }
let stringConcat = {Combine = (+); Identity = "" }
let listConcat = {Combine = (@); Identity = [] }

[1..10] |> reduce intAdd
[1..10] |> reduce intMultiply
["a"; "b"; "c"] |> reduce stringConcat
[ [1;2]; [3;4]; [5;6]] |> reduce listConcat


module OrderLineMonoidExample =

    type OrderLine = {
        Qty:int
        Total:float
        }

    let combine line1 line2 = 
       let newQty = line1.Qty + line2.Qty
       let newTotal = line1.Total + line2.Total
       {Qty=newQty; Total=newTotal}

    let identity = {Qty=0; Total=0.0}

    let orderLineMonoid = {Combine=combine; Identity=identity }

    let orderLines = [ 
       {Qty=2; Total=19.98}
       {Qty=1; Total= 1.99} 
       {Qty=3; Total= 3.99} ]

    orderLines |> reduce orderLineMonoid 


module MapReduceExample =

    type OrderLine = {
        OrderId:int
        ProductId:int
        Qty:int
        Total:float
        }
    
    let mapper (orderLine:OrderLine) =
        let ol : OrderLineMonoidExample.OrderLine = {
            Qty=orderLine.Qty
            Total=orderLine.Total
            }
        ol

    let orderLines = [ 
       {OrderId=1; ProductId=42; Qty=2; Total=19.98}
       {OrderId=2; ProductId=46; Qty=1; Total= 1.99} 
       {OrderId=3; ProductId=49; Qty=3; Total= 3.99} ]

    let monoid = OrderLineMonoidExample.orderLineMonoid
    
    orderLines 
    |> List.map mapper // map
    |> reduce monoid   // reduce


