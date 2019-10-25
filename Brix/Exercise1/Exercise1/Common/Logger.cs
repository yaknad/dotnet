using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
