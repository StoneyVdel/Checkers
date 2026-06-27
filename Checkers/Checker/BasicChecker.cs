using Checkers.Elements;

namespace Checkers.Checker;

public class BasicChecker
{
    public ElementClass element { get; set; }

    public bool IsKing { get; set; } = false;

    public bool IsAttacking { get; set; } = false;

    public bool IsUser { get; init; }

    public BasicChecker(ElementClass element, bool isUser)
    {
        this.element = element;
        this.IsUser = isUser;
    }
}
