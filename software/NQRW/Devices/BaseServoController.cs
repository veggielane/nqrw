using System.Collections.Generic;

namespace NQRW.Devices
{
    public abstract class BaseServoController : IServoController
    {
        public bool Active { get; protected set; }
        public IDictionary<int, IServo> Servos { get; } = new Dictionary<int, IServo>();
        public string Name { get; }
        public bool Connected { get; protected set; }
        protected BaseServoController(string name)
        {
            Name = name;
        }
        public abstract void Connect();
        public void Start()
        {
            Active = true;
        }

        public abstract void Disconnect();
        public abstract void Stop();
        public abstract void Update();
        public void Dispose()
        {
            if (Connected) Disconnect();
        }
    }
}
