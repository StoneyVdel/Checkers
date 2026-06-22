using SkiaSharp;
using System.Diagnostics;

namespace Checkers.Elements;

public class BoardRect : BaseElement
{
    private int rectWidth { get; set; }

    private int rectHeight { get; set; }

    private bool isSelectable { get; set; } = false;

    private bool isOccupied { get; set; } = false;

    public BoardRect(Point point, int rectWidth, int rectHeight)
    {
        this.Point = point;
        this.rectWidth = rectWidth;
        this.rectHeight = rectHeight;
        this.Color = SKColors.Black;
    }

    public void Draw(SKCanvas canvas)
    {
        canvas.DrawRect((float)Point.X, (float)Point.Y, rectWidth, rectHeight, new SKPaint() { Color = Color });
    }

    public void CheckSelectable(Point point)
    {
        var dx = (point.X - rectWidth / 2) - Point.X;
        var dy = (point.Y - rectHeight / 2 ) - Point.Y;

        isSelectable = (dx * dx) == (dy * dy);

        if (isSelectable && !isOccupied) {
            Color = SKColors.Brown;
        }
        else
        {
            Color = SKColors.Black;
        }
    }

    public bool checkOccupied(Point point)
    {
        var dx = (point.X - (rectWidth /2)) - Point.X;
        var dy = (point.Y - (rectHeight /2)) - Point.Y;

        isOccupied = (dx == 0) && (dy == 0);

        if (isOccupied)
        {
            Debug.WriteLine($"Is occupied at X: {Point.X} Y: {Point.Y} with point X: {point.X} Y: {point.Y}");
        }

        return isOccupied;
    }

}
