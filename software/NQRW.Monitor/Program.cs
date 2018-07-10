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
            //byte[] data = new byte[1024];
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 11000);
            //UdpClient newsock = new UdpClient(ipep);

            //Console.WriteLine("Waiting for a client...");

            //IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            //data = newsock.Receive(ref sender);

            //Console.WriteLine("Message received from {0}:", sender.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            //while (true)
            //{
            //    data = newsock.Receive(ref sender);
            //    Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            //}

            UdpClient udpServer = new UdpClient(11000);
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
