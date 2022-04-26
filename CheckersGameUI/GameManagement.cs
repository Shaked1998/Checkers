using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CheckersGame;

namespace CheckersGameUI
{
    class GameManagement
    {
        private const int k_MaxSizeOfPlayerName = 20;
        private Game m_CheckersGame = new Game();

        public void RunGame()
        {
            int sizeOfBoard;
            string nameOfPlayerOne, nameOfPlayerTwo, playerFormatedMove = null;
            ePlayerType typeOfPlayerTwo;
            bool hasAnotherSkip = false, moveSucceeded = true, isFirstMove = true;
            Move playerMove = null;

            GetGameData(out sizeOfBoard, out nameOfPlayerOne, out nameOfPlayerTwo, out typeOfPlayerTwo);
            m_CheckersGame.InitializeGame(sizeOfBoard, nameOfPlayerOne, nameOfPlayerTwo, typeOfPlayerTwo);
            m_CheckersGame.BuildPlayerMovesLists();
            Ex02.ConsoleUtils.Screen.Clear();
            do
            {
                PrintGameBoard();
                if (!moveSucceeded)
                {
                    System.Console.WriteLine("Invalid Move! Please try again. ");
                }
                printPlayersMovesDetails(isFirstMove);

                if (m_CheckersGame.CurrentPlayer.PlayerType == CheckersGame.ePlayerType.RealPlayer)
                {
                    playerFormatedMove = getValidFormatedMoveFromPlayer();
                    if (playerFormatedMove[0] == 'Q')
                    {
                        m_CheckersGame.IsGameOver = true;
                    }
                    else
                    {
                        playerMove = createMoveFromString(playerFormatedMove);
                        moveSucceeded = m_CheckersGame.MakeCurrentPlayerMove(playerMove, ref hasAnotherSkip);
                    }
                }
                else
                {
                    playerMove = m_CheckersGame.GetComputerMove();
                    playerFormatedMove = createFormatedStringFromMove(playerMove);
                    moveSucceeded = m_CheckersGame.MakeCurrentPlayerMove(playerMove, ref hasAnotherSkip);
                    System.Threading.Thread.Sleep(3000);
                }

                if (moveSucceeded == true && !hasAnotherSkip)
                {
                    m_CheckersGame.Opponent.LastMove = playerFormatedMove;
                }

                Ex02.ConsoleUtils.Screen.Clear();
                isFirstMove = false;

                if (m_CheckersGame.CheckGameStatus() != CheckersGame.eGameStatus.KeepPlaying && m_CheckersGame.IsGameOver == false)
                { 
                    m_CheckersGame.IsGameOver = true;
                }
                
                if (m_CheckersGame.IsGameOver == true)
                {
                    showEndGameDetails();
                    checkIfPlayerWantsToContinuePlaying();
                    isFirstMove = true;
                }
            } while (!m_CheckersGame.IsGameOver);
        }

        public void GetGameData(out int io_SizeOfBoard, out string io_NameOfPlayerOne, out string io_NameOfPlayerTwo, out ePlayerType io_TypeOfPlayerTwo)
        {
            io_NameOfPlayerOne = getValidNameOfPlayer();
            io_SizeOfBoard = getSizeOfBoard();
            if (checkIfPlayingAginstRealPlayer() == true)
            {
                io_NameOfPlayerTwo = getValidNameOfPlayer();
                io_TypeOfPlayerTwo = CheckersGame.ePlayerType.RealPlayer;
            }
            else
            {
                io_NameOfPlayerTwo = "Computer";
                io_TypeOfPlayerTwo = CheckersGame.ePlayerType.Computer;
            }
        }

        private string getValidNameOfPlayer()
        {
            System.Console.WriteLine("Please enter your name (up to 20 letters): ");
            string name = System.Console.ReadLine();
            bool validInput = false;
            do
            {
                if (name.Length <= k_MaxSizeOfPlayerName && !name.Contains(" "))
                {
                    validInput = true;
                }

                if (!validInput)
                {
                    System.Console.WriteLine("Invalid name, please try again!");
                    name = System.Console.ReadLine();
                }
            }
            while (!validInput);

            return name;
        }

        private int getSizeOfBoard()
        {
            System.Console.WriteLine("Please choose size of the board: " + Environment.NewLine + "1. 6X6" + Environment.NewLine + "2. 8X8" + Environment.NewLine + "3. 10X10");
            string choiceOfUser = System.Console.ReadLine();
            bool validInput = false;
            int NumOfChoice;
            int boardSize = 0;

            do
            {
                int.TryParse(choiceOfUser, out NumOfChoice);
                if (NumOfChoice >= 1 && NumOfChoice <= 3)
                {
                    validInput = true;
                }

                if (!validInput)
                {
                    System.Console.WriteLine("Invalid choice, please try again!");
                    choiceOfUser = System.Console.ReadLine();
                }
            }
            while (!validInput);

            switch(NumOfChoice)
            {
                case 1:
                    boardSize = (int)CheckersGame.eBoardSizes.Small;
                    break;
                case 2:
                    boardSize = (int)CheckersGame.eBoardSizes.Medium;
                    break;
                case 3:
                    boardSize = (int)CheckersGame.eBoardSizes.Large;
                    break;
            }

            return boardSize;
        }

        private bool checkIfPlayingAginstRealPlayer()
        {
            bool isAginstRealPlayer = false;
            System.Console.WriteLine("How would you like to play? " + Environment.NewLine + "1. Aginst a friend." + Environment.NewLine + "2. Aginst the computer.");
            string choiceOfUser = getValidChoiceBetweenTwoOptions();

            if (choiceOfUser[0] == '1')
            {
                isAginstRealPlayer = true;
            }

            return isAginstRealPlayer;
        }

