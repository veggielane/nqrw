using NQRW.Devices;
using NQRW.Devices.Input;
using NQRW.FiniteStateMachine;
using NQRW.FiniteStateMachine.States;
using NQRW.Gait;
using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Robotics;
using NQRW.Settings;
using NQRW.Timing;
using System;
using NQRW.FiniteStateMachine.Commands;

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
            IGaitEngine gaitEngine,
            IInputMapping inputMapping,
            IdleState idleState,
            MovingState movingState,
            StandingState standingState): base("NQRW")
        {
            Bus = bus;
            Timer = timer;
            StateMachine = stateMachine;
            ServoController = servoController;
            GaitEngine = gaitEngine;

            StateMachine.AddState(idleState);
            StateMachine.AddState(movingState);
            StateMachine.AddState(standingState);


            StateMachine.AddTransition<IdleState, StartCommand, StandingState>();
            StateMachine.AddTransition<StandingState, StartCommand, IdleState>();

            StateMachine.AddTransition<StandingState, MoveCommand, MovingState>();
            StateMachine.AddTransition<MovingState, MoveCommand, StandingState>();

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

            Body.Roll = Angle.FromDegrees(0);
            Body.Pitch = Angle.FromDegrees(0);
            Body.Yaw = Angle.FromDegrees(0);

            var leftFront = new Leg4DOF(Matrix4.Translate(-C, A, 0), new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftFront]);
            var leftMiddle = new Leg4DOF(Matrix4.Translate(-D, 0, 0), new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftMiddle]);
            var leftRear = new Leg4DOF(Matrix4.Translate(-E, -B, 0), new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftRear]);
            var rightFront = new Leg4DOF(Matrix4.Translate(C, A, 0), new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightFront]);
            var rightMiddle = new Leg4DOF(Matrix4.Translate(D, 0, 0), new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightMiddle]);
            var rightRear = new Leg4DOF(Matrix4.Translate(E, -B, 0), new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightRear]);

            Legs.Add(Leg.LeftFront, leftFront);
            Legs.Add(Leg.LeftMiddle, leftMiddle);
            Legs.Add(Leg.LeftRear, leftRear);
            Legs.Add(Leg.RightFront, rightFront);
            Legs.Add(Leg.RightMiddle, rightMiddle);
            Legs.Add(Leg.RightRear, rightRear);

            ServoController.Servos.Add(0, leftFront.CoxaServo);
            InputMapping = inputMapping;
        }

        public override void Boot()
        {
            Bus.System($"{Name} Starting");
            ServoController.Connect();
            if (ServoController.Connected)
            {
                Bus.System($"{ServoController.Name} Connected");
            }
            else
            {
                Bus.System($"{ServoController.Name} Not Connected");
                return;
            }

            Timer.Ticks.Subscribe(t => {
                if (GaitEngine.Moving)
                {
                    foreach (var kvp in GaitEngine.Update())
                    {
                        Legs[kvp.Key].FootOffset = kvp.Value;
                    }
                }
                InverseKinematics();
            });


            Timer.Start();
            Bus.System("Timer Started");
            Bus.System($"{Name} Started");
            StateMachine.Start<IdleState>();
        }

        public override void Dispose()
        {
  
        }

        public void Handle(ButtonEvent message)
        {
            if(message.Is(PS4Button.PS, ButtonState.Released))
            {
                Bus.Add(new StartCommand());
            }
        }

        public void Handle(AxisEvent e)
        {
            if(e.Axis == PS4Axis.LeftStickX)
            {
                Body.X = MathsHelper.Map(e.Value, -32767, 32767, -15, 15);
            }
            if (e.Axis == PS4Axis.LeftStickY)
            {
                Body.Y = MathsHelper.Map(e.Value, -32767, 32767, -15, 15);
            }

            if (e.Axis == PS4Axis.RightStickX || e.Axis == PS4Axis.RightStickY)
            {
                var x = MathsHelper.Map(e.Controller.Axes[PS4Axis.RightStickX], -32767, 32767, -1.0, 1.0);
                var y = MathsHelper.Map(e.Controller.Axes[PS4Axis.RightStickY], -32767, 32767, -1.0, 1.0);
                Bus.Add(new HeadingEvent(new Vector2(x, y)));
            }
        }
    }
}
