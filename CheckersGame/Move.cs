using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CheckersGame
{
    public class Move
    {
        private Point m_Source;
        private Point m_Destination;

        public Move(Point source, Point destination)
        {
            m_Source = source;
            m_Destination = destination;
        }
        public Point Source
        {
            get
            {
                return m_Source;
            }
            set
            {
                m_Source = value;
            }
        }

        public Point Destination
        {
            get
            {
                return m_Destination;
            }
            set
            {
                m_Destination = value;
            }
        }

        public bool IsMoveInList(List<Move> i_MovesList)
        {
            bool found = false;

            foreach(Move currentMove in i_MovesList)
            {
                if (this.IsEqualMove(currentMove) == true)
                {
                    found = true;
                }
            }

            return found;
        }
        public bool IsEqualMove(Move i_MoveInList)
        {
            return i_MoveInList.m_Source == this.Source && i_MoveInList.m_Destination == this.Destination;
        }

        public bool IsSkipMove()
        {
            int subtractionOfXValues = this.Source.X - this.Destination.X;
            int subtractionOfYValues = this.Source.Y - this.Destination.Y;

            return Math.Abs(subtractionOfXValues) == 2 && Math.Abs(subtractionOfYValues) == 2;
        }

        public bool IsSingleMove()
        {
            int subtractionOfXValues = this.Source.X - this.Destination.X;
            int subtractionOfYValues = this.Source.Y - this.Destination.Y;

            return Math.Abs(subtractionOfXValues) == 1 && Math.Abs(subtractionOfYValues) == 1;
        }
    }
}