        private string getValidChoiceBetweenTwoOptions()
        {
            string choiceOfUser = System.Console.ReadLine();
            bool validInput = false;

            do
            {
                if (choiceOfUser.Length == 1 && (choiceOfUser[0] == '1' || choiceOfUser[0] == '2'))
                {
                    validInput = true;
                }

                if (!validInput)
                {
                    System.Console.WriteLine("Invalid choice, please try again!");
                    choiceOfUser = System.Console.ReadLine();
                }
            }
            while (!validInput);

            return choiceOfUser;
        }

        private string getValidFormatedMoveFromPlayer()
        {
            string playerMove = System.Console.ReadLine();
            bool validFormat = false;

            do
            {
                if ((playerMove.Length == 5 && char.IsUpper(playerMove[0]) && char.IsLower(playerMove[1]) && playerMove[2] == '>' && char.IsUpper(playerMove[3]) && char.IsLower(playerMove[4])) || (playerMove.Length==1 && playerMove[0]=='Q'))
                {
                    validFormat = true;
                }
                else
                {
                    System.Console.Write("Invalid Input! please try again : ");
                    playerMove = System.Console.ReadLine();
                }
            } while (!validFormat);

            return playerMove;
        }

        private Move createMoveFromString(string i_Move)
        {
            int x_From = i_Move[1] - 'a';
            int y_From = i_Move[0] - 'A';
            int x_To = i_Move[4] - 'a';
            int y_To = i_Move[3] - 'A';
            Move NewMove = new Move(new Point(x_From, y_From),new Point(x_To, y_To));

            return NewMove;
        }

        private string createFormatedStringFromMove(Move i_Move)
        {
            StringBuilder formatedStringMove = new StringBuilder(5);

            formatedStringMove.Append((char)(i_Move.Source.Y + 'A'));
            formatedStringMove.Append((char)(i_Move.Source.X + 'a'));
            formatedStringMove.Append('>');
            formatedStringMove.Append((char)(i_Move.Destination.Y + 'A'));
            formatedStringMove.Append((char)(i_Move.Destination.X + 'a'));

            return formatedStringMove.ToString();
        }

        private void showEndGameDetails()
        {
            if (m_CheckersGame.CheckGameStatus() == CheckersGame.eGameStatus.Win)
            {
                System.Console.WriteLine("The winner is {0} !", m_CheckersGame.Opponent.PlayerName);
            }
            if (m_CheckersGame.CheckGameStatus() == CheckersGame.eGameStatus.Tie)
            {
                System.Console.WriteLine("It's a Tie! ");
            }

            m_CheckersGame.Opponent.Score = m_CheckersGame.Opponent.CalculatePlayerScore()-m_CheckersGame.CurrentPlayer.CalculatePlayerScore();
            System.Console.WriteLine("The winner's score is: {0}",m_CheckersGame.Opponent.Score);
            System.Console.WriteLine("The Loser's score is: {0}", m_CheckersGame.CurrentPlayer.Score);

        }

        private void printPlayersMovesDetails(bool i_IsFirstMove)
        {
            string opponentPlayerName = m_CheckersGame.Opponent.PlayerName;
            char opponentSignOfPlayer = (char)m_CheckersGame.Opponent.PawnSign;
            string currentPlayerName = m_CheckersGame.CurrentPlayer.PlayerName;
            char currentSignOfPlayer = (char)m_CheckersGame.CurrentPlayer.PawnSign;
            string opponentLastMove = m_CheckersGame.Opponent.LastMove;

            if (!i_IsFirstMove)
            {
                System.Console.WriteLine("{0}'s move was ({1}) : {2}", opponentPlayerName, opponentSignOfPlayer, opponentLastMove);
            }

            System.Console.Write("{0}'s turn ({1}) : ", currentPlayerName, currentSignOfPlayer);
        }

        private void checkIfPlayerWantsToContinuePlaying()
        {
            System.Console.WriteLine("Would you like to play another game? " + Environment.NewLine + "1. Yes!" + Environment.NewLine + "2. No, I'm tired.");
            string choiceOfUser = getValidChoiceBetweenTwoOptions();

            if (choiceOfUser[0] == '1')
            {
                //initGame
                //calc score
                //.....
                
                m_CheckersGame.NewGameRoundInitialize();
                m_CheckersGame.IsGameOver = false;
                Ex02.ConsoleUtils.Screen.Clear();
            }
        }


        public void PrintGameBoard()
        {
            int sizeOfBoard = m_CheckersGame.GameBoard.SizeOfBoard;
            char lowerCaseLetter = 'a';
            PrintUpperLettersLine();
            for (int i = 0; i < sizeOfBoard; i++)
            {
                PrintLine();
                System.Console.Write("{0}|", lowerCaseLetter);
                for (int j = 0; j < sizeOfBoard; j++)
                {
                    System.Console.Write("  {0}  |", (char)m_CheckersGame.GameBoard.GameBoard[i, j].Sign);
                }
                lowerCaseLetter++;
                System.Console.WriteLine(Environment.NewLine);
            }
            PrintLine();
        }

        public void PrintUpperLettersLine()
        {
            int sizeOfBoard = m_CheckersGame.GameBoard.SizeOfBoard;
            char upperCaseLetter = 'A';

            for (int i = 0; i <sizeOfBoard; i++)
            {
                System.Console.Write("   {0}  ", upperCaseLetter);
                upperCaseLetter++;
            }
            System.Console.WriteLine(Environment.NewLine);
        }

        public void PrintLine()
        {
            int sizeOfBoard = m_CheckersGame.GameBoard.SizeOfBoard;

            for (int i = 0; i < sizeOfBoard; i++)
            {
                System.Console.Write("======");
            }
            System.Console.WriteLine(Environment.NewLine);
        }
    }
}
    


