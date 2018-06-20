using NQRW.Devices.Input;
using NQRW.FiniteStateMachine;
using NQRW.Gait;
using NQRW.Kinematics;
using NQRW.Messaging;
using NQRW.Timing;
using System;
using System.Collections.Generic;

namespace NQRW.Robotics
{
    public interface IRobot : IDisposable
    {
        String Name { get; }
        IMessageBus Bus { get; }
        ITimer Timer { get; }
        IGaitEngine GaitEngine { get; }
        IStateMachine StateMachine { get;  }
        IPlatformInput Input { get; }


        Dictionary<Leg, ILeg> Legs { get; set; }
        IBody Body { get; set; }
        void Boot();
        void InverseKinematics();
    }

}
