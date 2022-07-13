using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class InvalidPositionException : Exception
    {
        public InvalidPositionException(string message)
           : base(message)
        { }
    }

    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(Move move)
            : base("Move " + move.ToString() + " is invalid")
        { }
    }

    public class MoveStackEmptyException : Exception
    {
        public MoveStackEmptyException()
            : base("No moves to undo")
        { }
    }
}
