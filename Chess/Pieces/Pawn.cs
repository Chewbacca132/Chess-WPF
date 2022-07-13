using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    //Need to implement En Passant
    public class Pawn : Piece
    {
        public override string str { get { return "p"; } }
        public int dRow { get { return color == Color.White ? -1 : 1; } }
        public int initialRow { get { return color == Color.White ? 6 : 1; } }
        public bool canBePromoted { get { return position.row == (color == Color.White ? 0 : 7); } }
        public Pawn(Vector position, Color color)
            : base(position, color)
        { }

        public override List<Vector> ValidMoves(ChessGame game)
        {
            List<Vector> validMoves = new List<Vector>();
            Vector? protectDirection = ProtectsOwnKingInDirection(game);
            Vector delta = new Vector(dRow, 0);
            Vector forward = position + delta;
            if (!forward.IsValid())
            {
                //the pawn is at the edge of the board
                //and cannot move further
                return validMoves;
            }
            Piece forwardPiece = game.PieceAt(forward);
            if (forwardPiece == null && CanMoveInDirection(delta, protectDirection))
            {
                if (!game.Check || MovingToWillPreventCheck(game, forward))
                {
                    validMoves.Add(forward);
                }
                Vector further = forward + delta;
                if (position.row == initialRow && game.PieceAt(further) == null
                    && (!game.Check || MovingToWillPreventCheck(game, further)))
                {
                    validMoves.Add(further);
                }
            }
            Vector leftDelta = new Vector(dRow, -1);
            Vector rightDelta = new Vector(dRow, 1);
            if (CanMoveInDirection(leftDelta, protectDirection) && ValidDiagonalCapture(game, position + leftDelta))
            {
                validMoves.Add(position + leftDelta);
            }
            if (CanMoveInDirection(rightDelta, protectDirection) && ValidDiagonalCapture(game, position + rightDelta))
            {
                validMoves.Add(position + rightDelta);
            }
            return validMoves;
        }

        bool ValidDiagonalCapture(ChessGame game, Vector targetPosition)
        {
            if (!targetPosition.IsValid()) return false;
            //can move diagonally if there is an enemy piece
            //at the target position
            Piece piece = game.PieceAt(targetPosition);
            return piece != null && !IsAlly(piece) && (!game.Check || MovingToWillPreventCheck(game, targetPosition));
        }
    }
}
