using Checkers.Checker;
using Checkers.Elements;
using SkiaSharp;

namespace Checkers.Player;

public partial class BasePlayer
{
    public List<CheckerObject> checkers = new List<CheckerObject>();

    public void AddChecker(ElementClass element)
    {
        checkers.Add(new CheckerObject(element));
    }

    public CheckerObject? CheckPosition(Point touch)
    {
        if (checkers != null)
        {
            foreach (var checker in checkers)
            {
                if(checker.isSelected(touch))
                {
                    checker.element.OldPoint = checker.element.Point;

                    return checker;
                }
            }
        }

        return null;
    }

    public void Draw(SKCanvas canvas)
    {
        if (checkers is not null)
        {
            foreach (var checker in checkers)
            {
                checker.Draw(canvas);
            }
        }
    }

    public List<Point> GetPoints()
    {
        var points = new List<Point>(); 

        foreach(var checker in checkers)
        {
            points.Add(checker.GetPoint());
        }

        return points;
    }
}
