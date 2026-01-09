using System;
using System.Collections.Generic;
using System.Linq;
using Riskified.SDK.Model;
using Riskified.SDK.Model.ChargebackElements;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Model.RefundElements;
using Riskified.SDK.Model.AccountActionElements;
using Riskified.SDK.Orders;
using Riskified.SDK.Utils;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Model.OrderCheckoutElements;
using System.Text;

namespace Riskified.SDK.Sample
{
    public static class OrderTransmissionExample
    {
        public static void SendOrdersToRiskifiedExample()
        {
            #region preprocessing and loading config

            // Configuration via environment variables (recommended for .NET 8)
            // Set these environment variables before running:
            //   RISKIFIED_MERCHANT_DOMAIN
            //   RISKIFIED_AUTH_TOKEN
            //   RISKIFIED_ENVIRONMENT (Debug, Sandbox, or Production)
            string domain = Environment.GetEnvironmentVariable("RISKIFIED_MERCHANT_DOMAIN") ?? "your_merchant_domain.com";
            string authToken = Environment.GetEnvironmentVariable("RISKIFIED_AUTH_TOKEN") ?? "your_auth_token";
            string envStr = Environment.GetEnvironmentVariable("RISKIFIED_ENVIRONMENT") ?? "Sandbox";
            RiskifiedEnvironment riskifiedEnv = (RiskifiedEnvironment)Enum.Parse(typeof(RiskifiedEnvironment), envStr);

            // Generating a random starting order number
            // we need to send the order with a new order number in order to create it on riskified
            var rand = new Random();
            int orderNum = rand.Next(1000, 200000);

            // Make orderNum a string to use as customer id
            string idString = $"customerId_{orderNum.ToString()}";

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
                                "'y' for chargeback submission\n" +
                                "'v' for decide (sync only)\n" +
                                "'l' for eligible for Deco payment \n" +
                                "'o' for opt-in to Deco payment \n" +
                                "'account' for account actions menu\n" +
                                "'q' to quit";

            const string accountActionsMenu = "Account Action Commands:\n" +
                                "'li' for login(account)\n" +
                                "'cc' for customer create (account)\n" +
                                "'cu' for customer update (account)\n" +
                                "'lo' for logout (account)\n" +
                                "'pw' for password reset (account)\n" +
                                "'wl' for wishlist (account)\n" +
                                "'re' for redeem (account)\n" +
                                "'co' for contact (account)\n" +
                                "'menu' for main menu\n" +
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
                    OrderNotification res = null;
                    AccountActionNotification accRes = null;
                    switch (commandStr)
                    {
                        case "menu":
                        case "account":
                            break;
                        case "p":
                            Console.WriteLine("Order checkout Generated with merchant order number: " + orderNum);
                            var orderCheckout = GenerateOrderCheckout(orderNum.ToString());
                            orderCheckout.Id = orderNum.ToString();

                            // sending order checkout for creation (if new orderNum) or update (if existing orderNum)
                            res = gateway.Checkout(orderCheckout);
                            break;
                        case "a":
                            Console.WriteLine("Order Advise Generated with merchant order number: " + orderNum);
                            var orderAdviseCheckout = GenerateAdviseOrderCheckout(orderNum.ToString());
                            orderAdviseCheckout.Id = orderNum.ToString();

                            // sending order checkout for creation (if new orderNum) or update (if existing orderNum)
                            res = gateway.Advise(orderAdviseCheckout);
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
                        case "v":
                            Console.WriteLine("Order Generated with merchant order number: " + orderNum);
                            order.Id = orderNum.ToString();
                            //orderNum++;
                            // sending order for synchronous decision
                            // it will generate a synchronous response with the decision regarding the order
                            // (for sync flow only)
                            res = gateway.Decide(order);
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
                                o.FinancialStatus = financialStatuses[i % 3];
                                orders.Add(o);
                            }
                            Console.WriteLine("Orders Generated with merchant order numbers: {0} to {1}", startOrderNum, orderNum - 1);
                            // sending 3 historical orders with different processing state
                            Dictionary<string, string> errors;
                            bool success = gateway.SendHistoricalOrders(orders, out errors);
                            if (success)
                            {
                                Console.WriteLine("All historical orders sent successfully");
                            }
                            else
                            {
                                Console.WriteLine("Some historical orders failed to send:");
                                Console.WriteLine(String.Join("\n", errors.Select(p => p.Key + ":" + p.Value).ToArray()));
                            }
                            break;
                        case "y":
                            Console.Write("Chargeback order id: ");
                            string chargebackOrderId = Console.ReadLine();
                            OrderChargeback orderChargeback = GenerateOrderChargeback(chargebackOrderId);
                            res = gateway.Chargeback(orderChargeback);

                            break;

                        case "l":
                            Console.Write("Check Deco eligibility on id: ");
                            string eligibleOrderId = Console.ReadLine();
                            OrderIdOnly eligibleOrderIdOnly = GenerateOrderIdOnly(eligibleOrderId);
                            res = gateway.Eligible(eligibleOrderIdOnly);

                            break;
                        case "o":
                            Console.Write("Opt-in to Deco payment on id: ");
                            string optInOrderId = Console.ReadLine();
                            OrderIdOnly optInOrderIdOnly = GenerateOrderIdOnly(optInOrderId);
                            res = gateway.OptIn(optInOrderIdOnly);

                            break;

                        case "li":
                            Console.Write("Login account action");
                            Login login = GenerateLogin(idString);

                            accRes = gateway.Login(login);
                            break;
                        case "cc":
                            Console.Write("Customer Create account action");
                            CustomerCreate customerCreate = GenerateCustomerCreate(idString);

                            accRes = gateway.CustomerCreate(customerCreate);
                            break;
                        case "cu":
                            Console.Write("Customer Update account action");
                            CustomerUpdate customerUpdate = GenerateCustomerUpdate(idString);

                            accRes = gateway.CustomerUpdate(customerUpdate);
                            break;
                        case "lo":
                            Console.Write("Logout account action");
                            Logout logout = GenerateLogout(idString);

                            accRes = gateway.Logout(logout);
                            break;
                        case "pw":
                            Console.Write("ResetPasswordRequest account action");
                            ResetPasswordRequest resetPasswordRequest = GenerateResetPasswordRequest(idString);

                            accRes = gateway.ResetPasswordRequest(resetPasswordRequest);
                            break;
                        case "wl":
                            Console.Write("WishlistChanges account action");
                            WishlistChanges wishlistChanges = GenerateWishlistChanges(idString);

                            accRes = gateway.WishlistChanges(wishlistChanges);
                            break;
                        case "re":
                            Console.Write("Redeem account action");
                            Redeem redeem = GenerateRedeem(idString);

                            accRes = gateway.Redeem(redeem);
                            break;
                        case "co":
                            Console.Write("Customer Reach-Out account action");
                            CustomerReachOut customerReachOut = GenerateCustomerReachOut(idString);

                            accRes = gateway.CustomerReachOut(customerReachOut);
                            break;
                    }


