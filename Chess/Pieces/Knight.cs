using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public static Vector[] moves = new Vector[]
        {
            new Vector(-2, -1),
            new Vector(-2, 1),
            new Vector(-1, -2),
            new Vector(-1, 2),
            new Vector(1, -2),
            new Vector(1, 2),
            new Vector(2, -1),
            new Vector(2, 1)
        };
        public override string str { get { return "n"; } }
        public Knight(Vector position, Color color)
            : base(position, color)
        { }

        public override List<Vector> ValidMoves(ChessGame game)
        {
            List<Vector> validMoves = new List<Vector>();
            Vector? protectDirection = ProtectsOwnKingInDirection(game);
            //if the knight protects own king, it can't be moved at all
            if (protectDirection != null)
            {
                return validMoves;
            }
            foreach (Vector delta in moves)
            {
                Vector potentialMove = position + delta;
                if (!potentialMove.IsValid())
                {
                    continue;
                }
                Piece piece = game.PieceAt(potentialMove);
                if ((piece == null || !IsAlly(piece)) && (!game.Check || MovingToWillPreventCheck(game, potentialMove)))
                {
                    validMoves.Add(potentialMove);
                }
            }
            return validMoves;
        }
    }
}
