using NQRW.Maths;

namespace NQRW.Messaging.Messages
{
    public class BodyMoveMessage : BaseMessage
    {
        public Matrix4 Transform { get; private set; }
        public BodyMoveMessage(Matrix4 transform)
        {
            Transform = transform;
        }
        public override string ToString()
        {
            return $"{Timestamp}: {Transform}";
        }
    }
}
