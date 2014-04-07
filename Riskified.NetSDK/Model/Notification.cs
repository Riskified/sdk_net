namespace Riskified.NetSDK.Model
{
    public struct Notification
    {
        /// <summary>
        /// A unique order ID received from the Riskified server for later submittion or notification regarding that specific order
        /// </summary>
        public int OrderId { get; private set; }

        /// <summary>
        /// Textual value 
        /// </summary>
        public OrderStatus Status { get; private set; }

        public Notification(int orderId, OrderStatus status) : this()
        {
            OrderId = orderId;
            Status = status;
        }
    }
}
