using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NQRW.Messaging.Logging
{
    public class UDPLogger:ILogger
    {
        private readonly Socket _socket;
        private readonly IPEndPoint _endPoint;
        private readonly bool _connected;
        public UDPLogger(string ip = "10.0.10.30", int port = 11000)
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                _connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Starting UDP:"+e);
            }
        }

        public void Log(string message)
        {
            if (_connected && _socket.Connected)
            {
                try
                {
                    _socket.SendTo(Encoding.ASCII.GetBytes(message), _endPoint);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

        }
    }
}

