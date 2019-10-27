using System.Threading;

namespace Exercise1.Common
{
    // to prevent the direct use of Thread.Sleep which is not testable
    public interface ISleepService
    {
        void Sleep(int intervalImMilliseconds);
    }
}
