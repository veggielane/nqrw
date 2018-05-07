using NQRW.Robotics;
using System;

namespace NQRW
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var robot = RobotModule.Build<IRobot>())
            {
                robot.Bus.Messages.Subscribe(m => {
                    Console.WriteLine(m.ToString());
                });
                robot.Boot();
            }
            Console.ReadLine();
        }
    }
}
