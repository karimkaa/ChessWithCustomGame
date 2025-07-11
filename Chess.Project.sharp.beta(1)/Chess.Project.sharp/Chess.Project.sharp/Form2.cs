using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.Project.sharp
{
    public partial class Form2 : Form
    {
        private string[,] pieces;
        private int selectedX = -1, selectedY = -1;
        private List<(int, int)> possibleMoves = new List<(int, int)>();
        private bool isWhiteTurn = true;
        private bool gameEnded = false;
        private bool whiteKingMoved = false;
        private bool blackKingMoved = false;
        private bool whiteLeftRookMoved = false;
        private bool whiteRightRookMoved = false;
        private bool blackLeftRookMoved = false;
        private bool blackRightRookMoved = false;

        public Form2()
        {
            InitializeComponent();
            this.Paint += Form2_Paint;
            button1.Click += Button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            this.MouseClick += Form2_MouseClick;
            progressBar1.Click += progressBar1_Click;
            progressBar2.Click += progressBar2_Click;
            InitPieces();
            UpdateProgressBars();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            InitPieces();
            isWhiteTurn = true;
            selectedX = selectedY = -1;
            possibleMoves.Clear();
            listBox1.Items.Clear();
            gameEnded = false;
            UpdateProgressBars();
            this.Invalidate();
        }

        private void InitPieces()
        {
            pieces = new string[8, 8]
            {
                { "♜", "♞", "♝", "♛", "♚", "♝", "♞", "♜" },
                { "♟", "♟", "♟", "♟", "♟", "♟", "♟", "♟" },
                { "",   "",   "",   "",   "",   "",   "",   ""   },
                { "",   "",   "",   "",   "",   "",   "",   ""   },
                { "",   "",   "",   "",   "",   "",   "",   ""   },
                { "",   "",   "",   "",   "",   "",   "",   ""   },
                { "♙", "♙", "♙", "♙", "♙", "♙", "♙", "♙" },
                { "♖", "♘", "♗", "♕", "♔", "♗", "♘", "♖" }
            };
            whiteKingMoved = false;
            blackKingMoved = false;
            whiteLeftRookMoved = false;
            whiteRightRookMoved = false;
            blackLeftRookMoved = false;
            blackRightRookMoved = false;
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameEnded) return;
            int boardSize = 8;
            int cellSize = Math.Min(this.ClientSize.Height, this.ClientSize.Width / 2) / boardSize;
            int offsetX = 0;
            int offsetY = (this.ClientSize.Height - cellSize * boardSize) / 2;
            if (e.X < offsetX || e.Y < offsetY) return;
            int x = (e.X - offsetX) / cellSize;
            int y = (e.Y - offsetY) / cellSize;
            if (x < 0 || x >= 8 || y < 0 || y >= 8) return;

            string piece = pieces[y, x];
            bool isPieceWhite = !string.IsNullOrEmpty(piece) && IsWhite(piece);
            bool isPieceBlack = !string.IsNullOrEmpty(piece) && !IsWhite(piece);

            if (selectedX == -1 && selectedY == -1)
            {
                if (!string.IsNullOrEmpty(piece) && ((isWhiteTurn && isPieceWhite) || (!isWhiteTurn && isPieceBlack)))
                {
                    selectedX = x;
                    selectedY = y;
                    possibleMoves = GetPossibleMoves(y, x, piece);
                }
            }
            else
            {
                if (possibleMoves.Contains((y, x)))
                {
                    // Рокировка
                    if ((pieces[selectedY, selectedX] == "♔" && selectedY == 7 && selectedX == 4 && y == 7 && (x == 6 || x == 2)) ||
                        (pieces[selectedY, selectedX] == "♚" && selectedY == 0 && selectedX == 4 && y == 0 && (x == 6 || x == 2)))
                    {
                        bool isWhite = pieces[selectedY, selectedX] == "♔";
                        // Короткая рокировка
                        if (x == 6)
                        {
                            pieces[y, x] = pieces[selectedY, selectedX];
                            pieces[selectedY, selectedX] = "";
                            pieces[y, 5] = pieces[y, 7];
                            pieces[y, 7] = "";
                            if (isWhite)
                            {
                                whiteKingMoved = true;
                                whiteRightRookMoved = true;
                            }
                            else
                            {
                                blackKingMoved = true;
                                blackRightRookMoved = true;
                            }
                        }
                        // Длинная рокировка
                        else if (x == 2)
                        {
                            pieces[y, x] = pieces[selectedY, selectedX];
                            pieces[selectedY, selectedX] = "";
                            pieces[y, 3] = pieces[y, 0];
                            pieces[y, 0] = "";
                            if (isWhite)
                            {
                                whiteKingMoved = true;
                                whiteLeftRookMoved = true;
                            }
                            else
                            {
                                blackKingMoved = true;
                                blackLeftRookMoved = true;
                            }
                        }
                        isWhiteTurn = !isWhiteTurn;
                        selectedX = selectedY = -1;
                        possibleMoves.Clear();
                        LogEvent($"Рокировка {(isWhite ? "белых" : "чёрных")}");
                        string currentKing = isWhiteTurn ? "♔" : "♚";
                        (int castlingKy, int castlingKx) = FindKing(pieces, currentKing);
                        if (IsSquareAttacked(pieces, castlingKy, castlingKx, !isWhiteTurn))
                            LogEvent($"Шах {currentKing}!");
                        CheckMateOrStalemate();
                        UpdateProgressBars();
                        this.Invalidate();
                        return;
                    }
                    // Обычный ход
                    string moveNotation = $"{pieces[selectedY, selectedX]}: {((char)('a' + selectedX))}{8 - selectedY} → {((char)('a' + x))}{8 - y}";
                    // Флаги для короля и ладей
                    if (pieces[selectedY, selectedX] == "♔")
                        whiteKingMoved = true;
                    if (pieces[selectedY, selectedX] == "♚")
                        blackKingMoved = true;
                    if (pieces[selectedY, selectedX] == "♖" && selectedY == 7 && selectedX == 0)
                        whiteLeftRookMoved = true;
                    if (pieces[selectedY, selectedX] == "♖" && selectedY == 7 && selectedX == 7)
                        whiteRightRookMoved = true;
                    if (pieces[selectedY, selectedX] == "♜" && selectedY == 0 && selectedX == 0)
                        blackLeftRookMoved = true;
                    if (pieces[selectedY, selectedX] == "♜" && selectedY == 0 && selectedX == 7)
                        blackRightRookMoved = true;
                    pieces[y, x] = pieces[selectedY, selectedX];
                    pieces[selectedY, selectedX] = "";
                    isWhiteTurn = !isWhiteTurn;
                    selectedX = selectedY = -1;
                    possibleMoves.Clear();
                    string checkedKing = isWhiteTurn ? "♔" : "♚";
                    (int checkedKy, int checkedKx) = FindKing(pieces, checkedKing);
                    LogEvent($"Ход: {moveNotation}");
                    if (IsSquareAttacked(pieces, checkedKy, checkedKx, !isWhiteTurn))
                        LogEvent($"Шах {checkedKing}!");
                    CheckMateOrStalemate();
                    UpdateProgressBars();
                    this.Invalidate();
                    return;
                }
                else if (!string.IsNullOrEmpty(piece) && ((isWhiteTurn && isPieceWhite) || (!isWhiteTurn && isPieceBlack)))
                {
                    selectedX = x;
                    selectedY = y;
                    possibleMoves = GetPossibleMoves(y, x, piece);
                    this.Invalidate();
                    return;
                }
                else if (selectedX != -1 && selectedY != -1)
                {
                    selectedX = selectedY = -1;
                    possibleMoves.Clear();
                }
            }
            this.Invalidate();
        }

        private List<(int, int)> GetPossibleMoves(int y, int x, string piece)
        {
            var moves = new List<(int, int)>();
            bool isWhite = IsWhite(piece);
            string enemyKing = isWhite ? "♚" : "♔";
            switch (piece)
            {
                case "♙":
                    if (y > 0 && string.IsNullOrEmpty(pieces[y - 1, x])) moves.Add((y - 1, x));
                    if (y == 6 && string.IsNullOrEmpty(pieces[y - 1, x]) && string.IsNullOrEmpty(pieces[y - 2, x])) moves.Add((y - 2, x));
                    if (y > 0 && x > 0 && !string.IsNullOrEmpty(pieces[y - 1, x - 1]) && !IsWhite(pieces[y - 1, x - 1]) && pieces[y - 1, x - 1] != enemyKing) moves.Add((y - 1, x - 1));
                    if (y > 0 && x < 7 && !string.IsNullOrEmpty(pieces[y - 1, x + 1]) && !IsWhite(pieces[y - 1, x + 1]) && pieces[y - 1, x + 1] != enemyKing) moves.Add((y - 1, x + 1));
                    break;
                case "♟":
                    if (y < 7 && string.IsNullOrEmpty(pieces[y + 1, x])) moves.Add((y + 1, x));
                    if (y == 1 && string.IsNullOrEmpty(pieces[y + 1, x]) && string.IsNullOrEmpty(pieces[y + 2, x])) moves.Add((y + 2, x));
                    if (y < 7 && x > 0 && !string.IsNullOrEmpty(pieces[y + 1, x - 1]) && IsWhite(pieces[y + 1, x - 1]) && pieces[y + 1, x - 1] != enemyKing) moves.Add((y + 1, x - 1));
                    if (y < 7 && x < 7 && !string.IsNullOrEmpty(pieces[y + 1, x + 1]) && IsWhite(pieces[y + 1, x + 1]) && pieces[y + 1, x + 1] != enemyKing) moves.Add((y + 1, x + 1));
                    break;
                case "♖": case "♜":
                    moves.AddRange(GetLinearMovesNoKing(y, x, isWhite, new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }, enemyKing));
                    break;
                case "♗": case "♝":
                    moves.AddRange(GetLinearMovesNoKing(y, x, isWhite, new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) }, enemyKing));
                    break;
                case "♕": case "♛":
                    moves.AddRange(GetLinearMovesNoKing(y, x, isWhite, new[] { (1, 0), (-1, 0), (0, 1), (0, -1), (1, 1), (1, -1), (-1, 1), (-1, -1) }, enemyKing));
                    break;
                case "♘": case "♞":
                    foreach (var (dy, dx) in new[] { (-2, -1), (-2, 1), (-1, -2), (-1, 2), (1, -2), (1, 2), (2, -1), (2, 1) })
                    {
                        int ny = y + dy, nx = x + dx;
                        if (ny >= 0 && ny < 8 && nx >= 0 && nx < 8 && (string.IsNullOrEmpty(pieces[ny, nx]) || (IsWhite(pieces[ny, nx]) != isWhite && pieces[ny, nx] != enemyKing)))
                            moves.Add((ny, nx));
                    }
                    break;
                case "♔":
                case "♚":
                    foreach (var (dy, dx) in new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) })
                    {
                        int ny = y + dy, nx = x + dx;
                        if (ny >= 0 && ny < 8 && nx >= 0 && nx < 8 && (string.IsNullOrEmpty(pieces[ny, nx]) || IsWhite(pieces[ny, nx]) != isWhite))
                            moves.Add((ny, nx));
                    }
                    // Рокировка
                    if (isWhite && !whiteKingMoved && y == 7 && x == 4)
                    {
                        // Короткая рокировка
                        if (!whiteRightRookMoved &&
                            pieces[7, 5] == "" && pieces[7, 6] == "" &&
                            pieces[7, 7] == "♖" &&
                            !IsSquareAttacked(pieces, 7, 4, false) &&
                            !IsSquareAttacked(pieces, 7, 5, false) &&
                            !IsSquareAttacked(pieces, 7, 6, false))
                        {
                            moves.Add((7, 6));
                        }
                        // Длинная рокировка
                        if (!whiteLeftRookMoved &&
                            pieces[7, 1] == "" && pieces[7, 2] == "" && pieces[7, 3] == "" &&
                            pieces[7, 0] == "♖" &&
                            !IsSquareAttacked(pieces, 7, 4, false) &&
                            !IsSquareAttacked(pieces, 7, 3, false) &&
                            !IsSquareAttacked(pieces, 7, 2, false))
                        {
                            moves.Add((7, 2));
                        }
                    }
                    else if (!isWhite && !blackKingMoved && y == 0 && x == 4)
                    {
                        // Короткая рокировка
                        if (!blackRightRookMoved &&
                            pieces[0, 5] == "" && pieces[0, 6] == "" &&
                            pieces[0, 7] == "♜" &&
                            !IsSquareAttacked(pieces, 0, 4, true) &&
                            !IsSquareAttacked(pieces, 0, 5, true) &&
                            !IsSquareAttacked(pieces, 0, 6, true))
                        {
                            moves.Add((0, 6));
                        }
                        // Длинная рокировка
                        if (!blackLeftRookMoved &&
                            pieces[0, 1] == "" && pieces[0, 2] == "" && pieces[0, 3] == "" &&
                            pieces[0, 0] == "♜" &&
                            !IsSquareAttacked(pieces, 0, 4, true) &&
                            !IsSquareAttacked(pieces, 0, 3, true) &&
                            !IsSquareAttacked(pieces, 0, 2, true))
                        {
                            moves.Add((0, 2));
                        }
                    }
                    break;
            }
          
            var filtered = new List<(int, int)>();
            foreach (var move in moves)
            {
                if (!MoveLeavesKingInCheck(y, x, move.Item1, move.Item2, isWhite))
                    filtered.Add(move);
            }
            return filtered;
        }

        private IEnumerable<(int, int)> GetLinearMovesNoKing(int y, int x, bool isWhite, (int, int)[] directions, string enemyKing)
        {
            foreach (var (dy, dx) in directions)
            {
                int ny = y + dy, nx = x + dx;
                while (ny >= 0 && ny < 8 && nx >= 0 && nx < 8)
                {
                    if (string.IsNullOrEmpty(pieces[ny, nx]))
                        yield return (ny, nx);
                    else
                    {
                        if (IsWhite(pieces[ny, nx]) != isWhite && pieces[ny, nx] != enemyKing)
                            yield return (ny, nx);
                        break;
                    }
                    ny += dy;
                    nx += dx;
                }
            }
        }

        private bool IsWhite(string piece)
        {
            return piece == "♔" || piece == "♕" || piece == "♖" || piece == "♗" || piece == "♘" || piece == "♙";
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            DrawChessBoard(e.Graphics);
        }

        private void DrawChessBoard(Graphics g)
        {
            int boardSize = 8;
            int cellSize = Math.Min(this.ClientSize.Height, this.ClientSize.Width / 2) / boardSize;
            int boardWidth = cellSize * boardSize;
            int boardHeight = cellSize * boardSize;
            int offsetX = 0;
            int offsetY = (this.ClientSize.Height - boardHeight) / 2;

            bool isWhiteCell;
            for (int y = 0; y < boardSize; y++)
            {
                isWhiteCell = (y % 2 == 0);
                for (int x = 0; x < boardSize; x++)
                {
                    Rectangle rect = new Rectangle(offsetX + x * cellSize, offsetY + y * cellSize, cellSize, cellSize);
                    Brush brush = isWhiteCell ? new SolidBrush(Color.FromArgb(220, 220, 220)) : new SolidBrush(Color.FromArgb(120, 120, 120));
                    g.FillRectangle(brush, rect);
                    g.DrawRectangle(Pens.Black, rect);
                    brush.Dispose();

                    if (possibleMoves.Contains((y, x)))
                    {
                        using (Brush highlight = new SolidBrush(Color.FromArgb(120, Color.Yellow)))
                        {
                            g.FillRectangle(highlight, rect);
                        }
                    }

                    string piece = pieces[y, x];
                    if (!string.IsNullOrEmpty(piece))
                    {
                        Brush pieceBrush = IsWhite(piece) ? Brushes.White : Brushes.Black;
                        StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        using (Font font = new Font("Segoe UI Symbol", cellSize * 0.75f, FontStyle.Bold, GraphicsUnit.Pixel))
                        {
                            g.DrawString(piece, font, pieceBrush, rect, sf);
                        }
                    }
                    isWhiteCell = !isWhiteCell;
                }
            }
            Rectangle boardRect = new Rectangle(offsetX, offsetY, boardWidth, boardHeight);
            using (Pen thickPen = new Pen(Color.Black, 3))
            {
                g.DrawRectangle(thickPen, boardRect);
            }
        }

        private bool MoveLeavesKingInCheck(int fromY, int fromX, int toY, int toX, bool isWhite)
        {
            string[,] temp = (string[,])pieces.Clone();
            temp[toY, toX] = temp[fromY, fromX];
            temp[fromY, fromX] = "";
            var king = isWhite ? "♔" : "♚";
            (int ky, int kx) = FindKing(temp, king);
            return IsSquareAttacked(temp, ky, kx, !isWhite);
        }

        private (int, int) FindKing(string[,] board, string king)
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    if (board[y, x] == king)
                        return (y, x);
            return (-1, -1);
        }

        private bool IsSquareAttacked(string[,] board, int y, int x, bool attackerIsWhite)
        {
            // Пешки
            int dir = attackerIsWhite ? -1 : 1;
            foreach (int dx in new[] { -1, 1 })
            {
                int ny = y + dir, nx = x + dx;
                if (ny >= 0 && ny < 8 && nx >= 0 && nx < 8)
                {
                    string p = board[ny, nx];
                    if ((attackerIsWhite && p == "♙") || (!attackerIsWhite && p == "♟"))
                        return true;
                }
            }
            // Конь
            foreach (var (dy, dx) in new[] { (-2, -1), (-2, 1), (-1, -2), (-1, 2), (1, -2), (1, 2), (2, -1), (2, 1) })
            {
                int ny = y + dy, nx = x + dx;
                if (ny >= 0 && ny < 8 && nx >= 0 && nx < 8)
                {
                    string p = board[ny, nx];
                    if ((attackerIsWhite && p == "♘") || (!attackerIsWhite && p == "♞"))
                        return true;
                }
            }
            // Слон ферзь
            foreach (var (dy, dx) in new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) })
            {
                int ny = y + dy, nx = x + dx;
                while (ny >= 0 && ny < 8 && nx >= 0 && nx < 8)
                {
                    string p = board[ny, nx];
                    if (!string.IsNullOrEmpty(p))
                    {
                        if ((attackerIsWhite && (p == "♗" || p == "♕")) || (!attackerIsWhite && (p == "♝" || p == "♛")))
                            return true;
                        break;
                    }
                    ny += dy;
                    nx += dx;
                }
            }
            // Ладья ферзь
            foreach (var (dy, dx) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
            {
                int ny = y + dy, nx = x + dx;
                while (ny >= 0 && ny < 8 && nx >= 0 && nx < 8)
                {
                    string p = board[ny, nx];
                    if (!string.IsNullOrEmpty(p))
                    {
                        if ((attackerIsWhite && (p == "♖" || p == "♕")) || (!attackerIsWhite && (p == "♜" || p == "♛")))
                            return true;
                        break;
                    }
                    ny += dy;
                    nx += dx;
                }
            }
            // Король
            foreach (var (dy, dx) in new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) })
            {
                int ny = y + dy, nx = x + dx;
                if (ny >= 0 && ny < 8 && nx >= 0 && nx < 8)
                {
                    string p = board[ny, nx];
                    if ((attackerIsWhite && p == "♔") || (!attackerIsWhite && p == "♚"))
                        return true;
                }
            }
            return false;
        }

        private void CheckMateOrStalemate()
        {
            bool enemyIsWhite = isWhiteTurn;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    string piece = pieces[y, x];
                    if (!string.IsNullOrEmpty(piece) && IsWhite(piece) == enemyIsWhite)
                    {
                        var moves = GetPossibleMoves(y, x, piece);
                        if (moves.Count > 0)
                        {
                            string king = enemyIsWhite ? "♔" : "♚";
                            (int ky, int kx) = FindKing(pieces, king);
                            bool inCheck = IsSquareAttacked(pieces, ky, kx, !enemyIsWhite);
                            if (inCheck)
                                LogEvent($"Шах {king}!");
                            return;
                        }
                    }
                }
            }
            string kingFinal = enemyIsWhite ? "♔" : "♚";
            (int kyFinal, int kxFinal) = FindKing(pieces, kingFinal);
            bool inCheckFinal = IsSquareAttacked(pieces, kyFinal, kxFinal, !enemyIsWhite);
            if (inCheckFinal)
            {
                LogEvent($"Мат {kingFinal}!");
                MessageBox.Show($"Мат! Победа {(enemyIsWhite ? "чёрных" : "белых")}", "Мат");
            }
            else
            {
                LogEvent("Пат! Ничья.");
                MessageBox.Show("Пат! Ничья.", "Пат");
            }
            InitPieces();
            isWhiteTurn = true;
            selectedX = selectedY = -1;
            possibleMoves.Clear();
            listBox1.Items.Clear();
            gameEnded = true;
            this.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!gameEnded)
            {
                LogEvent("Белые сдались. Победа черных!");
                gameEnded = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!gameEnded)
            {
                LogEvent("Черные сдались. Победа белых!");
                gameEnded = true;
            }
        }

        private void LogEvent(string message)
        {
            string time = DateTime.Now.ToLongTimeString();
            listBox1.Items.Add($"{message} ({time})");
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        private void UpdateProgressBars()
        {
            if (isWhiteTurn)
            {
                progressBar1.Value = progressBar1.Maximum;
                progressBar2.Value = 0;
            }
            else
            {
                progressBar1.Value = 0;
                progressBar2.Value = progressBar2.Maximum;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            if (isWhiteTurn)
            {
                progressBar1.Value = progressBar1.Maximum;
                progressBar2.Value = 0;
            }
        }

        private void progressBar1_Click_1(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {
            if (!isWhiteTurn)
            {
                progressBar1.Value = 0;
                progressBar2.Value = progressBar2.Maximum;
            }
        }   
    }
}
