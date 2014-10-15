using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Riskified.SDK.Model;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Model.RefundElements;
using Riskified.SDK.Orders;
using Riskified.SDK.Utils;
using Riskified.SDK.Exceptions;

namespace Riskified.SDK.Sample
{
    public static class OrderTransmissionExample
    {
        public static void SendOrdersToRiskifiedExample()
        {
            #region preprocessing and loading config
            
            string domain = ConfigurationManager.AppSettings["MerchantDomain"];
            string authToken = ConfigurationManager.AppSettings["MerchantAuthenticationToken"];
            RiskifiedEnvironment riskifiedEnv = (RiskifiedEnvironment) Enum.Parse(typeof (RiskifiedEnvironment),ConfigurationManager.AppSettings["RiskifiedEnvironment"]);

            // Generating a random starting order number
            // we need to send the order with a new order number in order to create it on riskified
            var rand = new Random();
            int orderNum = rand.Next(1000, 200000);

            #endregion

            #region order object creation

            // generate a new order - the sample generates a fixed order with same details but different order number each time
            // see GenerateOrder for more info on how to create the Order objects
            var order = GenerateOrder(orderNum);

            #endregion

            #region sending data to riskified

            // read action from console
            const string menu = "Commands:\n" +
                                "'c' for create\n" +
                                "'u' for update\n" +
                                "'s' for submit\n" +
                                "'d' for cancel\n" +
                                "'r' for partial refund\n" +
                                "'h' for historical sending\n" +
                                "'q' to quit";
            Console.WriteLine(menu);
            string commandStr = Console.ReadLine();

            
            // loop on console actions 
            while (commandStr != null && (!commandStr.Equals("q")))
            {
                
                

                // the OrdersGateway is responsible for sending orders to Riskified servers
                OrdersGateway gateway = new OrdersGateway(riskifiedEnv, authToken, domain);
                try
                {
                    OrderNotification res=null;
                    switch (commandStr)
                    {
                        case "c":
                            Console.WriteLine("Order Generated with merchant order number: " + orderNum);
                            order.Id = orderNum.ToString();
                            orderNum++;
                            // sending order for creation (if new orderNum) or update (if existing orderNum)
                            res = gateway.Create(order);
                            break;
                        case "s":
                            Console.WriteLine("Order Generated with merchant order number: " + orderNum);
                            order.Id = orderNum.ToString();
                            orderNum++;
                            // sending order for submitting and analysis 
                            // it will generate a callback to the notification webhook (if defined) with a decision regarding the order
                            res = gateway.Submit(order);
                            break;
                        case "u":
                            Console.Write("Updated order id: ");
                            string upOrderId = Console.ReadLine();
                            order.Id = int.Parse(upOrderId).ToString();
                            res = gateway.Update(order);
                            break;
                        case "d":
                            Console.Write("Cancelled order id: ");
                            string canOrderId = Console.ReadLine();
                            res = gateway.Cancel(
                                new OrderCancellation(
                                    merchantOrderId: int.Parse(canOrderId),
                                    cancelledAt: DateTime.Now,
                                    cancelReason: "Customer cancelled before shipping"));
                            break;
                        case "r":
                            Console.Write("Refunded order id: ");
                            string refOrderId = Console.ReadLine();
                            res = gateway.PartlyRefund(
                                new OrderPartialRefund(
                                    merchantOrderId: int.Parse(refOrderId),
                                    partialRefunds: new[]
                                    {
                                        new PartialRefundDetails(
                                            refundId: "12345",
                                            refundedAt: DateTime.Now,
                                            amount: 5.3,
                                            currency: "USD",
                                            reason: "Customer partly refunded on shipping fees")
                                    }));
                            break;
                        case "h":
                            int startOrderNum = orderNum;
                            var orders = new List<Order>();
                            var financialStatuses = new[] { "paid", "cancelled", "chargeback" };
                            for (int i = 0; i < 22; i++)
                            {
                                Order o = GenerateOrder(orderNum++);
                                o.FinancialStatus = financialStatuses[i%3];
                                orders.Add(o);
                            }
                            Console.WriteLine("Orders Generated with merchant order numbers: {0} to {1}",startOrderNum,orderNum-1);
                            // sending 3 historical orders with different processing state
                            Dictionary<string,string> errors;
                            bool success = gateway.SendHistoricalOrders(orders,out errors);
                            if(success)
                            {
                                Console.WriteLine("All historical orders sent successfully");
                            }
                            else
                            {
                                Console.WriteLine("Some historical orders failed to send:");
                                Console.WriteLine(String.Join("\n", errors.Select(p => p.Key + ":" + p.Value).ToArray()));
                            }
                            break;

                    }


                    if (res != null)
                    {
                        Console.WriteLine("Order sent successfully: " + 
                                              " Status at Riskified: " + res.Status +
                                              ". Order ID received: " + res.Id +
                                              ". Description: " + res.Description);
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
            var customer = new Customer(
                firstName: "John",
                lastName: "Doe",
                id: 405050606,
                ordersCount: 4,
                email: "test@example.com",
                verifiedEmail: true,
                createdAt: new DateTime(2013, 12, 8, 14, 12, 12),
                notes: "No additional info");

            // putting sample billing details
            var billing = new AddressInformation(
                firstName: "Ben",
                lastName: "Rolling",
                address1: "27 5th avenue",
                city: "Manhattan", 
                country: "United States",
                countryCode: "US",
                phone: "5554321234", 
                address2: "Appartment 5",
                zipCode: "54545",
                province: "New York",
                provinceCode: "NY",
                company: "IBM",
                fullName: "Ben Philip Rolling");

            var shipping = new AddressInformation(
                firstName: "Luke",
                lastName: "Rolling",
                address1: "4 Bermingham street", 
                city: "Cherry Hill",
                country: "United States",
                countryCode: "US", 
                phone: "55546665", 
                provinceCode: "NJ", 
                province: "New Jersey");

            var payments = new CreditCardPaymentDetails(
                avsResultCode: "Y",
                cvvResultCode: "n",
                creditCardBin: "124580", 
                creditCardCompany: "Visa",
                creditCardNumber: "XXXX-XXXX-XXXX-4242");

            var lines = new[]
            {
                new ShippingLine(price: 22.22,title: "Mail"),
                new ShippingLine(price: 2,title: "Ship",code: "A22F")
            };

            var items = new[]
            {
                new LineItem(title: "Bag",price: 55.44,quantityPurchased: 1,productId: 48484,sku: "1272727"),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3)
            };

            var discountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "1") };

            var order = new Order(
                merchantOrderId: orderNum, 
                email: "tester@exampler.com", 
                customer: customer, 
                paymentDetails: payments,
                billingAddress: billing,
                shippingAddress: shipping,
                lineItems: items,
                shippingLines: lines,
                gateway: "authorize_net",
                customerBrowserIp: "165.12.1.1",
                currency: "USD", 
                totalPrice: 100.60,
                createdAt: DateTime.Now,
                updatedAt: DateTime.Now,
                discountCodes: discountCodes);

            return order;
        }

