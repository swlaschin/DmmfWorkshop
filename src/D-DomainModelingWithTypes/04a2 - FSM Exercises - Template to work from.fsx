// ================================================
// FSM Exercises:
//
// These are a series of exercises for building finite state machines (FSMs) in F#.
// See "State machine diagram.png"
//
// ================================================


(*
Here is a general template for doing these.

First, create a union type that represents all the states.
For example, if there are three states called "A", "B" and "C", the type would look like this:

type State =
    | AState
    | BState
    | CState

In many cases, each state will need to store some data that is relevant to that state.
So you will need to create types to hold that data as well.
*)


type State =
    | AState of AStateData
    | BState of BStateData
    | CState
and AStateData =
    {something:int}
and BStateData =
    {somethingElse:int}


(*
Next, all possible events that can happen are defined in another union type.
If events have data associated with them, add that as well.
*)

type InputEvent =
    | XEvent
    | YEvent of YEventData
    | ZEvent
and YEventData =
    {eventData:string}

(*
Finally, create a "transition" function that, given a current state and input event, returns a new state.


let transition (currentState,inputEvent) =
    match currentState,inputEvent with
    | AState, XEvent -> // new state
    | AState, YEvent -> // new state
    | AState, ZEvent -> // new state
    | BState, XEvent -> // new state
    | BState, YEvent -> // new state
    | CState, XEvent -> // new state
    | CState, ZEvent -> // new state


Forcing yourself to consider every possible combination is thus a helpful design practice.

Now, even with a small number of states and events, the number of possible combinations gets large very quickly.
To make it more manageable in practice, you should create a series of helper functions, one for each state, like this:
*)


let aStateHandler stateData inputEvent =
    match inputEvent with
    | XEvent -> // new state
    | YEvent _ -> // new state
    | ZEvent -> // new state

let bStateHandler stateData inputEvent =
    match inputEvent with
    | XEvent -> // new state
    | YEvent _ -> // new state
    | ZEvent -> // new state

let cStateHandler inputEvent =
    match inputEvent with
    | XEvent -> // new state
    | YEvent _ -> // new state
    | ZEvent -> // new state

(*
And then assembly them into a single transition function
*)


let transition (currentState,inputEvent) =
    match currentState with
    | AState stateData ->
        // new state
        aStateHandler stateData inputEvent
    | BState stateData ->
        // new state
        bStateHandler stateData inputEvent
    | CState ->
        // new state
        cStateHandler inputEvent

