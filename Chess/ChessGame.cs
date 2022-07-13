using System.Collections.ObjectModel;
using Chess.Pieces;
namespace Chess
{
    public delegate void GameOverEventHandler(object sender, GameOverEventArgs e);
    public enum Color
    {
        White,
        Black
    }

    public enum State
    {
        Playing,
        Draw,
        WhiteWon,
        BlackWon
    }

    public class ChessGame
    {
        readonly Piece[,] board = new Piece[8, 8];
        Stack<Move> moves = new Stack<Move>();
        List<Piece> whitePieces;
        List<Piece> blackPieces;
        King whiteKing;
        King blackKing;
        
        public event GameOverEventHandler OnGameOver;
        public Color current { get; private set; }
        public State state { get; private set; }
        public List<Piece> checkBy = new List<Piece>();

        public bool Check { get { return checkBy.Count > 0; } }


        public ChessGame()
        {
            board = new Piece[8, 8];
            Init();
        }

        void Init()
        {
            //white pieces in rows 6 and 7
            //black pieces in rows 0 and 1
            whitePieces = new List<Piece>();
            blackPieces = new List<Piece>();
            current = Color.White;

            //initialize pawns;
            for (int col = 0; col < 8; col++)
            {
                Vector whitePos = new Vector(6, col);
                Vector blackPos = new Vector(1, col);
                SpawnPiece<Pawn>(whitePos, Color.White);
                SpawnPiece<Pawn>(blackPos, Color.Black);
            }
            //initialize kings
            whiteKing = SpawnPiece<King>(new Vector(7, 4), Color.White);
            blackKing = SpawnPiece<King>(new Vector(0, 4), Color.Black);
            //initialize other pieces
            InitPiece<Rook>(0);
            InitPiece<Knight>(1);
            InitPiece<Bishop>(2);
            InitPiece<Queen>(3);
            InitPiece<Bishop>(5);
            InitPiece<Knight>(6);
            InitPiece<Rook>(7);
        }
        T SpawnPiece<T>(Vector position, Color color) where T : Piece
        {
            var constructor = typeof(T).GetConstructor(new Type[] { typeof(Vector), typeof(Color) });
            T piece = (T)constructor.Invoke(new object[] { position, color });
            SetPieceAt(position, piece);
            List<Piece> pieces = GetPieces(color);
            pieces.Add(piece);
            return piece;
        }

        //calls spawnPiece<T> to initialize pieces of type T
        //black in row 0, white in row 7
        void InitPiece<T>(int col) where T : Piece
        {
            Vector whitePos = new Vector(7, col);
            Vector blackPos = new Vector(0, col);
            SpawnPiece<T>(whitePos, Color.White);
            SpawnPiece<T>(blackPos, Color.Black);
        }

        public Piece? PieceAt(Vector position)
        {
            if (!position.IsValid())
            {
                throw new InvalidPositionException(position.ToString() + " is invalid");
            }
            return board[position.row, position.col];
        }

        private void SetPieceAt(Vector position, Piece? piece)
        {
            board[position.row, position.col] = piece;
        }

        public King GetKing(Color color)
        {
            return color == Color.White ? whiteKing : blackKing;
        }

        public IEnumerator<Piece> Pieces (Color color)
        {
            List<Piece> pieces = GetPieces(color);
            foreach(Piece piece in pieces)
            {
                yield return piece;
            }
        }

        private List<Piece> GetPieces(Color color)
        {
            return color == Color.White ? whitePieces : blackPieces;
        }

        public void RequestMove(Vector from, Vector to)
        {
            if (state != State.Playing) return;
            Piece? piece = PieceAt(from);
            if (piece == null || piece.color != current || piece.ValidMoves(this).All(move => move != to))
            {
                throw new InvalidMoveException(new Move(from, to, null));
            }
            Move(from, to);
        }

        void Move(Vector from, Vector to)
        {
            Piece piece = PieceAt(from);
            Piece? capturedPiece = PieceAt(to);
            Move move = new Move(from, to, capturedPiece);
            if (capturedPiece != null)
            {
                GetPieces(capturedPiece.color).Remove(capturedPiece);
            }
            SetPieceAt(to, piece);
            SetPieceAt(from, null);
            piece.position = to;
            if (piece is Pawn pawn)
            {
                if (pawn.canBePromoted)
                {
                    Promote<Queen>(pawn);
                    move.promoted = true;
                }
            }
            moves.Push(move);
            current = current == Color.White ? Color.Black : Color.White;
            checkBy = GetKing(current).IsCheckedBy(this);
            UpdateState();
        }

        public void Undo()
        {
            if (moves.Count == 0)
            {
                throw new MoveStackEmptyException();
            }
            Move move = moves.Pop();
            SetPieceAt(move.from, PieceAt(move.to));
            PieceAt(move.from).position = move.from;
            if (move.capturedPiece != null)
            {
                GetPieces(move.capturedPiece.color).Add(move.capturedPiece);
                SetPieceAt(move.to, move.capturedPiece);
            }
            else
            {
                SetPieceAt(move.to, null);
            }
            if (move.promoted)
            {
                Unpromote(PieceAt(move.from));
            }
            current = current == Color.White ? Color.Black : Color.White;
        }

        void Promote<T>(Pawn pawn) where T : Piece
        {
            List<Piece> pieces = GetPieces(pawn.color);
            pieces.Remove(pawn);
            SpawnPiece<T>(pawn.position, pawn.color);
        }

        //to support undo with promotion
        void Unpromote(Piece piece)
        {
            List<Piece> pieces = GetPieces(piece.color);
            pieces.Remove(piece);
            SpawnPiece<Pawn>(piece.position, piece.color);
        }

        public void UpdateState()
        {
            IEnumerator<Vector> allValidMoves = AllValidMoves(current);
            //if there are no validMoves
            if (!allValidMoves.MoveNext())
            {
                //if current player is checked
                //the other player wins
                if (Check)
                {
                    state = current == Color.White ? State.BlackWon : State.WhiteWon;
                }
                //if not checked
                //it's a stalemate
                else
                {
                    state = State.Draw;
                }
                //either way, the game is over
                OnGameOver(this, new GameOverEventArgs(state));
            }
        }

        //returns an IEnumerator so that it is doesn't evaluate all moves
        //when not necessary
        public IEnumerator<Vector> AllValidMoves(Color color)
        {
            foreach (Piece piece in GetPieces(color))
            {
                foreach (Vector move in piece.ValidMoves(this))
                {
                    yield return move;
                }
            }
        }
        public override string ToString()
        {
            String str = "";
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = PieceAt(new Vector(row, col));
                    str += (piece != null) ? piece.ToString() : ". ";
                }
                str += "\n";
            }
            return str;
        }
    }
}