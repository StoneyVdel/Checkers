using SkiaSharp;

namespace Checkers.Classes;

public class Checker
{
    private float x {  get; set; }

    private float y { get; set; }

    private float radius { get; set; }

    private SKColor color { get; set; }

    Checker( float x, float y, float radius, SKColor color)
    {
        this.x = x;
        this.y = y;
        this.radius = radius;
        this.color = color;
    }
}
