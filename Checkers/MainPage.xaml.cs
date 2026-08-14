using Checkers.Checker;
using Checkers.Elements;
using Checkers.Game;
using Checkers.Player;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Checkers;

public partial class MainPage : ContentPage
{
    private CheckerObject? selectedChecker;
    private BoardElement boardElement = new();
    private UserPlayer user = new();
    private OpponentPlayer opponent = new();
    private bool FirstLoad = true;
    private string commandMessage = string.Empty;
    public static MainPage? Instance { get; private set; }

    public ObservableCollection<string> LogEntries { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        Instance = this;
        GameLogic.OnLogicChanged += CheckLogicChanges;
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        if (FirstLoad)
        {
            PrepareBoard(canvas);
            FirstLoad = false;
        }

        UpdateElements();
        DrawElements(canvas);
    }

    private void DrawElements(SKCanvas canvas)
    {
        boardElement.Draw(canvas);

        opponent.Draw(canvas);
        user.Draw(canvas);
    }

    private void UpdateElements()
    {
        user.DeleteAttacked();
        opponent.DeleteAttacked();
        var gameEnd = false;

        if((user.checkers.Count == 0 || opponent.checkers.Count == 0)) {
            gameEnd = true;
        }
        if (user.Starts != null)
        {
            if (!gameEnd)
            {
                var points = GetAllCheckers();
                if (points != null)
                {
                    boardElement.UpdateState(points);
                }
            }
            else if (gameEnd)
            {
                var winner = user.checkers.Count > 0 ? "User" : "Opponent";
                EndGame(winner);
            }
        }
    }

    private void EndGame(string winner)
    {
        var logString = $"Game Over! Winner: {winner}";
        LogEntries.Add(logString);
        user.Reset();
        opponent.Reset();
        FirstLoad = true;
        boardElement = new();
        if (user.isServer)
        {
            RestartButton.IsEnabled = true;
        }
        GameLogic.StartGame();
    }

    public void SetSide(bool isUserStarts)
    {
        user.Starts = GameLogic.Starts.HasValue && GameLogic.Starts.Value;
        opponent.Starts = GameLogic.Starts.HasValue && !GameLogic.Starts.Value;
    }

    public async void CheckLogicChanges()
    {
        if (GameLogic.GameStart)
        {
            user.IsTurn = GameLogic.WhiteTurn == user.Starts;
            opponent.IsTurn = GameLogic.WhiteTurn == opponent.Starts;

            canvas.InvalidateSurface();

            if (!user.IsTurn)
            {
                await user.WaitForMessage();
            }
        }
    }

    private void PrepareBoard(SKCanvas canvas)
    {
        using (var paint = new SKPaint { Color = SKColors.Black })
        {
            for (int row = 0; row < boardElement.boardHeight; row += boardElement.rectHeight)
            {
                for (int col = 0; col < boardElement.boardWidth; col += boardElement.rectWidth)
                {
                    bool isRowEven = (row / boardElement.rectHeight % 2 == 0);
                    bool isColEven = (col / boardElement.rectWidth % 2 == 0);

                    if ((isRowEven && !isColEven) || (!isRowEven && isColEven))
                    {
                        var boardCol = col / boardElement.rectWidth;
                        var boardRow = row / boardElement.rectWidth;
                        var cord = new BoardCordElement(boardCol, boardRow);
                        var squarePoint = new Point(col, row);
                        var baseElement = new BaseElement()
                        {
                            Point = squarePoint,
                            Cord = cord
                        };
                        boardElement.AddRect(baseElement);

                        var sqRadius = boardElement.rectWidth / 2;
                        var checkerPoint = new Point(col + sqRadius, row + sqRadius);
                        var radius = sqRadius - 5;
                        var ccolor = SKColors.Transparent;

                        if ((row < (boardElement.rectHeight * 3)))
                        {
                            var element = new ElementClass(checkerPoint, cord, radius, ccolor);
                            opponent.AddChecker(element, false);
                        }
                        else if ((row >= 5 * boardElement.rectHeight))
                        {
                            var element = new ElementClass(checkerPoint, cord, radius, ccolor);
                            user.AddChecker(element, true);
                        }
                    }
                }
            }
        }
    }

    private List<BasicChecker> GetAllCheckers()
    {
        var points = opponent.GetPoints();
        points.AddRange(user.GetPoints());

        return points;
    }

    private async void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        float x = e.Location.X;
        float y = e.Location.Y;
        Point touch = new Point(x, y);
        CoordinateLabel.Text = $"X: {x:F2}, Y: {y:F2}";

