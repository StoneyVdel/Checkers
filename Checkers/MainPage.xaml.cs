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

    private void DrawElements(SKCanvas canvas)
    {
        boardElement.Draw(canvas);

        opponent.Draw(canvas);
        user.Draw(canvas);
    }

    private void UpdateElements()
    {
        var points = opponent.GetPoints();
        points.AddRange(user.GetPoints());

        boardElement.UpdateState(points);
    }

    private async void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        float x = e.Location.X;
        float y = e.Location.Y;
        Point touch = new Point(x, y);
        CoordinateLabel.Text = $"X: {x:F2}, Y: {y:F2}";

        if (user.IsTurn && GameLogic.GameStart)
        {

            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    selectedChecker = user.CheckPosition(touch);
                    if (selectedChecker != null)
                    {
                        boardElement.CheckDiagonals(selectedChecker.basic);
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
                            var commandMessage = BaseMessage.ConcatMessages(CommandCategory.EndTurn.Value, new GameStatus(GameStatus.CHECKER_DATA, selectedChecker.basic).Value);
                            await user.SendCommand(commandMessage);
                            GameLogic.EndTurn();
                        }
                        else
                        {
                            selectedChecker.SnapBack();
                        }
                        selectedChecker = null;
                    }
                    break;
            }

            e.Handled = true;

            ((SKCanvasView)sender).InvalidateSurface();
        }
    }

    public async Task CheckerChange(BasicChecker checker)
    {
        checker.MirrorCoordinates(boardElement);

        var targetChecker = opponent.checkers.FirstOrDefault(c => c.basic.element.Point == checker.element.OldPoint);
        if (targetChecker != null)
        {
            targetChecker.SetPointAndCord(checker.element.Point, checker.element.Cord);
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
        string endpoint = ClientHostEntry.Text;

        await user.JoinServer(endpoint);
    }

    private async void OnHostServer(object sender, EventArgs args)
    {
        ConnectButton.IsEnabled = false;

        await StartServer();
    }
}
