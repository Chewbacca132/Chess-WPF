using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public class Bishop : DirectionalPiece
    {
        public override string str { get { return "b"; } }
        public Bishop(Vector position, Color color)
            : base(position, color, Vector.diagonalDirections)
        { }
    }
}
