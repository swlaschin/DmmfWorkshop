// ====================================
// Common questions about constrained types
// ====================================

// ====================================
// Question: Is this overkill to use wrapped types? Does everything have to be wrapped?
// Answer: It depends.
//         * Does the type have special behavior?
//         * Does the type need to kept distinct?
//         * What would be the worse that would happen if it wasn't wrapped?
// ====================================

// EmailAddress? Yes, this should probably be a wrapped type
type EmailAddress = EmailAddress of string

// String50? Maybe not.
// * Does it have special behavior? No.
// * Does the type need to kept distinct?
//   Maybe, if you wanted to pre-validate before inserting in a database
// * What would be the worse that would happen if it wasn't wrapped?
//   A database exception? Or a mailing label that overran the page?
type String50 = private String50 of string

// FirstName? No
// * Does it have special behavior? No.
// * Does the type need to kept distinct? No.
// * What would be the worse that would happen if it wasn't wrapped? Nothing
type FirstName = private FirstName of string

// instead I might use it as an aliase for String or String50
(*
type FirstName = string    // OK
type FirstName = String50  // OK
*)

// ====================================
// Question: When to use public vs. private constructors?
// Answer: It depends on how much yout trust the callers
// ====================================

(*
If I have a library with a public API used by clients I don't know,
I would definitely want to keep the constructor private and only use "factory" functions
that did validation.

If it is just me (or colleagues I trust), I might not bother.
If the wrapper was ever used directly, then hopefully this wold be caught in code review.
*)

// ====================================
// Question: How does using a constrained type compare with
//           using a validation attribute?
// Answer: It's much better
// ====================================

(*
Here's an example of the two ways.
* Email1 is of type EmailAddress. It MUST always be valid. Also, the constraints are self documenting.
* Email2 is of type string. It is only validated if you remember to call the validation function!
    Even if it is validated once, it could be changed to something invalid as it
    is passed through other code.
*)
 type Contact = {
     // 1. putting the validation in the type
     Email1: EmailAddress

     // 2. putting the validation in a property attribute
     //[Validation(EmailAddress)]
     Email2: string

     }
