// ================================================
// Strategy pattern in FP
// ================================================

(*
You are given a list of takeaway foods

1) Create a function "getBestFood" that uses a selection
   strategy passed in to determine which food to get
2) Create a function "cheapestFood" that accepts two foods
   and returns -1 for the one with the lowest price.
   (e.g. it should use the built-in "compareTo" method)
3) Create a function "fastestDelivery" that accepts two foods
   and returns -1 for the one with the lowest delivery time.
   (e.g. it should use the built-in "compareTo" method)

// ====================
CODING TIPS

-- To sort a list using a compareTo style, use List.sortWith
-- To return the first element of a list, use List.head

*)

type TakeawayFood =
    {
    name: string
    price: int
    deliveryTime: int
    }

let availableFoods = [
    {name="Indian"; price=12; deliveryTime=20}
    {name="Pizza"; price=17; deliveryTime=15}
    {name="Gourmet"; price=30; deliveryTime=25}
    ]

let getBestFood selectionStrategy =
    availableFoods
    |> List.sortWith ??
    |> ??


// return -1 if food1 is cheaper than food2
let cheapestFoodStrategy food1 food2 =
    ??

// return -1 if food1 has faster delivery than food2
let fastestDeliveryStrategy food1 food2 =
    ??


// test
getBestFood cheapestFoodStrategy
getBestFood fastestDeliveryStrategy


