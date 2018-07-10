namespace NQRW.Messaging.Messages
{
    public class RotateEvent : BaseMessage
    {
        public double Magnitude { get; }
        public RotateEvent(double magnitude)
        {
            Magnitude = magnitude;
        }

        public override string ToString() => $"{base.ToString()}: {Magnitude}";
    }
}