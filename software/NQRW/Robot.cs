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
        private RobotSettings _settings;
        private Leg4DOF _leftFront;
        private Leg4DOF _leftMiddle;
        private Leg4DOF _leftRear;
        private Leg4DOF _rightFront;
        private Leg4DOF _rightMiddle;
        private Leg4DOF _rightRear;

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

            //StateMachine.AddTransition<StandingState, MoveCommand, MovingState>();
            //StateMachine.AddTransition<MovingState, MoveCommand, StandingState>();

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
             _settings  = new RobotSettings();
            //_settings.Legs[Leg.LeftFront].CoxaOffset = Angle.PI;
            //_settings.Legs[Leg.LeftMiddle].CoxaOffset = Angle.PI;
            //_settings.Legs[Leg.LeftRear].CoxaOffset = Angle.PI;

            _settings.Legs[Leg.LeftMiddle].CoxaOffset = Angle.FromDegrees(0.0);
            _settings.Legs[Leg.LeftMiddle].FemurOffset = Angle.FromDegrees(12);
            _settings.Legs[Leg.LeftMiddle].TibiaOffset = Angle.FromDegrees(0.0);
            _settings.Legs[Leg.LeftMiddle].TarsusOffset = Angle.FromDegrees(0.0);

            var A = 95.0;
            var B = 95.0;

            var C = 50.0;
            var D = 60.0;
            var E = 50.0;
            var F = 60.0;

            var footPosition = 100.0;

            Body.Z = F;

            Body.Roll = Angle.FromDegrees(0);
            Body.Pitch = Angle.FromDegrees(0);
            Body.Yaw = Angle.FromDegrees(0);

            _leftFront = new Leg4DOF(Matrix4.Translate(-C, A, 0), new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftFront]);
            _leftMiddle = new Leg4DOF(Matrix4.Translate(-D, 0, 0), new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftMiddle]);
            _leftRear = new Leg4DOF(Matrix4.Translate(-E, -B, 0), new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0), _settings.Legs[Leg.LeftRear]);
            _rightFront = new Leg4DOF(Matrix4.Translate(C, A, 0), new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightFront]);
            _rightMiddle = new Leg4DOF(Matrix4.Translate(D, 0, 0), new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightMiddle]);
            _rightRear = new Leg4DOF(Matrix4.Translate(E, -B, 0), new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0), _settings.Legs[Leg.RightRear]);

            Legs.Add(Leg.LeftFront, _leftFront);
            Legs.Add(Leg.LeftMiddle, _leftMiddle);
            Legs.Add(Leg.LeftRear, _leftRear);
            Legs.Add(Leg.RightFront, _rightFront);
            Legs.Add(Leg.RightMiddle, _rightMiddle);
            Legs.Add(Leg.RightRear, _rightRear);

            //ServoController.Servos.Add(20, leftMiddle.TarsusServo);

            //ServoController.Servos.Add(21, leftMiddle.TibiaServo);
            ServoController.Servos.Add(22, _leftMiddle.FemurServo);
            ServoController.Servos.Add(23, _leftMiddle.CoxaServo);

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
                //if (GaitEngine.Moving)
                //{
                //    foreach (var kvp in GaitEngine.Update())
                //    {
                //        Legs[kvp.Key].FootOffset = kvp.Value;
                //    }
                //}

                //Console.WriteLine($"kin");
                //InverseKinematics();
                //Bus.Debug(Legs[Leg.LeftMiddle].ToString());
                ServoController.Update();
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

            if (message.Is(PS4Button.Circle, ButtonState.Released))
            {
                _leftMiddle.FemurServo.Offset += Angle.FromDegrees(1);
                Console.WriteLine(_leftMiddle.FemurServo.Offset.Degrees);
            }
            if (message.Is(PS4Button.Triangle, ButtonState.Released))
            {
                _leftMiddle.FemurServo.Offset -= Angle.FromDegrees(1);
                Console.WriteLine(_leftMiddle.FemurServo.Offset.Degrees);
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
