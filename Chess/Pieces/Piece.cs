using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public abstract class Piece
    {
        public Vector position;
        public readonly Color color;

        //string representation
        public abstract string str { get; }
        public Piece(Vector position, Color color)
        {
            this.position = position;
            this.color = color;
        }
        public abstract List<Vector> ValidMoves(ChessGame game);
        public bool IsAlly(Piece piece)
        {
            return color == piece.color;
        }

        //if this piece is between own king and an enemy piece
        //that would attack the king if we moved this piece
        public Vector? ProtectsOwnKingInDirection(ChessGame game)
        {
            Vector kingPosition = game.GetKing(color).position;
            Vector? directionToKing = position.DirectionTo(kingPosition);
            if (directionToKing == null) return null;

            Vector dirToKing = (Vector)directionToKing;
            if (IsThreatenedInDirectionBy(game, dirToKing) != null)
            {
                return dirToKing;
            }
            if (IsThreatenedInDirectionBy(game, -dirToKing) != null)
            {
                return -dirToKing;
            }
            return null;
        }

        //return the piece it is threatened by in the specified direction
        //in the specified direction
        ////if pos is specified, returns true if this piece will be threatened
        ////after moving it to pos
        protected Piece IsThreatenedInDirectionBy(ChessGame game, Vector direction, Vector? pos = null)
        {
            Vector position = (pos == null) ? this.position : (Vector)pos;
            bool isDiagonal = direction.row != 0 && direction.col != 0;
            Vector position_ = position + direction;
            while (position_.IsValid())
            {
                Piece piece = game.PieceAt(position_);
                if (piece != null && piece != this)
                {
                    if (!IsAlly(piece) && (piece is Queen || (isDiagonal && piece is Bishop) || (!isDiagonal && piece is Rook)))
                    {
                        return piece;
                    }
                    return null;
                }
                position_ += direction;

            }
            return null;
        }

        protected bool CanMoveInDirection(Vector direction, Vector? protectDirection)
        {
            return protectDirection == null || direction == protectDirection || direction == -protectDirection;
        }

        //white pieces in lowercase
        //black pieces in uppercase
        public override string ToString()
        {
            if (color == Color.White)
            {
                return str;
            }
            return str.ToUpper();
        }

        //returns true if moving this piece to the target position will prevent own check

        protected bool MovingToWillPreventCheck(ChessGame game, Vector targetPosition)
        {
            //if own king is checked by multiple pieces
            //moving this piece to any position would not prevent the check
            if (game.checkBy.Count > 1)
            {
                return false;
            }
            Piece checkBy = game.checkBy[0];
            //capturing the enemy piece prevents the check
            if (checkBy.position == targetPosition) return true;

            Vector? directionToKing = targetPosition.DirectionTo(game.GetKing(color).position);
            Vector? directionToCheckBy = targetPosition.DirectionTo(checkBy.position);
            //if there is no direction between either the target position and the king
            //or the target position and the piece that causes the check
            //moving this piece to the target position won't prevent the check
            if (directionToKing == null || directionToCheckBy == null)
            {
                return false;
            }

            //the directions have to be opposite so that this piece
            //gets between own king and the enemy piece that causes the check
            return directionToKing == -directionToCheckBy;
        }
    }
}