        private static Order PayPalGenerateOrder(int orderNum)
        {
            // IMPORTANT: all objects created here may throw OrderFieldBadFormatException 
            // if one or more of the parameters values doesn't match the required format
            // In the sample - this exception is handled in the rapping method

            // putting sample customer details
            var customer = new Customer(
                firstName: "John",
                lastName: "Doe",
                id: 405050606,
                ordersCount: 4,
                email: "test@example.com",
                verifiedEmail: true,
                createdAt: new DateTime(2013, 12, 8, 14, 12, 12),
                notes: "No additional info");

            // putting sample billing details
            var billing = new AddressInformation(
                firstName: "Ben",
                lastName: "Rolling",
                address1: "27 5th avenue",
                city: "Manhattan",
                country: "United States",
                countryCode: "US",
                phone: "5554321234",
                address2: "Appartment 5",
                zipCode: "54545",
                province: "New York",
                provinceCode: "NY",
                company: "IBM",
                fullName: "Ben Philip Rolling");

            var shipping = new AddressInformation(
                firstName: "Luke",
                lastName: "Rolling",
                address1: "4 Bermingham street",
                city: "Cherry Hill",
                country: "United States",
                countryCode: "US",
                phone: "55546665",
                provinceCode: "NJ",
                province: "New Jersey");

            var payments = new PaypalPaymentDetails(
                paymentStatus: "Authorized",
                authorizationId: "AFSDF332432SDF45DS5FD",
                payerEmail: "payer@gmail.com",
                payerStatus: "Verified",
                payerAddressStatus: "Unverified",
                protectionEligibility: "Partly Eligibile",
                pendingReason: "Review");

            var lines = new[]
            {
                new ShippingLine(price: 22.22,title: "Mail"),
                new ShippingLine(price: 2,title: "Ship",code: "A22F")
            };

            var items = new[]
            {
                new LineItem(title: "Bag",price: 55.44,quantityPurchased: 1,productId: 48484,sku: "1272727"),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3)
            };

            var discountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "1") };

            var order = new Order(
                merchantOrderId: orderNum,
                email: "tester@exampler.com",
                customer: customer,
                paymentDetails: payments,
                billingAddress: billing,
                shippingAddress: shipping,
                lineItems: items,
                shippingLines: lines,
                gateway: "authorize_net",
                customerBrowserIp: "165.12.1.1",
                currency: "USD",
                totalPrice: 100.60,
                createdAt: DateTime.Now,
                updatedAt: DateTime.Now,
                discountCodes: discountCodes);

            return order;
        }

    }
}
