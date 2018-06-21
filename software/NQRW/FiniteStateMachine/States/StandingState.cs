using NQRW.Messaging;
using NQRW.Messaging.Messages;
using System;
using System.Reactive.Linq;
using NQRW.Devices;
using NQRW.FiniteStateMachine.Commands;

namespace NQRW.FiniteStateMachine.States
{
    public class StandingState : BaseState
    {
        private readonly IServoController _servoController;

        public StandingState(IMessageBus bus, IServoController servoController) : base("Standing", bus)
        {
            _servoController = servoController;
        }

        public override void Start()
        {
            base.Start();
            _servoController.Start();
            Sub(Bus.Messages.OfType<ButtonEvent>().Subscribe(OnNext));
        }

        private void OnNext(ButtonEvent e)
        {
            if (e.Is(PS4Button.L1, ButtonState.Pressed))
            {
                Bus.Add(new BodyMoveCommand());
            }
        }

        private void Check(HeadingEvent e)
        {
            if (e.Heading.Length > 0.1)
            {
                
                Bus.Add(new MoveCommand());
            }

        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
