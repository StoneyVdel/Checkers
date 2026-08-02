using Checkers.Elements;
using System.Text.Json.Serialization;

namespace Checkers.Checker;

public class BasicChecker
{
    [JsonPropertyName("element")]
    public ElementClass element { get; set; }

    [JsonPropertyName("IsKing")]
    public bool IsKing { get; set; } = false;

    [JsonPropertyName("IsAttacking")]
    public bool IsAttacking { get; set; } = false;

    [JsonPropertyName("IsUser")]
    public bool IsUser { get; init; }

    public BasicChecker(ElementClass element, bool isUser)
    {
        this.element = element;
        this.IsUser = isUser;
    }

    public void MirrorCoordinates(BoardElement board)
    {
        element.OldPoint = new Point(board.boardWidth -  element.OldPoint.X, board.boardHeight - element.OldPoint.Y);
        element.Point = new Point(board.boardWidth - element.Point.X, board.boardHeight - element.Point.Y);
    }

    public void SetCord(BoardCordElement cord)
    {
        element.Cord = cord;
    }
}
