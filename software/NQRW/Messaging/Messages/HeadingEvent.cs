using NQRW.Maths;

namespace NQRW.Messaging.Messages
{
    public class HeadingEvent : BaseMessage
    {
        public Vector2 Heading { get; }
        public HeadingEvent(Vector2 heading)
        {
            Heading = heading;
        }

        public override string ToString() => $"{base.ToString()}: {Heading}";
    }
}
