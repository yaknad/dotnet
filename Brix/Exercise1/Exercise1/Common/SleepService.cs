using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1.Common
{
    // to prevent the direct use of Thread.Sleep which is not testable
    public class SleepService
    {
        public void Sleep(int intervalImMilliseconds)
        {
            Thread.Sleep(intervalImMilliseconds);
        }
    }
}
