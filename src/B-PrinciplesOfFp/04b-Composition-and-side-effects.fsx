// ================================================
// Composition doesn't work properly
// when there are side-effects :(
// ================================================


module SideEffectFree =

  // a function with no side-effects
  let two() = 2

  let result1 =
    two() + two()

  let result2 =
    let x = two()  // we can substitute "x" for "two" and it still works
    x + x



module WithSideEffects =

  // a function with side-effects
  let two() =
    printfn "Calculating value of 2"
    2

  let result1 =
    two() + two()

  let result2 =
    let x = two()   // substitute "x" for "two" does NOT have the same effect
    x + x
