using Checkers.Elements;
using Checkers.Player;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Checkers;

public partial class MainPage : ContentPage
{
    private bool gameStart;
    private int boardWidth;
    private int boardHeight;
    private int rectSize;
    public static UserPlayer user = new UserPlayer();
    public static OpponentPlayer opponent = new OpponentPlayer();

    public MainPage()
    {
        InitializeComponent();
        gameStart = true;
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        boardWidth = 480;
        boardHeight = 480;
        rectSize = boardWidth/8;

        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        if (gameStart)
        {
            gameStart = false;
            PrepareBoard(canvas);
        }

        DrawElements(canvas);
    }

    private void PrepareBoard(SKCanvas canvas)
    {
        using (var paint = new SKPaint { Color = SKColors.Black })
        {
            for (int row = 0; row < boardHeight; row += rectSize)
            {
                for (int col = 0; col < boardWidth; col += rectSize)
                {
                    bool isRowEven = (row / rectSize % 2 == 0);
                    bool isColEven = (col / rectSize % 2 == 0);

                    if ((isRowEven && !isColEven) || (!isRowEven && isColEven))
                    {
                        var sqRadius = rectSize / 2;
                        var point = new Point(col + sqRadius, row + sqRadius);
                        var radius = rectSize / 2 - 5;

                        if ((row < (rectSize * 3)))
                        {
                            var element = new ElementClass(point, radius, SKColors.Green);
                            opponent.AddChecker(element);
                        }
                        else if ((row >= 5 * rectSize))
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
        DrawBoard(canvas);
        opponent.Draw(canvas);
        user.Draw(canvas);
    }

    private void DrawBoard(SKCanvas canvas)
    {
        using (var paint = new SKPaint { Color = SKColors.Black })
        {
            for (int row = 0; row<=boardHeight; row += rectSize)
            {
                for (int col = 0; col<=boardWidth; col += rectSize)
                {
                    bool isRowEven = (row / rectSize % 2 == 0);
                    bool isColEven = (col / rectSize % 2 == 0);

                    if ((isRowEven && !isColEven) || (!isRowEven && isColEven))
                    {
                        canvas.DrawRect((float)col, (float)row, rectSize, rectSize, paint);
                    }
                }
            }
        }
    }

    private void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        float x = e.Location.X;
        float y = e.Location.Y;
        Point touch = new Point(x, y);
        
        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
                user.CheckPosition(touch);
                break;

            case SKTouchAction.Moved:
                break;

            case SKTouchAction.Released:
                Console.WriteLine($"Released at ({x}, {y})");
                break;
        }

        e.Handled = true;

        ((SKCanvasView)sender).InvalidateSurface();
    }
}
