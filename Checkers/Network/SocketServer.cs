using System.Net;
using System.Net.Sockets;
using Checkers.Game;
using System.Diagnostics;

namespace Checkers.Network;

public class SocketServer : BaseSocket
{
    public async Task<string?> ListenServer()
    {
        client = new Socket(SocketType.Stream, ProtocolType.Tcp);

        client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 1);
        client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 2);
        client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 2);
        client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

        var hostName = Dns.GetHostName();

        IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

        IPAddress localIpAddress = localhost.AddressList[1]; //IPAddress.Parse(Network.Loopback);

        IPEndPoint ep = new IPEndPoint(localIpAddress, Network.Port);

        client.Bind(ep);

        client.Listen(Network.Backlog);

        var handler = await client.AcceptAsync();

        if (handler.Connected)
        {
            client = handler;
            await SendMessage(GameStatus.Connected.Value);
            var response = await base.ReceiveMessage();

            return response;
        }

        return null;
    }

    public new async Task ProcessMessage(string response)
    {
        var messages = response.Split(';');

        foreach (var message in messages)
        {
            if (message.StartsWith(GameStatus.Tag))
            {
                var gameStateData = message.Substring(GameStatus.Tag.Length);
                Debug.WriteLine($"Socket server received game state: \"{gameStateData}\"");

                switch (gameStateData)
                {
                    case GameStatus.CONNECTED:
                        if (MainPage.Instance != null)
                        {
                            GameLogic.StartGame();
                            await MainPage.Instance.StartGame();
                        }
                        break;
                }
            }

            await base.ProcessMessage(message);
        }
    }
}
