using Checkers.Checker;
using SkiaSharp;

namespace Checkers.Player;

public partial class BasePlayer
{
    private List<CheckerObject>? checkers;

    public void AddChecker(CheckerObject checker)
    {
        if (checkers == null)
        {
            checkers = new List<CheckerObject>();
        }
        checkers.Add(checker);
    }

    public void CheckPosition(Point touch)
    {
        checkers.ForEach(checker => checker.isSelected(touch));
    }
}
