using Autofac;
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
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Robot>().As<IRobot>().SingleInstance();

            builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
            builder.RegisterType<Timer>().As<ITimer>().SingleInstance();
            builder.RegisterType<StateMachine>().As<IStateMachine>().SingleInstance();

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                Console.WriteLine($"Detecting OS: Windows");
                builder.RegisterType<FakeServoController>().As<IServoController>().SingleInstance();
            }
            else
            {
                Console.WriteLine($"Detecting OS: Linux");
                builder.RegisterType<LinuxSSC32>().As<IServoController>().SingleInstance();
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
