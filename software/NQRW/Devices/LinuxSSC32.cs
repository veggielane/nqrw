using RJCP.IO.Ports;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using JetBrains.Annotations;

namespace NQRW.Devices
{
    [UsedImplicitly]
    public class LinuxSSC32 : BaseServoController
    {
        private readonly SerialPortStream _port;
        public LinuxSSC32() : base("LinuxSSC32")
        {
            _port = new SerialPortStream("/dev/ttyUSB0", 115200, 8, Parity.None, StopBits.One);
        }
        public override void Connect()
        {
            _port.Open();
            Connected = true;
        }
        public override void Disconnect()
        {
            if (Connected) _port.Close();
        }
        public override void Stop()
        {
            foreach (var kvp in Servos)
            {
                Write(new[] { (byte)(0x80 + kvp.Key), (byte)((0 >> 8) & 0xff), (byte)(0 & 0xff) });
                _port.Write(Convert.ToChar(13).ToString(CultureInfo.InvariantCulture));
            }
            //_port.Write("STOP" + Convert.ToChar(13).ToString(CultureInfo.InvariantCulture));
            Active = false;
        }

        private void Write(byte[] bytes) => _port.Write(bytes, 0, bytes.Length);
        public override void Update()
        {
            foreach (var kvp in Servos)
            {
                Write(new[] { (byte)(0x80 + kvp.Key), (byte)((kvp.Value.Pulse >> 8) & 0xff), (byte)(kvp.Value.Pulse & 0xff) });
            }
            //if (speed != null)
            //{
            //    bw.Write(new[] { (byte)0xA0, (byte)((speed >> 8) & 0xff), (byte)(speed & 0xff) });
            //}
            //if (time != null)
            //{
            //    bw.Write(new[] { (byte)0xA1, (byte)((time >> 8) & 0xff), (byte)(time & 0xff) });
            //}
            _port.Write(Convert.ToChar(13).ToString(CultureInfo.InvariantCulture));
        }
    }
}
