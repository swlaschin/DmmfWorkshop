// define a domain model for your own domain
module rec YourOwnDomain
//     ^for the meaning of "rec" see below

// IMPORTANT When running this file,
// highlight the entire file and run it all, including
// the "module rec" above.
//
// The "rec" means that the types do not have to
// be defined in order!

// ==============================
// F# Syntax for types
// see 02a-DomainModel-HowToDefineTypes.fsx
// ==============================


//============================================
// Your code starts here

// TIP define a "undefined" type to use when you don't know the answer yet
type undefined = exn

type MyWorkflow = MyInputData -> MyOutputData

type MyInputData = undefined
