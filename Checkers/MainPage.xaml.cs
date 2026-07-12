using Checkers.Checker;
using Checkers.Elements;
using Checkers.Player;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Sockets;

namespace Checkers;

public partial class MainPage : ContentPage
{
    private bool gameStart;
    private CheckerObject? selectedChecker;
    private BoardElement boardElement = new BoardElement();
    private static UserPlayer user = new UserPlayer();
    private static OpponentPlayer opponent = new OpponentPlayer();

    public ObservableCollection<string> LogEntries { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        StartServer();
        gameStart = true;
        BindingContext = this;
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        if (gameStart)
        {
            gameStart = false;
            PrepareBoard(canvas);
        }

        Debug.WriteLine("Update Started!");
        UpdateElements();
        Debug.WriteLine("Update Ended!");
        DrawElements(canvas);
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

                        if ((row < (boardElement.rectHeight * 3)))
                        {
                            var element = new ElementClass(checkerPoint, cord, radius, SKColors.White);
                            opponent.AddChecker(element, false);
                        }
                        else if ((row >= 5 * boardElement.rectHeight))
                        {
                            var element = new ElementClass(checkerPoint, cord, radius, SKColors.Red);
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

    private void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        float x = e.Location.X;
        float y = e.Location.Y;
        Point touch = new Point(x, y);
        
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

                    if(newPoint.HasValue && newCord != null)
                    {
                        selectedChecker.SetPointAndCord(newPoint.Value, newCord);
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

    private void StartServer()
    {
        var ipAddress = Network.Network.GetWindowsIpAddress();

        var logString = "Server created on : " + ipAddress; 

        LogEntries.Add(logString);

        user.ConnectionSocket.Listen();
    }

    private async void OnJoinServer(object sender, EventArgs args)
    {

    }
}
