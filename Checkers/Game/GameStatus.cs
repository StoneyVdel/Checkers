namespace Checkers.Game;

public class GameStatus : BaseMessage
{
    public static readonly string Tag = "STATUS:";

    protected GameStatus(string value) : base(value, Tag)
    {
    }

    public static GameStatus White { get; } = new GameStatus(WHITE);

    public static GameStatus Red { get; } = new GameStatus(RED);

    public static GameStatus Connected { get; } = new GameStatus(CONNECTED);

    public static GameStatus Disconnected { get; } = new GameStatus(DISCONNECTED);

    public const string CONNECTED = "CONNECTED";    

    public const string DISCONNECTED = "DISCONNECTED";

    public const string WHITE = "WHITE";

    public const string RED = "RED";

}
