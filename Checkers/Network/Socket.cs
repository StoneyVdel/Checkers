using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Checkers.Network;

public class SocketClient
{
    public async void ClientConnect(EndPoint endpoint)
    {
        using Socket client = new(
        SocketType.Stream, 
        ProtocolType.Tcp);

        await client.ConnectAsync(endpoint);
        while (true)
        {
            // Send message.
            var message = "Hi friends 👋!<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _ = await client.SendAsync(messageBytes, SocketFlags.None);
            Debug.WriteLine($"Socket client sent message: \"{message}\"");

            // Receive ack.
            var buffer = new byte[1_024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            if (response == "<|ACK|>")
            {
                Debug.WriteLine(
                    $"Socket client received acknowledgment: \"{response}\"");
                break;
            }
            // Sample output:
            //     Socket client sent message: "Hi friends 👋!<|EOM|>"
            //     Socket client received acknowledgment: "<|ACK|>"
        }

        client.Shutdown(SocketShutdown.Both);
    }
}

