module TicTacToe


type Player = X | O

type MoveInformation = {
  Player : Player
  Row: int
  Col: int
}

type MoveResult =
  | Draw
  | Winner of Player
  | KeepPlaying

type PlayMove = MoveInformation -> MoveResult

