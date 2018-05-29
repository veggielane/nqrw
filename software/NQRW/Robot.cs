using NQRW.Devices;
using NQRW.FiniteStateMachine;
using NQRW.Gait;
using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Robotics;
using NQRW.Settings;
using NQRW.Timing;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace NQRW
{
    public class Robot : BaseRobot, IHandle<ButtonEvent>, IHandle<AxisEvent>
    {
        private RobotSettings _settings = new RobotSettings();

        public Robot(
            IMessageBus bus, 
            ITimer timer, 
            IStateMachine stateMachine,
            IServoController servoController, 
            IController controller, 
            IGaitEngine gaitEngine,
            IdleState idleState, MovingState movingState): base("NQRW")
        {
            Bus = bus;
            Timer = timer;
            StateMachine = stateMachine;
            ServoController = servoController;
            Controller = controller;
            GaitEngine = gaitEngine;

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

            Body.Z = F;

            Body.Roll = Angle.FromDegrees(10);
            Body.Pitch = Angle.FromDegrees(10);
            Body.Yaw = Angle.FromDegrees(10);


            

            var LeftFront = new Leg4DOF(Matrix4.Translate(-C, A, 0), new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftFront]);
            var LeftMiddle = new Leg4DOF(Matrix4.Translate(-D, 0, 0), new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftMiddle]);
            var LeftRear = new Leg4DOF(Matrix4.Translate(-E, -B, 0), new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftRear]);
            var RightFront = new Leg4DOF(Matrix4.Translate(C, A, 0), new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightFront]);
            var RightMiddle = new Leg4DOF(Matrix4.Translate(D, 0, 0), new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightMiddle]);
            var RightRear = new Leg4DOF(Matrix4.Translate(E, -B, 0), new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightRear]);

            Legs.Add(Leg.LeftFront, LeftFront);
            Legs.Add(Leg.LeftMiddle, LeftMiddle);
            Legs.Add(Leg.LeftRear, LeftRear);
            Legs.Add(Leg.RightFront, RightFront);
            Legs.Add(Leg.RightMiddle, RightMiddle);
            Legs.Add(Leg.RightRear, RightRear);

            ServoController.Servos.Add(0, LeftFront.CoxaServo);

            //KinematicEngine.Legs.Add(Leg.LeftFront, new Leg3DOF(Matrix4.Translate(-C, A, 0)) { FootPosition = new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.LeftMiddle, new Leg3DOF(Matrix4.Translate(-D, 0, 0)) { FootPosition = new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.LeftRear, new Leg3DOF(Matrix4.Translate(-E, -B, 0)) { FootPosition = new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.RightFront, new Leg3DOF(Matrix4.Translate(C, A, 0)) { FootPosition = new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.RightMiddle, new Leg3DOF(Matrix4.Translate(D, 0, 0)) { FootPosition = new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0) });
            //KinematicEngine.Legs.Add(Leg.RightRear, new Leg3DOF(Matrix4.Translate(E, -B, 0)) { FootPosition = new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0) });
        }
        public override void Boot()
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
                foreach (var kvp in GaitEngine.Update())
                {
                    Legs[kvp.Key].FootOffset = kvp.Value;
                }
                InverseKinematics();
            });
            Timer.Start();
            Bus.Add(new SystemMessage("Timer Started"));
            Bus.Add(new SystemMessage($"{Name} Started"));
            StateMachine.Start<IdleState>();
        }

        public override void Dispose()
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
                Body.X = MathsHelper.Map(message.Value, -32767, 32767, -15, 15);
            }
            if (message.Axis == PS4Axis.LeftStickY)
            {
                Body.Y = MathsHelper.Map(message.Value, -32767, 32767, -15, 15);
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
