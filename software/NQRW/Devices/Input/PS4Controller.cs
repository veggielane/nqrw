using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Timing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace NQRW.Devices
{
    public class PS4Controller
    {
        enum EventType : byte { Axis = 0x02, Button = 0x01 }
        enum EventMode : byte { Config = 0x80, Value = 0x00 }


        private BackgroundWorker _backgroundWorker;
        private readonly IMessageBus _bus;

        public ConcurrentDictionary<PS4Button, ButtonState> Buttons { get; set; } = new ConcurrentDictionary<PS4Button, ButtonState>();
        public ConcurrentDictionary<PS4Axis, short> Axes { get; set; } = new ConcurrentDictionary<PS4Axis, short>();

        public PS4Controller(ITimer timer, IMessageBus bus)
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

            if (checkBit(buff[6], (byte)EventMode.Config))
            {
                if (checkBit(buff[6], (byte)EventType.Button))
                {
                    var button = (PS4Button)buff[7];
                    if (!Buttons.ContainsKey(button))
                    {
                        Console.WriteLine(button);
                        Buttons.TryAdd(button, ButtonState.Released);
                    }
                }
                if (checkBit(buff[6], (byte)EventType.Axis))
                {
                    var axis = (PS4Axis)buff[7];
                    if (!Axes.ContainsKey(axis))
                    {
                        Axes.TryAdd(axis, 0);
                    }
                }
            }

            if (checkBit(buff[6], (byte)EventMode.Value))
            {
                if (checkBit(buff[6], (byte)EventType.Button))
                {
                    var button = (PS4Button)buff[7];
                    var state = (ButtonState)buff[4];
                    Buttons[button] = state;
                    _bus.Add(new ButtonEvent(this, button, state));
                }
                if (checkBit(buff[6], (byte)EventType.Axis))
                {
                    var axis = (PS4Axis)buff[7];
                    var value = BitConverter.ToInt16(new byte[2] { buff[4], buff[5] }, 0);
                    Axes[axis] = value;

                    _bus.Add(new AxisEvent(this, axis, value));
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
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
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
