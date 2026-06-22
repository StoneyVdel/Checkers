using Checkers.Checker;
using Checkers.Elements;
using Checkers.Player;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;

namespace Checkers;

public partial class MainPage : ContentPage
{
    private bool gameStart;
    private CheckerObject? selectedChecker;
    private BoardElement boardElement = new BoardElement();
    public static UserPlayer user = new UserPlayer();
    public static OpponentPlayer opponent = new OpponentPlayer();

    public MainPage()
    {
        InitializeComponent();
        gameStart = true;
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
                        boardElement.AddRect(new Point(col, row));

                        var sqRadius = boardElement.rectWidth / 2;
                        var point = new Point(col + sqRadius, row + sqRadius);
                        var radius = sqRadius - 5;

                        if ((row < (boardElement.rectHeight * 3)))
                        {
                            var element = new ElementClass(point, radius, SKColors.White);
                            opponent.AddChecker(element);
                        }
                        else if ((row >= 5 * boardElement.rectHeight))
                        {
                            var element = new ElementClass(point, radius, SKColors.Red);
                            user.AddChecker(element);
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
                    boardElement.CheckSelectableRects(selectedChecker.element.OldPoint);
                }

                break;

            case SKTouchAction.Moved:
                bool isXValid = x > 0 && x < boardElement.boardWidth;
                bool isYValid = y > 0 && y < boardElement.boardHeight;
                if (selectedChecker != null && isXValid && isYValid)
                {
                    selectedChecker.element.Point = touch;
                }
                break;

            case SKTouchAction.Released:
                if (selectedChecker != null)
                {
                    selectedChecker.SnapBack();
                    selectedChecker = null;
                }

                break;
        }

        e.Handled = true;

        ((SKCanvasView)sender).InvalidateSurface();
    }
}
