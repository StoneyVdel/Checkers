using SkiaSharp;

namespace Checkers.Elements;

public class ElementClass : BaseElement
{
    public Point OldPoint { get; set; }

    public float Radius { get; set; }

    public ElementClass(Point point, float radius, SKColor color)
    {
        Point = point;
        Radius = radius;
        Color = color;
    }
}
