using NQRW.Maths;
using NQRW.Messaging.Messages;
using NQRW.Robotics;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace NQRW
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var robot = RobotModule.Build<IRobot>())
            {
                robot.Bus.Messages.OfType<SystemMessage>().Subscribe(m => {
                    Console.WriteLine(m.ToString());
                });

                robot.Boot();
                ConsoleKey key;
                while ((key = Console.ReadKey().Key)!= ConsoleKey.Escape){
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            robot.Bus.Add(new BodyMoveMessage(Matrix4.Translate(0, 10, 0)));
                            break;
                        case ConsoleKey.DownArrow:
                            robot.Bus.Add(new BodyMoveMessage(Matrix4.Translate(0, -10, 0)));
                            break;
                        case ConsoleKey.LeftArrow:
                            robot.Bus.Add(new BodyMoveMessage(Matrix4.Translate(10, 0, 0)));
                            break;
                        case ConsoleKey.RightArrow:
                            robot.Bus.Add(new BodyMoveMessage(Matrix4.Translate(-10, 0, 0)));
                            break;
                        case ConsoleKey.R:
                            robot.Bus.Add(new BodyMoveMessage(Matrix4.Translate(0, 0, 0)));
                            break;
                    }
                }
            }
            
        }
    }
}
