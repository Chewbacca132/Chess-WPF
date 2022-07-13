using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public class Queen : DirectionalPiece
    {
        public override string str { get { return "q"; } }
        public Queen(Vector position, Color color)
            : base(position, color, Vector.allDirections)
        { }
    }
}
