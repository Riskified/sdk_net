using System;
using Riskified.NetSDK.Control;
using Riskified.NetSDK.Exceptions;
using Riskified.NetSDK.Logging;
using Riskified.NetSDK.Model;

namespace Riskified.SDK.Sample
{
    class Program
    {
        private const string client_webhook = "http://192.168.1.236:4000/notifications/";// "http://requestb.in/16y9j7s1";
        private const string DOMAIN = "test.pass.com";
        private const string AUTH_TOKEN = "1388add8a99252fc1a4974de471e73cd";
        private const string riskified_url = "http://192.168.1.32:3000/webhooks/merchant_order_created";
            //"http://sandbox.riskified.com/webhooks/merchant_order_created";
        //"http://192.168.1.32:3000/webhooks/merchant_order_created";

        static void Main(string[] args)
        {
            MyLogger logger = new MyLogger();

            NotificationHandler notifier = new NotificationHandler(client_webhook,NotificationReceived,AUTH_TOKEN,logger);
            notifier.Start();
            Random rand = new Random();
            int orderNum = rand.Next(100000,200000);
            
            Console.WriteLine("press 's' for submit, 'c' for create or 'q' to end");
            string quitStr = Console.ReadLine();
            while (!quitStr.Equals("q") && (quitStr.Equals("s") || quitStr.Equals("c")))
            {
                try
                {
                    var order = GenerateOrder(orderNum);
                    Console.WriteLine("Order Generated with ID "+orderNum);
                    orderNum++;

                    RiskifiedGateway gateway = new RiskifiedGateway(new Uri(riskified_url), AUTH_TOKEN, DOMAIN);

                    int orderIdAtRiskified;
                    if (quitStr.Equals("c"))
                    {
                        orderIdAtRiskified = gateway.CreateOrUpdateOrder(order);
                        Console.WriteLine("Order created. RiskifiedID received: " + orderIdAtRiskified);
                    }
                    else
                    {
                        orderIdAtRiskified = gateway.SubmitOrder(order);
                        Console.WriteLine("Order submitted. RiskifiedID received: " + orderIdAtRiskified);
                    }
                }
                catch (OrderFieldBadFormatException e)
                {
                    Console.WriteLine("Exception thrown on order field validation: " + e.Message);
                }
                catch (OrderTransactionException e)
                {
                    Console.WriteLine("Exception thrown on transaction: " + e.Message);
                }

                Console.WriteLine("press 's' for submit, 'c' for create or 'q' to end");
                quitStr = Console.ReadLine();
            }
            notifier.Stop();
        }

        private static void NotificationReceived(Notification notification)
        {
            Console.WriteLine("New "+ notification.Status + " Notification Received for order with ID:" + notification.OrderId + ". Notification verified? " + notification.IsValidatedNotification);
        }

        private static Order GenerateOrder(int orderNum)
        {
            var customer = new Customer
            {
                CreatedAt = new DateTime(2003, 12, 12, 13, 12, 59),
                Email = "my@email.com",
                FirstName = "John",
                Id = 405050606,
                LastName = "Doe",
                Note = "",
                OrdersCount = 4,
                VerifiedEmail = true
            };

            var billing = new AddressInformation
            {
                Address1 = "Rotshild 12",
                //Address2 = "",
                City = "Tel Aviv",
                //Company = "",
                Country = "Israel",
                CountryCode = "IL",
                FirstName = "Ben",
                LastName = "approved",
                //FullName = "",
                Phone = "00000000",
                //Province = "Gush Dan",
                //ProvinceCode = "gd",
                ZipCode = "12345"
            };

            var shipping = new AddressInformation
            {
                Address1 = "Taa22",
                Address2 = "",
                City = "Tel Aviv",
                Company = "",
                Country = "USA",
                CountryCode = "US",
                FirstName = "Ben",
                LastName = "Gurion",
                //FullName = "",
                Phone = "00000000",
                ZipCode = "12345"
            };

            var payments = new PaymentDetails("Y", "n", "", "", "");

            var lines = new[]
            {
                new ShippingLine {Code = "", Price = 22.22, Title = "Mail"},
                new ShippingLine {Code = "", Price = 2, Title = "Ship"}
            };

            var items = new[]
            {
                new LineItem("Bag",55.44,1,48484,"1272727"),
                new LineItem("Monster",22.3,3)
            };

            Order order = new Order
            {
                BillingAddress = billing,
                CancelReason = "",
                CartToken = orderNum+"asfansdf$dsDGFGS",
                Currency = "USD",
                Customer = customer,
                CustomerIp = "127.0.0.1",
                DiscountCodes = new[] {new DiscountCode {Code = "1", MoneyDiscountSum = 500}},
                Email = "Ab@gm.com",
                Gateway = "authorize_net",
                LineItems = items,
                OrderCancellationTime = null,
                ClosedAt = DateTime.MinValue,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.MinValue,
                Id = orderNum,
                PaymentDetails = payments,
                ShippingAddress = shipping,
                ShippingLines = lines,
                TotalDiscounts = 2.5,
                TotalPrice = 96.44,
                TotalPriceUsd = 33
            };
            return order;
        }
    }

    internal class MyLogger : ILogger
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
            Fatal(string.Format("{0}. Exception was: message: {1}. StackTrace {2}",message,exception.Message,exception.StackTrace));
        }
    }
    
}
