using System.Net.Sockets;

namespace Checkers.Game;

public static class GameLogic
{
    public static event Action? OnLogicChanged;

    public static bool GameStart { get; private set; } = false;  

    public static bool? Starts { get; private set; }

    public static bool SelectSides()
    {
        var random = new Random();
        int playerSide = random.Next(0, 2);
        var side = playerSide == 0;

        return side;
    }

    public static void StartGame()
    {
        GameStart = true;
        OnLogicChanged?.Invoke();
    }

    public static void SetSide(bool starts)
    {
        Starts = starts;
        OnLogicChanged?.Invoke();
    }

}
