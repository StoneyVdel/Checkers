using Checkers.Checker;
using System.Text.Json;
namespace Checkers.Game;

public class GameStatus : BaseMessage
{
    public static readonly string Tag = "STATUS:";

    public GameStatus(string value, BasicChecker? checker = null) : base(value, Tag)
    {
        if (value.Equals(CHECKER_DATA) && checker != null)
        {
            Value = Value + JsonSerializer.Serialize(checker);
        }
    }

    public static GameStatus White { get; } = new GameStatus(WHITE);

    public static GameStatus Red { get; } = new GameStatus(RED);

    public static GameStatus Connected { get; } = new GameStatus(CONNECTED);

    public static GameStatus Disconnected { get; } = new GameStatus(DISCONNECTED);

    public const string CONNECTED = "CONNECTED";    

    public const string DISCONNECTED = "DISCONNECTED";

    public const string WHITE = "WHITE";

    public const string RED = "RED";

    public const string CHECKER_DATA = "CHECKER_DATA|";

}
