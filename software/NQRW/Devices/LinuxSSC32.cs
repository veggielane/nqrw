using RJCP.IO.Ports;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace NQRW.Devices
{
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
        public override void Execute()
        {
            _port.Write(Convert.ToChar(13).ToString(CultureInfo.InvariantCulture));
        }
        public void Execute(string command)
        {
            _port.Write(command + Convert.ToChar(13).ToString(CultureInfo.InvariantCulture));
        }

        public override void Stop()
        {
            Execute("STOP");
        }


        private void Write(byte[] bytes)
        {
            _port.Write(bytes,0,bytes.Length);
        }
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
            Execute();
        }
        public string ReadData()
        {
            var sb = new StringBuilder();
            Thread.Sleep(100);
            while (_port.BytesToRead > 0)
            {
                sb.Append(_port.ReadExisting());
                Thread.Sleep(100);
            }
            return sb.ToString();
        }

        public string WriteRead(string data)
        {
            Execute(data);
            return ReadData();
        }
    }
}
