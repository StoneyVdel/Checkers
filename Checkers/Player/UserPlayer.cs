using System.Net;
using Checkers.Network;
using Checkers.Game;
using System.Net.Sockets;

namespace Checkers.Player;

public class UserPlayer : BasePlayer
{
    private SocketClient socketClient = new ();

    private SocketServer socketServer = new();

    public async Task<bool>  ListenForOpponent()
    {
        var result = await socketServer.ListenServer();

        return result;
    }

    public async Task JoinServer(string endpoint)
    {
        var ipAddress = IPAddress.Parse(endpoint);
        var ep = new IPEndPoint(ipAddress, Network.Network.Port);

        await socketClient.ClientConnect(ep);
    }

    public async Task EndMove(Socket client)
    {
        await socketClient.SendMessage(client, CommandCategory.EndTurn.Value);
    }

    public async Task SendCommand(Socket client, string command)
    {
        await socketClient.SendMessage(client, command);
    }
}
