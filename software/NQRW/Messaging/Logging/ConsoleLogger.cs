using System;

namespace NQRW.Messaging.Logging
{
    public class ConsoleLogger:ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
