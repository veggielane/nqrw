using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NQRW.Monitor
{
    class Program
    {
        static void Main()
        {
            var udpServer = new UdpClient(11000);
            Console.WriteLine("Running");
            while (true)
            {
                var ep = new IPEndPoint(IPAddress.Any, 11000);
                var data = udpServer.Receive(ref ep); // listen on port 11000
                Console.WriteLine(Encoding.ASCII.GetString(data));
            }
        }
    }
}
