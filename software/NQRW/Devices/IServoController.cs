using System;
using System.Collections.Generic;

namespace NQRW.Devices
{
    public interface IServoController : IDisposable
    {
        IDictionary<int, IServo> Servos { get; }
        string Name { get; }
        bool Connected { get; }
        void Connect();
        void Stop();
        void Disconnect();
        void Update();
    }
}
