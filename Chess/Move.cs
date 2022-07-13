using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Pieces;

namespace Chess
{
    public class Move
    {
        public Vector from;
        public Vector to;
        public Piece? capturedPiece;
        public bool promoted;

        public Move(Vector from, Vector to, Piece? capturedPiece, bool promoted = false)
        {
            this.from = from;
            this.to = to;
            this.capturedPiece = capturedPiece;
            this.promoted = promoted;
        }

        public override string ToString()
        {
            return $"[{from}, {to}]";
        }
    }
}
