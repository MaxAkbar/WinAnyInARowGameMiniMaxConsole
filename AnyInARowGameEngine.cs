using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using WinAnyInARowGameConsole;

namespace TicTacToeGameConsole
{
    public class AnyInARowGameEngine
    {
        private int[,] _gameBoard;
        private int _winInARow;
        private readonly AnyInARowPlayer[] players;

        public AnyInARowGameEngine(int gameBoardRows, int gameBoardColumns, int winInARow, AnyInARowPlayer[] players)
        {
            ValidateInInARow(gameBoardRows, gameBoardColumns, winInARow);
            InitGameBoard(gameBoardRows, gameBoardColumns);

            _winInARow = winInARow;
            this.players = players;
        }

        private void InitGameBoard(int gameBoardRows, int gameBoardColumns)
        {
            _gameBoard = new int[gameBoardRows, gameBoardColumns];

            for (int row = 0; row < _gameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < _gameBoard.GetLength(1); col++)
                {
                    _gameBoard[row, col] = 0;
                }
            }
        }

        private void ValidateInInARow(int gameBoardRows, int gameBoardColumns, int winInARow)
        {
            int minBoardSize = Math.Min(gameBoardRows, gameBoardColumns);

            if (minBoardSize < winInARow)
            {
                throw new ArgumentException("Win in a row value " + winInARow + " is greater than board size!");
            }

            if (minBoardSize < 3)
            {
                throw new ArgumentException("Board size " + minBoardSize + " must be greater than 3!");
            }

            if (winInARow < 3)
            {
                throw new ArgumentException("Win in a row " + winInARow + " must be greater than 3!");
            }
        }

        public int GameStatus { get; set; }

        public int[,] GameBoard
        {
            get
            {
                return _gameBoard;
            }
        }

        public GameResult MakeAMove(int[,] gameBoard, int currentPlayer, int userInputRow, int userImputCol)
        {
            GameResult gameResult = new GameResult();

            gameResult.ValidMove = false;

            if (userInputRow < gameBoard.GetLength(0) && userImputCol < gameBoard.GetLength(1))
            {
                int currentMarker = gameBoard[userInputRow, userImputCol];

                if (currentMarker.Equals(1) || currentMarker.Equals(2))
                {
                    gameResult.Message = "Placement has already a marker please select another placement.";
                }
                else
                {
                    gameBoard[userInputRow, userImputCol] = currentPlayer;

                    gameResult.ValidMove = true;
                }
            }
            else
            {
                gameResult.Message = "Invalid value please select another placement.";
            }

            return gameResult;
        }

        public int CheckWinner(int[,] gameBoard, int row, int col)
        {
            // 3.3 If we have a winner, announce who won and stop the game
            if (IsGameWinner(gameBoard, row, col))
            {
                return 1;
            }

            // 3.3 If all markers are placed and no winner then it's a draw stop the game
            if (IsGameDraw(gameBoard))
            {
                return 2;
            }

            return 0;
        }

