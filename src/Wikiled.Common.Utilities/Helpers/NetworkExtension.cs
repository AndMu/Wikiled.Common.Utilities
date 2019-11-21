using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Helpers
{
    public static class NetworkExtension
    {
        public static async Task<bool> ScanPort(IPAddress address, int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(address, port).ConfigureAwait(false);
                    return client.Connected;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

        public static IEnumerable<IPAddress> GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    yield return ip;
                }
            }
        }
    }
}
