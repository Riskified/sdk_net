using System;
using System.Linq;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Model;
using Riskified.SDK.Utils;
using System.Collections.Generic;
using Riskified.SDK.Model.Internal;

namespace Riskified.SDK.Orders
{
    /// <summary>
    /// Main class to handle order creation and submittion to Riskified Servers
    /// </summary>
    public class OrdersGateway
    {
        private readonly RiskifiedEnvironment _env;
        private readonly string _authToken;
        private readonly string _shopDomain;
        private readonly Validations _validationMode;

        /// <summary>
        /// Creates the mediator class used to send order data to Riskified
        /// </summary>
        /// <param name="env">The Riskified environment to send to</param>
        /// <param name="authToken">The merchant's auth token</param>
        /// <param name="shopDomain">The merchant's shop domain</param>
        public OrdersGateway(RiskifiedEnvironment env, string authToken, string shopDomain) : this(env,authToken,shopDomain,Validations.All)
        {            
        }

        /// <summary>
        /// Old version - Deprecated
        /// Creates the mediator class used to send order data to Riskified        
        /// </summary>
        /// <param name="env">The Riskified environment to send to</param>
        /// <param name="authToken">The merchant's auth token</param>
        /// <param name="shopDomain">The merchant's shop domain</param>
        /// <param name="shouldUseWeakValidation">Should weakly validate before sending</param>
        public OrdersGateway(RiskifiedEnvironment env, string authToken, string shopDomain, bool shouldUseWeakValidation)
            : this(env, authToken, shopDomain, shouldUseWeakValidation ? Validations.Weak : Validations.All)
        {
        }

        /// <summary>
        /// Creates the mediator class used to send order data to Riskified
        /// </summary>
        /// <param name="env">The Riskified environment to send to</param>
        /// <param name="authToken">The merchant's auth token</param>
        /// <param name="shopDomain">The merchant's shop domain</param>
        /// <param name="validationMode">Validation mode to use</param>
        public OrdersGateway(RiskifiedEnvironment env, string authToken, string shopDomain, Validations validationMode)
        {
            _env = env;
            _authToken = authToken;
            _shopDomain = shopDomain;
            _validationMode = validationMode;
        }

        /// <summary>
        /// Validates the Order checkout object fields (All fields except merchendOrderId are optional)
        /// Sends a new order checkout to Riskified Servers (without Submit for analysis)
        /// </summary>
        /// <param name="order">The Order checkout to create</param>
        /// <returns>The order checkout notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Checkout(OrderCheckout orderCheckout)
        {
            return SendOrderCheckout(orderCheckout, HttpUtils.BuildUrl(_env, "/api/checkout_create"));
        }

