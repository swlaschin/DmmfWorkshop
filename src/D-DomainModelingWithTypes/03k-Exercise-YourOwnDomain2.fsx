// ================================================
// DDD Exercise: define a domain model for your own domain
//
// ================================================


module rec YourOwnDomain2


// ==============================
// Basic F# Syntax for types
// (see also fsharp-basic-syntax.fsx in parent directory)
// ==============================

(*

// --------------------
// Use Functions for workflows
// --------------------

// a single input and a single output
type Workflow = InputData -> OutputData

// a pair of inputs and a pair of outputs
type Workflow2 = InputData * State -> OutputData * State

// --------------------
// Use Records or tuples for AND
// --------------------

// a record with named fields
type ContactInfo = {
    // FieldName : FieldType
    Name : Name
    Address : Address
    }

// a pair
type ContactInfo = Name * Address

// a triplet
type ContactInfo = Name * Address * Email


// --------------------
// Use Choices for OR
// --------------------

type MyChoice =
    | Choice1 of Choice1Data
    | Choice2 of Choice2Data

// --------------------
// Use type aliases for primitives
// --------------------

// document constraints in a comment
type OrderQuantity = int // must be > 0
type EmailAddress = string // must contain @ symbol

// --------------------
// Use list and option if needed
// --------------------

type Order = {
    OrderLines : OrderLine list
    DeliveryAddress : Address option // optional data
    }


*)


//============================================
// Your code starts here


type MyWorkflow = MyInputData -> MyOutputData
