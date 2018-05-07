using System.Collections.Generic;

namespace NQRW.Devices
{
    public abstract class BaseServoController : IServoController
    {
        public IDictionary<int, IServo> Servos { get; protected set; }
        public string Name { get; protected set; }
        public bool Connected { get; protected set; } = false;
        protected BaseServoController(string name)
        {
            Name = name;
            Servos = new Dictionary<int, IServo>();
        }


        public abstract void Connect();
        public abstract void Disconnect();
        public void Dispose()
        {
            if (Connected) Disconnect();
        }

        public abstract void Stop();
        public abstract void Update();
        public abstract void Execute();
    }
}
