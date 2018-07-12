using RJCP.IO.Ports;
using System;
using System.Globalization;
using JetBrains.Annotations;

namespace NQRW.Devices
{
    [UsedImplicitly]
    public class LinuxSSC32 : BaseServoController
    {
        private readonly SerialPortStream _port;
        private object _lock = new object();
        private char[] _executeCommand;
        public LinuxSSC32() : base("LinuxSSC32")
        {
            _port = new SerialPortStream("/dev/ttyUSB0", 115200, 8, Parity.None, StopBits.One);
            _executeCommand = new [] { Convert.ToChar(13) };

        }
        public override void Connect()
        {
            lock (_lock)
            {
                _port.Open();
            }

            Connected = true;
        }
        public override void Disconnect()
        {
            if (Connected)
            {
                lock (_lock)
                {
                    _port.Close();
                }
            }
        }
        public override void Stop()
        {
            lock (_lock)
            {
                foreach (var kvp in Servos)
                {
                    Write(new[] { (byte)(0x80 + kvp.Key), (byte)((0 >> 8) & 0xff), (byte)(0 & 0xff) });
                    Execute();
                }

            }
            Active = false;
        }


        private void Execute()
        {
            _port.Write(_executeCommand, 0, 1);
        }

        private void Write(byte[] bytes) => _port.Write(bytes, 0, bytes.Length);
        public override void Update()
        {
            lock (_lock)
            {
                foreach (var kvp in Servos)
                {
                    Write(new[] { (byte)(0x80 + kvp.Key), (byte)((kvp.Value.Pulse >> 8) & 0xff), (byte)(kvp.Value.Pulse & 0xff) });
                }
                Execute();
            }
        }
    }
}
