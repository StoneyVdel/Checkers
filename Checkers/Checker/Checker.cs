using SkiaSharp;

namespace Checkers.Checker;

public class CheckerObject
{
    private Point point {  get; set; }

    private float radius { get; set; }

    public SKColor color { get; set; }

    public CheckerObject(Point point, float radius, SKColor color)
    {
        this.point = point;
        this.radius = radius;
        this.color = color;
    }

    public void isSelected(Point touch)
    {
        var dx = touch.X - this.point.X;
        var dy = touch.Y - this.point.Y;

        if ((dx * dx + dy * dy) <= (radius * radius))
        {
            this.color = SKColors.Purple;
        }
    }
}
