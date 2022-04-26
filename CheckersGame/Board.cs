using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CheckersGame
{
    public enum eBoardSizes
    {
        Small = 6,
        Medium = 8,
        Large = 10
    }
    public class Board
    {
        private Cell[,] m_GameBoard;
        private int m_SizeOfBoard;

        public Board(int i_SizeOfBoard)
        {
            m_SizeOfBoard = i_SizeOfBoard;
            m_GameBoard = new Cell[i_SizeOfBoard, i_SizeOfBoard];
        }

        public void InitializeGameBoard()
        {
            int oPlayerEndOfLoop = m_SizeOfBoard / 2 - 1;
            int xPlayerStartOfLoop = m_SizeOfBoard / 2 + 1;
            for (int i = 0; i < m_SizeOfBoard; i++)
            {
                for (int j = 0; j < m_SizeOfBoard; j++)
                {
                    m_GameBoard[i, j] = new Cell();
                    if (((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0)) && i < oPlayerEndOfLoop)
                    {
                        m_GameBoard[i, j].Sign = eCharactersOnBoard.SoliderO;
                    }
                    else if (((i % 2 != 0 && j % 2 == 0) || (i % 2 == 0 && j % 2 != 0)) && i >= xPlayerStartOfLoop)
                    {
                        m_GameBoard[i, j].Sign = eCharactersOnBoard.SoliderX;
                    }
                    m_GameBoard[i, j].Position = new Point(i, j);
                }
            }
        }

        public Cell[,] GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }
        public int SizeOfBoard
        {
            get
            {
                return m_SizeOfBoard;
            }
            set
            {
                m_SizeOfBoard = value;
            }
        }
    }
}
    
