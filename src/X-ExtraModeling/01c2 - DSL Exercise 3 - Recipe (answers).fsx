// ================================================
// DSL Exercise: Create a DSL to represent a recipe
//
//
// ================================================

(*
A Recipe consists of a list of steps.

Each RecipeStep is one of the following:
* add some ingredients  (e.g "take 2 eggs")
* follow an instruction (e.g. "beat for 1 minute")
* oven (e.g. "put in 200C oven")
* timed (e.g. "bake for 20 minutes")

An ingredient is either a Thing (e.g. an egg) or Stuff (e.g. milk)
* If a Thing, you need an integer quantity plus the name of the thing
* If Stuff, you need an integer quantity, plus the unit of measure, plus the name of the stuff


1) Create a vocabulary for a creating Recipes

2) Write code that will make the example recipes (shown below) work


*)


// -----------------------------------------
// The core domain types
// -----------------------------------------


// define the types
type IngredientName = string
type IngredientAmount = int
type UnitOfMeasure = Grams | Mils | Tsp | Tsps | Tbsp | Tbsps 

type Ingredient = 
   // Ingredient is a Thing (which is counted)
   | IngredientThing of IngredientAmount * IngredientName 
   // OR it is Stuff (which is measured)
   | IngredientStuff of IngredientAmount * UnitOfMeasure * IngredientName 

type Comment = string
type TemperatureUnit = C | F 
type TemperatureLevel = int
type TimeDuration = int
type TimeUnit = Mins | Hours

type RecipeStep = 
   | IngredientStep of Comment * Ingredient list
   | InstructionStep of Comment 
   | OvenStep of TemperatureLevel * TemperatureUnit 
   | TimedStep of Comment * TimeDuration * TimeUnit

type Recipe = RecipeStep list

// -----------------------------------------
// Helper functions form the DSL vocabulary
// -----------------------------------------

// create an IngredientThing with eggs
let takeEggs amount  = IngredientThing (amount, "eggs")

// create an IngredientStuff
let take amount unit ingredientName = 
    IngredientStuff(amount, unit, ingredientName)

// create an IngredientStep
let combine str ingredients = IngredientStep ("Combine "+str,ingredients)
let toStart ingredient = IngredientStep ("Start with",[ingredient])
let thenAdd ingredients = IngredientStep ("Add",ingredients)

// create an InstructionStep 
let thenDo action = InstructionStep ("Then " + action)

// create a OvenStep
let bakeAt temp unit = OvenStep(temp,unit)

// create a TimedStep 
let beatFor time unit = TimedStep ("Beat for",time, unit)
let cookFor time unit = TimedStep ("Cook for",time, unit)


// -----------------------------------------
// Examples of recipes using the DSL
// -----------------------------------------

// create a recipe for chocolate cake
let chocolateCake = [
    combine "in a large bowl: " [
        take 225 Grams "flour" 
        take 350 Grams "sugar" 
        take 85 Grams "cocoa" 
        take 2 Tsps "baking soda" 
        take 1 Tsp "baking powder" 
        take 1 Tsp "salt" ] 
    thenDo "make a well in the centre"
    thenAdd [ 
        takeEggs 2 
        take 125 Mils "oil" 
        take 250 Mils "milk"]
    beatFor 2 Mins
    bakeAt 175 C
    thenDo "add icing"
    ]


// create a recipe for pasta with sauce
let pastaWithSauce = [
    take 400 Grams "pasta" |> toStart 
    cookFor 8 Mins
    thenDo "drain." 
    thenAdd [ 
        take 100 Grams "tomato sauce" 
        take 1 Tsp "pepper"]
    thenDo "serve hot"
    ]

// Add your own examples from a recipe site, such as 
// http://www.bbc.co.uk/food/recipes/easy_chocolate_cake_31070

// -----------------------------------------
// Create utility functions that work with recipes to
// * print the ingredient list
// * print the preperation (e.g. preheat oven)
// * print the steps 
//
// See below for examples of the output
// -----------------------------------------

// create a function that takes a single ingredient and prints it in the form
// "400 Grams pasta"
// "2 Eggs"
let printSingleIngredient (ingredient:Ingredient) = 
    match ingredient with
    | IngredientThing (amount,ingr) -> 
        printfn "%i %s" amount ingr
    | IngredientStuff (amount,unit,ingr) -> 
        printfn "%i %A %s" amount unit ingr

