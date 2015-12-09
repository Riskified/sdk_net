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
using Riskified.SDK.Model.OrderCheckoutElements;

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
                                "'p' for checkout\n" +
                                "'e' for checkout denied\n" +
                                "'c' for create\n" +
                                "'u' for update\n" +
                                "'s' for submit\n" +
                                "'d' for cancel\n" +
                                "'r' for partial refund\n" +
                                "'f' for fulfill\n" +
                                "'x' for decision\n" +
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
                        case "p":
                            Console.WriteLine("Order checkout Generated with merchant order number: " + orderNum);
                            var orderCheckout = GenerateOrderCheckout(orderNum.ToString());
                            orderCheckout.Id = orderNum.ToString();
                            
                            // sending order checkout for creation (if new orderNum) or update (if existing orderNum)
                            res = gateway.Checkout(orderCheckout);
                            break;
                        case "e":
                            Console.WriteLine("Order checkout Generated.");
                            var orderCheckoutDenied = GenerateOrderCheckoutDenied(orderNum);
                            
                            Console.Write("checkout to deny id: ");
                            string orderCheckoutDeniedId = Console.ReadLine();

                            orderCheckoutDenied.Id = orderCheckoutDeniedId;
                            



                            // sending order checkout for creation (if new orderNum) or update (if existing orderNum)
                            res = gateway.CheckoutDenied(orderCheckoutDenied);
                            break;
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
                                            refundedAt: DateTime.Now,  // make sure to initialize DateTime with the correct timezone
                                            amount: 5.3,
                                            currency: "USD",
                                            reason: "Customer partly refunded on shipping fees")
                                    }));
                            break;
                        case "f":
                            Console.Write("Fulfill order id: ");
                            string fulfillOrderId = Console.ReadLine();
                            OrderFulfillment orderFulfillment = GenerateFulfillment(int.Parse(fulfillOrderId));
                            res = gateway.Fulfill(orderFulfillment);

                            break;
                        case "x":
                            Console.Write("Decision order id: ");
                            string decisionOrderId = Console.ReadLine();
                            OrderDecision orderDecision = GenerateDecision(int.Parse(decisionOrderId));
                            res = gateway.Decision(orderDecision);

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
                        Console.WriteLine("\n\nOrder sent successfully:" + 
                                              "\nStatus at Riskified: " + res.Status +
                                              "\nOrder ID received:" + res.Id +
                                              "\nDescription: " + res.Description + 
                                              "\nWarnings: " + (res.Warnings==null ? "---" : string.Join("        \n",res.Warnings)) + "\n\n");
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

        private static OrderDecision GenerateDecision(int p)
        {
            // make sure to initialize DateTime with the correct timezone
            OrderDecision orderDecision = new OrderDecision(p, new DecisionDetails(ExternalStatusType.ChargebackFraud, DateTime.Now, "used proxy and stolen credit card."));
            return orderDecision;
        }


        private static OrderCheckout GenerateOrderCheckout(string orderNum)
        {
            var orderCheckout = new OrderCheckout(orderNum);
            
            // Fill optional fields
            var customer = new Customer(
                firstName: "John",
                lastName: "Doe",
                id: "405050606",
                ordersCount: 4,
                email: "test@example.com",
                verifiedEmail: true,
                createdAt: new DateTime(2013, 12, 8, 14, 12, 12, DateTimeKind.Local), // make sure to initialize DateTime with the correct timezone
                notes: "No additional info");

            var items = new[]
            {
                new LineItem(title: "Bag",price: 55.44,quantityPurchased: 1,productId: 48484,sku: "1272727"),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3)
            };

            orderCheckout.Customer = customer;
            orderCheckout.LineItems = items;


            return orderCheckout;

        }

        private static OrderCheckoutDenied GenerateOrderCheckoutDenied(int orderNum)
        {
            var authorizationError = new AuthorizationError(
                                    createdAt: new DateTime(2013, 12, 8, 14, 12, 12, DateTimeKind.Local), // make sure to initialize DateTime with the correct timezone
                                    errorCode: AuthorizationErrorCode.IncorrectNumber,
                                    message: "credit card expired.");

            var orderCheckoutDenied = new OrderCheckoutDenied(orderNum, authorizationError);



            return orderCheckoutDenied;

        }


        private static OrderFulfillment GenerateFulfillment(int fulfillOrderId)
        {
            FulfillmentDetails[] fulfillmentList = new FulfillmentDetails[] {
                                        new FulfillmentDetails(
                                            fulfillmentId: "123",
                                            createdAt: new DateTime(2013, 12, 8, 14, 12, 12, DateTimeKind.Local),
                                            status: FulfillmentStatusCode.Success,
                                            lineItems: new LineItem[] { new LineItem("Bag", 10.0, 1) },
                                            trackingCompany: "TestCompany")
                                    };

            OrderFulfillment orderFulfillment = new OrderFulfillment(
                                    merchantOrderId: fulfillOrderId,
                                    fulfillments: fulfillmentList);

            return orderFulfillment;
        }

        /// <summary>
        /// Generates a new order object
        /// Mind that some of the fields of the order (and it's sub-objects) are optional
        /// </summary>
        /// <param name="orderNum">The order number to put in the order object</param>
        /// <returns></returns>
        private static Order GenerateOrder(int orderNum)
        {
            var customerAddress = new BasicAddress(
                address1: "27 5th avenue",
                city: "Manhattan",
                country: "United States",
                countryCode: "US",
                phone: "5554321234",
                address2: "Appartment 5",
                zipCode: "54545"
                );

            // putting sample customer details
            var customer = new Customer(
                firstName: "John",
                lastName: "Doe",
                id: "405050606",
                ordersCount: 4,
                email: "test@example.com",
                verifiedEmail: true,
                createdAt: new DateTime(2013, 12, 8, 14, 12, 12, DateTimeKind.Local), // make sure to initialize DateTime with the correct timezone
                notes: "No additional info",
                address: customerAddress);

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

            var noChargeAmount = new NoChargeDetails(
                refundId: "123444",
                amount: 20.5,
                currency: "GBP",
                reason: "giftcard"
                );

            var lines = new[]
            {
                new ShippingLine(price: 22.22,title: "Mail"),
                new ShippingLine(price: 2,title: "Ship",code: "A22F")
            };

            var items = new[]
            {
                new LineItem(title: "Bag",price: 55.44,quantityPurchased: 1,productId: 48484,sku: "1272727"),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3, seller: new Seller(customer: customer,correspondence: 1, priceNegotiated: true, startingPrice: 120)),
                // Events Tickets Industry 
                new LineItem(title: "Concert", 
                             price: 123, 
                             quantityPurchased: 1, 
                             category: "Singers", 
                             subCategory: "Rock", 
                             eventName: "Bon Jovy", 
                             eventSectionName: "Section", 
                             eventCountry: "USA", 
                             eventCountryCode: "US",
                             latitude: 0,
                             longitude: 0)
            };

            var discountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "1") };

            DecisionDetails decisionDetails = new DecisionDetails(ExternalStatusType.Approved, DateTime.Now); // make sure to initialize DateTime with the correct timezone

            // This is an example for an order with charge free sums (e.g. gift card payment)
            var chargeFreePayments = new ChargeFreePaymentDetails(
                gateway: "giftcard",
                amount: 45
                );

            // This is an example for client details section
            var clientDetails = new ClientDetails(
                accept_language: "en-CA",   
                user_agent: "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"
                );


            var order = new Order(
                merchantOrderId: orderNum.ToString(), 
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
                createdAt: DateTime.Now, // make sure to initialize DateTime with the correct timezone
                updatedAt: DateTime.Now, // make sure to initialize DateTime with the correct timezone
                discountCodes: discountCodes,
                source: "web",
                noChargeDetails: noChargeAmount,
                decisionDetails: decisionDetails,
                vendorId: "2",
                vendorName: "domestic",
                additionalEmails: new [] {"a@a.com","b@b.com"},
                chargeFreePaymentDetails: chargeFreePayments,
                clientDetails: clientDetails);

            return order;
        }

        private static Order PayPalGenerateOrder(int orderNum)
        {
            // putting sample customer details
            var customer = new Customer(
                firstName: "John",
                lastName: "Doe",
                id: "405050606",
                ordersCount: 4,
                email: "test@example.com",
                verifiedEmail: true,
                createdAt: new DateTime(2013, 12, 8, 14, 12, 12, DateTimeKind.Local), // make sure to initialize DateTime with the correct timezone
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
                merchantOrderId: orderNum.ToString(),
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
                createdAt: DateTime.Now, // make sure to initialize DateTime with the correct timezone
                updatedAt: DateTime.Now, // make sure to initialize DateTime with the correct timezone
                discountCodes: discountCodes);

            return order;
        }

    }
}
