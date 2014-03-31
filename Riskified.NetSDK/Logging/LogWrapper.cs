using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Logging
{
    internal class LogWrapper : ILogger
    {
        private ILogger _logger;

        public LogWrapper(ILogger logger)
        {
            _logger = logger;
        }

        public void Debug(string message)
        {
            if (_logger != null)
            {
                _logger.Debug(message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (_logger != null)
            {
                _logger.Debug(message,exception);
            }
        }

        public void Info(string message)
        {
            if (_logger != null)
            {
                _logger.Info(message);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (_logger != null)
            {
                _logger.Info(message,exception);
            }
        }

        public void Error(string message)
        {
            if (_logger != null)
            {
                _logger.Error(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (_logger != null)
            {
                _logger.Error(message,exception);
            }
        }

        public void Fatal(string message)
        {
            if (_logger != null)
            {
                _logger.Fatal(message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (_logger != null)
            {
                _logger.Fatal(message,exception);
            }
        }
    }
}
