// ================================================
// DSL Exercise: Create a DSL to move a turtle around
//
// ================================================


(*

A turtle has a Position (in a x,y coordinate grid), a Direction and a Color.

You can instruct the turtle to do something using the following instructions:

* Move N, where n is an int
* Turn left, which rotates the direction anti-clockwise
* Turn right, which rotates the direction clockwise
* Change color to black
* Change color to red
 
1) Create a vocabulary for a turtle

2) Write code that will make the above instructions work.

3) Then write code that will take a list of instructions and apply them all, as follows:

let instructions = [
    Turn Left
    Move 100
    SetColor Red
    Turn Right
    Move 10
    ]

instructions |> applyListOfInstructions 

*)

// set up the vocabulary
type Position = int * int
type Color = Black | Red
type Direction = North | South | East | West
type Turtle = {
    pos: Position
    direction: Direction
    color: Color
    }

type TurnInstruction = Left | Right

type TurtleInstruction = 
    | Move of int
    | Turn of TurnInstruction
    | SetColor of Color


// define a function that changes the position given a distance and direction and returns a new Position 
let changePosition (distance:int) (direction:Direction) (pos:Position)  :Position =
    let x,y = pos
    match direction with
    | North -> 
        x, y + distance
    | South -> 
        x, y - distance
    | East -> 
        x + distance, y
    | West -> 
        x - distance, y

// define a function that changes the direction given a turn instruction and returns a new Direction 
let turnDirection (turnInstruction:TurnInstruction) (direction:Direction) :Direction =
    match direction with
    | North -> 
        match turnInstruction with Left -> West | Right -> East
    | South -> 
        match turnInstruction with Left -> East | Right -> West
    | East -> 
        match turnInstruction with Left -> North | Right -> South
    | West -> 
        match turnInstruction with Left -> South | Right -> North

// define a function that moves the turtle given a TurtleInstruction and returns a new Turtle
let moveTurtle (instruction:TurtleInstruction) (turtle:Turtle) :Turtle =
    match instruction with
    | Move distance -> 
        let newPos = turtle.pos |> changePosition distance turtle.direction
        {turtle with pos = newPos}
    | Turn turnInstruction -> 
        let newDirection = turtle.direction |> turnDirection turnInstruction
        {turtle with direction = newDirection}
    | SetColor newColor -> 
        {turtle with color = newColor}

// define function that applies a list of instructions
let applyListOfInstructions (instructions:TurtleInstruction list) =
    // List.fold has parameters [action] [initialValue] [list]
    //   - action has two params - the state and the new instruction
    let foldAction turtle instruction =
        moveTurtle instruction turtle
    let initialState = { pos=0,0; direction=North; color=Black}
    List.fold foldAction initialState instructions 

// ---------------------------------------------
// test some examples
// ---------------------------------------------
let turtle0 = { pos=0,0; direction=North; color=Black}

let instruction1 = Turn Left
let turtle1  = turtle0 |> moveTurtle instruction1 

let instruction2 = Move 100
let turtle2  = turtle1 |> moveTurtle instruction2 

// ---------------------------------------------
// test a whole set of instructions
// ---------------------------------------------
let instructions = [
    Turn Left
    Move 100
    SetColor Red
    Turn Right
    Move 10
    ]

instructions |> applyListOfInstructions 