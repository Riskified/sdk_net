using System;
using Riskified.NetSDK.Control;
using Riskified.NetSDK.Model;

namespace Riskified.SDK.Sample
{
    class Program
    {
        private const string DOMAIN = "test.pass.com";
        private const string AUTH_TOKEN = "1388add8a99252fc1a4974de471e73cd";
        private const string riskified_url = "http://sandbox.riskified.com/webhooks/merchant_order_created";

        static void Main(string[] args)
        {
            //NotificationHandler notifier = new NotificationHandler("http://localhost:1234/notification_receiver",
             //   NotificationReceived);
            //notifier.Start();

            var order = GenerateOrder();
            
            RiskifiedGateway gateway = new RiskifiedGateway(new Uri(riskified_url),AUTH_TOKEN,DOMAIN );

            int orderID = gateway.CreateOrUpdateOrder(order);

            Console.WriteLine("Order created. ID received: " + orderID);

            orderID = gateway.SubmitOrder(order);

            Console.WriteLine("Order submitted. ID received: "+orderID);

            Console.ReadLine();
        }

        private static void NotificationReceived(Notification notification)
        {
            Console.WriteLine("New "+ notification.Status + " Notification Received for order with ID:" + notification.OrderId);
        }

        private static Order GenerateOrder()
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
                Address2 = "",
                City = "Tel Aviv",
                Company = "",
                Country = "Israel",
                CountryCode = "IL",
                FirstName = "Ben",
                LastName = "Gurion",
                FullName = "",
                Phone = "00000000",
                Province = "Gush Dan",
                ProvinceCode = "gd",
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
                FullName = "",
                Phone = "00000000",
                ZipCode = "12345"
            };

            var payments = new PaymentDetails
            {
                AvsResultCode = "",
                CreditCardBin = "",
                CreditCardCompany = "",
                CreditCardNumber = "",
                CvvResultCode = ""
            };

            var lines = new[] {new ShippingLine {Code = "", Price = 22.22, Title = "Mail"}};

            var items = new[]
            {new LineItem {Price = 55.44, ProductId = 22, ProductTitle = "Bag", QuantityPurchased = 1, Sku = "123"}};

            Order order = new Order
            {
                BillingAddress = billing,
                CancelReason = "",
                CartToken = "asfansdf$dsDGFGS",
                Currency = "USD",
                Customer = customer,
                CustomerIp = "127.0.0.1",
                DiscountCodes = new[] {new DiscountCode {Code = "1", MoneyDiscountSum = 500}},
                Email = "Ab@gm.com",
                Gateway = "authorize_net",
                LineItems = items,
                OrderCancellationTime = DateTime.MinValue,
                OrderCloseTime = DateTime.MinValue,
                OrderCreationTime = new DateTime(2014, 2, 2, 14, 22, 10),
                OrderLastModifiedTime = DateTime.MinValue,
                Id = "555",
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
}
