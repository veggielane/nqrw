using NQRW.Devices;
using NQRW.Devices.Input;
using NQRW.Maths;

namespace NQRW.Messaging.Messages
{
    public class AxisEvent : BaseMessage
    {

        public AxisEvent(PS4Controller controller, PS4Axis axis, short value )
        {
            Controller = controller;
            Axis = axis;
            Value = value;

        }

        public override string ToString() => $"{base.ToString()}: {Axis} - {Value}";

        public PS4Controller Controller { get; }
        public PS4Axis Axis { get; }
        public short Value { get; }
        public double UnitValue => MathsHelper.Map(Value, -32767, 32767, -1.0, 1.0);
    }
}
