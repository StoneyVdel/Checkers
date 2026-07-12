using System.Net.Sockets;
using System.Net;

namespace Checkers.Network;

public static class Network
{
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