                    if (res != null)
                    {
                        StringBuilder message = new StringBuilder();

                        // Basic order information
                        message.AppendLine("\nOrder sent successfully:");
                        message.AppendLine($"Status at Riskified: {res.Status}");
                        message.AppendLine($"Order ID received: {res.Id}");
                        message.AppendLine($"Description: {res.Description}");
                        // Conditional policy response
                        if (res.PolicyProtect != null && res.PolicyProtect.Policies.Any())
                        {
                            //the example only retrieve the first item, in prod env, merchant should implement policy response it in for loop format. 
                            message.AppendLine($"Policy Response: {res.PolicyProtect.Policies.First().PolicyType}");
                        }

                        if(res.RiskIndicators != null && res.RiskIndicators.Count > 0)
                        {
                            Console.WriteLine("=== Risk Indicators ===");

                            // Iterate all
                            foreach (var indicator in res.RiskIndicators)
                            {
                                Console.WriteLine($"{indicator.Key}: {indicator.Value}");
                            }

                            // Check for specific field
                            if (res.RiskIndicators.ContainsKey("email_age"))
                            {
                                Console.WriteLine($"Email Age: {res.RiskIndicators["email_age"]}");
                            }

                        }

                        // Warnings or a placeholder if there are no warnings
                        if (res.Warnings != null && res.Warnings.Any())
                        {
                            message.AppendLine("Warnings: " + string.Join("\n        ", res.Warnings));
                        }
                        else
                        {
                            message.AppendLine("Warnings: ---");
                        }
                        Console.WriteLine(message.ToString());

                    }
                    if (accRes != null)
                    {
                        Console.WriteLine("\n\nAccount Action sent successfully:" +
                                          "\nDecision: " + accRes.Decision);
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
                if (commandStr.Equals("account")) {
                    Console.WriteLine(accountActionsMenu);
                }
                else {
                    Console.WriteLine(menu);
                }
                commandStr = Console.ReadLine();
            }

