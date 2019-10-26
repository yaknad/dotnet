using System;

namespace Exercise1.Common
{
    public class Logger : ILogger
    {
        public void Error(string error)
        {
            Console.Error.WriteLine(error);
        }

        public void Info(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
