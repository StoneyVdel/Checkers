namespace Checkers.Game;

public partial class BaseMessage
{
    public string Value { get; protected set; }

    protected BaseMessage(string value, string tag = "")
    {
        Value = tag + value;
    }

    public static string ConcatMessages(params string[] messages)
    {
        return string.Join(';', messages);
    }
}
