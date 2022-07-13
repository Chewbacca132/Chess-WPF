using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public class King : Piece
    {
        public override string str { get { return "k"; } }
        public King(Vector position, Color color)
            : base(position, color)
        { }
        public override List<Vector> ValidMoves(ChessGame game)
        {
            List<Vector> validMoves = new List<Vector>();
            foreach (Vector direction in Vector.allDirections)
            {
                Vector potentialMove = position + direction;
                if (!potentialMove.IsValid())
                {
                    continue;
                }
                Piece piece = game.PieceAt(potentialMove);
                if ((piece == null || !IsAlly(piece)) && CanMoveTo(game, potentialMove))
                {
                    validMoves.Add(potentialMove);
                }
            }
            return validMoves;
        }
        public bool CanMoveTo(ChessGame game, Vector targetPosition)
        {
            return IsCheckedByAt(game, targetPosition).Count == 0;
        }

        public List<Piece> IsCheckedBy(ChessGame game)
        {
            return IsCheckedByAt(game, position);
        }

        //returns a List of the pieces this king would be checked by at the specified position
        //or an empty list if not
        public List<Piece> IsCheckedByAt(ChessGame game, Vector position)
        {
            List<Piece> checkBy = new List<Piece>();
            //check for directional pieces
            foreach(Vector direction in Vector.allDirections)
            {
                Piece threat = IsThreatenedInDirectionBy(game, direction, position);
                if (threat != null)
                {
                    checkBy.Add(threat);
                }
            }

            //check for knights
            foreach (Vector delta in Knight.moves)
            {
                Vector position_ = position + delta;
                if (!position_.IsValid())
                {
                    continue;
                }
                Piece piece = game.PieceAt(position_);
                if (piece != null && !IsAlly(piece) && piece is Knight)
                {
                    checkBy.Add(piece);
                }
            }

            //check for enemy king
            foreach(Vector delta in Vector.allDirections)
            {
                Vector position_ = position + delta;
                if (!position_.IsValid())
                {
                    continue;
                }
                Piece piece = game.PieceAt(position_);
                if (piece != null && !IsAlly(piece) && piece is King)
                {
                    checkBy.Add(piece);
                }
            }

            //check for enemy pawns
            foreach (Vector delta in Vector.diagonalDirections)
            {
                Vector position_ = position + delta;
                if (!position_.IsValid())
                {
                    continue;
                }
                Pawn pawn = game.PieceAt(position_) as Pawn;
                if (pawn != null && !IsAlly(pawn) && pawn.dRow == -delta.row)
                {
                    checkBy.Add(pawn);
                }
            }
            return checkBy;
        }
    }
}
