using SkiaSharp;

namespace Checkers.Elements;

public class ElementClass : BaseElement
{
    public Point OldPoint { get; set; }

    public float Radius { get; set; }

    public BoardCordElement Cord { get; set; }

    public ElementClass(Point point, BoardCordElement cord, float radius, SKColor color)
    {
        Point = point;
        Radius = radius;
        Color = color;
        Cord = cord;
    }
}