        private static bool IsGameDraw(int[,] gameBoard)
        {
            for (int row = 0; row < gameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoard.GetLength(1); col++)
                {
                    if (gameBoard[row, col].Equals(0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsGameWinner(int[,] gameBoard, int row, int col)
        {
            // Is Horizontal Cells Same
            if (IsHorizontalCellsSame(gameBoard, row, col))
            {
                return true;
            }

            // Is Vertical Cells Same
            if (IsVerticalCellsSame(gameBoard, row, col))
            {
                return true;
            }

            // Is Diagonal Left To Right Cells Same
            if (IsDiagonalLeftToRightCellsSame(gameBoard, row, col))
            {
                return true;
            }

            // Is Diagonal Right To Left Cells Same
            if (IsDiagonalRightToLeftCellsSame(gameBoard, row, col))
            {
                return true;
            }

            return false;
        }

        private bool IsDiagonalRightToLeftCellsSame(int[,] gameBoard, int row, int col)
        {
            int intARowWin = 0;

            for (int i = row + 1, j = col - 1; i < gameBoard.GetLength(0) && j > 0; i++, j--)
            {
                if (ValidateBoardCells(gameBoard, row, col, i, j, ref intARowWin))
                {
                    continue;
                }

                break;
            }

            if (intARowWin < _winInARow - 1)
            {
                for (int i = row - 1, j = col + 1; i >= 0 && j < gameBoard.GetLength(1); i--, j++)
                {
                    if (ValidateBoardCells(gameBoard, row, col, i, j, ref intARowWin))
                    {
                        continue;
                    }

                    break;
                }
            }

            return ValidateWinInARow(intARowWin);
        }

        private bool IsDiagonalLeftToRightCellsSame(int[,] gameBoard, int row, int col)
        {
            int intARowWin = 0;

            for (int i = row + 1, j = col + 1; i < gameBoard.GetLength(0) && j < gameBoard.GetLength(1); i++, j++)
            {
                if (ValidateBoardCells(gameBoard, row, col, i, j, ref intARowWin))
                {
                    continue;
                }

                break;
            }

            if (intARowWin < _winInARow - 1)
            {
                for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (ValidateBoardCells(gameBoard, row, col, i, j, ref intARowWin))
                    {
                        continue;
                    }

                    break;
                }
            }

            return ValidateWinInARow(intARowWin);
        }

        private bool IsVerticalCellsSame(int[,] gameBoard, int row, int col)
        {
            int intARowWin = 0;

            for (int i = row + 1; i < gameBoard.GetLength(1); i++)
            {
                if (ValidateBoardCells(gameBoard, row, col, i, col, ref intARowWin))
                {
                    continue;
                }

                break;
            }

            if (intARowWin < _winInARow - 1)
            {
                for (int i = row - 1; i >= 0; i--)
                {
                    if (ValidateBoardCells(gameBoard, row, col, i, col, ref intARowWin))
                    {
                        continue;
                    }

                    break;
                }
            }

            return ValidateWinInARow(intARowWin);
        }

        private bool IsHorizontalCellsSame(int[,] gameBoard, int row, int col)
        {
            int intARowWin = 0;

            for (int i = col + 1; i < gameBoard.GetLength(1); i++)
            {
                if (ValidateBoardCells(gameBoard, row, col, row, i, ref intARowWin))
                {
                    continue;
                }

                break;
            }

            if (intARowWin < _winInARow - 1)
            {
                for (int i = col - 1; i >= 0; i--)
                {
                    if (ValidateBoardCells(gameBoard, row, col, row, i, ref intARowWin))
                    {
                        continue;
                    }

                    break;
                }
            }

            return ValidateWinInARow(intARowWin);
        }

        private bool ValidateBoardCells(int[,] board, int currentRow, int currentCol, int compareRow, int compareCol, ref int nextInARow)
        {
            int currentCell = board[currentRow, currentCol];
            int compareCell = board[compareRow, compareCol];

            // compare
            if (currentCell.Equals(compareCell) && !currentCell.Equals(0))
            {
                nextInARow++;

                return true;
            }

            return false;
        }

        private bool ValidateWinInARow(int intARowWin)
        {
            if (intARowWin == _winInARow - 1)
            {
                return true;
            }

            return false;
        }

        public int GetNextPlayer(int player)
        {
            if (player.Equals(1))
            {
                return 2;
            }

            return 1;
        }

        public (int bestRow, int bestCol) GetBestMove(AnyInARowPlayer player)
        {
            int bestValue = int.MinValue;
            int bestRow = -1;
            int bestCol = -1;

            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1); col++)
                {
                    // if empty
                    if (GameBoard[row, col] == 0)
                    {
                        // place palyer marker
                        GameBoard[row, col] = player.PlayerId;

                        // scoring
                        int moveValue = MinimaxAlgorithm(0, GetOpponent(player));

                        GameBoard[row, col] = 0;

                        // if good or bad
                        if (moveValue > bestValue)
                        {
                            bestValue = moveValue;
                            bestRow = row;
                            bestCol = col;
                        }
                    }
                }
            }

            return (bestRow, bestCol);
        }

        private int MinimaxAlgorithm(int depth, AnyInARowPlayer currentPlayer)
        {
            if (IsWinner(Player.AI)) return 10;
            if (IsWinner(Player.Human)) return -10;
            if (IsFull()) return 0;

            int bestValue = 0;

            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1); col++)
                {
                    // if empty
                    if (GameBoard[row, col] == 0)
                    {
                        // place palyer marker
                        GameBoard[row, col] = currentPlayer.PlayerId;

                        // scoring
                        int moveValue = MinimaxAlgorithm(depth + 1, GetOpponent(currentPlayer));

                        GameBoard[row, col] = 0;

                        // score the move
                        bestValue = currentPlayer.Player == Player.AI ? Math.Max(bestValue, moveValue) : Math.Min(bestValue, moveValue);
                    }
                }
            }

            return bestValue;
        }

        private bool IsFull()
        {
            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1); col++)
                {
                    if (GameBoard[row, col] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsWinner(Player player)
        {
            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1); col++)
                {
                    if (GameBoard[row, col] == (int)player && IsGameWinner(GameBoard, row, col))
                    {
                       return true;
                    }
                }
            }

            return false;
        }

        private AnyInARowPlayer GetOpponent(AnyInARowPlayer currentPlayer)
        {
            AnyInARowPlayer nextPlayer = null;

            foreach (var player in players) 
            {
                if (currentPlayer.Player != player.Player)
                {
                    nextPlayer = player;
                    break;
                }
            }

            return nextPlayer;
        }
    }
}
