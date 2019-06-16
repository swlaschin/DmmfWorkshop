#load "TicTacToe1_Server.fsx"

// -----------------------------------------------------------
// TicTacToe web client - uses the version 1 implementation via the API
// -----------------------------------------------------------

open TicTacToe1_Server

let result0 = TicTacToeApi.Start()
let result1 = TicTacToeApi.Move(result0.gameStateToken,"X",1)
let result2 = TicTacToeApi.Move(result1.gameStateToken,"O",4)
let result3 = TicTacToeApi.Move(result2.gameStateToken,"X",2)
let result4 = TicTacToeApi.Move(result3.gameStateToken,"O",5)
let result5 = TicTacToeApi.Move(result4.gameStateToken,"X",3)



// oops - client doesn't notice that the game has ended
TicTacToeApi.Move(result5.gameStateToken,"O",6)


// oops - replay same state - REPLAY ATTACK OH NOES
TicTacToeApi.Move(result4.gameStateToken,"X",3)








// oops - player O plays twice in a row
TicTacToeApi1.Move("xx-oo----","O",6)










// oops - player Z doesn't exist
TicTacToeApi1.Move("xx-oo----","Z",6)








// oops - call position doesn't exist
TicTacToeApi1.Move("xx-oo----","X",-1)








