using Checkers.Game;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Checkers.Network;

public class SocketClient : BaseSocket
{
    public async Task ClientConnect(EndPoint endpoint)
    {
        client = new(
        SocketType.Stream,
        ProtocolType.Tcp);

        client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 1);
        client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 2);
        client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 2);
        client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

        try
        {
            await client.ConnectAsync(endpoint);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to server: {ex.Message}");
        }

        if (client.Connected)
        {
            await WaitForMessage();
        }
    }

    private async Task WaitForMessage()
    {
        var response = await base.ReceiveMessage();
        await ProcessMessage(response);
    }

    public new async Task ProcessMessage(string response)
    {
        var messages = response.Split(';');

        foreach (var message in messages)
        { 
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
                        await base.SendMessage(GameStatus.Connected.Value);
                        await WaitForMessage();

                        break;
                }
            }

            await base.ProcessMessage(message);
        }
    }
}