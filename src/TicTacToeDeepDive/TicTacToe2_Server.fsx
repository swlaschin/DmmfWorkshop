open System

// -----------------------------------------------------------
// TicTacToe server - version 2
// Uses a parameterized GameState to hide the implementation
//
// "Reset Interactive Session" before using
// -----------------------------------------------------------

(*

To play in a IDE:
1) first highlight all code in the file and "Execute in Interactive" or equivalent
2) Uncomment the ConsoleApplication.startGame() line at the bottom and execute it

To play in command line:
1) Uncomment the ConsoleApplication.startGame() line at the bottom and execute the entire file using FSI

*)

module TicTacToeDomain =

    type HorizPosition = Left | HCenter | Right
    type VertPosition = Top | VCenter | Bottom
    type CellPosition = HorizPosition * VertPosition

    type Player = PlayerO | PlayerX

    type CellState =
        | Played of Player
        | Empty

    type Cell = {
        pos : CellPosition
        state : CellState
        }

    type PlayerXPos = PlayerXPos of CellPosition
    type PlayerOPos = PlayerOPos of CellPosition

    type ValidMovesForPlayerX = PlayerXPos list
    type ValidMovesForPlayerO = PlayerOPos list

    type MoveResult =
        | PlayerXToMove of ValidMovesForPlayerX
        | PlayerOToMove of ValidMovesForPlayerO
        | GameWon of Player
        | GameTied

    // the "use-cases"
    type NewGame<'GameState> =
        'GameState * MoveResult
    type PlayerXMoves<'GameState> =
        'GameState -> PlayerXPos -> 'GameState * MoveResult
    type PlayerOMoves<'GameState> =
        'GameState -> PlayerOPos -> 'GameState * MoveResult

    // helper function
    type GetCells<'GameState> =
        'GameState -> Cell list

    // the functions exported from the implementation
    // for the UI to use.
    type TicTacToeAPI<'GameState>  =
        {
        newGame : NewGame<'GameState>
        playerXMoves : PlayerXMoves<'GameState>
        playerOMoves : PlayerOMoves<'GameState>
        getCells : GetCells<'GameState>
        }

// -----------------------------------------------------------
// TicTacToeImplementation
// -----------------------------------------------------------

