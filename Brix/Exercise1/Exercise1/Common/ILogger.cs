using System;

namespace Exercise1.Common
{
    public interface ILogger
    {
        void Info(String msg);
        void Error(String error);
    }
}
