using System;

namespace Riskified.SDK.Logging
{
    public static class LoggingServices
    {
        private static ILogger _loggerProxy;
        //private static LoggingServices _sdkLogger;
        /*
        public static LoggingServices GetInstance()
        {
            return _sdkLogger ?? (_sdkLogger = new LoggingServices());
        }
        */

        public static void InitializeLogger(ILogger logger)
        {
            if(logger != null)
                _loggerProxy = logger;
        }

        public static void Debug(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Debug(message);
            }
        }

        public static void Debug(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Debug(message, exception);
            }
        }

        public static void Info(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Info(message);
            }
        }

        public static void Info(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Info(message, exception);
            }
        }

        public static void Error(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Error(message);
            }
        }

        public static void Error(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Error(message, exception);
            }
        }

        public static void Fatal(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Fatal(message);
            }
        }

        public static void Fatal(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Fatal(message, exception);
            }
        }
    }
}
