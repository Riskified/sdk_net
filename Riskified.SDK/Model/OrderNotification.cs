using Riskified.SDK.Model.Internal;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model
{
    public class OrderNotification
    {
        internal OrderNotification(OrderWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            Description = notificationInfo.Order.Description;
            Custom = notificationInfo.Order.Custom;
            Warnings = notificationInfo.Warnings;
        }

        internal OrderNotification(OrderCheckoutWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            Description = notificationInfo.Order.Description;
            Custom = notificationInfo.Order.Custom;
            Warnings = notificationInfo.Warnings;
        }

        public string Id { get; private set; }
        public string Status { get; private set; }
        public string Description { get; private set; }
        public Custom Custom { get; private set; }
        public string[] Warnings { get; private set; }
    }
}
