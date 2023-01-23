// =================================
// Domain for 00b-Exercise-IO-AtEdges.fsx
// =================================

/// The core domain
module Domain =

    type CustomerId = CustomerId of int
    type EmailAddress = EmailAddress of string

    // A customer record in the DB
    type Customer = {
        Id : CustomerId
        Name : string
        EmailAddress : EmailAddress
        }

// The database API
module CustomerDatabase =
    open Domain

    let dummyCustomer =
        {
        Id = CustomerId 1
        Name = "Alice"
        EmailAddress = EmailAddress "alice@example.com"
        }

    /// Read a customer from the database asynchronously
    let readCustomer (customerId:CustomerId) :Async<Customer> = async {
        // dummy implementation
        return dummyCustomer
        }

    /// Update a customer in the database asynchronously
    let updateCustomer (updatedCustomer:Customer) : Async<unit> = async {
        // dummy implementation
        printfn "Updating Customer in database"
        }

// The email server API
module EmailServer =
    open Domain

    type EmailMessage = {
        EmailAddress : Domain.EmailAddress
        EmailBody : string
        }

    /// Send an email message asynchronously
    let sendMessage (message:EmailMessage) : Async<unit> = async {
        printfn "Sending message to %A" message.EmailAddress
        printfn "... %s" message.EmailBody
        }


