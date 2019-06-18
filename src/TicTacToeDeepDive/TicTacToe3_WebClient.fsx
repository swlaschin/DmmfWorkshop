#load "TicTacToe3_Server.fsx"

open System.Text
open TicTacToe3_Server

// helper to make things look nice
let prettyPrint (apiResult:TicTacToeApi.MoveResultDTO) =
    printfn "%s %s" apiResult.cellDisplayStr apiResult.moveResultMsg
    for cap in apiResult.availableMoves do
        printfn "%i %A %s" cap.cellIndex cap.displayStr cap.moveToken


TicTacToeApi.Start() |> prettyPrint

// replace with the actual guids from the result above
TicTacToeApi.Move("093ca34b-683d-47e4-aa62-f28567778711") |> prettyPrint
TicTacToeApi.Move("e8030d6c-a981-4101-a833-492ecff08b1a") |> prettyPrint

// ================================
// HTML version
// ================================

let toHtml (apiResult:TicTacToeApi.MoveResultDTO) =
    let sb = StringBuilder()
    sb
        .Append("<html>\n")
        .Append("<body>\n")
        .AppendFormat("<h1>{0} {1}</h1>\n",apiResult.cellDisplayStr,apiResult.moveResultMsg ) |> ignore

    for cap in apiResult.availableMoves do
        sb.AppendFormat("<a href='/move/{1}'>Play {0}</>\n",cap.displayStr,cap.moveToken) |> ignore

    sb.Append("</body>\n")
      .Append("</html>\n")
      .ToString()


TicTacToeApi.Start() |> toHtml |> printfn "%s"

// again, replace with the actual guids from the result
TicTacToeApi.Move("a726cc13-fabe-4660-9689-26e8f6a97fdd") |> toHtml |> printfn "%s"


