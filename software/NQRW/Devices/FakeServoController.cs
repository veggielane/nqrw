using System;

namespace NQRW.Devices
{
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

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {

        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
