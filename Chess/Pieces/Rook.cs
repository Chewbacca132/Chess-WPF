using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public class Rook : DirectionalPiece
    {
        public override string str { get { return "r"; } }
        public Rook(Vector position, Color color)
            : base(position, color, Vector.straightDirections)
        { }
    }
}
