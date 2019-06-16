// ================================================
// Partial application for dependency injection
// ================================================

(*
Given 
a) a function that gets data from a database
b) a function that gets data from an in-memory list
 
Create versions of these two functions that have the SAME abstract "interface" 
so that either can passed to a client who does not care about the implementation.

Fill in the ?? in the Bootstrapper code below.
*)


// -------------------------
// Domain
// -------------------------

type CustomerId = CustomerId of int
type Customer = {Id:CustomerId; Name:string}

// Let's define a "interface" that we need to get a customer
// Note that there is no mention of a database.
type GetCustomer = CustomerId -> Customer

// How can we implement this to work with different implementations?


// -------------------------
// Client uses abstract interface
// -------------------------

let client (customerRepository:GetCustomer)  =    
    let customerId = CustomerId 1
    // use the passed in interface
    customerRepository customerId 


// -------------------------
// Database version
// -------------------------

let dummyCustomer = {Id=CustomerId 1;Name="Alice"}   // create a dummy customer

/// Database implementation
let getCustomerFromDatabase connection (customerId:CustomerId) =    
    // from connection    
    // select customer   
    // where customerId = customerId 
    
    dummyCustomer  // dummy response for now


// -------------------------
// In-memory version
// -------------------------

/// In-memory implementation
let getCustomerFromMemory map (customerId:CustomerId) = 
    map |> Map.find customerId


(*
So now we have two versions of a “getCustomer” function. 

One was created by partially applying the database connection, 
and one was created by partially applying the map. 

They both have the same signature and are completely interchangeable, 
and they have both been “injected” with a different dependency.
*)




// -------------------------
// Bootstrapper picks the implementation based on the config at startup
// -------------------------

type Config = DB | Memory
let config = DB

let getCustomer = 
    match config with
    | DB -> 
        let myConnection = "server=localhost;database=customers"
        getCustomerFromDatabase myConnection
    | Memory -> 
        let inMemoryMap = Map.ofList [dummyCustomer.Id,dummyCustomer]
        getCustomerFromMemory inMemoryMap 

// getCustomer : CustomerId -> Customer

// call the client
client getCustomer 