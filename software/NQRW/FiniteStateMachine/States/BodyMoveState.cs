using System;
using System.Reactive.Linq;
using NQRW.Devices;
using NQRW.FiniteStateMachine.Commands;
using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Settings;

namespace NQRW.FiniteStateMachine.States
{
    public class BodyMoveState : BaseState
    {
        private double delta = 25.0;
        private readonly Body _body;
        private readonly RobotSettings _setting;
        private IDisposable _sub;
        public BodyMoveState(IMessageBus bus, Body body, RobotSettings setting) : base("BodyMove", bus)
        {
            _body = body;
            _setting = setting;
        }
        public override void Start()
        {
            base.Start();
            _sub = Bus.Messages.OfType<ButtonEvent>().Subscribe(OnNext);
            _sub = Bus.Messages.OfType<AxisEvent>().Subscribe(OnNext);
        }

        private void OnNext(AxisEvent e)
        {
            if (e.Axis == PS4Axis.LeftStickX)
            {
                _body.X = MathsHelper.Map(e.Value, -32767, 32767, -delta, delta);
            }
            if (e.Axis == PS4Axis.LeftStickY)
            {
                _body.Y = MathsHelper.Map(e.Value, -32767, 32767, -delta, delta);
            }
            if (e.Axis == PS4Axis.RightStickX)
            {
                _body.Pitch = Angle.FromDegrees(MathsHelper.Map(e.Value, -32767, 32767, -delta, delta));
            }
            if (e.Axis == PS4Axis.RightStickY)
            {
                _body.Roll = Angle.FromDegrees(MathsHelper.Map(e.Value, -32767, 32767, -delta, delta));
            }

            if (e.Axis == PS4Axis.DPadY)
            {
                if (e.Value == 32767) _body.Z = _body.Z - 5 ;
                if (e.Value == -32767) _body.Z = _body.Z + 5;
            }

            if (e.Axis == PS4Axis.DPadX)
            {
                if (e.Value == 32767) _body.Yaw += Angle.FromDegrees(1);
                if (e.Value == -32767) _body.Yaw += Angle.FromDegrees(-1);

            }
        }

        public override void Stop()
        {
            _sub?.Dispose();
            base.Stop();
        }

        private void OnNext(ButtonEvent e)
        {
            if (e.Is(PS4Button.L1, ButtonState.Released))
            {
                Bus.Add(new BodyMoveCommand());
            }
            if (e.Is(PS4Button.Options, ButtonState.Released))
            {
                _body.Reset(_setting.Body.StartHeight);
            }
            
        }
    }
}