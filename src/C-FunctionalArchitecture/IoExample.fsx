// =================================
// Domain for 00b-Exercise-IO-AtEdges.fsx
// =================================


module Domain =

    type CustomerId = CustomerId of int
    type EmailAddress = EmailAddress of string

    // A customer record in the DB
    type Customer = {
        Id : CustomerId
        Name : string
        EmailAddress : EmailAddress
        }

// A dummy database
module CustomerDatabase =
    open Domain

    let dummyCustomer =
        {
        Id = CustomerId 1
        Name = "Alice"
        EmailAddress = EmailAddress "alice@example.com"
        }

    let readCustomer (customerId:CustomerId) :Async<Customer> = async {
        return dummyCustomer
        }

    let updateCustomer (updatedCustomer:Customer) : Async<unit> = async {
        printfn "Updating Customer in database"
        // ignore
        }

module EmailServer =
    open Domain

    type EmailMessage = {
        EmailAddress : Domain.EmailAddress
        EmailBody : string
        }

    // send a message
    let sendMessage (message:EmailMessage) : Async<unit> = async {
        printfn "Sending message to %A" message.EmailAddress
        printfn "... %s" message.EmailBody
        }


