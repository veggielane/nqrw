using NQRW.Devices;
using NQRW.FiniteStateMachine;
using NQRW.Gait;
using NQRW.Kinematics;
using NQRW.Messaging;
using NQRW.Timing;
using System.Collections.Generic;

namespace NQRW.Robotics
{
    public abstract class BaseRobot : IRobot
    {
        public string Name { get; }
        public IMessageBus Bus { get; protected set; }
        public ITimer Timer { get; protected set; }


        public IStateMachine StateMachine { get; protected set; }
        public IGaitEngine GaitEngine { get; protected set; }
        public IServoController ServoController { get; protected set; }
        public IController Controller { get; protected set; }

        public Dictionary<Leg, ILeg> Legs { get; set; } = new Dictionary<Leg, ILeg>();
        public IBody Body { get; set; } = new Body();

        public abstract void Boot();
        public abstract void Dispose();

        public void InverseKinematics()
        {
            var body = Body.Position;
            foreach (var leg in Legs.Values)
            {
                leg.Update(body);
            }
        }

        protected BaseRobot(string name)
        {
            Name = name;
        }
    }

}
