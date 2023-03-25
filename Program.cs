using System;
using WinAnyInARowGameConsole;

namespace TicTacToeGameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //     1   2   3   4   5   6   7   8
            //   ---------------------------------
            // 1 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 2 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 3 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 4 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 5 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 6 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 7 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 8 |   |   |   |   |   |   |   |   |
            //   ---------------------------------

            // 1.  Dynamic Board
            // 2.  Board will be square rows and column are same length
            // 3.  Win in a row must not be less than board game
            // 4.  Board can not be smaller than Tic Tac Toe
            // 5.  Game will support two players
            // 6.  Game board will be multi dimensional arrays
            // 7.  Game markers will be the player number
            // 8.  Game will be initialized with Zeros
            // 9.  Check winning move will validate from where the player position

            // default values
            int boardGameSize = 8;
            int winInARow = 5;
            bool keepLooping = true;

            while (keepLooping)
            {
                Console.Clear();

                Console.WriteLine("Welcome to our Super Duper Win in any Row game.");
                Console.Write("Please select the game board size by selecting a numeric value: ");

                if (!int.TryParse(Console.ReadLine(), out var userBoardSize))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please select a numeric value!");
                    Console.WriteLine("Select any key to start over.");
                    Console.Read();

                    continue;
                }

                Console.Write($"Please select the game win in a row must be smaller than {userBoardSize} : ");

                if (!int.TryParse(Console.ReadLine(), out var userWinInARow))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please select a numeric value!");
                    Console.WriteLine("Select any key to start over.");
                    Console.Read();

                    continue;
                }

                boardGameSize = userBoardSize;
                winInARow = userWinInARow;

                keepLooping = false;
            }

            // set up players
            AnyInARowPlayer[] gamePlayers = new AnyInARowPlayer[2];

            Console.WriteLine();
            Console.WriteLine("Would you like to play against the computer or another player!");
            Console.Write("To player with the computer press 1 any other input will be used against another player: ");
            var playerSetup = Console.ReadLine();

            if (playerSetup.ToString().Equals("1"))
            {
                Console.WriteLine();
                Console.Write("If you would you like to start enter 1 any other key entry will have the computer start: ");

                if (Console.ReadLine().Equals("1"))
                {
                    SetupPlayers(gamePlayers, Player.Human, Player.AI);
                }
                else
                {
                    SetupPlayers(gamePlayers, Player.AI, Player.Human);
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Player 1 will start with X marker and player 2 with O.");

                SetupPlayers(gamePlayers, Player.Human, Player.Human);
            }

            //int[,] board = {
            //    { 0,0,0,0,0,0,0,1 },
            //    { 0,0,0,0,0,0,1,0 },
            //    { 0,0,0,0,0,1,0,0 },
            //    { 0,0,0,0,1,0,0,0 },
            //    { 0,0,0,1,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0 },
            //};

            //DrawGameboard(board);

            AnyInARowPlayer currentPlayer = gamePlayers[1];
            AnyInARowGameEngine gameEngine = new AnyInARowGameEngine(boardGameSize, boardGameSize, winInARow, gamePlayers)
            {
                GameStatus = 0
            };

            do
            {
                Console.Clear();

                currentPlayer = gamePlayers[gameEngine.GetNextPlayer(currentPlayer.PlayerId) - 1];

                HeadsUpDisplay(currentPlayer.PlayerId);
                DrawGameboard(gameEngine.GameBoard);
                
                GameResult gameResult = new GameResult();

                do
                {
                    int userInputRow = 0;
                    int userInputCol = 0;

                    if (currentPlayer.Player.Equals(Player.Human))
                    {
                        // 3.  As the user places markers on the game update the board then notify which player has a turn
                        Console.Write("Please select a numeric value for the row: ");

                        userInputRow = Convert.ToInt32(Console.ReadLine()) - 1;

                        Console.Write("Please select a numeric value for the column: ");

                        userInputCol = Convert.ToInt32(Console.ReadLine()) - 1;
                    }
                    else
                    {
                        // AI moves
                        (int bestRow, int bestCol) = gameEngine.GetBestMove(currentPlayer);

                        userInputRow = bestRow;
                        userInputCol = bestCol;
                    }

                    //gameResult = gameEngine.MakeAMove(gameEngine.GameBoard, currentPlayer, userInputRow, userInputCol);
                    gameResult = gameEngine.MakeAMove(gameEngine.GameBoard, currentPlayer.PlayerId, userInputRow, userInputCol);

                    if (!gameResult.ValidMove)
                    {
                        Console.WriteLine(gameResult.Message);
                    }

                    // 3.1 After each turn judge if there is a winner
                    // 3.2 If no winner keep playing by going to step 1.
                    gameEngine.GameStatus = gameEngine.CheckWinner(gameEngine.GameBoard, userInputRow, userInputCol);

                } while (!gameResult.ValidMove);

            } while (gameEngine.GameStatus.Equals(0));

            Console.Clear();
            HeadsUpDisplay(currentPlayer.PlayerId);
            DrawGameboard(gameEngine.GameBoard);

            if (gameEngine.GameStatus.Equals(1))
            {
                Console.WriteLine($"Player {currentPlayer.PlayerId} is the winner!");
            }

            if (gameEngine.GameStatus.Equals(2))
            {
                Console.WriteLine($"The game is a draw!");
            }
        }

        private static void SetupPlayers(AnyInARowPlayer[] gamePlayers, Player player1, Player player2)
        {
            gamePlayers[0] = new AnyInARowPlayer("X", 1, player1);
            gamePlayers[1] = new AnyInARowPlayer("O", 2, player2);
        }

        static char GetPlayerMarker(int player)
        {
            if (player % 2 == 0)
            {
                return 'O';
            }

            return 'X';
        }

        static void HeadsUpDisplay(int PlayerNumber)
        {
            // 1.  Provide instructions
            // 1.1 A greeting
            Console.WriteLine("Welcome to the Super Duper Tic Tac Toe Game!");

            // 1.2 Display player sign, Player 1 is X and Player 2 is O
            Console.WriteLine("Player 1: X");
            Console.WriteLine("Player 2: O");
            Console.WriteLine();

            // 1.3 Who's turn is it?
            // 1.4 Instruct the user to enter a number between 1 and 9
            Console.WriteLine($"Player {PlayerNumber} to move, select 1 through 9 from the game board.");
            Console.WriteLine();
        }

        static void DrawGameboard(int[,] board)
        {
            PrintConsole(board.GetLength(1), "     ", "{0}   ", 1);
            PrintConsole(board.GetLength(1), "   -", new string('-', 4), 0);

            for (int row = 0; row < board.GetLength(0); row++)
            {
                string space = "  ";

                if (row > 8)
                {
                    space = " ";
                }
                
                Console.Write($"{row + 1}{space}");
                
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    int currentColumn = board[row, col];
                    char currentColumnUI = ' ';
                    
                    if (currentColumn != 0)
                    {
                        currentColumnUI = GetPlayerMarker(currentColumn);
                    }

                    Console.Write($"| {currentColumnUI} ");
                }

                Console.WriteLine("|");

                PrintConsole(board.GetLength(1), "   -", new string('-', 4), 0);
            }
        }

        static void PrintConsole(int loopCounter, string printDigit, string printDigits, int incrementPrintDigits)
        {
            Console.Write(printDigit);

            for (int i = 0; i < loopCounter; i++)
            {
                if (i + incrementPrintDigits > 8)
                {
                    printDigits = printDigits.Replace("   ", "  ");
                }

                Console.Write(printDigits, i + incrementPrintDigits);
            }

            Console.WriteLine();
        }
    }
}