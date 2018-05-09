using Autofac;
using Autofac.Core;
using NQRW.Devices;
using NQRW.FiniteStateMachine;
using NQRW.Messaging;
using NQRW.Robotics;
using NQRW.Timing;
using System;
using System.Runtime.InteropServices;

namespace NQRW
{
    public class RobotModule : Module
    {
       
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Activated += OnComponentActivated;
        }
        static void OnComponentActivated(object sender, ActivatedEventArgs<object> e)
        {
            if (e == null) return;
            if (e.Instance is IHandle handler)
                e.Context.Resolve<IMessageBus>().Handle(handler);
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Robot>().As<IRobot>().SingleInstance();

            builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
            builder.RegisterType<Timer>().As<ITimer>().SingleInstance();
            builder.RegisterType<StateMachine>().As<IStateMachine>().SingleInstance();

            builder.RegisterType<MovingState>().AsSelf().SingleInstance();
            builder.RegisterType<IdleState>().AsSelf().SingleInstance();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine($"Detecting OS: Windows");
                builder.RegisterType<FakeServoController>().As<IServoController>().SingleInstance();
                builder.RegisterType<KeyboardController>().As<IController>().SingleInstance();
            }
            else
            {
                Console.WriteLine($"Detecting OS: Linux");
                builder.RegisterType<LinuxSSC32>().As<IServoController>().SingleInstance();
                builder.RegisterType<LinuxController>().As<IController>().SingleInstance();
            }

        }

        public static T Build<T>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RobotModule());
            var container = builder.Build();
            return container.Resolve<T>();
        }
    }
}
