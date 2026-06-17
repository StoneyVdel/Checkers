using Checkers.Classes;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using static Android.Webkit.WebSettings;

namespace Checkers;

public partial class MainPage : ContentPage
{
    private double boardWidth;
    private double boardHeight;
    private double rectSize;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        boardWidth = 480;
        boardHeight = 480;
        rectSize = boardWidth/8;

        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        using (var paint = new SKPaint { Color = SKColors.Black })
        {
            for (double y = 0; y < boardHeight; y+=rectSize)
            {
                for (double x = rectSize; x < boardWidth; x+=(2*rectSize))
                {
                    if ((y/rectSize % 2 != 0) && (x/rectSize % 2 != 0))
                    {
                        x -= rectSize;
                    }
                    canvas.DrawRect((float)x, (float)y, (float)rectSize, (float)rectSize, paint);
                }
            }
        }
    }

    private void DrawCheckers(SKCanvas canvas)
    {
        using (var paint = new SKPaint { Color = SKColors.White })
        {
            for (double y = 0; y < rectSize * 3; y += rectSize)
            {
                for (double x = rectSize; x < boardWidth; x += (2 * rectSize))
                {
                    if ((y / rectSize % 2 != 0) && (x / rectSize % 2 != 0))
                    {
                        x -= rectSize;
                    }
                    canvas.DrawCircle((float)(x + rectSize / 2), (float)(y + rectSize / 2), (float)(rectSize / 2 - 5), paint);
                }
            }
        }

        using (var paint = new SKPaint { Color = SKColors.Red })
        {
            for (double y = boardHeight; y > boardHeight - rectSize * 4; y -= rectSize)
            {
                for (double x = 0; x < boardWidth; x += (2 * rectSize))
                {
                    if ((y / rectSize % 2 == 0) && (x / rectSize % 2 == 0))
                    {
                        x -= rectSize;
                    }
                    canvas.DrawCircle((float)(x + rectSize / 2), (float)(y + rectSize / 2), (float)(rectSize / 2 - 5), paint);
                }
            }
        }

    private void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        float x = e.Location.X;
        float y = e.Location.Y;

        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
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