            #endregion

        }

        private static OrderChargeback GenerateOrderChargeback(string orderNum)
        {
            var chargebackDetails = new ChargebackDetails(id: "id1234",
                                charegbackAt: new DateTime(2018, 12, 8, 14, 12, 12),
                                chargebackCurrency: "USD",
                                chargebackAmount: (float)50.5,
                                reasonCode: "4863",
                                reasonDesc: "Transaction not recognised",
                                type: "cb",
                                mid: "t_123",
                                creditCardCompany: "visa",
                                respondBy: new DateTime(2016, 9, 1),
                                arn: "a123456789012bc3de45678901f23a45",
                                feeAmount: 20,
                                feeCurrency: "USD",
                                cardIssuer: "Wells Fargo Bank",
                                gateway: "braintree",
                                cardholder: "John Smith",
                                message: "Cardholder disputes quality/ mischaracterization of service/merchandise. Supply detailed refute of these claims, along with any applicable/supporting doc");

            List<FulfillmentDetails> fulfillments = new List<FulfillmentDetails>();
            var fulfillmentDetails = new FulfillmentDetails(
                                             fulfillmentId: "123",
                                             createdAt: new DateTimeOffset(2018, 12, 8, 14, 12, 12, new TimeSpan(-7, 0, 0)),
                                             status: FulfillmentStatusCode.Success,
                                             lineItems: new LineItem[] { new LineItem("Bag", 10.0, 1) },
                                             trackingCompany: "TestCompany");
            fulfillments.Add(fulfillmentDetails);
            var disputeDetails = new DisputeDetails(
                                        disputeType: "first_dispute",
                                        caseId: "a1234",
                                        status: "pending",
                                        issuerPocPhoneNumber: "+1-877-111-1111",
                                        disputedAt: new DateTime(2016, 9, 15),
                                        expectedResolutionDate: new DateTime(2016, 11, 1, 0, 0, 0, DateTimeKind.Local));

            return new OrderChargeback(orderNum, chargebackDetails, fulfillments, disputeDetails);

        }

        private static OrderDecision GenerateDecision(int p)
        {
            // make sure to initialize DateTime with the correct timezone
            OrderDecision orderDecision = new OrderDecision(p, new DecisionDetails(ExternalStatusType.ChargebackFraud, DateTime.Now, "used proxy and stolen credit card."));
            // add payment details from the gateway in pre-auth flow 
            var paymentDetails = new[] { new CreditCardPaymentDetails(null, null, null, null, null, "016225891") };
            orderDecision.PaymentDetails = paymentDetails; 
            return orderDecision;
        }

