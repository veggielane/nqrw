using NQRW.Devices;

namespace NQRW.Messaging.Messages
{
    public class ButtonEvent : BaseMessage
    {
        public PS4Controller Controller { get; }
        public PS4Button Button { get; }
        public ButtonState State { get; }
        public ButtonEvent(PS4Controller controller, PS4Button button, ButtonState state)
        {
            Controller = controller;
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
}
