open System

// -----------------------------------------------------------
// TicTacToe server - version 1
// -----------------------------------------------------------

/// The TicTacToe Domain
module TicTacToeDomain =

    type HorizPosition = Left | Middle| Right
    type VertPosition = Top | Center | Bottom
    type CellPosition = HorizPosition * VertPosition

    type Player = PlayerO | PlayerX

    type CellState =
        | Played of Player
        | Empty

    type Cell = {
        pos : CellPosition
        state : CellState
        }

    type GameState = {
        cells : Cell list
        }

    type PlayerXMove = PlayerXMove of CellPosition
    type PlayerOMove = PlayerOMove of CellPosition

    type MoveResult =
        | OToMove of GameState * PlayerOMove list
        | XToMove of GameState * PlayerXMove list
        | GameWon of GameState * Player
        | GameTied of GameState
        | QuitGame

/// The TicTacToe Implementation
module TicTacToeImplementation =
    open TicTacToeDomain

    let allHorizPositions = [Left; Middle; Right]
    let allVertPositions = [Top; Center; Bottom]

    let linesToCheck =
        let mkRow v = [for h in allHorizPositions do yield (h,v)]
        let rows = [for v in allVertPositions do yield mkRow v]

        let mkCol h = [for v in allVertPositions do yield (h,v)]
        let cols = [for h in allHorizPositions do yield mkCol h]

        let diagonal1 = [Left,Top; Middle,Center; Right,Bottom]
        let diagonal2 = [Left,Bottom; Middle,Center; Right,Top]

        // return all the lines to check
        [
        yield! rows
        yield! cols
        yield diagonal1
        yield diagonal2
        ]

    // get the cell corresponding to the cell position
    let getCell gameState posToFind =
        gameState.cells
        |> List.find (fun cell -> cell.pos = posToFind)

    /// update a cell the in the GameState and return
    /// a new GameState
    let private updateCell newCell gameState =

        // create a helper function
        let replaceIfMatching newCell oldCell =
            if oldCell.pos = newCell.pos then
                newCell
            else
                oldCell

        let newCells = gameState.cells |> List.map (replaceIfMatching newCell)
        // return a new game state with the new cells
        {gameState with cells = newCells }

    let private isGameWonBy player gameState =

        // helper to check if a cell was played by a particular player
        let cellWasPlayedBy playerToCompare cell =
            match cell.state with
            | Played player -> player = playerToCompare
            | Empty -> false

        // helper to see if every cell in the list has been played by the same player
        let lineIsAllSamePlayer player cellPosList =
            //cellList |> List.forall (fun cell -> cell |> wasPlayedBy player)
            cellPosList
            |> List.map (getCell gameState)
            |> List.forall (cellWasPlayedBy player)

        linesToCheck
        |> List.exists (lineIsAllSamePlayer player)


    let private isGameTied gameState =
        // helper to check if a cell was played by any player
        let cellWasPlayed cell =
            match cell.state with
            | Played player -> true
            | Empty -> false

        gameState.cells
        |> List.forall cellWasPlayed

    let private remainingMovesForPlayer playerMove gameState =

        let playableCell cell =
            match cell.state with
            | Played player -> None
            | Empty -> Some (playerMove cell.pos)

        gameState.cells
        |> List.choose playableCell

    let newGameState =
        let allPositions = [
            for h in allHorizPositions do
            for v in allVertPositions do
                yield (h,v)
            ]
        let emptyCells =
            allPositions
            |> List.map (fun pos -> {pos = pos; state = Empty})

        // create initial game state
        let gameState = { cells=emptyCells }

        // get list of valid moves for player O
        let playerOMoves =
            allPositions
            |> List.map PlayerOMove

        // return new game
        OToMove (gameState,playerOMoves)

    // player X makes a move
    let xMove gameState (PlayerXMove cellPos) =
        let newCell = {pos = cellPos; state = Played PlayerX}
        let newGameState = gameState |> updateCell newCell

        if newGameState |> isGameWonBy PlayerX then
            GameWon (newGameState , PlayerX)
        elif newGameState |> isGameTied then
            GameTied newGameState
        else
            let remainingMoves =
                newGameState |> remainingMovesForPlayer PlayerOMove
            OToMove (newGameState,remainingMoves)

    // player O makes a move
    let oMove gameState (PlayerOMove cellPos) =
        let newCell = {pos = cellPos; state = Played PlayerO}
        let newGameState = gameState |> updateCell newCell

        if newGameState |> isGameWonBy PlayerO then
            GameWon (newGameState, PlayerO)
        elif newGameState |> isGameTied then
            GameTied newGameState
        else
            let remainingMoves =
                newGameState |> remainingMovesForPlayer PlayerXMove
            XToMove (newGameState,remainingMoves)

