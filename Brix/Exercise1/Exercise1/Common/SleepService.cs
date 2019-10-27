using System.Threading;

namespace Exercise1.Common
{
    // to prevent the direct use of Thread.Sleep which is not testable
    public class SleepService : ISleepService
    {
        public void Sleep(int intervalImMilliseconds)
        {
            Thread.Sleep(intervalImMilliseconds);
        }
    }
}
