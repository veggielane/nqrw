using NQRW.Devices;

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
        public override string ToString()
        {
            return $"{Timestamp}: {Axis} - {Value}";
        }

        public PS4Controller Controller { get; }
        public PS4Axis Axis { get; }
        public short Value { get; }
    }
}
