using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Core
{
    public class RolodexLogger<T> : ILogger<T>
    {
        //public static ILoggerFactory LoggerFactory { get; set; }

        private ILogger<T> instance;

        public RolodexLogger(ILoggerFactory loggerFactory)
        {
            //if(LoggerFactory == null)
            //{
            //    LoggerFactory = new LoggerFactory();
            //}

            instance = loggerFactory.CreateLogger<T>();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            instance.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return instance.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return instance.BeginScope<TState>(state);
        }
    }
}
