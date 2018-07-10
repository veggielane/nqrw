using Autofac;
using Autofac.Core;
using NQRW.Devices;
using NQRW.Devices.Input;
using NQRW.FiniteStateMachine;
using NQRW.Gait;
using NQRW.Messaging;
using NQRW.Robotics;
using NQRW.Timing;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using NQRW.Kinematics;
using NQRW.Messaging.Logging;
using NQRW.Settings;
using Module = Autofac.Module;

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
            builder.RegisterType<Body>().AsSelf().SingleInstance();
            builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
            builder.RegisterType<Timer>().As<ITimer>().SingleInstance();
            builder.RegisterType<StateMachine>().As<IStateMachine>().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => typeof(IState).IsAssignableFrom(t)).AsImplementedInterfaces();
            builder.RegisterType<GaitEngine>().As<IGaitEngine>().SingleInstance();
            builder.Register(ctx => RobotSettings.LoadFromFile("settings.json")).SingleInstance();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine($"Detecting OS: Windows");
                builder.RegisterType<FakeServoController>().As<IServoController>().SingleInstance();
                builder.RegisterType<WindowsInputMapping>().As<IPlatformInput>().SingleInstance();
                builder.RegisterType<ConsoleLogger>().As<ILogger>().SingleInstance();
            }
            else
            {
                Console.WriteLine($"Detecting OS: Linux");
                builder.RegisterType<LinuxSSC32>().As<IServoController>().SingleInstance();
                builder.RegisterType<PS4Controller>().AsSelf().SingleInstance();
                builder.RegisterType<LinuxInputMapping>().As<IPlatformInput>().SingleInstance();
                builder.RegisterType<UDPLogger>().As<ILogger>().SingleInstance();
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
