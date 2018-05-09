using NQRW.Maths;
using System;
using System.Collections.Generic;
using System.Text;

namespace NQRW.Messaging.Messages
{
    public class BodyMoveMessage : BaseMessage
    {
        public Matrix4 Transform { get; private set; }
        public BodyMoveMessage(Matrix4 transform)
        {
            Transform = transform;
        }
    }
}