        private static OrderCheckout GenerateOrderCheckout(string orderNum)
        {
            var orderCheckout = new OrderCheckout(orderNum);

            var address = new AddressInformation(
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

            var payments = new[] {
                new CreditCardPaymentDetails(
                    avsResultCode: "Y",
                    cvvResultCode: "n",
                    creditCardBin: "124580",
                    creditCardCompany: "Visa",
                    creditCardNumber: "XXXX-XXXX-XXXX-4242",
                    creditCardToken: "2233445566778899"
                )
            };

            var lines = new[]
            {
                new ShippingLine(price: 22.22,title: "Mail")
            };

            // This is an example for client details section
            var clientDetails = new ClientDetails(
                accept_language: "en-CA",
                user_agent: "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"
            );


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
                new LineItem(title: "Bag",price: 55.44,quantityPurchased: 1,productId: "48484",sku: "1272727", registryType: RegistryType.Other),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3)
            };

            var discountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "1") };

            orderCheckout.Email = "test@example.com";
            orderCheckout.Currency = "USD";
            orderCheckout.UpdatedAt = DateTime.Now; // make sure to initialize DateTime with the correct timezone
            orderCheckout.Gateway = "authorize_net";
            orderCheckout.CustomerBrowserIp = "165.12.1.1";
            orderCheckout.TotalPrice = 100.60;
            orderCheckout.CartToken = "a68778783ad298f1c80c3bafcddeea02f";
            orderCheckout.ReferringSite = "nba.com";
            orderCheckout.LineItems = items;
            orderCheckout.DiscountCodes = discountCodes;
            orderCheckout.ShippingLines = lines;
            orderCheckout.PaymentDetails = payments;
            orderCheckout.Customer = customer;
            orderCheckout.BillingAddress = address;
            orderCheckout.ShippingAddress = address;
            orderCheckout.ClientDetails = clientDetails;

            return orderCheckout;

        }

        private static OrderCheckoutDenied GenerateOrderCheckoutDenied(int orderNum)
        {
            var authorizationError = new AuthorizationError(
                                    createdAt: new DateTime(2013, 12, 8, 14, 12, 12, DateTimeKind.Local), // make sure to initialize DateTime with the correct timezone
                                    errorCode: "Card Declined",
                                    message: "Card was Declined.");

            var payments = new[] {
                new CreditCardPaymentDetails(
                    avsResultCode: "Y",
                    cvvResultCode: "n",
                    creditCardBin: "124580",
                    creditCardCompany: "Visa",
                    creditCardNumber: "XXXX-XXXX-XXXX-4242",
                    creditCardToken: "2233445566778899"
                )
                {
                    AuthorizationError = authorizationError
                }
            };

            var orderCheckoutDenied = new OrderCheckoutDenied(orderNum.ToString())
            {
                PaymentDetails = payments
            };

            return orderCheckoutDenied;

        }

        private static OrderFulfillment GenerateFulfillment(int fulfillOrderId)
        {
            FulfillmentDetails[] fulfillmentList = {
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



        private static OrderCheckout GenerateAdviseOrderCheckout(string orderNum)
        {
            var orderCheckout = new OrderCheckout(orderNum);

            var address = new AddressInformation(
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
            AuthenticationResult ar = new AuthenticationResult("05");


            var payments = new[] {
                new CreditCardPaymentDetails(
                    avsResultCode: "Y",
                    cvvResultCode: "n",
                    creditCardBin: "124580",
                    creditCardCompany: "Visa",
                    creditCardNumber: "XXXX-XXXX-XXXX-4242",
                    creditCardToken: "2233445566778899"
                ) {AuthenticationResult = ar}

            };

            var lines = new[]
            {
                new ShippingLine(price: 22.22,title: "Mail")
            };

            // This is an example for client details section
            var clientDetails = new ClientDetails(
                accept_language: "en-CA",
                user_agent: "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"
            );


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
                new LineItem(title: "Bag",price: 55.44,quantityPurchased: 1,productId: "48484",sku: "1272727"),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3)
            };

            var discountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "1") };

            orderCheckout.Email = "tester@exampler.com";
            orderCheckout.Currency = "USD";
            orderCheckout.UpdatedAt = DateTime.Now; // make sure to initialize DateTime with the correct timezone
            orderCheckout.Gateway = "authorize_net";
            orderCheckout.CustomerBrowserIp = "165.12.1.1";
            orderCheckout.TotalPrice = 100.60;
            orderCheckout.CartToken = "a68778783ad298f1c80c3bafcddeea02f";
            orderCheckout.ReferringSite = "nba.com";
            orderCheckout.LineItems = items;
            orderCheckout.DiscountCodes = discountCodes;
            orderCheckout.ShippingLines = lines;
            orderCheckout.PaymentDetails = payments;
            orderCheckout.Customer = customer;
            orderCheckout.BillingAddress = address;
            orderCheckout.ShippingAddress = address;
            orderCheckout.ClientDetails = clientDetails;

            return orderCheckout;

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
                createdAt: new DateTimeOffset(2013, 12, 8, 14, 12, 12, new TimeSpan(-6, 0, 0)),
                updatedAt: new DateTimeOffset(DateTime.Now),
                verifiedEmailAt: new DateTimeOffset(new DateTime(2020, 2, 28), new TimeSpan(-8, 0, 0)),
                verifiedPhone: true, 
                verifiedPhoneAt: new DateTime(2019, 8, 28, 12, 0, 0),
                firstPurchaseAt: new DateTime(2013, 6, 18, 0, 0, 0, DateTimeKind.Local),
                notes: "No additional info",
                address: customerAddress,
                accountType: "Premium");

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

            var payments = new[] {
                new CreditCardPaymentDetails(
                    avsResultCode: "Y",
                    cvvResultCode: "n",
                    creditCardBin: "124580",
                    creditCardCompany: "Visa",
                    creditCardNumber: "XXXX-XXXX-XXXX-4242",
                    creditCardToken: "2233445566778899"
                )
            };
            payments[0].VerificationType = "no_auth";

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

            var recipientSocial = new SocialDetails(
                                                    network: "Facebook",
                                                    publicUsername: "john.smith",
                                                    accountUrl: "http://www.facebook.com/john.smith");

            var recipient = new Recipient(
                email: "aa@bb.com",
                phone: "96522444221",
                social: recipientSocial);
            recipient.Fingerprint = "fingerprint";
            recipient.BankName = "Chase";

            Wallet wallet = new Wallet();
            wallet.Id = "57";
            wallet.Type = "apple walleet";
            recipient.Wallet = wallet;


            var items = new[]
            {
                new LineItem(title: "Bag", price: 55.44, quantityPurchased: 1, productId: "48484", sku: "1272727",
                    deliveredTo: DeliveredToType.StorePickup,
                    delivered_at: new DateTime(2016, 12, 8, 14, 12, 12, DateTimeKind.Local), registryType: RegistryType.Wedding, recipient: recipient, productType:ProductType.Remittance),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3,
                    seller: new Seller(customer: customer, correspondence: 1, priceNegotiated: true, startingPrice: 120)),
                // Events Tickets Product (aplicaible for event industry merchants)
                new EventTicketLineItem(
                    title: "Concert",
                    price: 123,
                    quantityPurchased: 1,
                    eventDate: new DateTime(2019, 7, 12, 11, 40, 00, DateTimeKind.Local),
                    city: "New York", 
                    category: "Singers",
                    subCategory: "Rock",
                    section: "Section",
                    countryCode: "US",
                    latitude: 0,
                    longitude: 0),
                // Giftcard Product (appliciable for giftcard industry merchants)
                new DigitalLineItem(
                    title: "Concert",
                    price: 123,
                    quantityPurchased: 1,
                    senderName: "John",
                    displayName: "JohnJohn",
                    photoUploaded: true,
                    photoUrl: "http://my_pic_url",
                    greetingPhotoUrl: "http://my_greeting_pic_url",
                    message: "Happy Birthday",
                    greetingMessage: "Happy Birthday from John",
                    cardType: "regular",
                    cardSubType: "birthday",
                    senderEmail: "new_email@bb.com",
                    recipient: recipient), 
                // Travel ticket product (appliciable for travel industry merchants)
                new TravelTicketLineItem(title: "Concert",
                    price: 123,
                    quantityPurchased: 1,
                    departureCity: "ashdod",
                    departureCountryCode: "IL",
                    transportMethod: TransportMethodType.Plane), 
                // Accommodation reservation product (appliciable for travel industry merchants)
                new AccommodationLineItem(
                    title: "Hotel Arcadia - Standard Room", 
                    price: 476, 
                    quantityPurchased: 1, 
                    productId: "123", 
                    city: "London",
                    countryCode: "GB",
                    rating: "5",
                    numberOfGuests: 2,
                    cancellationPolicy: "Not appliciable",
                    accommodationType: "Hotel"),
                    // Ride Ticket Product 
                new RideTicketLineItem(
                    title: "Ride to JFK airport",
                    price: 74,
                    quantityPurchased: 1,
                    pickupAddress: shipping, 
                    dropoffAddress: billing, 
                    pickupDate: new DateTime(2019, 8, 1, 12, 1, 1, DateTimeKind.Local),
                    pickupLatitude: 0,
                    pickupLongitude: 0,
                    dropoffLatitude: 1, 
                    dropoffLongitude: 1, 
                    routeIndex: 1, 
                    legIndex: 1,
                    transportMethod: "Taxi",
                    priceBy: "fixed",
                    vehicleClass: "executive",
                    carrierName: "Best darn taxi company in the world!",
                    driverId: "15EGT701",
                    meetNGreet: "Whenever you meet me, please greet me.",
                    cancellationPolicy: "24 hours in advance",
                    authorizedPayments: 74
                )
            };

            var discountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "1") };

            DecisionDetails decisionDetails = new DecisionDetails(ExternalStatusType.Approved, DateTime.Now); // make sure to initialize DateTime with the correct timezone

            // This is an example for an order with charge free sums (e.g. gift card payment)
            var chargeFreePayments = new ChargeFreePaymentDetails(
                gateway: "giftcard",
                amount: 45);

            // This is an example for client details section
            var clientDetails = new ClientDetails(
                accept_language: "en-CA",
                user_agent: "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
            
            var custom = new Custom(
                app_dom_id: "D2C"
                );

            var order = new Order(
                merchantOrderId: orderNum.ToString(),
                email: "test@example.com",
                customer: customer,
                paymentDetails: payments,
                billingAddress: billing,
                shippingAddress: shipping,
                lineItems: items,
                shippingLines: lines,
                cartToken: "68778783ad298f1c80c3bafcddeea02f",
                deviceId: "01234567-89ABCDEF-01234567-89ABCDEF",
                gateway: "authorize_net",
                customerBrowserIp: "165.12.1.1",
                currency: "USD",
                totalPrice: 100.60,
                createdAt: new DateTime(2020, 4, 18, 12, 0, 0, DateTimeKind.Local), // make sure to initialize DateTime with the correct timezone
                updatedAt: DateTime.Now, // make sure to initialize DateTime with the correct timezone
                discountCodes: discountCodes,
                source: "web",
                noChargeDetails: noChargeAmount,
                decisionDetails: decisionDetails,
                vendorId: "2",
                vendorName: "domestic",
                additionalEmails: new[] { "a@a.com", "b@b.com" },
                chargeFreePaymentDetails: chargeFreePayments,
                clientDetails: clientDetails,
                custom: custom,
                groupFounderOrderID: "2222",
                submissionReason: "Manual Decision"
                );

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

            var payments = new[] {
                new PaypalPaymentDetails(
                    paymentStatus: "Authorized",
                    authorizationId: "AFSDF332432SDF45DS5FD",
                    payerEmail: "payer@gmail.com",
                    payerStatus: "Verified",
                    payerAddressStatus: "Unverified",
                    protectionEligibility: "Partly Eligibile",
                    pendingReason: "Review"
                )
            };

            var lines = new[]
            {
                new ShippingLine(price: 22.22,title: "Mail"),
                new ShippingLine(price: 2,title: "Ship",code: "A22F")
            };

            var items = new[]
            {
                new LineItem(title: "Bag",price: 55.44,quantityPurchased: 1,productId: "48484", sku: "1272727", registryType: RegistryType.Baby),
                new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3)
            };

            var discountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "1") };

            var order = new Order(
                merchantOrderId: orderNum.ToString(),
                email: "test@example.com",
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

        private static OrderIdOnly GenerateOrderIdOnly(string orderNum)
        {
            return new OrderIdOnly(orderNum);
        }

        private static ClientDetails GenerateClientDetails()
        {
            return new ClientDetails("en-CA", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
        }

        private static SessionDetails GenerateSessionDetails()
        {
            return new SessionDetails(DateTime.Now, "68778783ad298f1c80c3bafcddeea02f", "111.111.111.111", SessionSource.DesktopWeb)
            {
                DeviceId = "01234567-89ABCDEF-01234567-89ABCDEF"
            };
        }

        private static Customer GenerateCustomer(string idString)
        {
            return new Customer("Bob", "Norman", idString)
            {
                CreatedAt = DateTime.Now,
                VerifiedEmail = true,
                Email = "bob.norman@hostmail.com"
            };
        }

        private static IPaymentDetails[] GenerateCreditCardPaymentDetails()
        {
            return new[] {
                new CreditCardPaymentDetails("Y", "M", "123456", "Visa", "4242")
                {
                    AuthorizationError = new AuthorizationError(DateTime.Now, "card_declined", "insufficient funds")
                }
            };
        }

        private static AddressInformation GenerateAddressInformation()
        {
            return new AddressInformation("Bob", "Norman", "Chestnut Street 92", "Louisville", "United States", "US", "555-625-1199")
            {
                FullName = "Bob Norman",
                Province = "Kentucky",
                ProvinceCode = "KY",
                ZipCode = "40202"
            };
        }

        private static Login GenerateLogin(string idString)
        {
            var loginStatus = new LoginStatus(LoginStatusType.Success);
            var clientDetails = GenerateClientDetails();
            var sessionDetails = GenerateSessionDetails();

            return new Login(idString, "bob.norman@hostmail.com", loginStatus, clientDetails, sessionDetails)
            {
                LoginAtCheckout = true,
                SocialLoginType = SocialType.Amazon,
                CustomerCreatedAt = DateTime.Now 
            };
        }

        private static CustomerCreate GenerateCustomerCreate(string idString)
        {
            var addresses = new[] {
                GenerateAddressInformation()
            };

            return new CustomerCreate(idString, GenerateClientDetails(), GenerateSessionDetails(), GenerateCustomer(idString))
            {
                PaymentDetails = GenerateCreditCardPaymentDetails(),
                BillingAddress = addresses,
                ShippingAddress = addresses
            };
        }

        private static CustomerUpdate GenerateCustomerUpdate(string idString)
        {
            var addresses = new[] {
                GenerateAddressInformation()
            };

            return new CustomerUpdate(idString, false, GenerateClientDetails(), GenerateSessionDetails(), GenerateCustomer(idString))
            {
                PaymentDetails = GenerateCreditCardPaymentDetails(),
                BillingAddress = addresses,
                ShippingAddress = addresses
            };
        }

        private static Logout GenerateLogout(string idString)
        {
            return new Logout(idString, GenerateClientDetails(), GenerateSessionDetails());
        }

        private static ResetPasswordRequest GenerateResetPasswordRequest(string idString)
        {
            return new ResetPasswordRequest(idString, GenerateClientDetails(), GenerateSessionDetails());
        }

        private static Redeem GenerateRedeem(string idString)
        {
            return new Redeem(idString, RedeemType.GiftCard, GenerateClientDetails(), GenerateSessionDetails());
        }

        private static WishlistChanges GenerateWishlistChanges(string idString)
        {
            var lineItem = new LineItem("IPod Nano - 8gb - green", 199, 1)
            {
                Brand = "Apple",
                ProductId = "632910392",
                ProductType = ProductType.Physical,
                Category = "Electronics"
            };

            return new WishlistChanges(idString, WishlistAction.Add, GenerateClientDetails(), GenerateSessionDetails(), lineItem);
        }

        private static CustomerReachOut GenerateCustomerReachOut(string idString)
        {
            ContactMethod contactMethod = new ContactMethod(ContactMethodType.Email)
            {
                Email = "moo@gmail.com"
            };
            return new CustomerReachOut(idString, contactMethod);
        }

        #region Run all endpoints
        public static int runAll()
        {
            try
            {
                string domain = Environment.GetEnvironmentVariable("RISKIFIED_MERCHANT_DOMAIN") ?? "your_merchant_domain.com";
                string authToken = Environment.GetEnvironmentVariable("RISKIFIED_AUTH_TOKEN") ?? "your_auth_token";
                string envStr = Environment.GetEnvironmentVariable("RISKIFIED_ENVIRONMENT") ?? "Sandbox";
                RiskifiedEnvironment riskifiedEnv = (RiskifiedEnvironment)Enum.Parse(typeof(RiskifiedEnvironment), envStr);

                OrderNotification res = null;
                var rand = new Random();
                int orderNum = rand.Next(1000, 200000);
                var order = GenerateOrder(orderNum);

                OrdersGateway gateway = new OrdersGateway(riskifiedEnv, authToken, domain);

                var orderCheckout = GenerateOrderCheckout(orderNum.ToString());
                orderCheckout.Id = orderNum.ToString();
                res = gateway.Checkout(orderCheckout);

                var orderCheckoutDenied = GenerateOrderCheckoutDenied(orderNum);
                orderNum++;
                orderCheckoutDenied.Id = orderNum.ToString();
                res = gateway.CheckoutDenied(orderCheckoutDenied);

                orderNum++;
                order.Id = orderNum.ToString();
                res = gateway.Create(order);

                order.Id = orderNum.ToString();
                orderNum++;
                res = gateway.Submit(order);

                res = gateway.Update(order);
                res = gateway.Cancel(
                                new OrderCancellation(
                                    merchantOrderId: order.Id,
                                    cancelledAt: DateTime.Now,
                                    cancelReason: "Customer cancelled before shipping"));

                order.Id = orderNum.ToString();
                orderNum++;
                // sending order for creation (if new orderNum) or update (if existing orderNum)
                res = gateway.Create(order);

                order.Id = orderNum.ToString();
                orderNum++;
                // sending order for submitting and analysis 
                // it will generate a callback to the notification webhook (if defined) with a decision regarding the order
                res = gateway.Submit(order);

                order.Id = order.Id;
                res = gateway.Update(order);

                res = gateway.Cancel(
                    new OrderCancellation(
                        merchantOrderId: int.Parse(order.Id),
                        cancelledAt: DateTime.Now,
                        cancelReason: "Customer cancelled before shipping"));

                res = gateway.PartlyRefund(
                    new OrderPartialRefund(
                        merchantOrderId: int.Parse(order.Id),
                        partialRefunds: new[]
                                    {
                                        new PartialRefundDetails(
                                            refundId: "12345",
                                            refundedAt: DateTime.Now,  // make sure to initialize DateTime with the correct timezone
                                            amount: 5.3,
                                            currency: "USD",
                                            reason: "Customer partly refunded on shipping fees")
                                    }));

                OrderFulfillment orderFulfillment = GenerateFulfillment(int.Parse(order.Id));
                res = gateway.Fulfill(orderFulfillment);


                OrderDecision orderDecision = GenerateDecision(int.Parse(order.Id));
                res = gateway.Decision(orderDecision);



            }
            catch (Exception ex)
            {
                Console.WriteLine("[failed] " + ex.ToString());
                return -1;
            }

            return 0;

        } 
        #endregion

    }
}
