using NQRW.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NQRW.Messaging.Messages
{
    public class ButtonEvent : BaseMessage
    {
        public PS4Button Button { get; }
        public ButtonState State { get; }
        public ButtonEvent(PS4Button button, ButtonState state)
        {
            Button = button;
            State = state;
        }

        public bool Is(PS4Button button, ButtonState state)
        {
            return button == Button && state == State;
        }
        public override string ToString()
        {
            return $"{Timestamp}: {Button} - {State}";
        }
    }
    public class AxisEvent : BaseMessage
    {

        public AxisEvent(PS4Axis axis, short value )
        {
            Axis = axis;
            Value = value;
        }
        public override string ToString()
        {
            return $"{Timestamp}: {Axis} - {Value}";
        }

        public PS4Axis Axis { get; }
        public short Value { get; }
    }
}
