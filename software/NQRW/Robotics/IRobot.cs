using NQRW.Gait;
using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Timing;
using System;
using System.Collections.Generic;
using System.Text;

namespace NQRW.Robotics
{
    public interface IRobot : IDisposable
    {
        String Name { get; }
        IMessageBus Bus { get; }
        ITimer Timer { get; }

        IGaitEngine GaitEngine { get; }

        void Boot();

        Dictionary<Leg, ILeg> Legs { get; set; }
        IBody Body { get; set; }

        void InverseKinematics();
    }

}