        if (user.IsTurn && GameLogic.GameStart)
        {
            user.ClearStatus();
            CheckAttacking();
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    selectedChecker = user.CheckPosition(touch);
                    if (selectedChecker != null)
                    {
                        if(user.checkers.Any(c => c.basic.IsAttacking) && !selectedChecker.basic.IsAttacking)
                        {
                            selectedChecker = null;
                            break;
                        }

                        selectedChecker.basic.IsSelected = true;
                        boardElement.CheckDiagonals(selectedChecker.basic, opponent.GetPoints());
                    }
                    break;

                case SKTouchAction.Moved:
                    bool isXValid = x > 0 && x < boardElement.boardWidth;
                    bool isYValid = y > 0 && y < boardElement.boardHeight;
                    if (selectedChecker != null && isXValid && isYValid)
                    {
                        selectedChecker.basic.element.Point = touch;
                    }
                    break;

                case SKTouchAction.Released:
                    if (selectedChecker != null)
                    {
                        var (newPoint, newCord) = boardElement.CheckHoveredSquares(touch);

                        if (newPoint.HasValue && newCord != null && newCord != selectedChecker.basic.element.Cord)
                        {
                            selectedChecker.SetPointAndCord(newPoint.Value, newCord);

                            if(selectedChecker.basic.IsAttacking)
                            {
                                var killedChecker = KilledChecker(selectedChecker.basic);
                                commandMessage = BaseMessage.ConcatMessages(commandMessage, new GameStatus(GameStatus.CHECKER_DATA, killedChecker).Value);
                                selectedChecker.basic.IsAttacking = false;
                                commandMessage = BaseMessage.ConcatMessages(commandMessage, new GameStatus(GameStatus.CHECKER_DATA, selectedChecker.basic).Value);
                                UpdateElements();
                                boardElement.CheckDiagonals(selectedChecker.basic, opponent.GetPoints());
                            }

                            if (!selectedChecker.basic.IsAttacking)
                            {
                                commandMessage = BaseMessage.ConcatMessages(commandMessage, new GameStatus(GameStatus.CHECKER_DATA, selectedChecker.basic).Value);
                                commandMessage = BaseMessage.ConcatMessages(commandMessage, CommandCategory.EndTurn.Value);
                                await user.SendCommand(commandMessage);
                                GameLogic.EndTurn();
                                commandMessage = string.Empty;
                            }
                        }
                        else
                        {
                            selectedChecker.SnapBack();
                        }
                        selectedChecker.basic.IsSelected = false;
                        selectedChecker = null;
                    }
                    boardElement.ClearSelectable();
                    break;
            }

            e.Handled = true;

            ((SKCanvasView)sender).InvalidateSurface();
        }
    }
    
    private BasicChecker? KilledChecker(BasicChecker checker)
    {
        double offsetX = (checker.element.Point.X - checker.element.OldPoint.X) > 0 ? checker.element.Point.X - boardElement.rectWidth : checker.element.Point.X + boardElement.rectWidth;
        double offsetY = (checker.element.Point.Y - checker.element.OldPoint.Y) > 0 ? checker.element.Point.Y - boardElement.rectHeight : checker.element.Point.Y + boardElement.rectHeight;

        var targetChecker = opponent.checkers.FirstOrDefault(c => c.basic.element.Point == new Point(offsetX, offsetY));
        if (targetChecker != null)
        {
            targetChecker.basic.IsDeleted = true;
        }

        return targetChecker?.basic;
    }

    private void CheckAttacking()
    {
        foreach(var checker in user.GetPoints()) {
            boardElement.CheckDiagonals(checker, opponent.GetPoints());
        }
    }
    
    public async Task CheckerChange(BasicChecker checker)
    {
        checker.MirrorCoordinates(boardElement);

        var allCheckers = GetAllCheckers();
        BasicChecker? targetChecker = null;
        if (!checker.IsDeleted)
        {
            targetChecker = allCheckers.FirstOrDefault(c => c.element.Point == checker.element.OldPoint);
        }
        else
        {
            targetChecker = allCheckers.FirstOrDefault(c => c.element.Point == checker.element.Point);
        }

        if (targetChecker != null)
        {
            targetChecker.element.Point = checker.element.Point;
            targetChecker.element.Cord = checker.element.Cord;
            targetChecker.IsDeleted = checker.IsDeleted;
            targetChecker.IsKing = checker.IsKing;
        }

        CheckLogicChanges();
    }

    private async Task StartServer()
    {
        var hostAddress = Network.Network.GetWindowsIpAddress();

        var logString = "Server created on : " + hostAddress; 

        LogEntries.Add(logString);

        await user.ListenServer();
    }

    public async Task StartGame()
    {
        var playerSide = GameLogic.SelectSides();

        GameLogic.SetSide(playerSide);

        var gameStatusMessage = BaseMessage.ConcatMessages(CommandCategory.StartGame.Value,
        opponent.Starts.HasValue && opponent.Starts.Value ? GameStatus.White.Value : GameStatus.Red.Value);

        await user.SendCommand(gameStatusMessage);
    }

    private async void OnJoinServer(object sender, EventArgs args)
    {
        HostServer.IsEnabled = false;
        ConnectButton.IsEnabled = false;
        DisconnectButton.IsEnabled = true;
        string endpoint = ClientHostEntry.Text;

        await user.JoinServer(endpoint);
    }

    private async void OnHostServer(object sender, EventArgs args)
    {
        ConnectButton.IsEnabled = false;
        HostServer.IsEnabled = false;
        DisconnectButton.IsEnabled = true;
        await StartServer();
    }

    private async void OnDisconnect(object sender, EventArgs args)
    {
        ConnectButton.IsEnabled = true;
        HostServer.IsEnabled = true;
        DisconnectButton.IsEnabled = false;
        await user.CloseConnection();
    }
    private async void OnRestartGame(object sender, EventArgs args)
    {
        RestartButton.IsEnabled = false;
        GameLogic.StartGame();
        await StartGame();
    }
}
