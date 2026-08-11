using Checkers.Checker;
using Checkers.Elements;
using SkiaSharp;

namespace Checkers.Player;

public partial class BasePlayer
{
    public List<CheckerObject> checkers = new List<CheckerObject>();

    public bool? Starts { get; set; }

    public bool IsTurn { get; set; } = false;

    public void AddChecker(ElementClass element, bool isUser)
    {
        checkers.Add(new CheckerObject(element, isUser));
    }

    public CheckerObject? CheckPosition(Point touch)
    {
        if (checkers != null)
        {
            foreach (var checker in checkers)
            {
                if(checker.isSelected(touch))
                {
                    checker.basic.element.OldPoint = checker.basic.element.Point;

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
                if (Starts.HasValue && !checker.basic.IsKing)
                {
                    checker.SetColor(Starts.Value ? SKColors.White : SKColors.Red);
                }
                else if (Starts.HasValue && checker.basic.IsKing)
                {
                    checker.SetColor(Starts.Value ? SKColors.Gray : SKColors.DarkRed);
                }
                checker.Draw(canvas);
            }
        }
    }

    public void ClearStatus()
    {
        foreach (var checker in checkers)
        {
            checker.ClearStatus();
        }
    }

    public List<BasicChecker> GetPoints()
    {
        return checkers.Select(c => c.basic).ToList();
    }

    public void DeleteAttacked()
    {
        checkers.RemoveAll(x => x.basic.IsDeleted);
    }
}
