using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
using Chess.Pieces;

namespace ChessApp
{
    public class Display
    {
        //contains the strings to display
        public static Dictionary<Type, PieceRepr> pieceRepr = new Dictionary<Type, PieceRepr>()
        {
            { typeof(King), new PieceRepr("♔", "♚") },
            { typeof(Queen), new PieceRepr("♕", "♛") },
            { typeof(Rook), new PieceRepr("♖", "♜") },
            { typeof(Bishop), new PieceRepr("♗", "♝") },
            { typeof(Knight), new PieceRepr("♘", "♞") },
            { typeof(Pawn), new PieceRepr("♙", "♟") }
        };


        //converts an index in a 1D array to a position on the chessboard
        public static Chess.Vector IndexToPosition(int index)
        {
            int row = (int)System.Math.Floor((float)(index / 8));
            int col = index - 8 * row;
            return new Chess.Vector(row, col);
        }

        //converts a position on the chessboard to an index in a 1D array
        public static int PositionToIndex(Chess.Vector position)
        {
            return 8 * position.row + position.col;
        }


        public static string GetString(Piece piece)
        {
            return Display.pieceRepr[piece.GetType()].GetRepr(piece.color);
        }
    }


    public class PieceRepr
    {
        public string white;
        public string black;
        public PieceRepr(string white, string black)
        {
            this.white = white;
            this.black = black;
        }
        public string GetRepr(Chess.Color color)
        {
            if (color == Chess.Color.White)
            {
                return white;
            }
            return black;
        }
    }
}
