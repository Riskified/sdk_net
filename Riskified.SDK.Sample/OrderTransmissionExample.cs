using System;
using System.Configuration;
using Riskified.SDK.Model;
using Riskified.SDK.Model.Orders;
using Riskified.SDK.Model.Orders.OrderElements;
using Riskified.SDK.Model.Orders.RefundElements;
using Riskified.SDK.Orders;
using Riskified.SDK.Utils;
using Riskified.SDK.Exceptions;

namespace Riskified.SDK.Sample
{
    public static class OrderTransmissionExample
    {
        public static void SendOrdersToRiskifiedExample()
        {
            string domain = ConfigurationManager.AppSettings["MerchantDomain"];
            string authToken = ConfigurationManager.AppSettings["MerchantAuthenticationToken"];
            RiskifiedEnvironment riskifiedEnv = (RiskifiedEnvironment) Enum.Parse(typeof (RiskifiedEnvironment),ConfigurationManager.AppSettings["RiskifiedEnvironment"]);
            
            #region order creation and submittion
            // Generating a random starting order number
            // we need to send the order with a new order number in order to create it on riskified
            // if order number already exists in riskified server - the order will be updated
            var rand = new Random();
            int orderNum = rand.Next(1000, 200000);

            // sample for order creating and submitting to servers via console

            // read action from console
            const string menu = "Commands:\n" +
                                "'c' for create\n" +
                                "'u' for update\n" +
                                "'s' for submit\n" +
                                "'d' for cancel\n" +
                                "'r' for partial refund\n" +
                                "'q' to quit";
            Console.WriteLine(menu);
            string commandStr = Console.ReadLine();

            // loop on console actions 
            while (commandStr != null && (!commandStr.Equals("q")))
            {
                // generate a new order - the sample generates a fixed order with same details but different order number each time
                // see GenerateOrder for more info on how to create the Order objects
                var order = GenerateOrder(orderNum);
                Console.WriteLine("Order Generated with merchant order number: " + orderNum);
                orderNum++;

                // the OrdersGateway is responsible for sending orders to Riskified servers
                OrdersGateway gateway = new OrdersGateway(riskifiedEnv, authToken, domain);
                try
                {
                    OrderNotification res=null;
                    switch (commandStr)
                    {
                        case "c":
                            // sending order for creation (if new orderNum) or update (if existing orderNum)
                            res = gateway.Create(order);
                            break;
                        case "s":
                            // sending order for submitting and analysis 
                            // it will generate a callback to the notification webhook (if defined) with a decision regarding the order
                            res = gateway.Submit(order);
                            break;
                        case "u":
                            Console.Write("Updated order id: ");
                            string upOrderId = Console.ReadLine();
                            order.Id = int.Parse(upOrderId);
                            res = gateway.Update(order);
                            break;
                        case "d":
                            Console.Write("Cancelled order id: ");
                            string canOrderId = Console.ReadLine();
                            res = gateway.Cancel(new OrderCancellation(int.Parse(canOrderId), DateTime.Now,
                                "Customer cancelled before shipping"));
                            break;
                        case "r":
                            Console.Write("Refunded order id: ");
                            string refOrderId = Console.ReadLine();
                            res = gateway.Refund(new OrderRefund(int.Parse(refOrderId), new [] {new RefundDetails(DateTime.Now,5.3,"USD","Customer partly refunded on shipping fees")}));
                            break;
                    }


                    if (res != null)
                    {
                        Console.WriteLine("Order sent successfully: " + res.Status +
                                              ". Riskified order ID received: " + res.Id +
                                              " Description: " + res.Description);
                    }
                    else
                    {
                        Console.WriteLine("Unknown error sending order.");
                    }
                }
                catch (OrderFieldBadFormatException e)
                {
                    // catching 
                    Console.WriteLine("Exception thrown on order field validation: " + e.Message);
                }
                catch (RiskifiedTransactionException e)
                {
                    Console.WriteLine("Exception thrown on transaction: " + e.Message);
                }

                // ask for next action to perform
                Console.WriteLine();
                Console.WriteLine(menu);
                commandStr = Console.ReadLine();
            }

            #endregion

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
            var customer = new Customer("John", "Doe", 405050606, 4, "test@example.com", true,
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
}