module TicTacToeImplementation =
    open TicTacToeDomain

    /// private implementation of game state
    type GameState = {
        cells : Cell list
        }

    /// the list of all horizontal positions
    let allHorizPositions = [Left; HCenter; Right]

    /// the list of all horizontal positions
    let allVertPositions = [Top; VCenter; Bottom]

    /// A type to store the list of cell positions in a line
    type Line = Line of CellPosition list

    /// a list of the eight lines to check for 3 in a row
    let linesToCheck =
        let mkHLine v = Line [for h in allHorizPositions do yield (h,v)]
        let hLines= [for v in allVertPositions do yield mkHLine v]

        let mkVLine h = Line [for v in allVertPositions do yield (h,v)]
        let vLines = [for h in allHorizPositions do yield mkVLine h]

        let diagonalLine1 = Line [Left,Top; HCenter,VCenter; Right,Bottom]
        let diagonalLine2 = Line [Left,Bottom; HCenter,VCenter; Right,Top]

        // return all the lines to check
        [
        yield! hLines
        yield! vLines
        yield diagonalLine1
        yield diagonalLine2
        ]

    /// get the cells from the gameState
    let getCells gameState =
        gameState.cells

    /// get the cell corresponding to the cell position
    let getCell gameState posToFind =
        gameState.cells
        |> List.find (fun cell -> cell.pos = posToFind)

    /// update a particular cell in the GameState
    /// and return a new GameState
    let private updateCell newCell gameState =

        // create a helper function
        let substituteNewCell oldCell =
            if oldCell.pos = newCell.pos then
                newCell
            else
                oldCell

        // get a copy of the cells, with the new cell swapped in
        let newCells = gameState.cells |> List.map substituteNewCell

        // return a new game state with the new cells
        {gameState with cells = newCells }

    /// Return true if the game was won by the specified player
    let private isGameWonBy player gameState =

        // helper to check if a cell was played by a particular player
        let cellWasPlayedBy playerToCompare cell =
            match cell.state with
            | Played player -> player = playerToCompare
            | Empty -> false

        // helper to see if every cell in the Line has been played by the same player
        let lineIsAllSamePlayer player (Line cellPosList) =
            cellPosList
            |> List.map (getCell gameState)
            |> List.forall (cellWasPlayedBy player)

        linesToCheck
        |> List.exists (lineIsAllSamePlayer player)


    /// Return true if all cells have been played
    let private isGameTied gameState =
        // helper to check if a cell was played by any player
        let cellWasPlayed cell =
            match cell.state with
            | Played _ -> true
            | Empty -> false

        gameState.cells
        |> List.forall cellWasPlayed

    /// determine the remaining moves for a player
    let private remainingMovesForPlayer playerMove gameState =

        // helper to return Some if a cell is playable
        let playableCell cell =
            match cell.state with
            | Played player -> None
            | Empty -> Some (playerMove cell.pos)

        gameState.cells
        |> List.choose playableCell


    /// create the state of a new game
    let newGame =

        // allPositions is the cross-product of the positions
        let allPositions = [
            for h in allHorizPositions do
            for v in allVertPositions do
                yield (h,v)
            ]

        // all cells are empty initially
        let emptyCells =
            allPositions
            |> List.map (fun pos -> {pos = pos; state = Empty})

        // create initial game state
        let gameState = { cells=emptyCells }

        // initial of valid moves for player X is all positions
        let validMoves =
            allPositions
            |> List.map PlayerXPos

        // return new game
        gameState, PlayerXToMove validMoves

    // player X makes a move
    let playerXMoves gameState (PlayerXPos cellPos) =
        let newCell = {pos = cellPos; state = Played PlayerX}
        let newGameState = gameState |> updateCell newCell

        if newGameState |> isGameWonBy PlayerX then
            // return the new state and the move result
            newGameState, GameWon PlayerX
        elif newGameState |> isGameTied then
            // return the new state and the move result
            newGameState, GameTied
        else
            let remainingMoves =
                newGameState |> remainingMovesForPlayer PlayerOPos
            newGameState, PlayerOToMove remainingMoves

    // player O makes a move
    let playerOMoves gameState (PlayerOPos cellPos) =
        let newCell = {pos = cellPos; state = Played PlayerO}
        let newGameState = gameState |> updateCell newCell

        if newGameState |> isGameWonBy PlayerO then
            // return the new state and the move result
            newGameState, GameWon PlayerO
        elif newGameState |> isGameTied then
            // return the new state and the move result
            newGameState, GameTied
        else
            let remainingMoves =
                newGameState |> remainingMovesForPlayer PlayerXPos
            newGameState, PlayerXToMove remainingMoves

        // Exercise - refactor to remove the duplicate code from
        // playerXMoved  and playerOMoved


    /// export the API to the application
    let api = {
        newGame = newGame
        playerXMoves = playerXMoves
        playerOMoves = playerOMoves
        getCells = getCells
        }

// -----------------------------------------------------------
// ConsoleUi
// -----------------------------------------------------------

