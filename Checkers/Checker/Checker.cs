using Checkers.Elements;
using SkiaSharp;

namespace Checkers.Checker;

public class CheckerObject
{
    public ElementClass element { get; set; }

    public bool isKing {  get; set; }

    public CheckerObject(ElementClass element)
    {
        this.element = element;
        this.isKing = false;
    }

    public bool isSelected(Point touch)
    {
        var dx = touch.X - this.element.Point.X;
        var dy = touch.Y - this.element.Point.Y;

        return ((dx * dx + dy * dy) <= (element.Radius * element.Radius));
    }

    public void Draw(SKCanvas canvas)
    {
        canvas.DrawCircle((float)element.Point.X, (float)element.Point.Y, element.Radius, new SKPaint() { Color = element.Color });
    }
}