//=============================================
// API layer maps core domain
// to C# and JSON friendly code
//=============================================


/// An API that hides the F# types and uses simple types instead
/// (ints, strings, etc)
module TicTacToeApi =

    open TicTacToeDomain
    open TicTacToeImplementation

    /// DTO class to expose to non-F# client
    type MoveResultDTO = {
        gameStateToken : string
        moveResult : string
        availableMoves: int list
        }


    module ApiConverter =

        let posToIndex pos =
            match pos with
            | Left,Top -> 1
            | Middle,Top -> 2
            | Right,Top -> 3
            | Left,Center -> 4
            | Middle,Center -> 5
            | Right,Center -> 6
            | Left,Bottom -> 7
            | Middle,Bottom -> 8
            | Right,Bottom -> 9

        let posFromIndex i =
            match i with
            | 1 -> Left,Top
            | 2 -> Middle,Top
            | 3 -> Right,Top
            | 4 -> Left,Center
            | 5 -> Middle,Center
            | 6 -> Right,Center
            | 7 -> Left,Bottom
            | 8 -> Middle,Bottom
            | 9 -> Right,Bottom
            | _ -> failwithf "Cell index '%i' not valid" i

        let cellIndex cell =
            posToIndex cell.pos

        let stateToCh cellState =
            match cellState with
            | Played PlayerX -> 'x'
            | Played PlayerO -> 'o'
            | Empty-> '-'

        let stateFromCh ch =
            match ch with
            | 'x' -> Played PlayerX
            | 'o' -> Played PlayerO
            | '-' -> Empty
            | _ -> failwithf "Unexpected char '%c' in game token" ch

        let parseCell arrayIndex ch =
            let pos = posFromIndex (arrayIndex+1) //0-based to 1-based
            let state = stateFromCh ch
            {state=state; pos=pos}

        let gameStateFromToken gameToken =
            if String.IsNullOrEmpty(gameToken) then
                failwith "gameToken should not be null"
            let cells =
                gameToken.ToCharArray()
                |> Array.mapi parseCell
                |> Array.toList
            {cells=cells}

        let gameStateToToken (gameState:GameState) =
            gameState.cells
            |> List.sortBy cellIndex
            |> List.map (fun cell -> cell.state |> stateToCh)
            |> List.toArray
            |> fun chars -> System.String(chars)

        let moveResultToDTO moveResult =
            match moveResult with
            | OToMove (gameState,moveList) ->
                let token =
                    gameState
                    |> gameStateToToken
                let availableMoves =
                    moveList
                    |> List.map (fun (PlayerOMove pos) -> posToIndex pos)
                    |> List.sort
                let moveResult = "O to move"
                {gameStateToken=token;
                 moveResult=moveResult;
                 availableMoves=availableMoves}

            | XToMove (gameState,moveList) ->
                let token =
                    gameState
                    |> gameStateToToken
                let availableMoves =
                    moveList
                    |> List.map (fun (PlayerXMove pos) -> posToIndex pos)
                    |> List.sort
                let moveResult = "X to move"
                {gameStateToken=token;
                 moveResult=moveResult;
                 availableMoves=availableMoves}

            | GameWon (gameState,player) ->
                let token =
                    gameState
                    |> gameStateToToken
                let availableMoves = []
                let moveResult = sprintf "%A won" player
                {gameStateToken=token;
                 moveResult=moveResult;
                 availableMoves=availableMoves}

            | GameTied gameState ->
                let token =
                    gameState
                    |> gameStateToToken
                let availableMoves = []
                let moveResult = sprintf "Game was a draw"
                {gameStateToken=token;
                 moveResult=moveResult;
                 availableMoves=availableMoves}

            | QuitGame ->
                let token = ""
                let availableMoves = []
                let moveResult = sprintf "Game ended"
                {gameStateToken=token;
                 moveResult=moveResult;
                 availableMoves=availableMoves}

    //===============================
    //C# friendly API

    open ApiConverter

    /// Callable by client
    let Start() =
        newGameState |> moveResultToDTO

    /// Callable by client
    let Move(gameStateToken,player,cellIndex) =
        if cellIndex = 0 then
            QuitGame |> moveResultToDTO
        else
            let gameState = gameStateFromToken gameStateToken
            let cellPos = posFromIndex cellIndex
            match player with
            | "X" ->
                xMove gameState (PlayerXMove cellPos)
                |> moveResultToDTO
            | "O" ->
                oMove gameState (PlayerOMove cellPos)
                |> moveResultToDTO
            | _ ->
                failwithf "Player '%s' not recognized" player