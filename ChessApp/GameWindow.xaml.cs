using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chess;
using Chess.Pieces;

namespace ChessApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        ChessGame game;
        bool selected = false;
        Border selectedSquare;
        Chess.Vector selectedPosition;
        public List<Chess.Vector> markedSquares;
        public SolidColorBrush whiteBrush = new SolidColorBrush(Colors.LightYellow);
        public SolidColorBrush blackBrush = new SolidColorBrush(Colors.DarkRed);
        public SolidColorBrush markedBrush = new SolidColorBrush(Colors.LightGreen);
        public GameWindow()
        {
            InitializeComponent();
            InitializeBoard();
            game = new ChessGame();
            UpdateBoard();
            game.OnGameOver += GameOver;
        }

        public void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Border border = new Border();
                    Viewbox viewbox = new Viewbox();
                    TextBlock textBlock = new TextBlock();
                    border.Child = viewbox;
                    border.Background = (row + col) % 2 == 0 ? whiteBrush : blackBrush;
                    border.MouseDown += OnClick;     
                    viewbox.Child = textBlock;
                    chessBoard.Children.Add(border);
                }
            }
        }

        public void UpdateBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Chess.Vector position = new Chess.Vector(row, col);
                    Piece? piece = game.PieceAt(position);
                    Set(position, piece != null ? Display.GetString(piece) : "");
                }
            }
        }

        public void Set(Chess.Vector position, string str)
        {
            int index = Display.PositionToIndex(position);
            ((TextBlock)((Viewbox)((Border)chessBoard.Children[index]).Child).Child).Text = str;
        }

        public void OnClick(object sender, MouseButtonEventArgs a)
        {
            Border border = (Border)sender;
            int index = chessBoard.Children.IndexOf((System.Windows.UIElement)sender);
            Chess.Vector position = Display.IndexToPosition(index);
            if (selected)
            {
                try
                {
                    game.RequestMove(selectedPosition, position);
                    UpdateBoard();
                }
                catch (InvalidMoveException)
                {
                }
                selected = false;
                UnmarkSquares(markedSquares);
            }
            if (game.PieceAt(position) != null && game.PieceAt(position).color == game.current)
            {
                Select(border, position);
            }
        }

        public void Select(Border border, Chess.Vector position)
        {
            selected = true;
            selectedPosition = position;
            selectedSquare = border;
            MarkSquares(game.PieceAt(position).ValidMoves(game));
        }

        public void MarkSquares(List<Chess.Vector> positions)
        {
            foreach (Chess.Vector position in positions)
            {
                int index = Display.PositionToIndex(position);
                Border square = (Border)chessBoard.Children[index];
                square.Background = markedBrush;
            }
            markedSquares = positions;
        }

        public void UnmarkSquares(List<Chess.Vector> positions)
        {
            foreach (Chess.Vector position in positions)
            {
                int index = Display.PositionToIndex(position);
                Border square = (Border)(chessBoard.Children[index]);
                square.Background = 
                    (position.row + position.col) % 2 == 0 ? whiteBrush : blackBrush;
            }
        }

        public void DisplayValidMoves(Piece piece)
        {
            List<Chess.Vector> validMoves = piece.ValidMoves(game);
            string str = "";
            foreach(Chess.Vector position in validMoves)
            {
                str += position.ToString();
                str += ", ";
            }
            MessageBox.Show(str);
        }

        public void GameOver(object sender, GameOverEventArgs e)
        {
            string message;
            switch (e.state)
            {
                case State.WhiteWon:
                    message = "White won!";
                    break;
                case State.BlackWon:
                    message = "White won!";
                    break;
                case State.Draw:
                    message = "Draw!";
                    break;
                default:
                    message = "";
                    break;
            }
            MessageBox.Show(message);
        }
    }
}
