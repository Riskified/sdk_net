namespace Riskified.SDK.Exceptions
{
    public class NotifierAlreadyRunningException : RiskifiedException
    {
        public NotifierAlreadyRunningException(string message) : base(message)
        {
        }
    }
}
