
// ======================================
// A set of functions for working with distributions
// ======================================

module DistributionLibrary = 

    type Probability = float

    type Distribution<'a> = {
        Sample: unit -> 'a
        Expectation: ('a -> Probability) -> Probability
    }

    /// Always return x
    let always x = {
        Sample = fun () -> x
        Expectation = fun f -> f x
    }

    let rnd = System.Random()

    /// Check that a probability is valid
    let assertValidProbability (p:Probability) =
        if p < 0.0 || p > 1.0 then
            failwith "Invalid probability"
        else
            () // nothing to return

    /// Choose between two distributions with probability p
    let choose (p:Probability) (dist1:Distribution<'a>) (dist2:Distribution<'a>) :Distribution<'a> =
        assertValidProbability p
        let sample() =
            if rnd.NextDouble() < p then
                dist1.Sample()
            else
                dist2.Sample()
        let expectation f = 
            (  p        * dist1.Expectation(f) ) +
            ( (1.0 - p) * dist2.Expectation(f) )
        {Sample=sample; Expectation=expectation}        


    /// dist is a Distribution 
    /// k is a Distribution creator
    let bind dist k =
        let sample() =
            let newDist = k(dist.Sample())
            newDist.Sample()
        let expectation f =
            dist.Expectation(fun x -> 
                let newDist = k x
                newDist.Expectation(f) 
                )
        {Sample=sample; Expectation=expectation}        

    type DistributionBuilder() =
        member this.Bind(d,k) = bind d k
        member this.Return(x) = always x
        member this.ReturnFrom(d) = d

    let dist = DistributionBuilder()


    // ======================================
    // helper functions to build distributions
    // ======================================


    type WeightedCase<'a> = 'a * Probability

    /// Given a set of cases, each weighted with a probability
    /// create a distribution
    let weightedCases (weightedCases:WeightedCase<_> list) =

        let rec loop pUsed list =
            match list with
            | [] -> 
                failwith "should never happen"
            // one pair left
            | [ (x,_) ] ->
                always x
            // more than one pair left
            | (x,p)::rest ->
                let probX = p / (1.0 - pUsed)
                let distX = (always x)
                let pUsed = pUsed + p
                let distOther = loop pUsed rest
                choose probX distX distOther
        loop 0.0 weightedCases

    type CountedCase<'a> = 'a * int

    /// Given a set of cases, each with a count, 
    /// create a distribution from them
    let countedCases (cases:CountedCase<_> list) = 
        let totalCount =
            cases 
            |> List.sumBy (fun (x,count) -> count)
            |> float

        cases
        |> List.map (fun (x,count) -> 
            let weight = float count / totalCount
            x,weight
            )
        |> weightedCases 

open DistributionLibrary

// count the items in a list
let countItems list = 
    list
    |> List.groupBy (id)
    |> List.map (fun (x,items) -> (x,List.length items) )


// =====================================
// Coin example
// =====================================

type Coin = Heads | Tails

let fairCoin = countedCases [ Heads,50; Tails,50 ]

// I bet £1 on heads, and I win £2 if heads comes up.
// How much do I expect to win?
fairCoin.Expectation (fun x ->
    match x with
    | Heads -> 2.0
    | Tails -> 0.0
    )

List.init 1000 (fun _ -> fairCoin.Sample())
|> countItems


let biasedCoin = countedCases [ Heads,70; Tails,30 ]


// I bet £1 on heads, and I win £2 if heads comes up.
// How much do I expect to win?
biasedCoin.Expectation (fun x ->
    match x with
    | Heads -> 2.0
    | Tails -> 0.0
    )

List.init 1000 (fun _ -> biasedCoin.Sample())
|> countItems




// =====================================
// Roulette example
// =====================================

type Outcome = Odd | Even | Zero
let roulette = countedCases [ Odd,18; Even,18; Zero,1 ]

// I bet £5 on Odd, and I win £10 if Odd comes up.
// How much do I expect to win?
roulette.Expectation (fun x ->
    match x with
    | Odd -> 10.0
    | Even -> 0.0
    | Zero -> 0.0
    )

// 4.864864865 -- you'll expect to win this on a £5 bet.

// simulate 3700 rolls. Expect 1800 Odd, 1800 Even and 100 Zeros, 
List.init 3700 (fun _ -> roulette.Sample())
|> countItems



// =====================================
// Driving example
// =====================================

type LightColour = Red | Green | Amber

let trafficLight = 
    weightedCases [
        Red, 0.50
        Amber, 0.10
        Green, 0.40
        ]

type DriverAction = Stop | Go

let cautiousDriver lightColour :Distribution<DriverAction> = 
    dist {
        match lightColour with
        | Red -> 
            return Stop
        | Amber -> 
            return! weightedCases [ Stop,0.9; Go,0.1]
        | Green -> 
            return Go
    }
    
let aggressiveDriver lightColour = 
    dist {
        match lightColour with
        | Red -> 
            return! weightedCases [ Stop,0.9; Go,0.1]
        | Amber -> 
            return! weightedCases [ Stop,0.1; Go,0.9]
        | Green -> 
            return Go
    }
    
let otherLight lightColour =
    match lightColour with
    | Red -> 
        Green
    | Amber -> 
        Red
    | Green -> 
        Red

type CrashResult = Crash | NoCrash

let crash driver1 driver2 trafficLight = 
    dist {
        // get the light
        let! lightColour = trafficLight

        // get the first drivers behaviour
        let! action1 = driver1 lightColour

        // get the first drivers behaviour
        let! action2 = driver2 (otherLight lightColour)

        // test if there is an accident
        match action1,action2 with
        | Go,Go -> 
            return! weightedCases [ Crash,0.9; NoCrash,0.1]
        | Go,Stop 
        | Stop,Go 
        | Stop,Stop ->
            return NoCrash            
    }

let modelCC = crash cautiousDriver cautiousDriver trafficLight
let modelCA = crash cautiousDriver aggressiveDriver trafficLight
let modelAA = crash aggressiveDriver aggressiveDriver trafficLight


List.init 1000 (fun _ -> modelCC.Sample()) |> countItems

List.init 1000 (fun _ -> modelCA.Sample()) |> countItems

List.init 1000 (fun _ -> modelAA.Sample()) |> countItems

let badness crash = 
    match crash with
    | Crash -> 1.0
    | NoCrash -> 0.0

modelCC.Expectation badness  // 0.0
modelCA.Expectation badness  // 0.0369
modelAA.Expectation badness  // 0.0891
