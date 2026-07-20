using System.Net;
using Checkers.Network;

namespace Checkers.Player;

public class UserPlayer : BasePlayer
{
    private SocketClient socketClient = new SocketClient();
    private SocketServer socketServer = new SocketServer();

    public void ListenForOpponent()
    {
        socketServer.ListenServer();
    }

    public void JoinServer(string endpoint)
    {
        var ipAddress = IPAddress.Parse(endpoint);
        var ep = new IPEndPoint(ipAddress, Network.Network.Port);

        socketClient.ClientConnect(ep);
    }
}
