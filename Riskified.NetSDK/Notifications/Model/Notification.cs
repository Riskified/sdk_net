namespace Riskified.NetSDK.Notifications
{
    public class Notification
    {
        /// <summary>
        /// A unique order ID received from the Riskified server for later submittion or notification regarding that specific order
        /// </summary>
        public int OrderId { get; private set; }

        /// <summary>
        /// Textual value 
        /// </summary>
        public OrderStatus Status { get; private set; }

        /// <summary>
        /// A human readable description of the status received
        /// </summary>
        public string Description { get; private set; }

        public Notification(int orderId, OrderStatus status, string description)
        {
            OrderId = orderId;
            Status = status;
            Description = description;
        }
    }
}
