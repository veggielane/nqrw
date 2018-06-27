using JetBrains.Annotations;

namespace NQRW.Devices
{
    [UsedImplicitly]
    public class FakeServoController : BaseServoController
    {
        public FakeServoController() : base("FakeSSC")
        {

        }
        public override void Connect()
        {
            Connected = true;
        }
        public override void Disconnect()
        {
            Connected = false;
        }
        public override void Stop()
        {

        }
        public override void Update()
        {

        }
    }
}
