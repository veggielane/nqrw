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
    public class Robot : IRobot, IHandle<BodyMoveMessage>, IHandle<ButtonEvent>, IHandle<AxisEvent>
    {















        public string Name => "Stompy";

        public IMessageBus Bus { get; private set; }
        public ITimer Timer { get; private set; }
        public IStateMachine StateMachine { get; private set; }
        public IBody Body { get; private set; }


        public Leg3DOF LeftFront { get; private set; }
        public Leg3DOF LeftMiddle { get; private set; }
        public Leg3DOF LeftRear { get; private set; }
        public Leg3DOF RightFront { get; private set; }
        public Leg3DOF RightMiddle { get; private set; }
        public Leg3DOF RightRear { get; private set; }

        public IKinematicEngine Kinematic { get; private set; }

        public IGaitEngine Gait { get; private set; }

        public IServoController ServoController { get; private set; }
        public IController Controller { get; private set; }

        public Robot( IMessageBus bus, ITimer timer, 
            IStateMachine stateMachine, IServoController servoController, IController controller,
            IdleState idleState, MovingState movingState)
        {
            Bus = bus;
            Timer = timer;
            StateMachine = stateMachine;
            ServoController = servoController;
            Controller = controller;

            StateMachine.AddState(idleState);
            StateMachine.AddState(movingState);

            StateMachine.AddTransition<IdleState, StartCommand, MovingState>();
            StateMachine.AddTransition<MovingState, StartCommand, IdleState>();

            Gait = new GaitEngine();

            Body = new Body();
            LeftFront = new Leg3DOF(Matrix4.Identity) { FootPosition = new Vector3(10, 10, 10) };
            LeftMiddle = new Leg3DOF(Matrix4.Identity) { FootPosition = new Vector3(10, 10, 10) };
            LeftRear = new Leg3DOF(Matrix4.Identity) { FootPosition = new Vector3(10, 10, 10) };
            RightFront = new Leg3DOF(Matrix4.Identity) { FootPosition = new Vector3(10, 10, 10) };
            RightMiddle = new Leg3DOF(Matrix4.Identity) { FootPosition = new Vector3(10, 10, 10) };
            RightRear = new Leg3DOF(Matrix4.Identity) { FootPosition = new Vector3(10, 10, 10) };

            Kinematic = new KinematicEngine(Body, LeftFront, LeftMiddle, LeftRear, RightFront, RightMiddle, RightRear);
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
            Timer.Ticks.SubSample(10).Subscribe(t => {
                //Bus.Add(new SystemMessage("Timer Tick"));
                //ServoController.Stop();
            });

            // Timer.Ticks.SubSample(10).Subscribe(t => Gait.Run(Vector2.UnitX));
            Timer.Start();
            Bus.Add(new SystemMessage("Timer Started"));
            Bus.Add(new SystemMessage($"{Name} Started"));
            StateMachine.Start<IdleState>();
        }

        public void Dispose()
        {
            Controller.Dispose();
        }

        public void Handle(BodyMoveMessage message)
        {
            //Body.Position = message.Transform;
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
                Body.X = MathsHelper.Map(message.Value, -32767, 32767, -10, 10);
            }
            if (message.Axis == PS4Axis.LeftStickY)
            {
                Body.Y = MathsHelper.Map(message.Value, -32767, 32767, -10, 10);
            }
        }
    }
}
