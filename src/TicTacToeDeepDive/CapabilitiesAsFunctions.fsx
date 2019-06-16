// ================================
// Capabilities as functions
// ================================

type CustomerId = CustomerId of int
type CustomerProfile = {name: string }

// a capability
type GetCustomerProfile = 
    CustomerId -> CustomerProfile 

// the database!
let dbGetProfile (CustomerId id) =
    match id with
    | 1 -> {name="Alice"}
    | 2 -> {name="Bob"}
    | _ -> {name="Someone else"}


// ================================
// Basic capability
// ================================

let basicCap = dbGetProfile 
// the interface of the function
// CustomerId -> CustomerProfile

// -----------------------------
// test it
basicCap (CustomerId 1)
basicCap (CustomerId 2)


// ================================
// Auditing 
// ================================

/// Uses of the capability will be audited
let withAuditing capabilityName userName cap = 
    fun x -> 
        // simple audit log
        let timestamp = System.DateTime.UtcNow.ToString("u")
        printfn "AUDIT: User %s used capability '%s' at %s" userName capabilityName timestamp 
        // use the capability
        cap x

// -----------------------------
// test it
let auditedCap = 
    basicCap |> withAuditing "dbGetProfile" "Scott"
// The new function has the SAME interface
//  CustomerId -> CustomerProfile

auditedCap (CustomerId 1)

// ================================
// Rate limiting
// ================================

/// Allow the function to be called once only
let onlyOnce cap = 
    let allow = ref true
    fun x -> 
        if !allow then   //! is dereferencing not negation!
            allow := false
            cap x
        else
            failwith "Only allowed once"


// -----------------------------
// test it
//    NOTE - we are delegating from a previous cap
let onceOnlyCap = onlyOnce auditedCap 
// The new function has the SAME interface
//  CustomerId -> CustomerProfile

onceOnlyCap (CustomerId 1)

// ================================
// Revoking
// ================================

/// Return a pair of functions: the revokable capability, 
/// and the revoker function
let revokable cap = 
    let allow = ref true
    let capability = fun x -> 
        if !allow then  //! is dereferencing not negation!
            cap x
        else
            failwith "capability revoked"
    let revoker() = 
        allow := false
    capability, revoker


// test
let revokableCap,revoker = revokable auditedCap 
// The function has the SAME interface
// revokableCap : (CustomerId -> CustomerProfile)

revokableCap (CustomerId 1)
revoker()