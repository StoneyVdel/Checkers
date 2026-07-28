using System.Net;
using System.Net.Sockets;
using Checkers.Game;
using System.Diagnostics;

namespace Checkers.Network;

public class SocketServer : BaseSocket
{
    public async Task<bool> ListenServer()
    {
        using Socket ConnectionSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        var hostName = Dns.GetHostName();

        IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

        IPAddress localIpAddress = localhost.AddressList[1]; //IPAddress.Parse(Network.Loopback);

        IPEndPoint ep = new IPEndPoint(localIpAddress, Network.Port);

        ConnectionSocket.Bind(ep);

        ConnectionSocket.Listen(Network.Backlog);

        var handler = await ConnectionSocket.AcceptAsync();

        if (handler.Connected)
        {
            await base.SendMessage(handler, GameStatus.Connected.Value);
            var response = await base.ReceiveMessage(handler);
            await ProcessMessage(handler, response);

            return true;
        }

        return false;
    }

    public async Task ProcessMessage(Socket client, string message)
    {
        if (message.StartsWith(GameStatus.Tag))
        {
            var gameStateData = message.Substring(GameStatus.Tag.Length);
            Debug.WriteLine($"Socket server received game state: \"{gameStateData}\"");

            switch (gameStateData)
            {
                case GameStatus.CONNECTED:
                    await MainPage.StartGame(client);
                    GameLogic.StartGame();
                
                    break;
            }
        }
    }
}
