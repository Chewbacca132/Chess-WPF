using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    //Pieces that can move in certain directions 
    //until they reach the end of the board:
    //Bishop, Rook, Queen
    public abstract class DirectionalPiece : Piece
    {
        //the directions the piece can move in
        IEnumerable<Vector> directions;
        public DirectionalPiece(Vector position, Color color, IEnumerable<Vector> directions) 
            : base (position, color) 
        {
            this.directions = directions;
        }

        public override List<Vector> ValidMoves(ChessGame game)
        {   
            List<Vector> validMoves = new List<Vector>();
            Vector? protectDirection = ProtectsOwnKingInDirection(game);
            foreach (Vector direction in directions)
            {
                if (!CanMoveInDirection(direction, protectDirection))
                {
                    continue;
                }
                Vector potentialMove = position + direction;
                while (potentialMove.IsValid())
                {
                    Piece piece = game.PieceAt(potentialMove);
                    //if either there is no piece or the piece is an enemy
                    //and either there is no check or moving to the position will prevent check
                    if ((piece == null || !IsAlly(piece)) && (!game.Check || MovingToWillPreventCheck(game, potentialMove)))
                    {
                        //this piece can move there
                        validMoves.Add(potentialMove);
                    }
                    //if there is a piece
                    //this piece cannot be moved any further
                    if (piece != null)
                    {
                        break;
                    }
                    potentialMove += direction;
                }
            }
            return validMoves;
        }
    }
}
