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
        private RobotSettingsOld _settings;
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
            RobotSettings settings,
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
            _settings = new RobotSettingsOld
            {
                Legs =
                {
                    {
                        Leg.LeftFront, new LegSettings(20, 77, 73, 90)
                        {
                            CoxaOffset = Angle.FromDegrees(54.0),
                            CoxaInvert = true,
                            FemurOffset = Angle.FromDegrees(12.0),
                            FemurInvert = true,
                            TibiaOffset = Angle.FromDegrees(70.0),
                            TibiaInvert = true,
                            TarsusOffset = Angle.FromDegrees(40.0),
                            TarsusInvert = true
                        }
                    }
                }
            };

            //FemurOffset = Angle.FromDegrees(12.0),
            //TibiaOffset = Angle.FromDegrees(70.0),
            //TarsusOffset = Angle.FromDegrees(40.0)

            
            
            _settings.Legs[Leg.LeftMiddle].CoxaOffset = Angle.FromDegrees(0.0);
            _settings.Legs[Leg.LeftMiddle].CoxaInvert = true;
            _settings.Legs[Leg.LeftMiddle].FemurOffset = Angle.FromDegrees(12.0);
            _settings.Legs[Leg.LeftMiddle].FemurInvert = true;
            _settings.Legs[Leg.LeftMiddle].TibiaOffset = Angle.FromDegrees(70.0);
            _settings.Legs[Leg.LeftMiddle].TibiaInvert = true;
            _settings.Legs[Leg.LeftMiddle].TarsusOffset = Angle.FromDegrees(40.0);
            _settings.Legs[Leg.LeftMiddle].TarsusInvert = true;

            _settings.Legs[Leg.LeftRear].CoxaOffset = Angle.FromDegrees(-42.0);
            _settings.Legs[Leg.LeftRear].CoxaInvert = true;
            _settings.Legs[Leg.LeftRear].FemurOffset = Angle.FromDegrees(12.0);
            _settings.Legs[Leg.LeftRear].FemurInvert = true;
            _settings.Legs[Leg.LeftRear].TibiaOffset = Angle.FromDegrees(70.0);
            _settings.Legs[Leg.LeftRear].TibiaInvert = true;
            _settings.Legs[Leg.LeftRear].TarsusOffset = Angle.FromDegrees(30.0);
            _settings.Legs[Leg.LeftRear].TarsusInvert = true;

            _settings.Legs[Leg.RightFront].CoxaOffset = Angle.FromDegrees(-40);
            _settings.Legs[Leg.RightFront].CoxaInvert = false;
            _settings.Legs[Leg.RightFront].FemurOffset = Angle.FromDegrees(-12.0);
            _settings.Legs[Leg.RightFront].FemurInvert = false;
            _settings.Legs[Leg.RightFront].TibiaOffset = Angle.FromDegrees(-70.0);
            _settings.Legs[Leg.RightFront].TibiaInvert = false;
            _settings.Legs[Leg.RightFront].TarsusOffset = Angle.FromDegrees(-40.0);
            _settings.Legs[Leg.RightFront].TarsusInvert = false;

            _settings.Legs[Leg.RightMiddle].CoxaOffset = Angle.FromDegrees(-5);
            _settings.Legs[Leg.RightMiddle].CoxaInvert = false;
            _settings.Legs[Leg.RightMiddle].FemurOffset = Angle.FromDegrees(-12.0);
            _settings.Legs[Leg.RightMiddle].FemurInvert = false;
            _settings.Legs[Leg.RightMiddle].TibiaOffset = Angle.FromDegrees(-70.0);
            _settings.Legs[Leg.RightMiddle].TibiaInvert = false;
            _settings.Legs[Leg.RightMiddle].TarsusOffset = Angle.FromDegrees(-40.0);
            _settings.Legs[Leg.RightMiddle].TarsusInvert = false;


            _settings.Legs[Leg.RightRear].CoxaOffset = Angle.FromDegrees(44.0);
            _settings.Legs[Leg.RightRear].CoxaInvert = false;
            _settings.Legs[Leg.RightRear].FemurOffset = Angle.FromDegrees(-12.0);
            _settings.Legs[Leg.RightRear].FemurInvert = false;
            _settings.Legs[Leg.RightRear].TibiaOffset = Angle.FromDegrees(-70.0);
            _settings.Legs[Leg.RightRear].TibiaInvert = false;
            _settings.Legs[Leg.RightRear].TarsusOffset = Angle.FromDegrees(-40.0);
            _settings.Legs[Leg.RightRear].TarsusInvert = false;

            var A = 97.5;
            var B = 97.5;
            var C = 50.0;
            var D = 55.0;
            var E = 50.0;
            var F = 90.0;

            var footPosition = 100;

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



            ServoController.Servos.Add(0, _rightRear.TarsusServo);
            ServoController.Servos.Add(1, _rightRear.TibiaServo);
            ServoController.Servos.Add(2, _rightRear.FemurServo);
            ServoController.Servos.Add(3, _rightRear.CoxaServo);

            ServoController.Servos.Add(4, _rightMiddle.TarsusServo);
            ServoController.Servos.Add(5, _rightMiddle.TibiaServo);
            ServoController.Servos.Add(6, _rightMiddle.FemurServo);
            ServoController.Servos.Add(7, _rightMiddle.CoxaServo);

            ServoController.Servos.Add(12, _rightFront.TarsusServo);
            ServoController.Servos.Add(13, _rightFront.TibiaServo);
            ServoController.Servos.Add(14, _rightFront.FemurServo);
            ServoController.Servos.Add(15, _rightFront.CoxaServo);


            ServoController.Servos.Add(16, _leftRear.TarsusServo);
            ServoController.Servos.Add(17, _leftRear.TibiaServo);
            ServoController.Servos.Add(18, _leftRear.FemurServo);
            ServoController.Servos.Add(19, _leftRear.CoxaServo);

            ServoController.Servos.Add(20, _leftMiddle.TarsusServo);
            ServoController.Servos.Add(21, _leftMiddle.TibiaServo);
            ServoController.Servos.Add(22, _leftMiddle.FemurServo);
            ServoController.Servos.Add(23, _leftMiddle.CoxaServo);

            ServoController.Servos.Add(28, _leftFront.TarsusServo);
            ServoController.Servos.Add(29, _leftFront.TibiaServo);
            ServoController.Servos.Add(30, _leftFront.FemurServo);
            ServoController.Servos.Add(31, _leftFront.CoxaServo);


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
                InverseKinematics();
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
            if (message.Is(PS4Button.L1, ButtonState.Released))
            {
                Body.Z -= 5.0;
            }
            if (message.Is(PS4Button.R1, ButtonState.Released))
            {
                Body.Z += 5.0;
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
