using NQRW.Messaging.Messages;
using NQRW.Robotics;
using System.Threading;

namespace NQRW
{
    class Program
    {
        static void Main()
        {
            var mre = new ManualResetEvent(false);
            Thread.Sleep(5000);
            using (var robot = RobotModule.Build<IRobot>())
            {
                robot.Bus.Log<SystemMessage>();
                robot.Boot();
                mre.WaitOne();
            }
        }
    }
}
