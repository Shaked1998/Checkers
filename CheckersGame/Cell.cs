using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CheckersGame
{
    public enum eCharactersOnBoard
    {
        SoliderO = 'O',
        SoliderX = 'X',
        KingO = 'U',
        KingX = 'K',
        EmptySign = ' '
    }
    public class Cell
    {
        private eCharactersOnBoard m_Sign;
        private Point m_Position;

        public Cell()
        {
            m_Sign = eCharactersOnBoard.EmptySign;
            m_Position = new Point();
        }
        public eCharactersOnBoard Sign
        {
            get
            {
                return m_Sign;
            }
            set
            {
                m_Sign = value;
            }
        }

        public Point Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        public bool IsKing()
        {
            return m_Sign == eCharactersOnBoard.KingO || m_Sign == eCharactersOnBoard.KingX;
        }
    }
}
