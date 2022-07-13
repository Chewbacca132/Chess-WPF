using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public struct Vector
    {
        public int row;
        public int col;

        public Vector(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        //returns the direction from this to other
        //where this and other represent positions on the board
        //if the vectors are equal or there is no direction, returns null
        public Vector? DirectionTo(Vector other)
        {
            if (this == other) return null;
            //in the same row
            if (row == other.row)
            {
                return new Vector(0, col > other.col ? -1 : 1);
            }
            //in the same column
            if (col == other.col)
            {
                return new Vector(row > other.row ? -1 : 1, 0);
            }
            //on the same diagonal
            if (row + col == other.row + other.col || row - col == other.row - other.col)
            {
                return new Vector(row > other.row ? -1 : 1, col > other.col ? -1 : 1);
            }
            return null;
        }

        public bool IsValid()
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }

        public static Vector operator +(Vector p)
        {
            return p;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.row + v2.row, v1.col + v2.col);
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.row, -v.col);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.row - v2.row, v1.col - v2.col);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.row == v2.row && v1.col == v2.col;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return v1.row != v2.row || v1.col != v2.col;
        }

        public static readonly Vector[] straightDirections = new Vector[]
        {
            new Vector(-1, 0),
            new Vector(1, 0),
            new Vector(0, -1),
            new Vector(0, 1)
        };

        public static readonly Vector[] diagonalDirections = new Vector[]
        {
            new Vector(-1, -1),
            new Vector(-1, 1),
            new Vector(1, -1),
            new Vector(1, 1)
        };

        public static readonly Vector[] allDirections = new Vector[]
        {
            new Vector(-1, 0),
            new Vector(1, 0),
            new Vector(0, -1),
            new Vector(0, 1),
            new Vector(-1, -1),
            new Vector(-1, 1),
            new Vector(1, -1),
            new Vector(1, 1)
        };

        public override string ToString()
        {
            return $"({row}, {col})";
        }
    }
}
