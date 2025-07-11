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
    public partial class CustomGame : Form
    {
        private int boardSize;
        private string[,] pieces;
        private bool isGuardian;
        private bool isSpy;
        private int selectedX = -1, selectedY = -1;
        private List<(int, int)> possibleMoves = new List<(int, int)>();
        private bool isWhiteTurn = true;
        private Button[,] boardButtons;
        // Red zone event
        private bool redZoneActive = false;
        private int redZoneTurnsLeft = 0;
        private List<(int, int)> redZoneCells = new List<(int, int)>();
        private bool redZoneEnabled = false;
        private int totalTurns = 0; // Счётчик ходов с начала партии
        private int lastEventTurn = 0; // Последний ход, когда был ивент
        private int nextEventDelay = 10; // Следующий промежуток между ивентами
        private Random eventRandom = new Random(); // Для рандомизации ивентов
        
        // Землетрясение
        private bool earthquakeActive = false;
        private int earthquakeTurnsLeft = 0;
        private List<(int, int)> earthquakeRocks = new List<(int, int)>();
        private bool earthquakeEnabled = false;

        // Вулкан
        private bool volcanoActive = false;
        private int volcanoTurnsLeft = 0;
        private List<(int, int)> volcanoLava = new List<(int, int)>();
        private bool volcanoEnabled = false;

        // Торнадо
        private bool tornadoActive = false;
        private int tornadoturnsLeft = 0;
        private List<(int, int)> tornadoCells = new List<(int, int)>();
        private bool tornadoEnabled = false;

        // Заморозка
        private bool freezingActive = false;
        private int freezingTurnsLeft = 0;
        private Dictionary<(int, int), int> frozenPieces = new Dictionary<(int, int), int>(); // (позиция, оставшиеся ходы)
        private bool freezingEnabled = false;

        // UI для логов
        private ListBox logListBox;
        private Button clearLogsButton;
        private Button restartButton;
        private Button surrenderWhiteButton;
        private Button surrenderBlackButton;

        public CustomGame(int boardSize, bool isGuardian, bool isSpy, bool isEarthquake, bool isVolcano, bool isRedZone = false, bool isTornado = false, bool isFreezing = false)
        {
            this.boardSize = boardSize;
            this.isGuardian = isGuardian;
            this.isSpy = isSpy;
            this.earthquakeEnabled = isEarthquake;
            this.volcanoEnabled = isVolcano;
            this.redZoneEnabled = isRedZone;
            this.tornadoEnabled = isTornado;
            this.freezingEnabled = isFreezing;
            InitializeComponent();
            pieces = new string[boardSize, boardSize];
            InitPieces();
            InitBoardButtons();
            InitLogUI();
            UpdateBoardButtons();
            AddLog("Игра началась!");
        }

        private void CustomGame_Load(object sender, EventArgs e)
        {
            InitPieces();
            InitBoardButtons();
            UpdateBoardButtons();
            // Red zone теперь не запускается сразу
        }

        private void InitPieces()
        {
            // Только шпион (или обе галочки) — 10x10
            if (boardSize == 10)
            {
                pieces = new string[10, 10];
                if (isSpy && !isGuardian)
                {
                    // Только шпион
                    pieces[0, 0] = "♜"; pieces[0, 1] = "♞"; pieces[0, 2] = "⫝"; pieces[0, 3] = "♝"; pieces[0, 4] = "♛"; pieces[0, 5] = "♚"; pieces[0, 6] = "♝"; pieces[0, 7] = "⫝"; pieces[0, 8] = "♞"; pieces[0, 9] = "♜";
                    for (int i = 0; i < 10; i++) pieces[1, i] = "♟";
                    pieces[9, 0] = "♖"; pieces[9, 1] = "♘"; pieces[9, 2] = "⫛"; pieces[9, 3] = "♗"; pieces[9, 4] = "♕"; pieces[9, 5] = "♔"; pieces[9, 6] = "♗"; pieces[9, 7] = "⫛"; pieces[9, 8] = "♘"; pieces[9, 9] = "♖";
                    for (int i = 0; i < 10; i++) pieces[8, i] = "♙";
                }
                else if (isSpy && isGuardian)
                {
                    // Шпион и страж
                    pieces[0, 0] = "♜"; pieces[0, 1] = "♞"; pieces[0, 2] = "⫝"; pieces[0, 3] = "♝"; pieces[0, 4] = "♛"; pieces[0, 5] = "♚"; pieces[0, 6] = "♝"; pieces[0, 7] = "⫝"; pieces[0, 8] = "♞"; pieces[0, 9] = "♜";
                    for (int i = 0; i < 10; i++) pieces[1, i] = "♟";
                    pieces[1, 3] = "⛊"; pieces[1, 6] = "⛊";
                    pieces[9, 0] = "♖"; pieces[9, 1] = "♘"; pieces[9, 2] = "⫛"; pieces[9, 3] = "♗"; pieces[9, 4] = "♕"; pieces[9, 5] = "♔"; pieces[9, 6] = "♗"; pieces[9, 7] = "⫛"; pieces[9, 8] = "♘"; pieces[9, 9] = "♖";
                    for (int i = 0; i < 10; i++) pieces[8, i] = "♙";
                    pieces[8, 3] = "⛨"; pieces[8, 6] = "⛨";
                }
                else
                {
                    // fallback: обычные пустые 10x10
                    for (int y = 0; y < 10; y++)
                        for (int x = 0; x < 10; x++)
                            pieces[y, x] = "";
                }
            }
            // Только страж — 8x8 с заменой пешек перед слонами
            else if (isGuardian && !isSpy && boardSize == 8)
            {
                pieces = new string[8, 8]
                {
                    { "♜", "♞", "♝", "♛", "♚", "♝", "♞", "♜" },
                    { "♟", "♟", "⛊", "♟", "♟", "⛊", "♟", "♟" },
                    { "",   "",   "",   "",   "",   "",   "",   ""   },
                    { "",   "",   "",   "",   "",   "",   "",   ""   },
                    { "",   "",   "",   "",   "",   "",   "",   ""   },
                    { "",   "",   "",   "",   "",   "",   "",   ""   },
                    { "♙", "♙", "⛨", "♙", "♙", "⛨", "♙", "♙" },
                    { "♖", "♘", "♗", "♕", "♔", "♗", "♘", "♖" }
                };
            }
            // Обычные шахматы
            else
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
            }
        }

        private void InitBoardButtons()
        {
            if (boardButtons != null)
            {
                foreach (var btn in boardButtons)
                    if (btn != null) this.Controls.Remove(btn);
            }
            boardButtons = new Button[boardSize, boardSize];
            int cellSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / boardSize;
            int offsetX = (this.ClientSize.Width - cellSize * boardSize) / 2;
            int offsetY = (this.ClientSize.Height - cellSize * boardSize) / 2;
            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    Button cell = new Button();
                    cell.Width = cell.Height = cellSize;
                    cell.Left = offsetX + x * cellSize;
                    cell.Top = offsetY + y * cellSize;
                    cell.FlatStyle = FlatStyle.Flat;
                    cell.Tag = (y, x);
                    cell.Click += Cell_Click;
                    this.Controls.Add(cell);
                    boardButtons[y, x] = cell;
                }
            }
        }

        private void UpdateBoardButtons()
        {
            int cellSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / boardSize;
            int offsetX = (this.ClientSize.Width - cellSize * boardSize) / 2;
            int offsetY = (this.ClientSize.Height - cellSize * boardSize) / 2;
            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    var cell = boardButtons[y, x];
                    cell.Width = cell.Height = cellSize;
                    cell.Left = offsetX + x * cellSize;
                    cell.Top = offsetY + y * cellSize;
                    cell.BackColor = (x + y) % 2 == 0 ? Color.White : Color.LightGray;
                    if (possibleMoves.Contains((y, x)))
                        cell.BackColor = Color.Yellow;
                    if (selectedX == x && selectedY == y)
                        cell.BackColor = Color.LightGreen;
                    if (redZoneActive && redZoneCells.Contains((y, x)))
                        cell.BackColor = Color.LightCoral;
                    if (earthquakeActive && earthquakeRocks.Contains((y, x)))
                        cell.BackColor = Color.Black;
                    if (volcanoActive && volcanoLava.Contains((y, x)))
                        cell.BackColor = Color.Orange;
                    if (tornadoActive && tornadoCells.Contains((y, x)))
                        cell.BackColor = Color.LightGray;
                    if (freezingActive && frozenPieces.ContainsKey((y, x)))
                        cell.BackColor = Color.LightCyan;
                    if (pieces != null && y < pieces.GetLength(0) && x < pieces.GetLength(1) && !string.IsNullOrEmpty(pieces[y, x]))
                    {
                        cell.Text = pieces[y, x];
                        cell.Font = new Font("Segoe UI Symbol", cellSize * 0.6f, FontStyle.Bold, GraphicsUnit.Pixel); // Увеличил размер и сделал жирным
                        
                        // Цветовое кодирование команд
                        if (IsWhite(pieces[y, x]))
                        {
                            cell.ForeColor = Color.DarkBlue; // Белые фигуры - синие
                        }
                        else
                        {
                            cell.ForeColor = Color.DarkRed; // Чёрные фигуры - красные
                        }
                    }
                    else
                    {
                        cell.Text = "";
                    }
                }
            }
        }

        private void DrawBoard() { UpdateBoardButtons(); }

        private void Cell_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var (y, x) = ((int, int))btn.Tag;
            string piece = pieces[y, x];
            bool isPieceWhite = IsWhite(piece);
            bool isPieceBlack = !isPieceWhite;
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
                    string movedPiece = pieces[selectedY, selectedX];
                    string capturedPiece = pieces[y, x];
                    string fromPos = $"{(char)('A' + selectedX)}{selectedY + 1}";
                    string toPos = $"{(char)('A' + x)}{y + 1}";
                    
                    // Особая логика для стража: swap
                    if ((pieces[selectedY, selectedX] == "⛨" || pieces[selectedY, selectedX] == "⛊") && !string.IsNullOrEmpty(pieces[y, x]) && IsWhite(pieces[y, x]) == IsWhite(pieces[selectedY, selectedX]))
                    {
                        string temp = pieces[y, x];
                        pieces[y, x] = pieces[selectedY, selectedX];
                        pieces[selectedY, selectedX] = temp;
                        AddLog($"Страж обменялся с {GetPieceName(temp)}: {fromPos} ↔ {toPos}");
                    }
                    // Особая логика для шпиона: превращение в побитую фигуру
                    else if ((pieces[selectedY, selectedX] == "⫛" || pieces[selectedY, selectedX] == "⫝") && !string.IsNullOrEmpty(pieces[y, x]) && IsWhite(pieces[y, x]) != IsWhite(pieces[selectedY, selectedX]))
                    {
                        string captured = pieces[y, x];
                        bool wasWhite = IsWhite(pieces[selectedY, selectedX]);
                        pieces[y, x] = wasWhite ? ToWhite(captured) : ToBlack(captured);
                        pieces[selectedY, selectedX] = "";
                        AddLog($"Шпион превратился в {GetPieceName(captured)}: {fromPos} → {toPos}");
                    }
                    else
                    {
                        // Превращение пешки с выбором фигуры
                        if ((pieces[selectedY, selectedX] == "♙" && y == 0) || (pieces[selectedY, selectedX] == "♟" && y == pieces.GetLength(0) - 1))
                        {
                            string[] options = IsWhite(pieces[selectedY, selectedX]) ? new[] { "♕", "♖", "♗", "♘" } : new[] { "♛", "♜", "♝", "♞" };
                            string choice = ShowPromotionDialog(options);
                            pieces[y, x] = choice;
                            AddLog($"Пешка превратилась в {GetPieceName(choice)}: {fromPos} → {toPos}");
                        }
                        else
                        {
                            pieces[y, x] = pieces[selectedY, selectedX];
                            if (!string.IsNullOrEmpty(capturedPiece))
                            {
                                AddLog($"{GetPieceName(movedPiece)} взял {GetPieceName(capturedPiece)}: {fromPos} → {toPos}");
                            }
                            else
                            {
                                AddLog($"{GetPieceName(movedPiece)}: {fromPos} → {toPos}");
                            }
                        }
                        pieces[selectedY, selectedX] = "";
                    }
                    isWhiteTurn = !isWhiteTurn;
                    selectedX = selectedY = -1;
                    possibleMoves.Clear();
                    totalTurns++;
                    AddLog($"🔄 Ход {(isWhiteTurn ? "белых" : "чёрных")} (ход #{totalTurns})");
                    
                    // --- Проверка возможности запуска ивента ---
                    bool canStartEvent = totalTurns >= 10 && (totalTurns - lastEventTurn) >= nextEventDelay;
                    
                    // --- Red zone обработка ---
                    if (redZoneActive)
                    {
                        redZoneTurnsLeft--;
                        if (redZoneTurnsLeft == 0)
                        {
                            // Удаляем фигуры, которые остались на красных клетках
                            int removedCount = 0;
                            foreach (var cell in redZoneCells)
                            {
                                if (!string.IsNullOrEmpty(pieces[cell.Item1, cell.Item2]) && pieces[cell.Item1, cell.Item2] != "♔" && pieces[cell.Item1, cell.Item2] != "♚")
                                {
                                    AddLog($"💀 {GetPieceName(pieces[cell.Item1, cell.Item2])} уничтожен в Красной зоне!");
                                    pieces[cell.Item1, cell.Item2] = "";
                                    removedCount++;
                                }
                            }
                            redZoneActive = false;
                            redZoneCells.Clear();
                            AddLog($"🔥 Красная зона завершена! Уничтожено фигур: {removedCount}");
                        }
                    }
                    
                    // --- Землетрясение обработка ---
                    if (earthquakeActive)
                    {
                        earthquakeTurnsLeft--;
                        if (earthquakeTurnsLeft == 0)
                        {
                            // Убираем скалы
                            earthquakeActive = false;
                            earthquakeRocks.Clear();
                            AddLog("🌋 Землетрясение завершено! Скалы исчезли");
                        }
                    }
                    
                    // --- Вулкан обработка ---
                    if (volcanoActive)
                    {
                        volcanoTurnsLeft--;
                        if (volcanoTurnsLeft == 0)
                        {
                            volcanoActive = false;
                            volcanoLava.Clear();
                            AddLog("🌋 Извержение вулкана завершено! Лава остыла");
                        }
                    }
                    
                    // --- Торнадо обработка ---
                    if (tornadoActive)
                    {
                        tornadoturnsLeft--;
                        if (tornadoturnsLeft == 0)
                        {
                            tornadoActive = false;
                            tornadoCells.Clear();
                            AddLog("🌪️ Торнадо утих! Ветер стих");
                        }
                    }
                    
                    // --- Заморозка обработка ---
                    if (freezingActive)
                    {
                        freezingTurnsLeft--;
                        if (freezingTurnsLeft == 0)
                        {
                            freezingActive = false;
                            frozenPieces.Clear();
                            AddLog("❄️ Заморозка отступила! Фигуры оттаяли");
                        }
                    }
                    
                    // --- Запуск случайного ивента ---
                    if (canStartEvent && !redZoneActive && !earthquakeActive && !volcanoActive && !tornadoActive && !freezingActive)
                    {
                        var availableEvents = new List<string>();
                        if (redZoneEnabled) availableEvents.Add("redzone");
                        if (earthquakeEnabled) availableEvents.Add("earthquake");
                        if (volcanoEnabled) availableEvents.Add("volcano");
                        if (tornadoEnabled) availableEvents.Add("tornado");
                        if (freezingEnabled) availableEvents.Add("freezing");
                        
                        if (availableEvents.Count > 0)
                        {
                            // Выбираем случайный ивент из доступных
                            string selectedEvent = availableEvents[eventRandom.Next(availableEvents.Count)];
                            
                            switch (selectedEvent)
                            {
                                case "redzone":
                                    StartRedZoneEvent();
                                    AddLog("🔥 Активирована Красная зона!");
                                    break;
                                case "earthquake":
                                    StartEarthquakeEvent();
                                    AddLog("🌋 Активировано Землетрясение!");
                                    break;
                                case "volcano":
                                    StartVolcanoEvent();
                                    AddLog("🌋 Активировано Извержение вулкана!");
                                    break;
                                case "tornado":
                                    StartTornadoEvent();
                                    AddLog("🌪️ Активирован Торнадо!");
                                    break;
                                case "freezing":
                                    StartFreezingEvent();
                                    AddLog("❄️ Активирована Заморозка!");
                                    break;
                            }
                            
                            lastEventTurn = totalTurns;
                            nextEventDelay = eventRandom.Next(10, 16); // Случайный промежуток 10-15 ходов
                        }
                    }
                    
                    CheckMateOrStalemate();
                }
                else if (!string.IsNullOrEmpty(piece) && ((isWhiteTurn && isPieceWhite) || (!isWhiteTurn && isPieceBlack)))
                {
                    selectedX = x;
                    selectedY = y;
                    possibleMoves = GetPossibleMoves(y, x, piece);
                }
                else if (selectedX != -1 && selectedY != -1)
                {
                    selectedX = selectedY = -1;
                    possibleMoves.Clear();
                }
            }
            UpdateBoardButtons();
        }

        private List<(int, int)> GetPossibleMoves(int y, int x, string piece)
        {
            var moves = new List<(int, int)>();
            bool isWhite = IsWhite(piece);
            string enemyKing = isWhite ? "♚" : "♔";
            int N = pieces.GetLength(0);
            // Если фигура стоит на лаве — не может ходить
            if (volcanoActive && volcanoLava.Contains((y, x)))
                return moves;
            // Если фигура заморожена — не может ходить
            if (freezingActive && frozenPieces.ContainsKey((y, x)))
                return moves;
            switch (piece)
            {
                case "♙":
                    if (y > 0 && string.IsNullOrEmpty(pieces[y - 1, x]) && !(earthquakeActive && earthquakeRocks.Contains((y - 1, x))) && !(volcanoActive && volcanoLava.Contains((y - 1, x)))) moves.Add((y - 1, x));
                    if (y == N - 2 && string.IsNullOrEmpty(pieces[y - 1, x]) && string.IsNullOrEmpty(pieces[y - 2, x]) && 
                        !(earthquakeActive && earthquakeRocks.Contains((y - 1, x))) && !(earthquakeActive && earthquakeRocks.Contains((y - 2, x))) &&
                        !(volcanoActive && volcanoLava.Contains((y - 1, x))) && !(volcanoActive && volcanoLava.Contains((y - 2, x)))) moves.Add((y - 2, x));
                    if (y > 0 && x > 0 && !string.IsNullOrEmpty(pieces[y - 1, x - 1]) && !IsWhite(pieces[y - 1, x - 1]) && pieces[y - 1, x - 1] != enemyKing && 
                        !(earthquakeActive && earthquakeRocks.Contains((y - 1, x - 1))) && !(volcanoActive && volcanoLava.Contains((y - 1, x - 1)))) moves.Add((y - 1, x - 1));
                    if (y > 0 && x < N - 1 && !string.IsNullOrEmpty(pieces[y - 1, x + 1]) && !IsWhite(pieces[y - 1, x + 1]) && pieces[y - 1, x + 1] != enemyKing && 
                        !(earthquakeActive && earthquakeRocks.Contains((y - 1, x + 1))) && !(volcanoActive && volcanoLava.Contains((y - 1, x + 1)))) moves.Add((y - 1, x + 1));
                    break;
                case "♟":
                    if (y < N - 1 && string.IsNullOrEmpty(pieces[y + 1, x]) && !(earthquakeActive && earthquakeRocks.Contains((y + 1, x))) && !(volcanoActive && volcanoLava.Contains((y + 1, x)))) moves.Add((y + 1, x));
                    if (y == 1 && string.IsNullOrEmpty(pieces[y + 1, x]) && string.IsNullOrEmpty(pieces[y + 2, x]) && 
                        !(earthquakeActive && earthquakeRocks.Contains((y + 1, x))) && !(earthquakeActive && earthquakeRocks.Contains((y + 2, x))) &&
                        !(volcanoActive && volcanoLava.Contains((y + 1, x))) && !(volcanoActive && volcanoLava.Contains((y + 2, x)))) moves.Add((y + 2, x));
                    if (y < N - 1 && x > 0 && !string.IsNullOrEmpty(pieces[y + 1, x - 1]) && IsWhite(pieces[y + 1, x - 1]) && pieces[y + 1, x - 1] != enemyKing && 
                        !(earthquakeActive && earthquakeRocks.Contains((y + 1, x - 1))) && !(volcanoActive && volcanoLava.Contains((y + 1, x - 1)))) moves.Add((y + 1, x - 1));
                    if (y < N - 1 && x < N - 1 && !string.IsNullOrEmpty(pieces[y + 1, x + 1]) && IsWhite(pieces[y + 1, x + 1]) && pieces[y + 1, x + 1] != enemyKing && 
                        !(earthquakeActive && earthquakeRocks.Contains((y + 1, x + 1))) && !(volcanoActive && volcanoLava.Contains((y + 1, x + 1)))) moves.Add((y + 1, x + 1));
                    break;
                case "♖": case "♜":
                    moves.AddRange(GetLinearMovesNoKingVolcano(y, x, isWhite, new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }, enemyKing));
                    break;
                case "♗": case "♝":
                    moves.AddRange(GetLinearMovesNoKingVolcano(y, x, isWhite, new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) }, enemyKing));
                    break;
                case "♕": case "♛":
                    moves.AddRange(GetLinearMovesNoKingVolcano(y, x, isWhite, new[] { (1, 0), (-1, 0), (0, 1), (0, -1), (1, 1), (1, -1), (-1, 1), (-1, -1) }, enemyKing));
                    break;
                case "♘": case "♞":
                    foreach (var (dy, dx) in new[] { (-2, -1), (-2, 1), (-1, -2), (-1, 2), (1, -2), (1, 2), (2, -1), (2, 1) })
                    {
                        int ny = y + dy, nx = x + dx;
                        if (ny >= 0 && ny < N && nx >= 0 && nx < N && 
                            !(earthquakeActive && earthquakeRocks.Contains((ny, nx))) &&
                            !(volcanoActive && volcanoLava.Contains((ny, nx))) &&
                            (string.IsNullOrEmpty(pieces[ny, nx]) || (IsWhite(pieces[ny, nx]) != isWhite && pieces[ny, nx] != enemyKing)))
                            moves.Add((ny, nx));
                    }
                    break;
                case "♔": case "♚":
                    foreach (var (dy, dx) in new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) })
                    {
                        int ny = y + dy, nx = x + dx;
                        if (ny >= 0 && ny < N && nx >= 0 && nx < N && 
                            !(earthquakeActive && earthquakeRocks.Contains((ny, nx))) &&
                            !(volcanoActive && volcanoLava.Contains((ny, nx))) &&
                            (string.IsNullOrEmpty(pieces[ny, nx]) || IsWhite(pieces[ny, nx]) != isWhite))
                        {
                            // Проверка: не под шахом ли эта клетка
                            if (!IsSquareAttacked(pieces, ny, nx, !isWhite))
                                moves.Add((ny, nx));
                        }
                    }
                    break;
                case "⛨": // Страж (белый)
                case "⛊": // Страж (чёрный)
                    foreach (var (dy, dx) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
                    {
                        for (int step = 1; step <= 2; step++)
                        {
                            int ny = y + dy * step, nx = x + dx * step;
                            if (ny < 0 || ny >= N || nx < 0 || nx >= N) break;
                            if (earthquakeActive && earthquakeRocks.Contains((ny, nx))) break;
                            if (volcanoActive && volcanoLava.Contains((ny, nx))) break;
                            if (string.IsNullOrEmpty(pieces[ny, nx]))
                            {
                                moves.Add((ny, nx));
                            }
                            else if (IsWhite(pieces[ny, nx]) == isWhite && !(ny == y && nx == x))
                            {
                                moves.Add((ny, nx));
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case "⫛": // Шпион (белый)
                case "⫝": // Шпион (чёрный)
                    foreach (var (dy, dx) in new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) })
                    {
                        for (int step = 1; step <= 3; step++)
                        {
                            int ny = y + dy * step, nx = x + dx * step;
                            if (ny < 0 || ny >= N || nx < 0 || nx >= N) break;
                            if (earthquakeActive && earthquakeRocks.Contains((ny, nx))) break;
                            if (volcanoActive && volcanoLava.Contains((ny, nx))) break;
                            if (string.IsNullOrEmpty(pieces[ny, nx]))
                            {
                                moves.Add((ny, nx));
                            }
                            else if (!string.IsNullOrEmpty(pieces[ny, nx]) && IsWhite(pieces[ny, nx]) != isWhite && pieces[ny, nx] != enemyKing)
                            {
                                moves.Add((ny, nx));
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
            return moves;
        }

        // Для вулкана: линейные ходы с учётом лавы
        private IEnumerable<(int, int)> GetLinearMovesNoKingVolcano(int y, int x, bool isWhite, (int, int)[] directions, string enemyKing)
        {
            int N = pieces.GetLength(0);
            foreach (var (dy, dx) in directions)
            {
                int ny = y + dy, nx = x + dx;
                while (ny >= 0 && ny < N && nx >= 0 && nx < N)
                {
                    if (earthquakeActive && earthquakeRocks.Contains((ny, nx)))
                        break;
                    if (volcanoActive && volcanoLava.Contains((ny, nx)))
                        break;
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
            // Для стража и шпиона: определяем цвет по символу
            return piece == "♔" || piece == "♕" || piece == "♖" || piece == "♗" || piece == "♘" || piece == "♙" || piece == "⛨" || piece == "⫛";
        }

        // Проверка шаха, мата, ничьи (адаптировано под любое поле)
        private void CheckMateOrStalemate()
        {
            bool enemyIsWhite = isWhiteTurn;
            for (int y = 0; y < pieces.GetLength(0); y++)
            {
                for (int x = 0; x < pieces.GetLength(1); x++)
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
                            {
                                AddLog($"⚡ Шах {(enemyIsWhite ? "белому" : "чёрному")} королю!");
                                MessageBox.Show($"Шах {king}!");
                            }
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
                string winner = enemyIsWhite ? "чёрных" : "белых";
                AddLog($"👑 Мат! Победа {winner}!");
                MessageBox.Show($"Мат! Победа {winner}. Был поставлен мат!", "Мат");
            }
            else
            {
                AddLog("🤝 Пат! Ничья.");
                MessageBox.Show("Пат! Ничья.", "Пат");
            }
            
            // Перезапуск игры
            RestartGameInternal();
            AddLog("🔄 Игра перезапущена после окончания партии");
        }

        private (int, int) FindKing(string[,] board, string king)
        {
            for (int y = 0; y < board.GetLength(0); y++)
                for (int x = 0; x < board.GetLength(1); x++)
                    if (board[y, x] == king)
                        return (y, x);
            return (-1, -1);
        }

        private bool IsSquareAttacked(string[,] board, int y, int x, bool attackerIsWhite)
        {
            int N = board.GetLength(0);
            // Пешки
            int dir = attackerIsWhite ? -1 : 1;
            foreach (int dx in new[] { -1, 1 })
            {
                int ny = y + dir, nx = x + dx;
                if (ny >= 0 && ny < N && nx >= 0 && nx < N)
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
                if (ny >= 0 && ny < N && nx >= 0 && nx < N)
                {
                    string p = board[ny, nx];
                    if ((attackerIsWhite && p == "♘") || (!attackerIsWhite && p == "♞"))
                        return true;
                }
            }
            // Слон, ферзь
            foreach (var (dy, dx) in new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) })
            {
                int ny = y + dy, nx = x + dx;
                while (ny >= 0 && ny < N && nx >= 0 && nx < N)
                {
                    // Проверяем, не скала ли это
                    if (earthquakeActive && earthquakeRocks.Contains((ny, nx)))
                        break;
                    
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
            // Ладья, ферзь
            foreach (var (dy, dx) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
            {
                int ny = y + dy, nx = x + dx;
                while (ny >= 0 && ny < N && nx >= 0 && nx < N)
                {
                    // Проверяем, не скала ли это
                    if (earthquakeActive && earthquakeRocks.Contains((ny, nx)))
                        break;
                    
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
                if (ny >= 0 && ny < N && nx >= 0 && nx < N)
                {
                    string p = board[ny, nx];
                    if ((attackerIsWhite && p == "♔") || (!attackerIsWhite && p == "♚"))
                        return true;
                }
            }
            return false;
        }

        // Диалог выбора фигуры для превращения пешки
        private string ShowPromotionDialog(string[] options)
        {
            Form form = new Form();
            form.Text = "Выберите фигуру";
            form.Width = 300;
            form.Height = 100;
            FlowLayoutPanel panel = new FlowLayoutPanel { Dock = DockStyle.Fill };
            string result = options[0];
            foreach (var opt in options)
            {
                Button b = new Button { Text = opt, Font = new Font("Segoe UI Symbol", 24), Width = 60, Height = 60 };
                b.Click += (s, e) => { result = opt; form.DialogResult = DialogResult.OK; form.Close(); };
                panel.Controls.Add(b);
            }
            form.Controls.Add(panel);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
            return result;
        }

        // Преобразование символа в чёрную фигуру (для превращения шпиона)
        private string ToBlack(string piece)
        {
            switch (piece)
            {
                case "♕": return "♛";
                case "♖": return "♜";
                case "♗": return "♝";
                case "♘": return "♞";
                case "♙": return "♟";
                case "♔": return "♚";
                case "⛨": return "⛊";
                case "⫛": return "⫝";
                default: return piece;
            }
        }
        // Преобразование символа в белую фигуру (для превращения шпиона)
        private string ToWhite(string piece)
        {
            switch (piece)
            {
                case "♛": return "♕";
                case "♜": return "♖";
                case "♝": return "♗";
                case "♞": return "♘";
                case "♟": return "♙";
                case "♚": return "♔";
                case "⛊": return "⛨";
                case "⫝": return "⫛";
                default: return piece;
            }
        }

        private bool Form3RedZoneEnabled()
        {
            // Попробуем найти Form3 и узнать состояние checkBox1 (Red zone)
            // Для простоты: всегда активируем, если нужно — доработать передачу состояния
            return true; // TODO: заменить на реальную проверку чекбокса
        }

        private void StartRedZoneEvent()
        {
            redZoneActive = true;
            redZoneTurnsLeft = 4; // 4 хода
            NextRedZoneCells(); // выбираем клетки один раз
        }

        private void NextRedZoneCells()
        {
            redZoneCells.Clear();
            var rand = new Random();
            int N = pieces.GetLength(0);
            var allCells = new List<(int, int)>();
            for (int y = 0; y < N; y++)
                for (int x = 0; x < N; x++)
                    if ((pieces[y, x] == null || pieces[y, x] != "♔" && pieces[y, x] != "♚"))
                        allCells.Add((y, x));
            int count = Math.Min(5, allCells.Count);
            while (redZoneCells.Count < count && allCells.Count > 0)
            {
                int idx = rand.Next(allCells.Count);
                redZoneCells.Add(allCells[idx]);
                allCells.RemoveAt(idx);
            }
        }

        private void StartEarthquakeEvent()
        {
            // Проверяем, есть ли место для скалы
            if (CanPlaceRock())
            {
                earthquakeActive = true;
                earthquakeTurnsLeft = 6; // 6 ходов
                PlaceRock();
            }
        }

        private bool CanPlaceRock()
        {
            int N = pieces.GetLength(0);
            // Ищем место для скалы (2 клетки по ширине)
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N - 1; x++) // -1 чтобы поместились 2 клетки
                {
                    if (string.IsNullOrEmpty(pieces[y, x]) && string.IsNullOrEmpty(pieces[y, x + 1]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void PlaceRock()
        {
            earthquakeRocks.Clear();
            int N = pieces.GetLength(0);
            var rand = new Random();
            var availablePositions = new List<(int, int)>();
            
            // Собираем все доступные позиции для скалы
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N - 1; x++)
                {
                    if (string.IsNullOrEmpty(pieces[y, x]) && string.IsNullOrEmpty(pieces[y, x + 1]))
                    {
                        availablePositions.Add((y, x));
                    }
                }
            }
            
            if (availablePositions.Count > 0)
            {
                // Выбираем случайную позицию
                int idx = rand.Next(availablePositions.Count);
                var (y, x) = availablePositions[idx];
                
                // Размещаем скалу (2 клетки)
                earthquakeRocks.Add((y, x));
                earthquakeRocks.Add((y, x + 1));
            }
        }

        private void StartVolcanoEvent()
        {
            if (CanPlaceLava())
            {
                volcanoActive = true;
                volcanoTurnsLeft = 5; // 5 ходов
                PlaceLava();
            }
        }

        private bool CanPlaceLava()
        {
            int N = pieces.GetLength(0);
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N - 2; x++) // -2 чтобы поместились 3 клетки
                {
                    if (string.IsNullOrEmpty(pieces[y, x]) && string.IsNullOrEmpty(pieces[y, x + 1]) && string.IsNullOrEmpty(pieces[y, x + 2]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void PlaceLava()
        {
            volcanoLava.Clear();
            int N = pieces.GetLength(0);
            var rand = new Random();
            var availablePositions = new List<(int, int)>();
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N - 2; x++)
                {
                    if (string.IsNullOrEmpty(pieces[y, x]) && string.IsNullOrEmpty(pieces[y, x + 1]) && string.IsNullOrEmpty(pieces[y, x + 2]))
                    {
                        availablePositions.Add((y, x));
                    }
                }
            }
            if (availablePositions.Count > 0)
            {
                int idx = rand.Next(availablePositions.Count);
                var (y, x) = availablePositions[idx];
                volcanoLava.Add((y, x));
                volcanoLava.Add((y, x + 1));
                volcanoLava.Add((y, x + 2));
            }
        }

        private void InitLogUI()
        {
            // Создаём ListBox для логов (увеличенный размер)
            logListBox = new ListBox();
            logListBox.Location = new Point(10, 10);
            logListBox.Size = new Size(400, 300); // Увеличил размер
            logListBox.Font = new Font("Consolas", 10, FontStyle.Bold); // Увеличил шрифт и сделал жирным
            logListBox.BackColor = Color.LightGray; // Светлый фон
            logListBox.ForeColor = Color.Black; // Чёрный текст
            this.Controls.Add(logListBox);
            
            // Кнопка очистки логов
            clearLogsButton = new Button();
            clearLogsButton.Text = "Очистить логи";
            clearLogsButton.Location = new Point(10, 320);
            clearLogsButton.Size = new Size(120, 35); // Увеличил размер
            clearLogsButton.Font = new Font("Arial", 10, FontStyle.Bold); // Крупный жирный шрифт
            clearLogsButton.Click += ClearLogs;
            this.Controls.Add(clearLogsButton);
            
            // Кнопка рестарта
            restartButton = new Button();
            restartButton.Text = "🔄 Рестарт";
            restartButton.Location = new Point(10, 365);
            restartButton.Size = new Size(120, 35); // Увеличил размер
            restartButton.Font = new Font("Arial", 10, FontStyle.Bold); // Крупный жирный шрифт
            restartButton.Click += RestartGame;
            this.Controls.Add(restartButton);
            
            // Кнопка сдачи белых
            surrenderWhiteButton = new Button();
            surrenderWhiteButton.Text = "🏳️ Сдаться (Б)";
            surrenderWhiteButton.Location = new Point(10, 410);
            surrenderWhiteButton.Size = new Size(120, 35); // Увеличил размер
            surrenderWhiteButton.Font = new Font("Arial", 10, FontStyle.Bold); // Крупный жирный шрифт
            surrenderWhiteButton.Click += SurrenderWhite;
            this.Controls.Add(surrenderWhiteButton);
            
            // Кнопка сдачи чёрных
            surrenderBlackButton = new Button();
            surrenderBlackButton.Text = "🏴 Сдаться (Ч)";
            surrenderBlackButton.Location = new Point(10, 455);
            surrenderBlackButton.Size = new Size(120, 35); // Увеличил размер
            surrenderBlackButton.Font = new Font("Arial", 10, FontStyle.Bold); // Крупный жирный шрифт
            surrenderBlackButton.Click += SurrenderBlack;
            this.Controls.Add(surrenderBlackButton);
            
            // Увеличиваем размер окна для логов и кнопок
            this.Size = new Size(this.Size.Width + 420, this.Size.Height + 50);
        }
        
        private void AddLog(string message)
        {
            if (logListBox.InvokeRequired)
            {
                logListBox.Invoke(new Action(() => AddLog(message)));
                return;
            }
            
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string logEntry = $"[{timestamp}] {message}";
            logListBox.Items.Add(logEntry);
            logListBox.SelectedIndex = logListBox.Items.Count - 1; // Прокрутка к последнему элементу
        }
        
        private void ClearLogs(object sender, EventArgs e)
        {
            logListBox.Items.Clear();
            AddLog("Логи очищены");
        }

        private string GetPieceName(string piece)
        {
            switch (piece)
            {
                case "♔": return "Белый король";
                case "♚": return "Чёрный король";
                case "♕": return "Белая королева";
                case "♛": return "Чёрная королева";
                case "♖": return "Белая ладья";
                case "♜": return "Чёрная ладья";
                case "♗": return "Белый слон";
                case "♝": return "Чёрный слон";
                case "♘": return "Белый конь";
                case "♞": return "Чёрный конь";
                case "♙": return "Белая пешка";
                case "♟": return "Чёрная пешка";
                case "⛨": return "Белый страж";
                case "⛊": return "Чёрный страж";
                case "⫛": return "Белый шпион";
                case "⫝": return "Чёрный шпион";
                default: return "Неизвестная фигура";
            }
        }

        private void RestartGame(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите перезапустить игру?", "Подтверждение рестарта", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                AddLog("🔄 Игра перезапущена по запросу игрока");
                RestartGameInternal();
            }
        }
        
        private void SurrenderWhite(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Белые сдаются? Это приведёт к победе чёрных.", "Подтверждение сдачи белых", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                AddLog("🏳️ Белые сдались! Победа чёрных!");
                MessageBox.Show("Белые сдались! Победа чёрных!", "Сдача");
                RestartGameInternal();
            }
        }
        
        private void SurrenderBlack(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Чёрные сдаются? Это приведёт к победе белых.", "Подтверждение сдачи чёрных", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                AddLog("🏴 Чёрные сдались! Победа белых!");
                MessageBox.Show("Чёрные сдались! Победа белых!", "Сдача");
                RestartGameInternal();
            }
        }
        
        private void RestartGameInternal()
        {
            // Сброс всех ивентов
            redZoneActive = false;
            redZoneCells.Clear();
            earthquakeActive = false;
            earthquakeRocks.Clear();
            volcanoActive = false;
            volcanoLava.Clear();
            tornadoActive = false;
            tornadoCells.Clear();
            freezingActive = false;
            frozenPieces.Clear();
            
            // Сброс игрового состояния
            totalTurns = 0;
            lastEventTurn = 0;
            nextEventDelay = 10;
            isWhiteTurn = true;
            selectedX = selectedY = -1;
            possibleMoves.Clear();
            
            // Переинициализация поля
            InitPieces();
            UpdateBoardButtons();
        }

        private void StartTornadoEvent()
        {
            if (CanPlaceTornado())
            {
                tornadoActive = true;
                tornadoturnsLeft = 3; // 3 хода
                PlaceTornado();
                ScatterPieces(); // Разбрасываем фигуры
            }
        }

        private bool CanPlaceTornado()
        {
            int N = pieces.GetLength(0);
            // Нужно найти 3 пустые клетки
            int emptyCells = 0;
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (string.IsNullOrEmpty(pieces[y, x]))
                        emptyCells++;
                }
            }
            return emptyCells >= 3;
        }

        private void PlaceTornado()
        {
            tornadoCells.Clear();
            int N = pieces.GetLength(0);
            var rand = new Random();
            var availablePositions = new List<(int, int)>();
            
            // Собираем все пустые позиции
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (string.IsNullOrEmpty(pieces[y, x]))
                    {
                        availablePositions.Add((y, x));
                    }
                }
            }
            
            // Выбираем 3 случайные позиции
            int count = Math.Min(3, availablePositions.Count);
            for (int i = 0; i < count; i++)
            {
                int idx = rand.Next(availablePositions.Count);
                tornadoCells.Add(availablePositions[idx]);
                availablePositions.RemoveAt(idx);
            }
        }

        private void ScatterPieces()
        {
            int N = pieces.GetLength(0);
            var rand = new Random();
            var scatteredPieces = new List<(string piece, int fromY, int fromX)>();
            
            // Собираем все фигуры в радиусе 1 клетки от торнадо (уменьшил с 2 до 1)
            foreach (var tornadoCell in tornadoCells)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        int y = tornadoCell.Item1 + dy;
                        int x = tornadoCell.Item2 + dx;
                        
                        if (y >= 0 && y < N && x >= 0 && x < N && 
                            !string.IsNullOrEmpty(pieces[y, x]) &&
                            pieces[y, x] != "♔" && pieces[y, x] != "♚" && // Короли не разбрасываются
                            rand.Next(100) < 50) // 50% шанс разбросать фигуру (делает эффект менее мощным)
                        {
                            scatteredPieces.Add((pieces[y, x], y, x));
                            pieces[y, x] = "";
                        }
                    }
                }
            }
            
            // Разбрасываем фигуры в случайные пустые клетки
            foreach (var (piece, fromY, fromX) in scatteredPieces)
            {
                var availablePositions = new List<(int, int)>();
                for (int y = 0; y < N; y++)
                {
                    for (int x = 0; x < N; x++)
                    {
                        if (string.IsNullOrEmpty(pieces[y, x]))
                        {
                            availablePositions.Add((y, x));
                        }
                    }
                }
                
                if (availablePositions.Count > 0)
                {
                    int idx = rand.Next(availablePositions.Count);
                    var (toY, toX) = availablePositions[idx];
                    pieces[toY, toX] = piece;
                    
                    string fromPos = $"{(char)('A' + fromX)}{fromY + 1}";
                    string toPos = $"{(char)('A' + toX)}{toY + 1}";
                    AddLog($"🌪️ {GetPieceName(piece)} унесён торнадо: {fromPos} → {toPos}");
                }
            }
        }

        private void StartFreezingEvent()
        {
            freezingActive = true;
            freezingTurnsLeft = 5; // 5 ходов
            FreezeRandomPieces();
        }

        private void FreezeRandomPieces()
        {
            int N = pieces.GetLength(0);
            var rand = new Random();
            frozenPieces.Clear();
            
            // Проходим по всем фигурам на поле
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (!string.IsNullOrEmpty(pieces[y, x]) && 
                        pieces[y, x] != "♔" && pieces[y, x] != "♚" && // Короли не замораживаются
                        rand.Next(100) < 30) // 30% шанс заморозить фигуру
                    {
                        frozenPieces[(y, x)] = 5; // 5 ходов заморозки
                        AddLog($"❄️ {GetPieceName(pieces[y, x])} заморожен!");
                    }
                }
            }
        }
    }
}
