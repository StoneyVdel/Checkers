using Checkers.Checker;
using Checkers.Game;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Checkers.Network;

public partial class BaseSocket : IBaseSocket
{
    protected Socket? client = null;

    public void SetClient(Socket socket)
    {
        client = socket;
    }

    public async Task SendMessage(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);

        if (client != null)
        {
            await client.SendAsync(messageBytes, SocketFlags.None);

            Debug.WriteLine($"Socket base sent message: \"{message}\"");
        }
    }

    public async Task<string> ReceiveMessage()
    {
        var buffer = new byte[1_024];
        if (client != null)
        {
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            Debug.WriteLine($"Socket base received message: \"{response}\"");

            return response;
        }

        return "";  
    }

    public async Task CloseConnection()
    {
        if (client != null)
        {
            await client.DisconnectAsync(false);
            client.Close();
        }
    }

    public Task ProcessMessage(string response)
    {

        if (response.StartsWith(CommandCategory.Tag))
        {
            var command = response.Substring(CommandCategory.Tag.Length);
            Debug.WriteLine($"Socket base received command: \"{command}\"");

            switch (command)
            {
                case CommandCategory.EndTurnTag:
                    GameLogic.EndTurn();
                    break;

                case CommandCategory.StartGameTag:
                    GameLogic.StartGame();
                    break;
            }
        }

        if (response.StartsWith(GameStatus.Tag))
        {
            var status = response.Substring(GameStatus.Tag.Length);
            Debug.WriteLine($"Socket base received status: \"{status}\"");

            if (status.Contains(GameStatus.CHECKER_DATA) ) {
                var checkerDataJson = status.Substring(GameStatus.CHECKER_DATA.Length);
                var checkerData = JsonSerializer.Deserialize<BasicChecker>(checkerDataJson);
                if (checkerData != null && MainPage.Instance != null)
                {
                    MainPage.Instance?.CheckerChange(checkerData);
                }
            }
        }

        return Task.CompletedTask;
    }
}
