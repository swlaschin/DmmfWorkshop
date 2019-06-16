// -----------------------------------------------------------
// TicTacToeDomain version 3 - Capability based
//
// "Reset Interactive Session" before using
// -----------------------------------------------------------

open System

// -----------------------------------------------------------
// TicTacToeDomain
// -----------------------------------------------------------

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

    /// Everything the UI needs to know to display the board
    type DisplayInfo = {
        cells : Cell list
        }

    /// The capability to make a move at a particular location.
    /// The gamestate, player and position are already "baked" into the function.
    type MoveCapability =
        unit -> MoveResult

    /// A capability along with the position the capability is associated with.
    /// This allows the UI to show information so that the user
    /// can pick a particular capability to exercise.
    and NextMoveInfo = {
        // the pos is for UI information only
        // the actual pos is baked into the cap.
        posToPlay : CellPosition
        capability : MoveCapability }

    /// The result of a move. It includes:
    /// * The information on the current board state.
    /// * The capabilities for the next move, if any.
    and MoveResult =
        | PlayerXToMove of DisplayInfo * NextMoveInfo list
        | PlayerOToMove of DisplayInfo * NextMoveInfo list
        | GameWon of DisplayInfo * Player
        | GameTied of DisplayInfo

    // Only the newGame function is exported from the implementation
    // all other functions come from the results of the previous move
    type TicTacToeAPI  =
        {
        newGame : MoveCapability
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

    /// get the DisplayInfo from the gameState
    let getDisplayInfo gameState =
        {DisplayInfo.cells = gameState.cells}

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

    /// determine the remaining moves
    let private remainingMoves gameState =

        // helper to return Some if a cell is playable
        let playableCell cell =
            match cell.state with
            | Played player -> None
            | Empty -> Some cell.pos

        gameState.cells
        |> List.choose playableCell

    // return the other player
    let otherPlayer player =
        match player with
        | PlayerX -> PlayerO
        | PlayerO -> PlayerX


    // return the move result case for a player
    let moveResultFor player displayInfo nextMoves =
        match player with
        | PlayerX -> PlayerXToMove (displayInfo, nextMoves)
        | PlayerO -> PlayerOToMove (displayInfo, nextMoves)

    // given a function, a player & a gameState & a position,
    // create a NextMoveInfo with the capability to call the function
    let makeNextMoveInfo f player gameState cellPos =
        // the capability has the player & cellPos & gameState baked in
        let capability() = f player cellPos gameState
        {posToPlay=cellPos; capability=capability}

    // given a function, a player & a gameState & a list of positions,
    // create a list of NextMoveInfos wrapped in a MoveResult
    let makeMoveResultWithCapabilities f player gameState cellPosList =
        let displayInfo = getDisplayInfo gameState
        cellPosList
        |> List.map (makeNextMoveInfo f player gameState)
        |> moveResultFor player displayInfo

    // player X or O makes a move
    let rec playerMove player cellPos gameState  =
        let newCell = {pos = cellPos; state = Played player}
        let newGameState = gameState |> updateCell newCell
        let displayInfo = getDisplayInfo newGameState

        if newGameState |> isGameWonBy player then
            // return the move result
            GameWon (displayInfo, player)
        elif newGameState |> isGameTied then
            // return the move result
            GameTied displayInfo
        else
            let otherPlayer = otherPlayer player
            let moveResult =
                newGameState
                |> remainingMoves
                |> makeMoveResultWithCapabilities playerMove otherPlayer newGameState
            moveResult

    /// create the state of a new game
    let newGame() =

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
        let moveResult =
            allPositions
            |> makeMoveResultWithCapabilities playerMove PlayerX gameState

        // return new game
        moveResult


    /// export the API to the application
    let api = {
        newGame = newGame
        }


// -----------------------------------------------------------
// Logging
// -----------------------------------------------------------

module Logger =
    open TicTacToeDomain

    /// Transform a MoveCapability into a logged version
    let transformCapability transformMR player cellPos (cap:MoveCapability) :MoveCapability =

        // create a new capability that logs the player & cellPos when run
        let newCap() =
            printfn "LOGINFO: %A played %A" player cellPos
            let moveResult = cap()
            transformMR moveResult
        newCap

    /// Transform a NextMove into a logged version
    let transformNextMove transformMR player (move:NextMoveInfo) :NextMoveInfo =
        let cellPos = move.posToPlay
        let cap = move.capability
        {move with capability = transformCapability transformMR player cellPos cap}

    /// Transform a MoveResult into a logged version
    let rec transformMoveResult (moveResult:MoveResult) :MoveResult =

        let tmr = transformMoveResult // abbreviate!

        match moveResult with
        | PlayerXToMove (display,nextMoves) ->
            let nextMoves' = nextMoves |> List.map (transformNextMove tmr PlayerX)
            PlayerXToMove (display,nextMoves')
        | PlayerOToMove (display,nextMoves) ->
            let nextMoves' = nextMoves |> List.map (transformNextMove tmr PlayerO)
            PlayerOToMove (display,nextMoves')
        | GameWon (display,player) ->
            printfn "LOGINFO: Game won by %A" player
            moveResult
        | GameTied display ->
            printfn "LOGINFO: Game tied"
            moveResult

    /// inject logging into the API
    let injectLogging api =

        // create a new API with the functions
        // replaced with logged versions
        { api with
            newGame = fun () -> api.newGame() |> transformMoveResult
            }



//=============================================
// API layer maps core domain
// to C# and JSON friendly code
//=============================================


module TicTacToeApi =

    open TicTacToeDomain
    let api = TicTacToeImplementation.api

    //===============================
    //C# friendly API

    type MoveInfo = {
        moveToken:string;
        displayStr:string;
        cellIndex:int
        }

    type MoveResultDTO = {
        moveResultMsg : string
        cellDisplayStr: string
        availableMoves: MoveInfo list
        }

    module ApiConverter =
        let dict = System.Collections.Generic.Dictionary<_,_>()

    //    /// Convert a string to a GUID consistently (for testing only)
    //    let guidFromString str =
    //        let bytes = System.Text.ASCIIEncoding.Default.GetBytes(s=str)
    //
    //        // create the md5 hash
    //        let md5Hasher = System.Security.Cryptography.MD5.Create()
    //        let data = md5Hasher.ComputeHash(bytes)
    //
    //        // convert the hash to a Guid
    //        Guid(data)

        let makeCapToken (cap:MoveCapability) =
            let guid = Guid.NewGuid().ToString()
            dict.Add(guid,cap)
            guid

        let getCapFromToken guid =
            match dict.TryGetValue(guid) with
            | true, cap ->
                dict.Remove(guid) |> ignore
                cap
            | false,_ -> failwithf "Capability no longer available for GUID '%s'" guid

        let posToIndex pos =
            match pos with
            | Left,Top -> 1
            | HCenter,Top -> 2
            | Right,Top -> 3
            | Left,VCenter -> 4
            | HCenter,VCenter -> 5
            | Right,VCenter -> 6
            | Left,Bottom -> 7
            | HCenter,Bottom -> 8
            | Right,Bottom -> 9

        let cellIndex cell =
            posToIndex cell.pos

        let stateToCh cellState =
            match cellState with
            | Played PlayerX -> 'x'
            | Played PlayerO -> 'o'
            | Empty-> '-'

        let makeCellDisplayStr display =
            display.cells
            |> List.sortBy cellIndex
            |> List.map (fun cell -> cell.state |> stateToCh)
            |> List.toArray
            |> fun chars -> System.String(chars)

        let toMoveInfo nextMove =
            let posToPlay = nextMove.posToPlay
            let moveCap = nextMove.capability

            let displayStr = sprintf "%A" posToPlay
            let moveToken = makeCapToken moveCap
            let cellIndex = posToIndex posToPlay
            { moveToken=moveToken;
             displayStr=displayStr;
             cellIndex=cellIndex}

        let moveResultToDTO moveResult =
            match moveResult with
            | PlayerOToMove (display,nextMoveList) ->
                let availableMoves =
                    nextMoveList
                    |> List.map toMoveInfo
                    |> List.sortBy (fun mi -> mi.cellIndex)
                let moveResultMsg = "O to move"
                let cellDisplayStr = makeCellDisplayStr display
                {cellDisplayStr=cellDisplayStr;
                 moveResultMsg = moveResultMsg;
                 availableMoves=availableMoves}

            | PlayerXToMove (display,nextMoveList) ->
                let availableMoves =
                    nextMoveList
                    |> List.map toMoveInfo
                    |> List.sortBy (fun mi -> mi.cellIndex)
                let moveResultMsg = "X to move"
                let cellDisplayStr = makeCellDisplayStr display
                {cellDisplayStr=cellDisplayStr;
                 moveResultMsg=moveResultMsg;
                 availableMoves=availableMoves}

            | GameWon (display,player) ->
                let availableMoves = []
                let cellDisplayStr = makeCellDisplayStr display
                let moveResultMsg = sprintf "%A won" player
                {cellDisplayStr=cellDisplayStr;
                 moveResultMsg=moveResultMsg;
                 availableMoves=availableMoves}

            | GameTied display ->
                let availableMoves = []
                let moveResultMsg = sprintf "Game was a draw"
                let cellDisplayStr = makeCellDisplayStr display
                {cellDisplayStr=cellDisplayStr;
                 moveResultMsg=moveResultMsg;
                 availableMoves=availableMoves}

    //===============================
    //C# friendly API

    open ApiConverter

    /// Callable by client
    let Start() =
        api.newGame() |> moveResultToDTO

    /// Callable by client
    let Move(moveToken) =
        let moveCap = getCapFromToken moveToken
        moveCap() |> moveResultToDTO
