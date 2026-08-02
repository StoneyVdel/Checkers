using SkiaSharp;
using System.Diagnostics;

namespace Checkers.Elements;

public class SquareElement : BaseElement
{
    private int RectWidth { get; set; }

    private int RectHeight { get; set; }

    private bool IsSelectable { get; set; } = false;

    public bool IsOccupied { get; set; } = false;

    public SquareElement(BaseElement baseElement, int rectWidth, int rectHeight)
    {
        this.Point = baseElement.Point;
        this.RectWidth = rectWidth;
        this.RectHeight = rectHeight;
        this.Color = SKColors.Black;
        this.Cord = baseElement.Cord;
    }

    public void Draw(SKCanvas canvas)
    {
        canvas.DrawRect((float)Point.X, (float)Point.Y, RectWidth, RectHeight, new SKPaint() { Color = Color });
    }

    public void SetSelectable(bool isSelectable)
    {
        IsSelectable = isSelectable; 

        if (IsSelectable && !IsOccupied) {
            Color = SKColors.Brown;
        }
        else
        {
            Color = SKColors.Black;
        }
    }

    public bool checkOccupied(Point point)
    {
        var dx = (point.X - (RectWidth / 2)) - Point.X;
        var dy = (point.Y - (RectHeight / 2)) - Point.Y;

        IsOccupied = (dx == 0) && (dy == 0);

        if (IsOccupied)
        {
            //Debug.WriteLine($"Is occupied at X: {Point.X} Y: {Point.Y} with point X: {point.X} Y: {point.Y}");
        }

        return IsOccupied;
    }

    public bool CheckHoveredSquare(Point point)
    {
        var hoveredSquare = (point.X > Point.X) && (point.X < (Point.X + RectWidth)) && (point.Y > Point.Y) && (point.Y < (Point.Y + RectHeight)) && IsSelectable;

        return hoveredSquare;
    }
}
