
let funny =
    seq {
        yield 1
        yield 3
        yield! [5..8]
    }

funny    
|> Seq.toList


let rec infinite() = seq {
    yield 1
    yield 3
    yield! [5..8]
    yield! infinite()
} 

infinite()
|> Seq.take 20
|> Seq.toList


let rec fib = seq {
    yield 0
    yield 1

    let fibOffBy1 = Seq.skip 1 fib
    let pairs = Seq.zip fib fibOffBy1
    yield! 
        pairs |> Seq.map (fun (x,y) -> x + y)
    } 

fib
|> Seq.take 20
|> Seq.toList
