using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Checkers.Network;

public class SocketServer
{
    public async void ListenServer()
    {
        using Socket ConnectionSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        var hostName = Dns.GetHostName();

        IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

        IPAddress localIpAddress = localhost.AddressList[1]; //IPAddress.Parse(Network.Loopback);

        IPEndPoint ep = new IPEndPoint(localIpAddress, Network.Port);

        ConnectionSocket.Bind(ep);

        ConnectionSocket.Listen(Network.Backlog);

        var handler = await ConnectionSocket.AcceptAsync();
        while (true)
        {
            // Receive message.
            var buffer = new byte[1_024];
            var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            var eom = "<|EOM|>";
            if (response.IndexOf(eom) > -1 /* is end of message */)
            {
                Debug.WriteLine(
                    $"Socket server received message: \"{response.Replace(eom, "")}\"");

                var ackMessage = "<|ACK|>";
                var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                await handler.SendAsync(echoBytes, 0);
                Debug.WriteLine(
                    $"Socket server sent acknowledgment: \"{ackMessage}\"");

                break;
            }
            // Sample output:
            //    Socket server received message: "Hi friends 👋!"
            //    Socket server sent acknowledgment: "<|ACK|>"
        }
    }
}
