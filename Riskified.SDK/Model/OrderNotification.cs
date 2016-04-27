using Riskified.SDK.Model.Internal;

namespace Riskified.SDK.Model
{
    public class OrderNotification
    {
        public OrderNotification(OrderWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            Description = notificationInfo.Order.Description;
            Warnings = notificationInfo.Warnings;
        }

        public OrderNotification(OrderCheckoutWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            Description = notificationInfo.Order.Description;
            Warnings = notificationInfo.Warnings;
        }

        public string Id { get; private set; }
        public string Status { get; private set; }
        public string Description { get; private set; }
        public string[] Warnings { get; private set; }
    }
}
