using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Checkers.Network;

public partial class BaseSocket : IBaseSocket
{

    public async Task SendMessage(Socket client,string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);

        if (client != null)
        {
            await client.SendAsync(messageBytes, SocketFlags.None);

            Debug.WriteLine($"Socket base sent message: \"{message}\"");
        }
    }

    public async Task<string> ReceiveMessage(Socket client)
    {
        var buffer = new byte[1_024];
        if (client != null)
        {
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            Debug.WriteLine($"Socket base received message: \"{response}\"");

            return response;
        }
        return "";  
    }

    public async Task CloseConnection(Socket client)
    {
        if (client != null)
        {
            await client.DisconnectAsync(false);
            client.Close();
        }
    }

    public Task ProcessMessage(string message)
    {
        return Task.CompletedTask;
    }
}
