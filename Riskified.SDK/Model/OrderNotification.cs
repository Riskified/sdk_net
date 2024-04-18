using Riskified.SDK.Model.Internal;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Model.PolicyElements;

namespace Riskified.SDK.Model
{
    public class OrderNotification
    {
        internal OrderNotification(OrderWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            OldStatus = notificationInfo.Order.OldStatus;
            Description = notificationInfo.Order.Description;
            Custom = notificationInfo.Order.Custom;
            Category = notificationInfo.Order.Category;
            DecisionCode = notificationInfo.Order.DecisionCode;
            Warnings = notificationInfo.Warnings;
            PolicyProtect = notificationInfo.Order.PolicyProtect;


        }

        internal OrderNotification(OrderCheckoutWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            OldStatus = notificationInfo.Order.OldStatus;
            Description = notificationInfo.Order.Description;
            Custom = notificationInfo.Order.Custom;
            Category = notificationInfo.Order.Category;
            Warnings = notificationInfo.Warnings;
            Score = notificationInfo.Order.Score;
            Action = notificationInfo.Order.Action;
            AuthenticationType = notificationInfo.Order.AuthenticationType;
            PolicyProtect = notificationInfo.Order.PolicyProtect;

            //PolicyProtect = notificationInfo.Order.polc
        }

        public string Id { get; private set; }
        public string Status { get; private set; }
        public string OldStatus { get; private set; }
        public string Description { get; private set; }
        public Custom Custom { get; private set; }
        public string Category { get; private set; }
        public string Action { get; private set; }
        public string DecisionCode { get; private set; }
        public string[] Warnings { get; private set; }
        public int Score { get; set; }
        public AuthenticationType AuthenticationType { get; private set; }
        public PolicyProtect PolicyProtect { get; private set; }
    }
}
