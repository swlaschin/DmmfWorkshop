//----------------------------------------------------------
//  Q. Write a `sayGreeting` function that takes two parameters: `greeting` and `name`.
//  Then create a `hello` function from it using partial application, where the greeting set to `"Hello"`.
//  Also create a `goodbye` function from it using partial application, where the greeting set to `"Goodbye"`.

let sayGreeting greeting aName =
    sprintf "%s %s" greeting aName

let hello = sayGreeting "Hello"
let goodbye = sayGreeting "Goodbye"

//----------------------------------------------------------
//  Q. Define a `sayGreeting_v2` _value_ (not a function) that is equivalent to the `sayGreeting` function. Hint: use a lambda.

let sayGreeting_v2 =
    fun greeting aName -> sprintf "%s %s" greeting aName

//----------------------------------------------------------
//  Q. Define a `sayGreeting_v3` function where the `greeting` parameter is a parameterless function (e.g. unit as input)

let sayGreeting_v3 greetingFn aName =
    let greeting = greetingFn()
    sprintf "%s %s" greeting aName

//----------------------------------------------------------
//  Q. Define a `sayGreeting_v4` function where the `greeting` parameter is a function that depends on the name

let sayGreeting_v4 greetingFn aName =
    let greeting = greetingFn aName
    sprintf "%s %s" greeting aName

//----------------------------------------------------------
//  Q. The function to filter a list is the `List.filter` function. It takes two parameters, the first is a predictate (returning bool) and the second is a list.
//
//  * Define a function `isLessThanFive` with signature `int->bool`
//  * Create a new function `filterLessThanFive` by partially applying this to the `List.filter` function.
//  * Test your function with the input list `[1..10]`

let isLessThanFive i = (i < 5)
let filterLessThanFive = List.filter isLessThanFive

filterLessThanFive [1..10] // [1; 2; 3; 4]


