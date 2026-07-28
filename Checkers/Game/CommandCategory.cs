namespace Checkers.Game;

public class CommandCategory : BaseMessage
{
    public static readonly string Tag = "COMMAND:";

    protected CommandCategory(string value) : base(value, Tag)
    {
    }

    public static CommandCategory EndTurn { get; } = new CommandCategory(EndTurnTag);

    public static CommandCategory StartGame { get; } = new CommandCategory(StartGameTag);

    public const string EndTurnTag = "END_TURN";

    public const string StartGameTag = "START_GAME";
}
