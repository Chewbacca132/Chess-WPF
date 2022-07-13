using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
using Chess.Pieces;

namespace Chess
{
    public class Chessboard//INotifyCollectionChanged, IEnumerable<>
    {
        public Piece[,] board = new Piece[8, 8];

    }
}
