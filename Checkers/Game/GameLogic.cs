namespace Checkers.Game;

public static class GameLogic
{
    public static event Action? OnLogicChanged;

    public static bool GameStart { get; private set; } = false;  

    public static bool? Starts { get; private set; }

    public static bool WhiteTurn { get; private set; } = true;

    public static bool SelectSides()
    {
        var random = new Random();
        int playerSide = random.Next(0, 2);
        var side = playerSide == 0;

        return side;
    }

    public static void StartGame()
    {
        GameStart = !GameStart;
    }

    public static void SetSide(bool starts)
    {
        Starts = starts;
        if (MainPage.Instance != null)
        {
            MainPage.Instance.SetSide(starts);
            OnLogicChanged?.Invoke();
        }
    }

    public static void EndTurn()
    {
        WhiteTurn = !WhiteTurn;
        OnLogicChanged?.Invoke();
    }

    public static void ResetGame()
    {
        GameStart = false;
        Starts = null;
        WhiteTurn = true;
        OnLogicChanged?.Invoke();
    }
}
