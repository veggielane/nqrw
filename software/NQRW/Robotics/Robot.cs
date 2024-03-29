﻿using NQRW.Devices;
using NQRW.Devices.Input;
using NQRW.FiniteStateMachine;
using NQRW.FiniteStateMachine.States;
using NQRW.Gait;
using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Settings;
using NQRW.Timing;
using System;
using JetBrains.Annotations;
using NQRW.FiniteStateMachine.Commands;

namespace NQRW.Robotics
{
    [UsedImplicitly]
    public class Robot : BaseRobot,
        IHandle<ButtonEvent>, 
        IHandle<AxisEvent>
    {
        private readonly RobotSettings _settings;
        private readonly Angle _eggSafeAngle = Angle.FromDegrees(-30);
        private readonly Angle _eggEmptyAngle = Angle.FromDegrees(80);
        private readonly Servo _eggServo;

        public Robot(
            IMessageBus bus, 
            ITimer timer, 
            IStateMachine stateMachine,
            IServoController servoController, 
            IGaitEngine gaitEngine,
            IPlatformInput input,
            RobotSettings settings,
            Body body
            ): base("NQRW", bus, timer, input, body)
        {
            _settings = settings;
            StateMachine = stateMachine;
            ServoController = servoController;
            GaitEngine = gaitEngine;


            StateMachine.AddTransition<IdleState, StartCommand, StandingState>();
            StateMachine.AddTransition<StandingState, StartCommand, IdleState>();

            StateMachine.AddTransition<StandingState, BodyMoveCommand, BodyMoveState>();

            StateMachine.AddTransition<BodyMoveState, BodyMoveCommand, StandingState>();



            StateMachine.AddTransition<StandingState, MoveCommand, MovingState>();
            StateMachine.AddTransition<MovingState, StopCommand, StandingState>();

            StateMachine.AddTransition<StandingState, RotateCommand, RotatingState>();
            StateMachine.AddTransition<RotatingState, StopCommand, StandingState>();


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

            Body.Z = settings.Body.StartHeight;

            Body.Roll = Angle.FromDegrees(0);
            Body.Pitch = Angle.FromDegrees(0);
            Body.Yaw = Angle.FromDegrees(0);

            var leftFront = new Leg4DOF(Matrix4.Translate(-settings.Body.C, settings.Body.A, 0), new Vector3(-settings.Body.C, settings.Body.A, 0) - new Vector3(settings.Body.FootOffsetX, settings.Body.FootOffsetY, 0), settings.Legs[Leg.LeftFront]);
            var leftMiddle = new Leg4DOF(Matrix4.Translate(-settings.Body.D, 0, 0), new Vector3(-settings.Body.D, 0, 0) - new Vector3(settings.Body.FootOffsetX, 0, 0), settings.Legs[Leg.LeftMiddle]);
            var leftRear = new Leg4DOF(Matrix4.Translate(-settings.Body.E, -settings.Body.B, 0), new Vector3(-settings.Body.E, -settings.Body.B, 0) - new Vector3(settings.Body.FootOffsetX, -settings.Body.FootOffsetY, 0), settings.Legs[Leg.LeftRear]);
            var rightFront = new Leg4DOF(Matrix4.Translate(settings.Body.C, settings.Body.A, 0), new Vector3(settings.Body.C, settings.Body.A, 0) + new Vector3(settings.Body.FootOffsetX, -settings.Body.FootOffsetY, 0), settings.Legs[Leg.RightFront]);
            var rightMiddle = new Leg4DOF(Matrix4.Translate(settings.Body.D, 0, 0), new Vector3(settings.Body.D, 0, 0) + new Vector3(settings.Body.FootOffsetX, 0, 0), settings.Legs[Leg.RightMiddle]);
            var rightRear = new Leg4DOF(Matrix4.Translate(settings.Body.E, -settings.Body.B, 0), new Vector3(settings.Body.E, -settings.Body.B, 0) + new Vector3(settings.Body.FootOffsetX, settings.Body.FootOffsetY, 0), settings.Legs[Leg.RightRear]);

            Legs.Add(Leg.LeftFront, leftFront);
            Legs.Add(Leg.LeftMiddle, leftMiddle);
            Legs.Add(Leg.LeftRear, leftRear);
            Legs.Add(Leg.RightFront, rightFront);
            Legs.Add(Leg.RightMiddle, rightMiddle);
            Legs.Add(Leg.RightRear, rightRear);

            ServoController.Servos.Add(0, rightRear.TarsusServo);
            ServoController.Servos.Add(1, rightRear.TibiaServo);
            ServoController.Servos.Add(2, rightRear.FemurServo);
            ServoController.Servos.Add(3, rightRear.CoxaServo);

            ServoController.Servos.Add(4, rightMiddle.TarsusServo);
            ServoController.Servos.Add(5, rightMiddle.TibiaServo);
            ServoController.Servos.Add(6, rightMiddle.FemurServo);
            ServoController.Servos.Add(7, rightMiddle.CoxaServo);

            ServoController.Servos.Add(12, rightFront.TarsusServo);
            ServoController.Servos.Add(13, rightFront.TibiaServo);
            ServoController.Servos.Add(14, rightFront.FemurServo);
            ServoController.Servos.Add(15, rightFront.CoxaServo);

            ServoController.Servos.Add(16, leftRear.TarsusServo);
            ServoController.Servos.Add(17, leftRear.TibiaServo);
            ServoController.Servos.Add(18, leftRear.FemurServo);
            ServoController.Servos.Add(19, leftRear.CoxaServo);

            ServoController.Servos.Add(20, leftMiddle.TarsusServo);
            ServoController.Servos.Add(21, leftMiddle.TibiaServo);
            ServoController.Servos.Add(22, leftMiddle.FemurServo);
            ServoController.Servos.Add(23, leftMiddle.CoxaServo);

            ServoController.Servos.Add(28, leftFront.TarsusServo);
            ServoController.Servos.Add(29, leftFront.TibiaServo);
            ServoController.Servos.Add(30, leftFront.FemurServo);
            ServoController.Servos.Add(31, leftFront.CoxaServo);

            _eggServo = new Servo(_eggSafeAngle, Angle.Zero);

            ServoController.Servos.Add(11, _eggServo);
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

            Timer.Ticks.Subscribe(t =>
            {
                GaitEngine.Update(Legs);
                foreach (var kvp in GaitEngine.Offsets)
                {
                    Legs[kvp.Key].FootOffset = kvp.Value;
                }
                InverseKinematics();
                if(ServoController.Active) ServoController.Update();
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

            //_eggServo
            if (message.Is(PS4Button.Circle, ButtonState.Pressed))
            {
                _eggServo.Angle = _eggEmptyAngle;
            }
            if (message.Is(PS4Button.Circle, ButtonState.Released))
            {
                _eggServo.Angle = _eggSafeAngle;
            }

            if (message.Is(PS4Button.R1, ButtonState.Pressed))
            {
                _mode = WalkMode.Shuffle;
            }
            if (message.Is(PS4Button.R1, ButtonState.Released))
            {
                _mode = WalkMode.Moving;
            }
        }

        private WalkMode _mode = WalkMode.Moving;
        public void Handle(AxisEvent e)
        {
            if (e.Axis == PS4Axis.RightStickX || e.Axis == PS4Axis.RightStickY)
            {
                var x = MathsHelper.Map(e.Controller.Axes[PS4Axis.RightStickX], -32767, 32767, -1.0, 1.0);
                var y = MathsHelper.Map(e.Controller.Axes[PS4Axis.RightStickY], -32767, 32767, -1.0, 1.0);

                Bus.Add(new HeadingEvent(new Vector2(x, y), _mode));
            }
            if (e.Axis == PS4Axis.LeftStickX)
            {
                Bus.Add(new RotateEvent(MathsHelper.Map(e.Value, -32767, 32767, -1.0, 1.0)));
            }

            if (e.Axis == PS4Axis.R2)
            {
                GaitEngine.StrideHeight = MathsHelper.Map(e.Value, -32767, 32767, 70.00, 100);
            }

            if (e.Axis == PS4Axis.L2)
            {
                Body.Z = MathsHelper.Map(e.Value, -32767, 32767, _settings.Body.StartHeight, _settings.Body.StartHeight + 50);
            }
        }
    }
}
