using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using NQRW.Messaging;
using NQRW.Messaging.Messages;

namespace NQRW.Devices.Input
{
    public partial class PS4Controller
    {

        private readonly BackgroundWorker _backgroundWorker;
        private readonly IMessageBus _bus;

        public Dictionary<PS4Button, ButtonState> Buttons { get; }

        public Dictionary<PS4Axis, short> Axes { get;}

        public PS4Controller(IMessageBus bus)
        {
            Buttons = new Dictionary<PS4Button, ButtonState>()
            {
                {PS4Button.X, ButtonState.Released},
                {PS4Button.Circle, ButtonState.Released},
                {PS4Button.Triangle, ButtonState.Released},
                {PS4Button.Square, ButtonState.Released},
                {PS4Button.L1, ButtonState.Released},
                {PS4Button.R1, ButtonState.Released},
                {PS4Button.L2, ButtonState.Released},
                {PS4Button.R2, ButtonState.Released},
                {PS4Button.Share, ButtonState.Released},
                {PS4Button.Options, ButtonState.Released},
                {PS4Button.PS, ButtonState.Released},
                {PS4Button.L3, ButtonState.Released},
                {PS4Button.R3, ButtonState.Released}
            };

            Axes = new Dictionary<PS4Axis, short>
            {
                {PS4Axis.LeftStickX, 0},
                {PS4Axis.LeftStickY, 0},
                {PS4Axis.L2, 0},
                {PS4Axis.RightStickX, 0},
                {PS4Axis.RightStickY, 0},
                {PS4Axis.R2, 0},
                {PS4Axis.DPadX, 0},
                {PS4Axis.DPadY, 0}
            };
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
        bool checkBit(byte value, byte flag)
        {
            return (byte)(value & flag) == flag;
        }
    }
}
