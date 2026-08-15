using Checkers.Game;
using Checkers.Network;
using System.Net;

namespace Checkers.Player;

public class UserPlayer : BasePlayer
{
    private SocketClient socketClient = new ();

    private SocketServer socketServer = new();

    public bool isServer = false;

    public async Task ListenServer()
    {
        socketServer = new();
        var result = await socketServer.ListenServer();
        if (result != null)
        {
            isServer = true;
            await socketServer.ProcessMessage(result);
        }
    }

    public async Task JoinServer(string endpoint)
    {
        socketClient = new();
        var ipAddress = IPAddress.Parse(endpoint);
        var ep = new IPEndPoint(ipAddress, Network.Network.Port);

        await socketClient.ClientConnect(ep);
    }

    public async Task EndMove()
    {
        if (isServer)
        {
            await socketClient.SendMessage(CommandCategory.EndTurn.Value);
        }
        else
            await socketServer.SendMessage(CommandCategory.EndTurn.Value);
    }

    public async Task SendCommand(string command)
    {
        if(isServer)
        {
            await socketServer.SendMessage(command);
        }
        else
            await socketClient.SendMessage(command);
    }

    public async Task WaitForMessage()
    {
        if (isServer)
        {
            var response = await socketServer.ReceiveMessage();

            await socketServer.ProcessMessage(response);
        }
        else
        {
            var response = await socketClient.ReceiveMessage();

            await socketClient.ProcessMessage(response);
        }
    }

    public async Task CloseConnection()
    {
        if (isServer)
        {
            await socketServer.CloseConnection();
        }
        else
            await socketClient.CloseConnection();
    }
}