/// Console based user interface
module ConsoleUi =
    open TicTacToeDomain

    /// Track the UI state
    type UserAction<'a> =
        | ContinuePlay of 'a
        | ExitGame

    /// Print each available move on the console
    let displayAvailableMoves moves =
        moves
        |> List.iteri (fun i move ->
            printfn "%i) %A" i move )

    /// Get the move corresponding to the
    /// index selected by the user
    let getMove moveIndex moves =
        if moveIndex < List.length moves then
            let move = List.item moveIndex moves
            Some move
        else
            None

    /// Given that the user has not quit, attempt to parse
    /// the input text into a index and then find the move
    /// corresponding to that index
    let processMoveIndex inputStr gameState availableMoves makeMove processInputAgain =
        match Int32.TryParse inputStr with
        // TryParse will output a tuple (parsed?,int)
        | true,inputIndex ->
            // parsed ok, now try to find the corresponding move
            match getMove inputIndex availableMoves with
            | Some move ->
                // corresponding move found, so make a move
                let moveResult = makeMove gameState move
                ContinuePlay moveResult // return it
            | None ->
                // no corresponding move found
                printfn "...No move found for inputIndex %i. Try again" inputIndex
                // try again
                processInputAgain()
        | false, _ ->
            // int was not parsed
            printfn "...Please enter an int corresponding to a displayed move."
            // try again
            processInputAgain()

    /// Ask the user for input. Process the string entered as
    /// a move index or a "quit" command
    let rec processInput gameState availableMoves makeMove =

        // helper that calls this function again with exactly
        // the same parameters
        let processInputAgain() =
            processInput gameState availableMoves makeMove

        printfn "Enter an int corresponding to a displayed move or q to quit:"
        let inputStr = Console.ReadLine()
        if inputStr = "q" then
            ExitGame
        else
            processMoveIndex inputStr gameState availableMoves makeMove processInputAgain

    /// Display the cells on the console in a grid
    let displayCells cells =
        let cellToStr cell =
            match cell.state with
            | Empty -> "-"
            | Played player ->
                match player with
                | PlayerO -> "O"
                | PlayerX -> "X"

        let printCells cells  =
            cells
            |> List.map cellToStr
            |> List.reduce (fun s1 s2 -> s1 + "|" + s2)
            |> printfn "|%s|"

        let topCells =
            cells |> List.filter (fun cell -> snd cell.pos = Top)
        let centerCells =
            cells |> List.filter (fun cell -> snd cell.pos = VCenter)
        let bottomCells =
            cells |> List.filter (fun cell -> snd cell.pos = Bottom)

        printCells topCells
        printCells centerCells
        printCells bottomCells
        printfn ""   // add some space

    /// After each game is finished,
    /// ask whether to play again.
    let rec askToPlayAgain api  =
        printfn "Would you like to play again (y/n)?"
        match Console.ReadLine() with
        | "y" ->
            ContinuePlay api.newGame
        | "n" ->
            ExitGame
        | _ -> askToPlayAgain api

    /// The main game loop, repeated
    /// for each user input
    let rec gameLoop api userAction =
        printfn "\n------------------------------\n"  // a separator between moves

        match userAction with
        | ExitGame ->
            printfn "Exiting game."
        | ContinuePlay (state,moveResult) ->
            // first, update the display
            state |> api.getCells |> displayCells

            // then handle each case of the result
            match moveResult with
            | GameTied ->
                printfn "GAME OVER - Tie"
                printfn ""
                let nextUserAction = askToPlayAgain api
                gameLoop api nextUserAction
            | GameWon player ->
                printfn "GAME WON by %A" player
                printfn ""
                let nextUserAction = askToPlayAgain api
                gameLoop api nextUserAction
            | PlayerOToMove availableMoves ->
                printfn "Player O to move"
                displayAvailableMoves availableMoves
                let newResult = processInput state availableMoves api.playerOMoves
                gameLoop api newResult
            | PlayerXToMove availableMoves ->
                printfn "Player X to move"
                displayAvailableMoves availableMoves
                let newResult = processInput state availableMoves api.playerXMoves
                gameLoop api newResult

    /// start the game with the given API
    let startGame api =
        let userAction = ContinuePlay api.newGame
        gameLoop api userAction

// -----------------------------------------------------------
// Logging
// -----------------------------------------------------------

module Logger =
    open TicTacToeDomain

    let logXMove (PlayerXPos cellPos)=
        printfn "X played %A" cellPos

    let logOMove (PlayerOPos cellPos)=
        printfn "O played %A" cellPos

    /// inject logging into the API
    let injectLogging api =

        // make a logged version of the game function
        let playerXMoves state move =
            logXMove move
            api.playerXMoves state move

        // make a logged version of the game function
        let playerOMoves state move =
            logOMove move
            api.playerOMoves state move

        // create a new API with
        // the move functions replaced
        // with logged versions
        { api with
            playerXMoves = playerXMoves
            playerOMoves = playerOMoves
            }

// -----------------------------------------------------------
// ConsoleApplication
// -----------------------------------------------------------

module ConsoleApplication =

    let startGame() =
        let api = TicTacToeImplementation.api
        let loggedApi = Logger.injectLogging api
        ConsoleUi.startGame loggedApi

(*

To play in a IDE:
1) first highlight all code in the file and "Execute in Interactive" or equivalent
2) Uncomment the ConsoleApplication.startGame() line below and execute it

To play in command line:
1) Uncomment the ConsoleApplication.startGame() line below and execute the entire file using FSI

*)


// ConsoleApplication.startGame()


