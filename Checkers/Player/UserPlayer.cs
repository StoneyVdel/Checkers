using System.Net;
using System.Net.Sockets;

namespace Checkers.Player;

public class UserPlayer : BasePlayer
{
    public Socket ConnectionSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

    public async void ListenForOpponent()
    {
        
        IPEndPoint ep = new IPEndPoint(hostIP, port);

        ConnectionSocket.Bind(ep);

        ConnectionSocket.Listen(backlog);
    }
}
