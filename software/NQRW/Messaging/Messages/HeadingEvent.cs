using NQRW.Gait;
using NQRW.Maths;

namespace NQRW.Messaging.Messages
{
    public class HeadingEvent : BaseMessage
    {
        public Vector2 Heading { get; }
        public WalkMode Mode { get; }

        public HeadingEvent(Vector2 heading, WalkMode mode)
        {
            Heading = heading;
            Mode = mode;
        }

        public override string ToString() => $"{base.ToString()}: {Heading}";
    }
}
