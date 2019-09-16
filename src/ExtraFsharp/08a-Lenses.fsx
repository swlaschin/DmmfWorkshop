// =====================================
// Lenses and other optics (not finished!)
// =====================================


#r "../../packages/Aether/lib/netstandard1.6/Aether.dll"

// =====================================
// Motivation for Lenses
// =====================================

module Example1 = 
    type Postcode = Postcode of string

    type Address = { Postcode: Postcode }

    type Location = { Address: Address }

    type Building = { Location: Location }

    // create a building
    let building =
        { Location =
            { Address =
                { Postcode = Postcode "W1"} } }

    // Getting the postcode is simple
    let postCode =
        building.Location.Address.Postcode

    let newPostcode = Postcode "E1"

    // Setting it to a new value is hard
    // Everything is immutable :(

    let newAddress =
       { building.Location.Address with Postcode = newPostcode }    

    let newLocation =
        { building.Location with Address =
            { building.Location.Address with Postcode = newPostcode } }    

    let newBuilding =
        { building with Location =
            { building.Location with Address =
                { building.Location.Address with Postcode = Postcode "E1" } } }    

    let newBuilding2 =
        { building with Location = newLocation }


// =====================================
// Using composition
// =====================================

module Example2 = 
    open Example1

    let getPostcode =
        fun address -> address.Postcode

    let getAddress =
        fun location -> location.Address

    let getLocationPostcode =
        getAddress >> getPostcode

    let setPostcode =
        fun newPostcode address -> { address with Postcode = newPostcode }

    let setAddress =
        fun newAddress location -> { location with Address = newAddress }        

    let setLocationPostcode =
        fun newPostcode location -> 
            let address = getAddress location
            setAddress (setPostcode newPostcode address) location

// =====================================
// Defining a simple lens
// =====================================

module SimpleLens =

    type Lens<'a,'b> =
        ('a -> 'b) * ('b -> 'a -> 'a)        

    let compose (lens1:Lens<_,_>) (lens2:Lens<_,_>) :Lens<_,_> =
        let (g1,s1) = lens1 
        let (g2,s2) = lens2 
        let composedGetter = fun a -> g2 (g1 a)
        let composedSetter = fun c a -> s1 (s2 c (g1 a)) a
        (composedGetter,composedSetter)


    (* Lens<'a,'b> -> 'a -> 'b *)
    let getter lens =
        let (g,_) = lens
        fun a -> g a

    (* Lens<'a,'b> -> 'b -> 'a -> 'a *)
    let setter lens =
        let (_,s) = lens
        fun b a -> s b a            


// =====================================
// Using the Aether lens library
// =====================================

module AetherExample =
    open Aether

    type Postcode = Postcode of string
   
    type Address = { Postcode: Postcode }
        with 
        static member Postcode_ :Lens<_,_> =
            let getter = fun a -> a.Postcode
            let setter = fun pc a -> { a with Postcode = pc}
            (getter,setter)

    type Location = { Address: Address }
        with 
        static member Address_ :Lens<_,_> =
            let getter = fun l -> l.Address
            let setter = fun a l -> { l with Address = a}
            (getter,setter)

    type Building = { Location: Location }
        with 
        static member Location_ :Lens<_,_> =
            let getter = fun b -> b.Location
            let setter = fun l b -> { b with Location = l}
            (getter,setter)

    let building =
        { Location =
            { Address =
                { Postcode = Postcode "W1"} } }

    (* A lens, composed using Aether *)
    let addressPostcode_ =
        Compose.lens Location.Address_ Address.Postcode_

    let locationAddressPostcode_ =
        Compose.lens Building.Location_ addressPostcode_

    // Get the value using the lens
    let postcode =
        Optic.get locationAddressPostcode_ building

    // Set the value using the lens
    let newBuilding =
        let newPostcode = Postcode "SW3"
        building |> Optic.set locationAddressPostcode_ newPostcode 

module AetherExampleWithOperators =
    open AetherExample
    open Aether.Operators

    // A lens, composed using Aether operators 
    let locationAddressPostcode_ =
        Building.Location_ >-> Location.Address_ >-> Address.Postcode_

    // Get the value using the lens
    let postcode =
        building ^. locationAddressPostcode_
        

    // Set the value using the lens
    let newBuilding =
        let newPostcode = Postcode "SW3"
        building |> newPostcode ^= locationAddressPostcode_  


// =====================================
// Using the Aether prism library
// =====================================

module AetherPrismExample =
    open Aether
    open Aether.Operators

    type Postcode = Postcode of string
    type Zipcode = Zipcode of string
   
    type Address = 
        | UkAddress of Postcode 
        | UsAddress of Zipcode
        with 
        static member Postcode_ :Prism<_,_> =
            let getter = fun a -> 
                match a with
                | UkAddress postcode -> Some postcode
                | UsAddress zipcode -> None
            
            let setter = fun newPostcode a ->                 
                match a with
                | UkAddress _  -> UkAddress newPostcode
                | UsAddress zipcode -> UsAddress zipcode

            (getter,setter)

    type Location = { Address: Address }
        with 
        static member Address_ :Lens<_,_> =
            let getter = fun l -> l.Address
            let setter = fun a l -> { l with Address = a}
            (getter,setter)

    type Building = { Location: Location }
        with 
        static member Location_ :Lens<_,_> =
            let getter = fun b -> b.Location
            let setter = fun l b -> { b with Location = l}
            (getter,setter)

    let building =
        { Location =
            { Address =
                UkAddress (Postcode "W1") } }

    let locationAddressPostcode_ =
        Building.Location_ >-> Location.Address_ >-> Address.Postcode_

    // Get the value using the lens
    let postcode =
        building ^. locationAddressPostcode_

    // Set the value using the lens
    let newBuilding =
        let newPostcode = Postcode "SW3"
        building |> newPostcode ^= locationAddressPostcode_  


// =====================================
// Using the Aether morphism library
// =====================================

module AetherMorphismExample =
    open Aether
    open Aether.Operators

    type Building = { Storeys: string}
        with 
        static member Storeys_ :Lens<_,_> =
            let getter = fun b -> b.Storeys
            let setter = fun s b -> { b with Storeys = s}
            (getter,setter)

    let string2int_ =
        let toInt s = 
            match System.Int32.TryParse s with
            | true, i -> Some i
            | _ -> None
        let fromInt (i:int) =
            string i
        (toInt, fromInt)

    let storeys_ =
        Building.Storeys_ >-> string2int_

    let building = { Storeys = "2" }

    let storeys = 
        building ^. storeys_

    let newBuilding =
        let newStoreys = 42
        building |> newStoreys ^= storeys_