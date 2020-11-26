// =================================
// This file is part of a complete app example.
// Part 2. The implementation of the workflows defined in the domain
//
// Exercise:
//    look at, execute, and understand all the code in this file
// =================================

open System


// uncomment when working with this file standalone
// comment when including this file in another file
(*
// Load the domain
#load "03a-CompleteApp-Domain.fsx"
open ``03a-CompleteApp-Domain``
*)


//===============================================
// Implementation of the workflow(s) defined in the domain
//===============================================

module EmailServiceImplementation =
    open EmailServiceDomain

    /// Implement the core workflow defined in the domain
    /// Note that this is completely pure. There is no I/O
    let sendAMessage : SendAMessage = 
        fun request ->
            let fromAddress = EmailAddress.value request.FromAddress
            let toAddress = EmailAddress.value request.ToAddress
            let body = EmailBody.value request.EmailBody

            if toAddress.Contains("example.com") then
                // bad request
                let errorMsg = sprintf "Can't send email to %s" toAddress
                Error (SmtpServerError errorMsg)
            else
                // create the email to send here but don't actually send it yet
                let emailMessage = sprintf "From: %s\nTo: %s\nSubject: Test message\n\n%s" fromAddress toAddress body
                let result = {ResponseId=42; EmailMessage=emailMessage}
                Ok result

