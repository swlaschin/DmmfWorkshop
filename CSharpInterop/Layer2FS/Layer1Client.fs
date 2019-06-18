module Layer1Client

open Layer1CS

/// call some CS code from F#
let getCustomer (repo:ICsCustomerRepository) id =
    repo.GetCustomer id

// object initialization using ctor only
let dummyCsCustomer1 =
    let cust = CsCustomer()
    cust.Id <- 1
    cust.Name <- "Alice"
    cust

// object initialization, similar to C# style
let dummyCsCustomer2 = CsCustomer(Id=1,Name="Alice")

let createNewCsRepo() =
    {new ICsCustomerRepository with
        member this.GetCustomer id =
            dummyCsCustomer1
            }