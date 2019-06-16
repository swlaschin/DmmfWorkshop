module TicTacToe

type Player = X | O

type Square = {
  Row: int
  Col: int
}

type MoveInformation = {
  Player : Player
  Square: Square
}

type MoveSuccess =
  | Draw
  | Winner of Player
  | KeepPlaying

type MoveError =
  | PlayedSameSquare of Square
  | PlayedTwiceInARow of Player
  | PlayedWhenGameIsOver of Player

type PlayMove = MoveInformation -> Result<MoveSuccess,MoveError>

