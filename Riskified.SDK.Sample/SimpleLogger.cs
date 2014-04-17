using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riskified.NetSDK.Logging;

namespace Riskified.SDK.Sample
{
    public class SimpleExampleLogger : ILogger
    {

        private static void Log(string message, string level)
        {
            Console.WriteLine("\nLOG:: {0}  {1}  {2}", DateTime.Now, level, message);
        }
        public void Debug(string message)
        {
            Log(message, "DEBUG");
        }

        public void Debug(string message, Exception exception)
        {
            Debug(string.Format("{0}. Exception was: {1}. StackTrace {2}", message, exception.Message, exception.StackTrace));
        }

        public void Info(string message)
        {
            Log(message, "INFO");
        }

        public void Info(string message, Exception exception)
        {
            Info(string.Format("{0}. Exception was: message: {1}. StackTrace {2}", message, exception.Message, exception.StackTrace));
        }

        public void Error(string message)
        {
            Log(message, "ERROR");
        }

        public void Error(string message, Exception exception)
        {
            Error(string.Format("{0}. Exception was: message: {1}. StackTrace {2}", message, exception.Message, exception.StackTrace));
        }

        public void Fatal(string message)
        {
            Log(message, "FATAL");
        }

        public void Fatal(string message, Exception exception)
        {
            Fatal(string.Format("{0}. Exception was: {1} StackTrace: {2}", message, exception.Message, exception.StackTrace));
        }
    }
}
