// ================================================
// State machine exercises:
//
// These are a series of exercises for building finite state machines (FSMs) in F#.
// See 04a0-StateMachine(diagram).png
//
// Here is a general template for doing these.
// ================================================

// -----------------------------------------------
// Model the domain types that are independent of state
// -----------------------------------------------

module MyDomain =
    type MyRecord = {myField:int}

// -----------------------------------------------
// Model the state machine with a type and transitions
// This is the "API" for the state machine
// -----------------------------------------------

module MyApi =
    open MyDomain

    type AStateData = {data:MyRecord; extraInfo:int}
    type BStateData = {somethingElse:int}

    // First, create a union type that represents all the states.
    // For example, if there are three states called "A", "B" and "C", the type would look like this:
    //
    // In many cases, each state will need to store some data that is relevant to that state.
    // So you will need to create types to hold that data as well.
    type State =
        | AState of AStateData
        | BState of BStateData
        | CState


    // Also, define an initial state, if appropriate
    let initialState myRec =
        AState {data = myRec; extraInfo = 0}

    // -----------------------------------------------
    // Next, write a function for each possible transition
    // If events have data associated with them, add that as well.
    // -----------------------------------------------

    /// transition starting from AState
    let transitionFromA aStateData =
        BState {somethingElse = aStateData.data.myField}

    /// transition starting from BState
    let transitionFromB bStateData extraInfo =
        if extraInfo > 1 then
            AState {data = {MyRecord.myField = bStateData.somethingElse}; extraInfo=extraInfo }
        else
            CState

// -----------------------------------------------
// The client makes decisions about which API to
// call based on the current state
// -----------------------------------------------

module MyClient =
    open MyDomain
    open MyApi

    let myRecord = {myField = 1}
    let initialState = MyApi.initialState myRecord

    // Attempt to transition
    let transition state =  // State -> State
        match state with
        | AState data ->
            MyApi.transitionFromA data
        | BState data ->
            let extraInfo = 42
            MyApi.transitionFromB data extraInfo
        | CState ->
            printfn "Can't transition. Returning original state"
            state