// create a function that prints the ingredients, if it is an ingredient step, otherwise do nothing
let printIngredientsForStep (step:RecipeStep) = 
    match step with
    | IngredientStep (_, ingredients) -> 
       ingredients |> List.iter printSingleIngredient 
    | InstructionStep _ ->  // use underscore to ignore data
       ()                   // return unit for ignored branches 
    | OvenStep _ -> 
       ()
    | TimedStep _ -> 
       ()

// create a function that prints the all ingredients in the recipe
let printIngredients (recipe:Recipe) = 
    recipe |> List.iter printIngredientsForStep 


// create a function that prints the "Preheat oven to 200 C", if it is an OvenStep, otherwise do nothing
let printPreparationForStep (step:RecipeStep) = 
    match step with
    | IngredientStep _ -> ()
    | InstructionStep _ -> () 
    | OvenStep (temp,unit) -> printfn "Preheat oven to %i %A" temp unit 
    | TimedStep _ -> ()

// create a function that prints the all preperation steps in the recipe
let printPreparation (recipe:Recipe) = 
    recipe |> List.iter printPreparationForStep 

// create a function that prints the instructions for each step
let printInstructionForStep (step:RecipeStep) = 

    // private helper method
    let rec printSingleIngredient ingredient = 
        match ingredient with
        | IngredientThing (_,ingr) ->  printf "%s, " ingr
        | IngredientStuff (_,_,ingr) -> printf "%s, " ingr


    match step with
    | IngredientStep (comment, ingredients) -> 
       printf "%s " comment
       ingredients |> List.iter printSingleIngredient 
       printfn ""
    | InstructionStep (comment) -> 
       printfn "%s" comment
    | OvenStep (temp,unit) -> 
       printfn "Bake for %i %A" temp unit
    | TimedStep(comment,time,unit) -> 
       printfn "%s %i %A" comment time unit

// create a function that prints the all instructions in the recipe
let printInstructions (recipe:Recipe) = 
    recipe |> List.iter printInstructionForStep 

// put all three functions together
let rec printRecipe recipe = 
    printfn "==== Ingredients ====="
    printIngredients recipe
    printfn "\n===== Preparation ====="
    printPreparation recipe
    printfn "\n===== Instructions ====="
    printInstructions recipe

// -----------------------------------------
// print the recipe for chocolateCake 
// -----------------------------------------
printRecipe chocolateCake 

printRecipe pastaWithSauce 

(*
==== Ingredients =====
225 Grams flour
350 Grams sugar
85 Grams cocoa
2 Tsps baking soda
1 Tsp baking powder
1 Tsp salt
2 eggs
125 Mils oil
250 Mils milk

===== Preparation =====
Preheat oven to 175 C

===== Instructions =====
Combine in a large bowl:  flour, sugar, cocoa, baking soda, baking powder, salt, 
Then make a well in the centre
Add eggs, oil, milk, 
Beat for 2 Mins
Bake for 175 C
Then add icing
*)

// -----------------------------------------
// print the recipe for pastaWithSauce
// -----------------------------------------
printRecipe pastaWithSauce

(*
==== Ingredients =====
400 Grams pasta
100 Grams tomato sauce
1 Tsp pepper

===== Preparation =====

===== Instructions =====
Start with pasta, 
Cook for 8 Mins
Then drain.
Add tomato sauce, pepper, 
Then serve hot
*)



// ====================================

let calories ingredient =
    match ingredient with
    | "eggs" -> 100
    | "flour" -> 100
    | "oil" -> 200
    | "pasta" -> 400
    | "tomato sauce" -> 50
    | _ -> 0

    
let ingredientCalories ingredient =
    match ingredient with
    | IngredientThing (count,ingr) ->  
        (calories ingr) * count
    | IngredientStuff (_,_,ingr) -> 
        (calories ingr) * 1

let stepCalories (step:RecipeStep) = 
    match step with
    | IngredientStep (comment, ingredients) -> 
       ingredients |> List.sumBy ingredientCalories
    | InstructionStep (comment) ->  0
    | OvenStep (temp,unit) -> 0
    | TimedStep(comment,time,unit) -> 0

// create a function that prints the all instructions in the recipe
let countCalories (recipe:Recipe) = 
    recipe |> List.sumBy stepCalories 


countCalories chocolateCake 

countCalories pastaWithSauce 

