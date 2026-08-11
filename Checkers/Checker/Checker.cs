using Checkers.Elements;
using SkiaSharp;

namespace Checkers.Checker;

public class CheckerObject
{
    public BasicChecker basic { get; set; }

    public CheckerObject(ElementClass element, bool isUser)
    {
        this.basic = new BasicChecker(element, isUser);
    }

    public bool isSelected(Point touch)
    {
        var dx = touch.X - this.basic.element.Point.X;
        var dy = touch.Y - this.basic.element.Point.Y;

        return ((dx * dx + dy * dy) <= (basic.element.Radius * basic.element.Radius));
    }

    public void Draw(SKCanvas canvas)
    {
        canvas.DrawCircle((float)basic.element.Point.X, (float)basic.element.Point.Y, basic.element.Radius, new SKPaint() { Color = basic.element.Color });
    }

    public void SnapBack()
    {
        basic.element.Point = basic.element.OldPoint;
    }

    public Point GetPoint()
    {
        return basic.element.Point;
    }

    public void SetPointAndCord(Point point, BoardCordElement cord)
    {
        basic.element.Point = point;
        basic.element.Cord = cord;
        if(cord.Row == 0)
        {
            basic.IsKing = true;
        }
    }

    public void SetColor(SKColor color)
    {
        basic.element.Color = color;
    }

    public void ClearStatus()
    {
        basic.IsAttacking = false;
        basic.IsSelected = false;
    }
}