        /// <summary>
        /// Validates the Order checkout object fields (All fields except merchendOrderId are optional)
        /// Sends a new order checkout to Riskified Servers (without Submit for analysis)
        /// </summary>
        /// <param name="order">The Order checkout to create</param>
        /// <returns>The order checkout notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification CheckoutDenied(OrderCheckoutDenied orderCheckout)
        {
            return SendOrderCheckout(orderCheckout, HttpUtils.BuildUrl(_env, "/api/checkout_denied"));
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends a new order to Riskified Servers (without Submit for analysis)
        /// </summary>
        /// <param name="order">The Order to create</param>
        /// <returns>The order notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Create(Order order)
        {            
            return SendOrder(order, HttpUtils.BuildUrl(_env, "/api/create"));
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends an updated order (already created) to Riskified Servers
        /// </summary>
        /// <param name="order">The Order to update</param>
        /// <returns>The order notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Update(Order order)
        {
            return SendOrder(order, HttpUtils.BuildUrl(_env, "/api/update"));
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends an order to Riskified Servers and submits it for analysis
        /// </summary>
        /// <param name="order">The Order to submit</param>
        /// <returns>The order notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Submit(Order order)
        {
            return SendOrder(order, HttpUtils.BuildUrl(_env, "/api/submit"));
        }

        /// <summary>
        /// Validates the Order object fields
        /// Send an Order to Riskified, will be synchronously reviewed based on current plan
        /// </summary>
        /// <param name="order">The Order to make a synchronous decision (sync plan only)</param>
        /// <returns>The order notification result containing status, decision, description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Decide(Order order)
        {
            return SendOrder(order, HttpUtils.BuildUrl(_env, "/api/decide", FlowStrategy.Sync));
        }

        /// <summary>
        /// Validates the cancellation data
        /// Sends a cancellation message for a specific order (id should already exist) to Riskified server for status and charge fees update
        /// </summary>
        /// <param name="orderCancellation"></param>
        /// <returns>The order notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Cancel(OrderCancellation orderCancellation)
        {
            return SendOrder(orderCancellation, HttpUtils.BuildUrl(_env, "/api/cancel"));
        }

        /// <summary>
        /// Validates the partial refunds data for an order
        /// Sends the partial refund data for an order to Riskified server for status and charge fees update
        /// </summary>
        /// <param name="orderPartialRefund"></param>
        /// <returns>The order notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification PartlyRefund(OrderPartialRefund orderPartialRefund)
        {
            return SendOrder(orderPartialRefund, HttpUtils.BuildUrl(_env, "/api/refund"));
        }

        /// <summary>
        /// Validates the cancellation data
        /// Sends a cancellation message for a specific order (id should already exist) to Riskified server for status and charge fees update
        /// </summary>
        /// <param name="orderCancellation"></param>
        /// <returns>The order notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Fulfill(OrderFulfillment orderFulfillment)
        {
            return SendOrder(orderFulfillment, HttpUtils.BuildUrl(_env, "/api/fulfill"));
        }

        /// <summary>
        /// Validates the decision data
        /// Sends a decision message for a specific order (id should already exist) to Riskified server.
        /// Update existing order external status. Lets Riskified know what was your decision on your order.
        /// </summary>
        /// <param name="OrderDecision">The decision details</param>
        /// <returns>The order notification result containing status,description and sent order id in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public OrderNotification Decision(OrderDecision orderDecision)
        {
            return SendOrder(orderDecision, HttpUtils.BuildUrl(_env, "/api/decision"));
        }

        public OrderNotification Chargeback(OrderChargeback orderChargeback)
        {
            return SendOrder(orderChargeback, HttpUtils.BuildUrl(_env, "/api/chargeback"));
        }

        /// <summary>
        /// Validates the list of historical orders and sends them in batches to Riskified Servers.
        /// The FinancialStatus field of each order should contain the latest order status as described at "http://apiref.riskified.com/net/#actions-historical"
        /// 
        /// </summary>
        /// <param name="order">The Orders to send</param>
        /// <param name="failedOrders">When the method returns false, contains a mapping from order_id (key) to error message (value), otherwise will be null</param>
        /// <returns>True if all orders were sent successfully, false if one or more failed due to bad format or tranfer error</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of an order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public bool SendHistoricalOrders(IEnumerable<Order> orders,out Dictionary<string,string> failedOrders)
        {
            const byte batchSize = 10;

            if(orders == null)
            {
                failedOrders=null;
                return true;
            }

            Dictionary<string, string> errors = new Dictionary<string, string>();
            var riskifiedEndpointUrl = HttpUtils.BuildUrl(_env, "/api/historical");

            List<Order> batch = new List<Order>(batchSize);
            var enumerator = orders.GetEnumerator();
            do
            {
                batch.Clear();
                while (batch.Count < batchSize && enumerator.MoveNext())
                {
                    // validate orders and assign to next batch until full
                    Order order = enumerator.Current;
                    try
                    {
                        if (_validationMode != Validations.Skip)
                        {
                            order.Validate(_validationMode);
                        }
                        batch.Add(order);
                    }
                    catch (OrderFieldBadFormatException e)
                    {
                        errors.Add(order.Id, e.Message);
                    }
                }
                if (batch.Count > 0)
                {
                    // send batch
                    OrdersWrapper wrappedOrders = new OrdersWrapper(batch);
                    try
                    {
                        HttpUtils.JsonPostAndParseResponseToObject<OrdersWrapper>(riskifiedEndpointUrl, wrappedOrders, _authToken, _shopDomain);
                    }
                    catch (RiskifiedTransactionException e)
                    {
                        batch.ForEach(o => errors.Add(o.Id, e.Message));
                    }
                }
            } while (batch.Count == batchSize);

            if(errors.Count == 0)
            {
                failedOrders = null;
                return true;
            }
            failedOrders = errors;
            return false;
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends the order to riskified server endpoint as configured in the ctor
        /// </summary>
        /// <param name="order">The order object to send</param>
        /// <param name="riskifiedEndpointUrl">the endpoint to which the order should be sent</param>
        /// <returns>The order tranaction result containing status and order id  in riskified servers (for followup only - not used latter) in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        private OrderNotification SendOrder(AbstractOrder order, Uri riskifiedEndpointUrl)
        {
            if(_validationMode != Validations.Skip)
            {
                order.Validate(_validationMode);
            }
            var wrappedOrder = new OrderWrapper<AbstractOrder>(order);
            var transactionResult = HttpUtils.JsonPostAndParseResponseToObject<OrderWrapper<Notification>, OrderWrapper<AbstractOrder>>(riskifiedEndpointUrl, wrappedOrder, _authToken, _shopDomain);
            return new OrderNotification(transactionResult);
            
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends the order to riskified server endpoint as configured in the ctor
        /// </summary>
        /// <param name="order">The order checkout object to send</param>
        /// <param name="riskifiedEndpointUrl">the endpoint to which the order should be sent</param>
        /// <returns>The order tranaction result containing status and order id  in riskified servers (for followup only - not used latter) in case of successful transfer</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        private OrderNotification SendOrderCheckout(AbstractOrder orderCheckout, Uri riskifiedEndpointUrl)
        {
            if (_validationMode != Validations.Skip)
            {
                orderCheckout.Validate(_validationMode);
            }
            var wrappedOrder = new OrderCheckoutWrapper<AbstractOrder>(orderCheckout);
            var transactionResult = HttpUtils.JsonPostAndParseResponseToObject<OrderCheckoutWrapper<Notification>, OrderCheckoutWrapper<AbstractOrder>>(riskifiedEndpointUrl, wrappedOrder, _authToken, _shopDomain);
            return new OrderNotification(transactionResult);
            
        }
    }

    
}
