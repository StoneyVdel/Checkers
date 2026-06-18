using Checkers.Draw;
using Checkers.Player;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Checkers;

public partial class MainPage : ContentPage
{
    private double boardWidth;
    private double boardHeight;
    private double rectSize;
    public static UserPlayer user = new UserPlayer();
    public static OpponentPlayer opponent = new OpponentPlayer();

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

        DrawElements.DrawBoardElements(new ElementClass(ElementForm.Rect, SKColors.Black, (float)boardHeight,  (float)boardWidth, (float)rectSize), canvas);
        DrawCheckers(canvas);
    }

    private void DrawCheckers(SKCanvas canvas)
    {
        //TODO color of checkers should be based on user side, not hardcoded
        DrawElements.DrawBoardElements(new ElementClass(ElementForm.Circle, SKColors.White, (float)(rectSize*3), (float)boardWidth, (float)rectSize), canvas);
        DrawElements.DrawBoardElements(new ElementClass(ElementForm.Circle, SKColors.Red, (float)((-1)*(boardHeight - rectSize * 4)), (float)boardWidth, (float)rectSize, true), canvas);
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
