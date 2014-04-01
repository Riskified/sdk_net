using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Logging
{
    internal class LogWrapper : ILogger
    {
        private static ILogger _loggerProxy;
        private static LogWrapper _sdkLogger;

        public static LogWrapper GetInstance()
        {
            if (_sdkLogger == null)
                _sdkLogger = new LogWrapper();
            return _sdkLogger;
        }

        private LogWrapper()
        {
        }

        public static void InitializeLogger(ILogger logger)
        {
            _loggerProxy = logger;
        }

        public void Debug(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Debug(message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Debug(message,exception);
            }
        }

        public void Info(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Info(message);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Info(message,exception);
            }
        }

        public void Error(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Error(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Error(message,exception);
            }
        }

        public void Fatal(string message)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Fatal(message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (_loggerProxy != null)
            {
                _loggerProxy.Fatal(message,exception);
            }
        }
    }
}
