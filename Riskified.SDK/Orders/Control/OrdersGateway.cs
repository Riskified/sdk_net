using System;
using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Orders.Model;
using Riskified.SDK.Orders.Model.OrderElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Orders.Control
{
    /// <summary>
    /// Main class to handle order creation and submittion to Riskified Servers
    /// </summary>
    public class OrdersGateway
    {
        private readonly string _riskifiedBaseWebhookUrl;
        private readonly string _authToken;
        private readonly string _shopDomain;
        
        public OrdersGateway(RiskifiedEnvironment env, string authToken, string shopDomain)
        {
            _riskifiedBaseWebhookUrl = EnvironmentsUrls.GetEnvUrl(env); 
            // TODO make sure signature and domain are of valid structure
            _authToken = authToken;
            _shopDomain = shopDomain;
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends a new order to Riskified Servers (without Submit for analysis)
        /// </summary>
        /// <param name="order">The Order to create</param>
        /// <returns>The order tranaction result containing status and order id  in riskified servers (for followup only - not used latter) in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        public OrderTransactionResult Create(Order order)
        {            
            return SendOrder(order, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/api/create"));
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends an updated order (already created) to Riskified Servers
        /// </summary>
        /// <param name="order">The Order to update</param>
        /// <returns>The order tranaction result containing status and order id  in riskified servers (for followup only - not used latter) in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        public OrderTransactionResult Update(Order order)
        {
            return SendOrder(order, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/api/update"));
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends an order to Riskified Servers and submits it for analysis
        /// </summary>
        /// <param name="order">The Order to submit</param>
        /// <returns>The order tranaction result containing status and order id  in riskified servers (for followup only - not used latter) in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        public OrderTransactionResult Submit(Order order)
        {
            return SendOrder(order, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/api/submit"));
        }

        //TODO add doc
        public OrderTransactionResult Cancel(OrderCancellation orderCancellation)
        {
            return SendOrder(orderCancellation, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/api/cancel"));
        }

        //TODO add doc
        public OrderTransactionResult Refund(OrderRefund orderRefund)
        {
            return SendOrder(orderRefund, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/api/refund"));
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends the order to riskified server endpoint as configured in the ctor
        /// </summary>
        /// <param name="order">The order object to send</param>
        /// <param name="riskifiedEndpointUrl">the endpoint to which the order should be sent</param>
        /// <returns>The order tranaction result containing status and order id  in riskified servers (for followup only - not used latter) in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        private OrderTransactionResult SendOrder(AbstractOrder order, Uri riskifiedEndpointUrl)
        {
            string jsonOrder;
            try
            {
                jsonOrder = JsonConvert.SerializeObject(new GenericOrder(order));
            }
            catch (Exception e)
            {
                throw new OrderFieldBadFormatException("The order could not be serialized to JSON: "+e.Message, e);
            }

            var transactionResult = HttpUtils.JsonPostAndParseResponseToObject<OrderTransactionResult>(riskifiedEndpointUrl, jsonOrder, _authToken, _shopDomain);
            return transactionResult;
            
        }
    }

    
}
