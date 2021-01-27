// =================================
// Understanding Wrapper types vs. aliases
// =================================

(*
An alias looks like
    type Age = int

An wrapper looks like
    type Age = Age of int
which is the same as this
    type Age = {Value:int}

*)


module AliasExample =

    // An alias:
    // * does not constrain the value.
    // * can be interchanged with any other value

    // hard to understand :(
    type Name = string * string

    type FirstName = string  // alias
    type LastName = string   // alias

    // better to understand!
    type Name_v2 = FirstName * LastName

module AliasExample2 =

    // define two aliases for int
    type Age = int
    type ProductId = int

    // an integer
    let i : int = 42

    // an Age
    let age : Age = i     // OK -- they are both ints
    let age2 = age + 1    // OK -- they are both ints

    // a ProductId
    let productId : ProductId = 42
    let productId2 : ProductId = age  // OK -- they are both ints


module WrapperExample =

    // A wrapper:
    // * must be created using a constructor
    // * often has constraints that can be enforced
    // * can NOT be interchanged with any other value

    // two different types
    type Age = Age of int
    type ProductId = ProductId of int

    // an integer
    let i : int = 42

    // an Age
    let age : Age = i     // Error: they are not the same type
    let age : Age = Age i // OK, using the "Age" constructor
    let age2 = age + 1    // Error -- Age is not an int

    // a ProductId
    let productId = ProductId 42      // OK, using the "ProductId" constructor
    let productId2 : ProductId = age  // Error: they are not the same type



