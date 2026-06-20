using Checkers.Checker;
using Checkers.Elements;
using SkiaSharp;

namespace Checkers.Player;

public partial class BasePlayer
{
    protected List<CheckerObject> checkers;

    public BasePlayer()
    {
        this.checkers = new List<CheckerObject>();
    }

    public void AddChecker(ElementClass element)
    {
        checkers.Add(new CheckerObject(element));
    }

    public void CheckPosition(Point touch)
    {
        if (checkers != null)
        {
            foreach (var checker in checkers)
            {
                if(checker.isSelected(touch))
                {
                    checker.element.Color = SKColors.Purple;
                    break;
                }
            }
        }
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
}
