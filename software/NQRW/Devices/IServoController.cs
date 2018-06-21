using System;
using System.Collections.Generic;

namespace NQRW.Devices
{
    public interface IServoController : IDisposable
    {
        bool Active { get; }
        IDictionary<int, IServo> Servos { get; }
        string Name { get; }
        bool Connected { get; }
        void Connect();
        void Start();
        void Stop();
        void Disconnect();
        void Update();
    }
}
