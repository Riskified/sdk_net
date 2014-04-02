using System;
using System.Configuration;
using System.Threading.Tasks;
using Riskified.NetSDK.Control;
using Riskified.NetSDK.Exceptions;
using Riskified.NetSDK.Logging;
using Riskified.NetSDK.Model;

namespace Riskified.SDK.Sample
{
    static class Program
    {
        
            //"http://sandbox.riskified.com/webhooks/merchant_order_created";
        //"http://192.168.1.32:3000/webhooks/merchant_order_created";

        static void Main(string[] args)
        {
            string ClientWebhook = ConfigurationManager.AppSettings["NotificationsWebhookUrl"];
            string domain = ConfigurationManager.AppSettings["MerchantDomain"];  
            string AuthToken = ConfigurationManager.AppSettings["MerchantAuthenticationToken"];
            string RiskifiedUrl = ConfigurationManager.AppSettings["RiskifiedOrderWebhookUrl"];


            #region logger setup [Optional]
            
            // setting up a logger facade to the system logger using the ILog interface
            // if a logger facade is created it will enable a peek into the logs created by the SDK and will help understand issues easier
            var logger = new SimpleExampleLogger();

            #endregion

            #region Notification Server setup and activation
            
            // setup of a notification server listening to incoming notification from riskified
            // the webhook is the url on the local server which the httpServer will be listening at
            // make sure the url is correct (internet reachable ip/address and port, firewall rules etc.)
            var notifier = new NotificationHandler(ClientWebhook,NotificationReceived,AuthToken,domain,logger);
            // the call to notifier.ReceiveNotifications() is blocking and will not return until we call StopReceiveNotifications 
            // so we run it on a different task in this example
            var t = new Task(notifier.ReceiveNotifications);
            t.Start();
            
            #endregion

            #region order creation and submittion
            // Generating a random starting order number
            // we need to send the order with a new order number in order to create it on riskified
            // if order number already exists in riskified server - the order will be updated
            var rand = new Random();
            int orderNum = rand.Next(1000,200000);
            
            // sample for order creating and submitting to servers via console
            
            // read action from console
            Console.WriteLine("press 's' for submit, 'c' for create or 'q' to end");
            string quitStr = Console.ReadLine();
            
            // loop on console actions 
            while (quitStr != null && (!quitStr.Equals("q") && (quitStr.Equals("s") || quitStr.Equals("c"))))
            {
                try
                {

                    // generate a new order - the sample generates a fixed order with same details but different order number each time
                    // see GenerateOrder for more info on how to create the Order objects
                    var order = GenerateOrder(orderNum);
                    Console.WriteLine("Order Generated with merchant order number: "+orderNum);
                    orderNum++;

                    // the RiskifieGateway is responsible for sending orders to Riskified servers
                    RiskifiedGateway gateway = new RiskifiedGateway(new Uri(RiskifiedUrl), AuthToken, domain,logger);

                    int orderIdAtRiskified;

                    if (quitStr.Equals("c"))
                    {
                        // sending order for creation (if new orderNum) or update (if existing orderNum)
                        orderIdAtRiskified = gateway.CreateOrUpdateOrder(order);
                        Console.WriteLine("Order created. RiskifiedID received: " + orderIdAtRiskified);
                    }
                    else
                    {
                        // sending order for submitting and analysis 
                        // it will generate a callback to the notification webhook (if defined) with a decision regarding the order
                        orderIdAtRiskified = gateway.SubmitOrder(order);
                        Console.WriteLine("Order submitted. RiskifiedID received: " + orderIdAtRiskified);
                    }
                }
                catch (OrderFieldBadFormatException e)
                {
                    // catching 
                    Console.WriteLine("Exception thrown on order field validation: " + e.Message);
                }
                catch (OrderTransactionException e)
                {
                    Console.WriteLine("Exception thrown on transaction: " + e.Message);
                }

                // ask for next action to perform
                Console.WriteLine("press 's' for submit, 'c' for create or 'q' to end");
                quitStr = Console.ReadLine();
            }

            #endregion

            // make sure you shut down the notification server on system shut dowwn
            notifier.StopReceiveNotifications();
        }

        /// <summary>
        /// A sample notifications callback from the NotificationHandler
        /// Will be called each time a new notification is received at the local webhook
        /// </summary>
        /// <param name="notification">The notification object that was received</param>
        private static void NotificationReceived(Notification notification)
        {
            Console.WriteLine("New "+ notification.Status + " Notification Received for order with ID:" + notification.OrderId + ". Notification verified? " + notification.IsValidatedNotification);
        }

        /// <summary>
        /// Generates a new order object
        /// Mind that some of the fields of the order (and it's sub-objects) are optional
        /// </summary>
        /// <param name="orderNum">The order number to put in the order object</param>
        /// <returns></returns>
        private static Order GenerateOrder(int orderNum)
        {
            // IMPORTANT: all objects created here may throw OrderFieldBadFormatException 
            // if one or more of the parameters values doesn't match the required format
            // In the sample - this exception is handled in the rapping method

            // putting sample customer details
            var customer = new Customer(405050606, "John", "Doe", 4, "test@example.com", true,
                new DateTime(2013, 12, 8, 14, 12, 12), "No additional info");

            // putting sample billing details
            var billing = new AddressInformation("Ben", "Rolling", "27 5th avenue", "Manhattan", "United States", "US",
                "5554321234", "Appartment 5", "54545", "New York", "NY", "IBM", "Ben Philip Rolling");

            var shipping = new AddressInformation("Luke", "Rolling", "4 Bermingham street", "Cherry Hill",
                "United States", "US", "55546665", provinceCode: "NJ", province: "New Jersey");
            
            var payments = new PaymentDetails("Y", "n", "4580", "Visa", "XXXX-XXXX-XXXX-4242");

            var lines = new[]
            {
                new ShippingLine(22.22,"Mail"),
                new ShippingLine(2,"Ship","A22F")
            };

            var items = new[]
            {
                new LineItem("Bag",55.44,1,48484,"1272727"),
                new LineItem("Monster",22.3,3)
            };

            var discountCodes = new[] { new DiscountCode(7, "1") };

            var order = new Order(orderNum, "tester@exampler.com", customer, payments, billing, shipping, items, lines,
                "authorize_net", "127.0.0.1", "USD", 100.60, DateTime.Now, DateTime.Now, discountCodes);
            
            return order;
        }
    }

    internal class SimpleExampleLogger : ILogger
    {

        private static void Log(string message,string level)
        {
            Console.WriteLine(string.Format("{0}  {1}  {2}",DateTime.Now,level,message));
        }
        public void Debug(string message)
        {
            Log(message,"DEBUG");
        }

        public void Debug(string message, Exception exception)
        {
            Debug(string.Format("{0}. Exception was: message: {1}. StackTrace {2}",message,exception.Message,exception.StackTrace));
        }

        public void Info(string message)
        {
            Log(message,"INFO");
        }

        public void Info(string message, Exception exception)
        {
            Info(string.Format("{0}. Exception was: message: {1}. StackTrace {2}",message,exception.Message,exception.StackTrace));
        }

        public void Error(string message)
        {
            Log(message,"ERROR");
        }

        public void Error(string message, Exception exception)
        {
            Error(string.Format("{0}. Exception was: message: {1}. StackTrace {2}",message,exception.Message,exception.StackTrace));
        }

        public void Fatal(string message)
        {
            Log(message,"FATAL");
        }

        public void Fatal(string message, Exception exception)
        {
            Fatal(string.Format("{0}. Exception was: {1} StackTrace: {2}",message,exception.Message,exception.StackTrace));
        }
    }
    
}
