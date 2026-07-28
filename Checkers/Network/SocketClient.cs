using Checkers.Game;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Checkers.Network;

public class SocketClient : BaseSocket
{
    public async Task ClientConnect(EndPoint endpoint)
    {
        using Socket client = new(
        SocketType.Stream,
        ProtocolType.Tcp);

        try
        {
            await client.ConnectAsync(endpoint);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to server: {ex.Message}");

            return;
        }

        if (client.Connected)
        {
            await WaitForMessaage(client);
        }
    }

    private async Task WaitForMessaage(Socket client)
    {
        var response = await base.ReceiveMessage(client);
        await ProcessMessage(client, response);
    }

    private async Task ProcessMessage(Socket client, string response)
    {
        var messages = response.Split(';');

        foreach (var message in messages)
        {

            if (message.StartsWith(CommandCategory.Tag))
            {
                var command = message.Substring(CommandCategory.Tag.Length);
                Debug.WriteLine($"Socket client received command: \"{command}\"");

                switch (command)
                {
                    case CommandCategory.EndTurnTag:
                        // Handle end turn command
                        break;
                    case CommandCategory.StartGameTag:
                        GameLogic.StartGame();
                        break;
                }
            }

            if (message.StartsWith(GameStatus.Tag))
            {
                var gameStateData = message.Substring(GameStatus.Tag.Length);
                Debug.WriteLine($"Socket client received game state: \"{gameStateData}\"");

                switch (gameStateData)
                {
                    case GameStatus.WHITE:
                        GameLogic.SetSide(true);
                        break;
                    case GameStatus.RED:
                        GameLogic.SetSide(false);
                        break;
                    case GameStatus.CONNECTED:
                        await base.SendMessage(client, GameStatus.Connected.Value);
                        await WaitForMessaage(client);

                        break;
                }
            }
        }
    }
}