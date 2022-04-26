using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CheckersGame
{
    public enum eGameStatus
    {
        Win,
        Tie,
        KeepPlaying
    }

    public class Game
    {
        private Board m_GameBoard;
        private Player m_CurrentPlayer;
        private Player m_Opponent;
        private bool m_IsGameOver;
        private List<Move> m_CurrentPlayerSingleMoves;
        private List<Move> m_CurrentPlayerSkipMoves;

        public Board GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }
        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
        }

        public Player Opponent
        {
            get
            {
                return m_Opponent;
            }
        }

        public bool IsGameOver
        {
            get
            {
                return m_IsGameOver;
            }
            set
            {
                m_IsGameOver = value;
            }
        }

        public void InitializeGame(int i_BoardSize,string i_NameOfPlayerOne, string i_NameOfPlayerTwo,ePlayerType i_TypeOfPlayerTwo)
        {
            m_GameBoard = new Board(i_BoardSize);
            m_CurrentPlayer = new Player(i_NameOfPlayerOne, ePlayerType.RealPlayer,eCharactersOnBoard.SoliderO);
            m_Opponent = new Player(i_NameOfPlayerTwo, i_TypeOfPlayerTwo, eCharactersOnBoard.SoliderX);
            m_IsGameOver = false;
            m_GameBoard.InitializeGameBoard();
            m_CurrentPlayer.CreatePlayerPawnsList(m_GameBoard);
            m_Opponent.CreatePlayerPawnsList(m_GameBoard);
            m_CurrentPlayerSingleMoves = new List<Move>();
            m_CurrentPlayerSkipMoves = new List<Move>();
        }

        public void NewGameRoundInitialize()
        {
            if (m_CurrentPlayer.PawnSign != eCharactersOnBoard.SoliderO)
            {
                Player.Swap(ref m_CurrentPlayer, ref m_Opponent);
            }
            m_GameBoard.InitializeGameBoard();
            m_CurrentPlayer.PlayerPawnsList.Clear();
            m_Opponent.PlayerPawnsList.Clear();
            m_CurrentPlayer.CreatePlayerPawnsList(m_GameBoard);
            m_Opponent.CreatePlayerPawnsList(m_GameBoard);
            m_CurrentPlayerSingleMoves.Clear();
            m_CurrentPlayerSkipMoves.Clear();
            BuildPlayerMovesLists();
        }

        public void BuildPlayerMovesLists()
        {
            foreach(Cell currentPawn in CurrentPlayer.PlayerPawnsList)
            {
                updatePlayerMoveList(currentPawn);
            }
        }

        private void updatePlayerMoveList(Cell i_CurrentPawn)
        {
            switch (i_CurrentPawn.Sign)
            {
                case eCharactersOnBoard.SoliderO:
                    checkIfCanMovePawnDownAndUpdateLists(i_CurrentPawn);
                    break;
                case eCharactersOnBoard.SoliderX:
                    checkIfCanMovePawnUpAndUpdateLists(i_CurrentPawn);
                    break;
                case eCharactersOnBoard.KingO:
                case eCharactersOnBoard.KingX:
                    checkIfCanMovePawnDownAndUpdateLists(i_CurrentPawn);
                    checkIfCanMovePawnUpAndUpdateLists(i_CurrentPawn);
                    break;
            }
        }

        private void checkIfCanMovePawnDownAndUpdateLists(Cell CurrentPawn)
        {
            Point[] optionalPositions = new Point[2];
            optionalPositions[0] =new Point(CurrentPawn.Position.X + 1, CurrentPawn.Position.Y + 1);
            optionalPositions[1] = new Point(CurrentPawn.Position.X + 1, CurrentPawn.Position.Y - 1);

            for (int i = 0; i < optionalPositions.Length; i++)
            {
                if (CheckIfPositionIsInBounds(optionalPositions[i]) == true)
                {
                    if (isEmptyCell(optionalPositions[i]) == true)
                    {
                        m_CurrentPlayerSingleMoves.Add(new Move(CurrentPawn.Position, optionalPositions[i]));
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (CheckIfCanDoSkipMove(optionalPositions[i], new Point(optionalPositions[i].X + 1, optionalPositions[i].Y + 1)) == true)
                            {
                                m_CurrentPlayerSkipMoves.Add(new Move(CurrentPawn.Position, new Point(optionalPositions[i].X + 1, optionalPositions[i].Y + 1)));
                            }
                        }
                        else
                        {
                            if (CheckIfCanDoSkipMove(optionalPositions[i], new Point(optionalPositions[i].X + 1, optionalPositions[i].Y - 1)) == true)
                            {
                                m_CurrentPlayerSkipMoves.Add(new Move(CurrentPawn.Position, new Point(optionalPositions[i].X + 1, optionalPositions[i].Y - 1)));
                            }

                        }

                    }
                }
            }
        }

        private void checkIfCanMovePawnUpAndUpdateLists(Cell CurrentPawn)
        {
            Point[] optionalPositions = new Point[2];
            optionalPositions[0] = new Point(CurrentPawn.Position.X - 1, CurrentPawn.Position.Y - 1);
            optionalPositions[1] = new Point(CurrentPawn.Position.X - 1, CurrentPawn.Position.Y + 1);

            for (int i = 0; i < optionalPositions.Length; i++)
            {
                if (CheckIfPositionIsInBounds(optionalPositions[i]) == true)
                {
                    if (isEmptyCell(optionalPositions[i]) == true)
                    {
                        m_CurrentPlayerSingleMoves.Add(new Move(CurrentPawn.Position, optionalPositions[i]));
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (CheckIfCanDoSkipMove(optionalPositions[i], new Point(optionalPositions[i].X - 1, optionalPositions[i].Y - 1)) == true)
                            {
                                m_CurrentPlayerSkipMoves.Add(new Move(CurrentPawn.Position, new Point(optionalPositions[i].X - 1, optionalPositions[i].Y - 1)));
                            }
                        }
                        else
                        {
                            if (CheckIfCanDoSkipMove(optionalPositions[i], new Point(optionalPositions[i].X - 1, optionalPositions[i].Y + 1)) == true)
                            {
                                m_CurrentPlayerSkipMoves.Add(new Move(CurrentPawn.Position, new Point(optionalPositions[i].X - 1, optionalPositions[i].Y + 1)));
                            }
                        }
                    }
                }
            }
        }

        public bool CheckIfCanDoSkipMove(Point i_OpponentCell,Point i_DestenasionCell)
        {
            bool canMakeSkipMove = false;

            if (CheckIfPositionIsInBounds(i_DestenasionCell) == true)
            {
                if (isThereOpponentPawnOnCell(i_OpponentCell) == true)
                {
                    if (isEmptyCell(i_DestenasionCell) == true)
                    {
                        canMakeSkipMove = true;
                    }
                }
            }

            return canMakeSkipMove;
        }

        private bool isEmptyCell(Point i_PointToCheck)
        {
            return m_GameBoard.GameBoard[i_PointToCheck.X, i_PointToCheck.Y].Sign == eCharactersOnBoard.EmptySign;
        }

        private bool isThereOpponentPawnOnCell(Point i_OpponentCell)
        {
            eCharactersOnBoard typeOfKing = (m_Opponent.PawnSign == eCharactersOnBoard.SoliderO) ? eCharactersOnBoard.KingO : eCharactersOnBoard.KingX;

            return m_GameBoard.GameBoard[i_OpponentCell.X, i_OpponentCell.Y].Sign == m_Opponent.PawnSign ||
                    m_GameBoard.GameBoard[i_OpponentCell.X, i_OpponentCell.Y].Sign == typeOfKing;
        }

        private bool isPlayerHasSkipMove()
        {
            return m_CurrentPlayerSkipMoves.Count != 0;
        }

        private bool isPlayerHasSingleMoves()
        {
            return m_CurrentPlayerSingleMoves.Count != 0;
        }

        public bool CheckIfPositionIsInBounds(Point i_PointToCheck)
        {
            bool isPointInBounds = false;
            if (i_PointToCheck.X >= 0 && i_PointToCheck.X < m_GameBoard.SizeOfBoard && i_PointToCheck.Y >= 0 && i_PointToCheck.Y < m_GameBoard.SizeOfBoard)
            {
                isPointInBounds = true;
            }

            return isPointInBounds;
        }

        public bool IsLegalMove(Move i_CurrentPlayerMove)
        {
            bool legal = false;
            if (CheckIfPositionIsInBounds(i_CurrentPlayerMove.Source) == true &&
                CheckIfPositionIsInBounds(i_CurrentPlayerMove.Destination) == true)
            {
                if (i_CurrentPlayerMove.IsSkipMove() == true && i_CurrentPlayerMove.IsMoveInList(m_CurrentPlayerSkipMoves) == true)
                {
                    legal = true;
                }
                else if (i_CurrentPlayerMove.IsSingleMove() == true && i_CurrentPlayerMove.IsMoveInList(m_CurrentPlayerSingleMoves) == true &&
                    !isPlayerHasSkipMove())
                {
                    legal = true;
                }
            }
            return legal;
        }

        public bool MakeCurrentPlayerMove(Move i_CurrentPlayerMove, ref bool io_HasAnotherSkip)
        {
            bool moveSucceeded = false;
            Point? eatenPoint = null;

            if (IsLegalMove(i_CurrentPlayerMove) == true)
            {
                updateGameBoard(i_CurrentPlayerMove);
                updatePlayerPawnsList(i_CurrentPlayerMove);

                if (i_CurrentPlayerMove.IsSkipMove() == true)
                {
                    eatenPoint = getEatenPointOfOpponent(i_CurrentPlayerMove);
                    deletePawnFromOpponentPawnsList((Point)eatenPoint);
                    eraseOpponentPawnFromBoardAfterSkipMove((Point)eatenPoint);

                    io_HasAnotherSkip = checkIfPlayerHasAnotherSkipMove(m_GameBoard.GameBoard[i_CurrentPlayerMove.Destination.X, i_CurrentPlayerMove.Destination.Y]);
                }
                if (io_HasAnotherSkip == false)
                {
                    switchTurnAndUpdateMovesLists();
                }
                
                moveSucceeded = true;
            }

            return moveSucceeded;
        }

        private void switchTurnAndUpdateMovesLists()
        {
            Player.Swap(ref m_CurrentPlayer, ref m_Opponent);
            m_CurrentPlayerSingleMoves.Clear();
            m_CurrentPlayerSkipMoves.Clear();
            BuildPlayerMovesLists();
        }

        private void updateGameBoard(Move i_CurrentPlayerMove)
        {
            eCharactersOnBoard typeOfKing = (CurrentPlayer.PawnSign == eCharactersOnBoard.SoliderO) ? eCharactersOnBoard.KingO : eCharactersOnBoard.KingX;

            if (m_GameBoard.GameBoard[i_CurrentPlayerMove.Source.X, i_CurrentPlayerMove.Source.Y].IsKing() == true)
            {
                m_GameBoard.GameBoard[i_CurrentPlayerMove.Destination.X, i_CurrentPlayerMove.Destination.Y].Sign = typeOfKing;
            }
            else
            {
                m_GameBoard.GameBoard[i_CurrentPlayerMove.Destination.X, i_CurrentPlayerMove.Destination.Y].Sign = checkIfTransferToKing(i_CurrentPlayerMove) == true ? typeOfKing : CurrentPlayer.PawnSign;
            }
            m_GameBoard.GameBoard[i_CurrentPlayerMove.Source.X, i_CurrentPlayerMove.Source.Y].Sign = eCharactersOnBoard.EmptySign;
        }

        private void eraseOpponentPawnFromBoardAfterSkipMove(Point i_EatenPointToErase)
        {
            m_GameBoard.GameBoard[i_EatenPointToErase.X, i_EatenPointToErase.Y].Sign = eCharactersOnBoard.EmptySign;
        }

        private void updatePlayerPawnsList(Move i_CurrentPlayerMove)
        {
            foreach(Cell currentPawn in m_CurrentPlayer.PlayerPawnsList)
            {
                if(i_CurrentPlayerMove.Source==currentPawn.Position)
                {
                    m_CurrentPlayer.PlayerPawnsList.Remove(currentPawn);
                    m_CurrentPlayer.PlayerPawnsList.Add(m_GameBoard.GameBoard[i_CurrentPlayerMove.Destination.X, i_CurrentPlayerMove.Destination.Y]);
                    break;
                }
            }
        }

        private Point getEatenPointOfOpponent(Move i_CurrentPlayerMove)
        {
            int xCorrd = (i_CurrentPlayerMove.Source.X + i_CurrentPlayerMove.Destination.X) / 2;
            int yCordd= (i_CurrentPlayerMove.Source.Y + i_CurrentPlayerMove.Destination.Y) / 2;

            return new Point(xCorrd, yCordd);
        }

        private void deletePawnFromOpponentPawnsList(Point i_LocationPawnToDelete)
        {
            foreach (Cell currentPawn in m_Opponent.PlayerPawnsList)
            {
                if (i_LocationPawnToDelete == currentPawn.Position)
                {
                    m_Opponent.PlayerPawnsList.Remove(currentPawn);
                    break;
                }
            }
        }

        private bool checkIfTransferToKing(Move i_CurrentPlayerMove)
        {
            bool transferToKing = false;

            if (CurrentPlayer.PawnSign == eCharactersOnBoard.SoliderO && i_CurrentPlayerMove.Destination.X == m_GameBoard.SizeOfBoard-1)
            {
                transferToKing = true;
            }
            else if (CurrentPlayer.PawnSign == eCharactersOnBoard.SoliderX && i_CurrentPlayerMove.Destination.X == 0)
            {
                transferToKing = true;
            }

            return transferToKing;
        }

        private bool checkIfPlayerHasAnotherSkipMove(Cell i_CurrentPawn)
        {
            m_CurrentPlayerSingleMoves.Clear();
            m_CurrentPlayerSkipMoves.Clear();
            updatePlayerMoveList(i_CurrentPawn);

            return isPlayerHasSkipMove();
        }

        public eGameStatus CheckGameStatus()
        {
            eGameStatus gameStatus = eGameStatus.KeepPlaying;

            if(m_CurrentPlayer.PlayerPawnsList.Count()==0)
            {
                gameStatus = eGameStatus.Win;
            }
            else if(!isPlayerHasSingleMoves()&&!isPlayerHasSkipMove())
            {
                Player.Swap(ref m_CurrentPlayer, ref m_Opponent);
                m_CurrentPlayerSingleMoves.Clear();
                m_CurrentPlayerSkipMoves.Clear();
                BuildPlayerMovesLists();

                if (!isPlayerHasSingleMoves() && !isPlayerHasSkipMove())
                {
                    gameStatus = eGameStatus.Tie;
                }
                else
                {
                    gameStatus = eGameStatus.Win;
                    Player.Swap(ref m_CurrentPlayer, ref m_Opponent);
                }
            }

            return gameStatus;
        }

        public Move GetComputerMove()
        {
            Move randomalicComputerMove = null;
            int randomalicIndex = 0;
            Random rand = new Random();

            if (isPlayerHasSkipMove() == true)
            {
                randomalicIndex = rand.Next(m_CurrentPlayerSkipMoves.Count());
                randomalicComputerMove = m_CurrentPlayerSkipMoves[randomalicIndex];
            }
            else
            {
                randomalicIndex = rand.Next(m_CurrentPlayerSingleMoves.Count());
                randomalicComputerMove = m_CurrentPlayerSingleMoves[randomalicIndex];
            }

            return randomalicComputerMove;
        }

    }
}
