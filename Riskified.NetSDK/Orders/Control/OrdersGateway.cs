using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Riskified.SDK.Logging;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;
using Riskified.SDK.Orders.Model;

namespace Riskified.SDK.Orders
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
            _riskifiedBaseWebhookUrl = Utils.EnvironmentsUrls.GetEnvUrl(env); 
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
            return SendOrder(order, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/webhooks/create"));
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
            return SendOrder(order, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/webhooks/update"));
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
            return SendOrder(order, HttpUtils.BuildUrl(_riskifiedBaseWebhookUrl, "/webhooks/submit"));
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
        private OrderTransactionResult SendOrder(Order order, Uri riskifiedEndpointUrl)
        {
            string jsonOrder;
            try
            {
                jsonOrder = JsonConvert.SerializeObject(order);
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
