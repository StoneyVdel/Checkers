using System.Net.Sockets;
using System.Net;

namespace Checkers.Network;

public static class Network
{
    public static readonly int Port = 8080;

    public static readonly string Loopback = "127.0.0.1";

    public static readonly int Backlog = 1;

    public static string? GetWindowsIpAddress()
    {
        #if WINDOWS
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        #endif

        return null;
    }
}