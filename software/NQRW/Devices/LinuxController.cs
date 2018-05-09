using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Timing;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace NQRW.Devices
{
    enum EventType : byte { Axis = 0x02, Button = 0x01 }
    enum EventMode : byte { Config = 0x80, Value = 0x00 }
    public class LinuxController: IController
    {
        private BackgroundWorker _backgroundWorker;
        private readonly IMessageBus _bus;

        public LinuxController(ITimer timer, IMessageBus bus)
        {
            _bus = bus;

            _backgroundWorker = new BackgroundWorker()
            { 
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            _backgroundWorker.RunWorkerAsync();

        }
        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            byte[] buff = e.UserState as byte[];
            if(checkBit(buff[6], (byte)EventMode.Value))
            {
                if (checkBit(buff[6], (byte)EventType.Button))
                {
                    _bus.Add(new ButtonEvent((PS4Button)buff[7], (ButtonState)buff[4]));
                }
                if (checkBit(buff[6], (byte)EventType.Axis))
                {
                    _bus.Add(new AxisEvent((PS4Axis)buff[7], BitConverter.ToInt16(new byte[2] { buff[4], buff[5] }, 0)));
                }
            }
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            using (var stream = new FileStream("/dev/input/js0", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] buff = new byte[8];
                while (!worker.CancellationPending)
                {
                    stream.Read(buff, 0, 8);
                    worker.ReportProgress(0, buff);
                }
            }
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        bool checkBit(byte value, byte flag)
        {
            return (byte)(value & flag) == flag;
        }
        public void Dispose()
        {

        }
    }
}
