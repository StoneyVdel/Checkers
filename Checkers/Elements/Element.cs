using SkiaSharp;

namespace Checkers.Elements;

public class ElementClass
{
    public Point Point {  get; set; }

    public float Radius { get; set; }

    public SKColor Color { get; set; }

    public ElementClass(Point point, float radius, SKColor color)
    {
        Point = point;
        Radius = radius;
        Color = color;
    }
}
