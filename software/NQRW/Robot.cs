using NQRW.Devices;
using NQRW.FiniteStateMachine;
using NQRW.Gait;
using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Robotics;
using NQRW.Timing;
using System;
using System.Reactive.Linq;

namespace NQRW
{
    public class Robot : IRobot, IHandle<ButtonEvent>, IHandle<AxisEvent>
    {
        public string Name => "Stompy";
        public IMessageBus Bus { get; private set; }
        public ITimer Timer { get; private set; }
        public IStateMachine StateMachine { get; private set; }
        public IGaitEngine GaitEngine { get; private set; }
        public IServoController ServoController { get; private set; }
        public IController Controller { get; private set; }
        public IKinematicEngine KinematicEngine { get; private set; }

        public Robot(
            IMessageBus bus, 
            ITimer timer, 
            IStateMachine stateMachine,
            IServoController servoController, 
            IController controller, 
            IKinematicEngine kinematicEngine,
            IGaitEngine gaitEngine,
            IdleState idleState, MovingState movingState)
        {
            Bus = bus;
            Timer = timer;
            StateMachine = stateMachine;
            ServoController = servoController;
            Controller = controller;
            GaitEngine = gaitEngine;
            KinematicEngine = kinematicEngine;


            StateMachine.AddState(idleState);
            StateMachine.AddState(movingState);

            StateMachine.AddTransition<IdleState, StartCommand, MovingState>();
            StateMachine.AddTransition<MovingState, StartCommand, IdleState>();

            /*
             *          ^
             *          |
             *          X
             *    <---Y
             *    
             *    *         *         *
             *    C         D         E
             *    |----A----|----B----|
             *    C         D         E
             *    *         *         *
             * 
             * 
             *  F Body Height
             */


            var A = 200.0;
            var B = 200.0;

            var C = 150.0;
            var D = 160.0;
            var E = 150.0;
            var F = 50.0;

            var footPosition = 150.0;

            KinematicEngine.BodyZ = F;

            KinematicEngine.BodyRoll = Angle.FromDegrees(10);
            KinematicEngine.BodyPitch = Angle.FromDegrees(10);
            KinematicEngine.BodyYaw = Angle.FromDegrees(10);



            KinematicEngine.Legs.Add(Leg.LeftFront,  new Leg4DOF(Matrix4.Translate(-C, A, 0), new Vector3(-C, A, 0) - new Vector3(footPosition,0,0)));
            KinematicEngine.Legs.Add(Leg.LeftMiddle, new Leg4DOF(Matrix4.Translate(-D, 0, 0), new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0)));
            KinematicEngine.Legs.Add(Leg.LeftRear,   new Leg4DOF(Matrix4.Translate(-E, -B, 0), new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0)));
            KinematicEngine.Legs.Add(Leg.RightFront, new Leg4DOF(Matrix4.Translate(C, A, 0), new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0)));
            KinematicEngine.Legs.Add(Leg.RightMiddle,new Leg4DOF(Matrix4.Translate(D, 0, 0), new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0)));
            KinematicEngine.Legs.Add(Leg.RightRear,  new Leg4DOF(Matrix4.Translate(E, -B, 0), new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0)));

            //KinematicEngine.Legs.Add(Leg.LeftFront, new Leg3DOF(Matrix4.Translate(-C, A, 0)) { FootPosition = new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.LeftMiddle, new Leg3DOF(Matrix4.Translate(-D, 0, 0)) { FootPosition = new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.LeftRear, new Leg3DOF(Matrix4.Translate(-E, -B, 0)) { FootPosition = new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.RightFront, new Leg3DOF(Matrix4.Translate(C, A, 0)) { FootPosition = new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.RightMiddle, new Leg3DOF(Matrix4.Translate(D, 0, 0)) { FootPosition = new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.RightRear, new Leg3DOF(Matrix4.Translate(E, -B, 0)) { FootPosition = new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0) });
        }
        public void Boot()
        {
            Bus.Add(new SystemMessage($"{Name} Starting"));
            ServoController.Connect();
            if (ServoController.Connected)
            {
                Bus.Debug($"{ServoController.Name} Connected");
            }
            else
            {
                Bus.Debug($"{ServoController.Name} Not Connected");
                return;
            }
            Timer.Ticks.Subscribe(t => {
                var offsets = GaitEngine.Update();
                KinematicEngine.SetOffsets(offsets);
                KinematicEngine.Update();
            });
            Timer.Start();
            Bus.Add(new SystemMessage("Timer Started"));
            Bus.Add(new SystemMessage($"{Name} Started"));
            StateMachine.Start<IdleState>();
        }

        public void Dispose()
        {
            Controller.Dispose();
        }

        public void Handle(ButtonEvent message)
        {
            if(message.Is(PS4Button.PS, ButtonState.Released))
            {
                StateMachine.Next<StartCommand>();
            }
        }

        public void Handle(AxisEvent message)
        {
            if(message.Axis == PS4Axis.LeftStickX)
            {
                KinematicEngine.BodyX = MathsHelper.Map(message.Value, -32767, 32767, -15, 15);
            }
            if (message.Axis == PS4Axis.LeftStickY)
            {
                KinematicEngine.BodyY = MathsHelper.Map(message.Value, -32767, 32767, -15, 15);
            }
            if (message.Axis == PS4Axis.RightStickX)
            {
                GaitEngine.HeadingX = MathsHelper.Map(message.Value, -32767, 32767, -15, 15);
            }
            if (message.Axis == PS4Axis.RightStickY)
            {
                GaitEngine.HeadingY = MathsHelper.Map(message.Value, -32767, 32767, -15, 15);
            }
        }
    }
}
