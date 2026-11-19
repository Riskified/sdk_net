using System;
using System.Collections.Generic;
using Riskified.SDK.Model;
using Riskified.SDK.Model.Internal;
using Riskified.SDK.Model.OrderCheckoutElements;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Tests.Golden
{
    /// <summary>
    /// Fully populated samples, wrapped the same way OrdersGateway wraps them before sending.
    /// Every date is fixed (DateTimeOffset with an explicit offset, DateTime with DateTimeKind.Utc)
    /// so the serialized output doesn't depend on the machine's clock or time zone.
    /// </summary>
    internal static class GoldenOrders
    {
        private static readonly TimeSpan NewYork = TimeSpan.FromHours(-5);

        public static IReadOnlyDictionary<string, object> All { get; } = new Dictionary<string, object>
        {
            ["order"] = new OrderWrapper<AbstractOrder>(Order()),
            ["order_checkout"] = new OrderCheckoutWrapper<AbstractOrder>(OrderCheckout()),
            ["order_checkout_denied"] = new OrderCheckoutWrapper<AbstractOrder>(OrderCheckoutDenied()),
            ["order_decision"] = new OrderWrapper<AbstractOrder>(OrderDecision()),
        };

        private static Order Order()
        {
            var customer = Customer();
            var billing = BillingAddress();
            var shipping = ShippingAddress();

            var order = new Order(
                merchantOrderId: "golden-1001",
                email: "john.doe@example.com",
                customer: customer,
                billingAddress: billing,
                shippingAddress: shipping,
                lineItems: LineItems(customer, billing, shipping),
                shippingLines: ShippingLines(),
                gateway: "authorize_net",
                customerBrowserIp: "165.12.1.1",
                currency: "USD",
                totalPrice: 1205.5,
                createdAt: new DateTimeOffset(2026, 3, 1, 10, 0, 0, NewYork),
                updatedAt: new DateTimeOffset(2026, 3, 1, 10, 5, 0, NewYork),
                passengers: Passengers(),
                paymentDetails: new IPaymentDetails[]
                {
                    CreditCardPaymentDetails(),
                    PaypalPaymentDetails(),
                    WalletPaymentDetails(),
                    BankWirePaymentDetails()
                },
                discountCodes: new[] { new DiscountCode(moneyDiscountSum: 7, code: "SPRING7", policy: new Policy(evaluate: true)) },
                totalDiscounts: 7,
                cartToken: "68778783ad298f1c80c3bafcddeea02f",
                deviceId: "01234567-89ABCDEF-01234567-89ABCDEF",
                totalPriceUsd: 1205.5,
                closedAt: new DateTimeOffset(2026, 3, 1, 11, 0, 0, NewYork),
                financialStatus: "paid",
                fulfillmentStatus: "fulfilled",
                source: "web",
                noChargeDetails: new NoChargeDetails(refundId: "refund-1", amount: 20.5, currency: "USD", reason: "giftcard"),
                additionalEmails: new[] { "a@example.com", "b@example.com" },
                vendorId: "vendor-2",
                vendorName: "domestic",
                vendorIntegrationType: "api",
                decisionDetails: new DecisionDetails(ExternalStatusType.Approved, new DateTimeOffset(2026, 3, 1, 10, 1, 0, NewYork), reason: "trusted customer"),
                clientDetails: new ClientDetails(accept_language: "en-US", user_agent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 14_0)"),
                chargeFreePaymentDetails: new ChargeFreePaymentDetails(gateway: "giftcard", amount: 45),
                groupFounderOrderID: "golden-1000",
                referringSite: "example.com",
                note: "leave at the door",
                name: "#1001",
                orderType: "test",
                submissionReason: "manual_decision",
                custom: new Custom(app_dom_id: "D2C"));

            order.AiAgent = AiAgent.Chatgpt;
            order.DecisionTimeout = 30;
            return order;
        }

        private static OrderCheckout OrderCheckout()
        {
            var customer = Customer();
            var billing = BillingAddress();
            var shipping = ShippingAddress();

            return new OrderCheckout("golden-2001")
            {
                Name = "#2001",
                Email = "john.doe@example.com",
                Currency = "USD",
                CreatedAt = new DateTimeOffset(2026, 3, 2, 9, 0, 0, NewYork),
                UpdatedAt = new DateTimeOffset(2026, 3, 2, 9, 1, 0, NewYork),
                Gateway = "authorize_net",
                CustomerBrowserIp = "165.12.1.1",
                TotalPrice = 100.6,
                TotalDiscounts = 7,
                CartToken = "a68778783ad298f1c80c3bafcddeea02f",
                DeviceId = "01234567-89ABCDEF-01234567-89ABCDEF",
                ReferringSite = "example.com",
                Source = "mobile_app",
                AiAgent = AiAgent.Gemini,
                LineItems = new[]
                {
                    new LineItem(title: "Bag", price: 55.44, quantityPurchased: 1, productId: "48484", sku: "1272727",
                        registryType: RegistryType.Other, productType: ProductType.Physical, brand: "Acme",
                        category: "Bags", subCategory: "Backpacks", seller: new Seller(customer, correspondence: 1, priceNegotiated: true, startingPrice: 120)),
                    new LineItem(title: "Monster", price: 22.3, quantityPurchased: 3)
                },
                DiscountCodes = new[] { new DiscountCode(moneyDiscountSum: 7, code: "SPRING7") },
                ShippingLines = ShippingLines(),
                PaymentDetails = new IPaymentDetails[] { CreditCardPaymentDetails() },
                Customer = customer,
                BillingAddress = billing,
                ShippingAddress = shipping,
                ClientDetails = new ClientDetails(accept_language: "en-CA", user_agent: "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)")
            };
        }

        private static OrderCheckoutDenied OrderCheckoutDenied()
        {
            // The root-level error, the card's own error and a wallet with no error of its own
            // together cover the cases step 3.4 changes.
            var rootError = new AuthorizationError(new DateTimeOffset(2026, 3, 3, 8, 0, 0, NewYork), "card_declined", "Card was declined.");

            var card = CreditCardPaymentDetails();
            card.AuthorizationError = new AuthorizationError(new DateTimeOffset(2026, 3, 3, 8, 0, 1, NewYork), "insufficient_funds", "Insufficient funds.")
            {
                DropOff = true
            };

            return new OrderCheckoutDenied("golden-3001", rootError)
            {
                Email = "john.doe@example.com",
                Currency = "USD",
                TotalPrice = 100.6,
                TotalDiscounts = 0,
                CreatedAt = new DateTimeOffset(2026, 3, 3, 7, 59, 0, NewYork),
                Gateway = "authorize_net",
                CustomerBrowserIp = "165.12.1.1",
                CartToken = "b68778783ad298f1c80c3bafcddeea02f",
                Source = "desktop_web",
                ClientDetails = new ClientDetails(accept_language: "en-US", user_agent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 14_0)"),
                LineItems = new[]
                {
                    new LineItem(title: "Bag", price: 100.6, quantityPurchased: 1, productId: "48484", requiresShipping: true,
                        category: "Bags", productType: ProductType.Physical)
                },
                PaymentDetails = new IPaymentDetails[] { card, WalletPaymentDetails() },
                Customer = Customer(),
                BillingAddress = BillingAddress()
            };
        }

        private static OrderDecision OrderDecision()
        {
            // Payment details and notes go through the constructors on purpose: 5.6.0 drops both
            // (steps 1.1 and 1.2), so this snapshot is where those fixes show up.
            var decision = new DecisionDetails(
                ExternalStatusType.DeclinedFraud,
                new DateTimeOffset(2026, 3, 4, 12, 0, 0, NewYork),
                reason: "used proxy and stolen credit card",
                amount: 100.5f,
                currency: "USD",
                notes: "reviewed by fraud team");

            return new OrderDecision("golden-4001", decision, new IPaymentDetails[] { CreditCardPaymentDetails() })
            {
                SubmissionReason = "manual_decision"
            };
        }

        private static Customer Customer()
        {
            var customer = new Customer(
                firstName: "John",
                lastName: "Doe",
                id: "405050606",
                ordersCount: 4,
                email: "john.doe@example.com",
                verifiedEmail: true,
                verifiedEmailAt: new DateTimeOffset(2020, 2, 28, 0, 0, 0, NewYork),
                verifiedPhone: true,
                verifiedPhoneAt: new DateTimeOffset(2019, 8, 28, 12, 0, 0, NewYork),
                createdAt: new DateTimeOffset(2013, 12, 8, 14, 12, 12, NewYork),
                updatedAt: new DateTimeOffset(2026, 1, 15, 9, 30, 0, NewYork),
                notes: "No additional info",
                social: new[] { SocialDetails() },
                // Customer.Address is typed BasicAddress, but the docs require first and last name,
                // which the AddressInformation subclass adds.
                address: new AddressInformation(firstName: "John", lastName: "Doe", address1: "27 5th avenue", city: "Manhattan",
                    country: "United States", countryCode: "US", phone: "5554321234", address2: "Apartment 5", zipCode: "10001",
                    province: "New York", provinceCode: "NY", company: "IBM")
                {
                    VerifiedPhone = true,
                    AdditionalPhone = "5554321235"
                },
                accountType: "premium",
                linkedAccounts: 2,
                firstPurchaseAt: new DateTimeOffset(2013, 6, 18, 0, 0, 0, NewYork),
                documentId: "123456789",
                documentType: "passport",
                totalSpent: 1500.5f);

            customer.AnonymousId = "anon-42";
            customer.SpendingLimit = 5000;
            customer.HasDefaulted = false;
            customer.DateOfBirth = new DateTimeOffset(1985, 5, 20, 0, 0, 0, TimeSpan.Zero);
            customer.KycDetails = new[]
            {
                new KycDetails
                {
                    VendorName = "Onfido",
                    UpdatedAt = new DateTimeOffset(2025, 11, 1, 10, 0, 0, NewYork),
                    KycVerified = true,
                    KycType = "strong"
                }
            };
            return customer;
        }

        private static SocialDetails SocialDetails()
        {
            return new SocialDetails(
                network: "facebook",
                publicUsername: "john.doe",
                communityScore: 12,
                profilePictureUrl: "https://example.com/john.png",
                email: "john.doe@example.com",
                bio: "Hello",
                accountUrl: "https://www.facebook.com/john.doe",
                following: 10,
                followed: 20,
                posts: 30);
        }

        private static AddressInformation BillingAddress()
        {
            return new AddressInformation(
                firstName: "Ben",
                lastName: "Rolling",
                address1: "27 5th avenue",
                city: "Manhattan",
                country: "United States",
                countryCode: "US",
                phone: "5554321234",
                address2: "Apartment 5",
                zipCode: "10001",
                province: "New York",
                provinceCode: "NY",
                company: "IBM",
                fullName: "Ben Philip Rolling");
        }

        private static AddressInformation ShippingAddress()
        {
            return new AddressInformation(
                firstName: "Luke",
                lastName: "Rolling",
                address1: "4 Bermingham street",
                city: "Cherry Hill",
                country: "United States",
                countryCode: "US",
                phone: "55546665",
                zipCode: "08002",
                province: "New Jersey",
                provinceCode: "NJ");
        }

        private static ShippingLine[] ShippingLines()
        {
            return new[]
            {
                new ShippingLine(price: 22.22, title: "Mail"),
                new ShippingLine(price: 2, title: "Ship", code: "A22F")
            };
        }

        private static Passenger[] Passengers()
        {
            return new[]
            {
                new Passenger(
                    firstname: "John",
                    lastname: "Doe",
                    dateOfBirth: new DateTime(1985, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                    nationalityCode: "US",
                    insurancePrice: 12.5f,
                    documentNumber: "X1234567",
                    documentType: "passport",
                    documentIssueDate: new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    documentExpirationDate: new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    passengerType: "adult",
                    insuranceType: "full")
            };
        }

        private static CreditCardPaymentDetails CreditCardPaymentDetails()
        {
            return new CreditCardPaymentDetails(
                avsResultCode: "Y",
                cvvResultCode: "M",
                creditCardBin: "424242",
                creditCardCompany: "Visa",
                creditCardNumber: "XXXX-XXXX-XXXX-4242",
                authorizationId: "auth-cc-1",
                creditCardToken: "2233445566778899",
                storedPaymentCreatedAt: new DateTimeOffset(2024, 1, 1, 0, 0, 0, NewYork),
                storedPaymentUpdatedAt: new DateTimeOffset(2025, 1, 1, 0, 0, 0, NewYork),
                installments: 3)
            {
                CardholderName = "John Doe",
                id = "pm-cc-1",
                gateway = "stripe",
                AcquirerBin = "411111",
                Mid = "mid-1",
                AuthenticationResult = AuthenticationResult(),
                InitialPaymentAmount = 400.5f,
                PaymentFrequency = 30,
                CreditCardCountry = "US",
                AcquirerRegion = "NONEU",
                VerificationType = "3ds"
            };
        }

        private static PaypalPaymentDetails PaypalPaymentDetails()
        {
            return new PaypalPaymentDetails(
                paymentStatus: "Authorized",
                authorizationId: "AFSDF332432SDF45DS5FD",
                payerEmail: "payer@example.com",
                payerStatus: "verified",
                payerAddressStatus: "unconfirmed",
                protectionEligibility: "Eligible",
                pendingReason: "authorization");
        }

        private static WalletPaymentDetails WalletPaymentDetails()
        {
            return new WalletPaymentDetails(PaymentType.ApplePay, authorizationId: "auth-wallet-1", avsResultCode: "Y")
            {
                CreditCardCompany = "Mastercard",
                CreditCardCountry = "US",
                CreditCardToken = "wallet-token-1",
                CardholderName = "John Doe",
                Mid = "mid-2",
                id = "pm-wallet-1",
                StoredPaymentCreatedAt = new DateTimeOffset(2024, 2, 1, 0, 0, 0, NewYork),
                StoredPaymentUpdatedAt = new DateTimeOffset(2025, 2, 1, 0, 0, 0, NewYork),
                Installments = 1,
                AcquirerBin = "522222",
                AcquirerRegion = "EU",
                AuthorizationType = AuthorizationType.Authorization,
                ExpiryMonth = 12,
                ExpiryYear = 2030,
                InitialPaymentAmount = 100.25f,
                PaymentFrequency = 0,
                BillingAddressId = "addr-1",
                AuthenticationResult = AuthenticationResult()
            };
        }

        private static BankWirePaymentDetails BankWirePaymentDetails()
        {
            return new BankWirePaymentDetails(accountNumber: "000123456789", routingNumber: "110000000")
            {
                Issuer = "Chase",
                RecipientName = "Acme Inc",
                AccountIdentity = new AccountIdentity
                {
                    Names = new List<string> { "John Doe" },
                    Addresses = new List<BasicAddress> { new BasicAddress(address1: "27 5th avenue", city: "Manhattan", countryCode: "US") },
                    PhoneNumbers = new List<string> { "5554321234" },
                    Emails = new List<string> { "john.doe@example.com" }
                },
                AccountBalance = new AccountBalance(
                    availableBalance: 2500.75,
                    serviceName: AccountBalanceServiceName.Plaid,
                    updatedAt: new DateTimeOffset(2026, 2, 28, 18, 0, 0, NewYork),
                    currencyCode: "USD")
            };
        }

        private static AuthenticationResult AuthenticationResult()
        {
            return new AuthenticationResult("05")
            {
                tranStatus = SDK.Model.OrderCheckoutElements.AuthenticationResult.TranStatus.Y,
                tranStatusReason = "01",
                CreatedAt = new DateTime(2026, 3, 1, 15, 0, 0, DateTimeKind.Utc),
                LiabilityShift = true,
                cavv = "AAABAWFlmQAAAABjRWWZEEFgFz+=",
                ThreeDChallenge = true,
                TraExemption = false
            };
        }

        private static LineItem[] LineItems(Customer customer, AddressInformation billing, AddressInformation shipping)
        {
            var recipient = new Recipient(
                email: "recipient@example.com",
                phone: "96522444221",
                social: new SocialDetails(network: "facebook", publicUsername: "jane.smith", accountUrl: "https://www.facebook.com/jane.smith"))
            {
                AccountNumber = "000987654321",
                RoutingNumber = "110000000",
                BankName = "Chase",
                Fingerprint = "fingerprint-1",
                Wallet = new Wallet { Id = "57", Type = "apple_pay" }
            };

            return new LineItem[]
            {
                new LineItem(title: "Bag", price: 55.44, quantityPurchased: 1, productId: "48484", sku: "1272727",
                    condition: "new", requiresShipping: true, seller: new Seller(customer, correspondence: 1, priceNegotiated: true, startingPrice: 120),
                    deliveredTo: DeliveredToType.StorePickup, delivered_at: new DateTime(2026, 3, 5, 14, 0, 0, DateTimeKind.Utc),
                    productType: ProductType.Physical, brand: "Acme", category: "Bags", subCategory: "Backpacks",
                    policy: new Policy(evaluate: true), registryType: RegistryType.Wedding),
                new LineItem(title: "Money transfer", price: 200, quantityPurchased: 1, productId: "remit-1", requiresShipping: true,
                    category: "Remittance", productType: ProductType.Remittance, recipient: recipient),
                new EventTicketLineItem(
                    title: "Concert",
                    price: 123,
                    quantityPurchased: 1,
                    productId: "event-1",
                    requiresShipping: false,
                    category: "Singers",
                    subCategory: "Rock",
                    section: "Section A",
                    eventDate: new DateTime(2026, 7, 12, 19, 30, 0, DateTimeKind.Utc),
                    city: "New York",
                    countryCode: "US",
                    latitude: 40.75f,
                    longitude: -73.5f),
                new DigitalLineItem(
                    title: "Gift card",
                    price: 50,
                    quantityPurchased: 1,
                    productId: "digital-1",
                    requiresShipping: false,
                    category: "Gift cards",
                    senderName: "John",
                    displayName: "JohnJohn",
                    photoUploaded: true,
                    photoUrl: "https://example.com/photo.png",
                    greetingPhotoUrl: "https://example.com/greeting.png",
                    message: "Happy Birthday",
                    greetingMessage: "Happy Birthday from John",
                    cardType: "regular",
                    cardSubType: "birthday",
                    senderEmail: "john.doe@example.com",
                    recipient: recipient),
                new TravelTicketLineItem(
                    title: "Flight TLV-JFK",
                    price: 600,
                    quantityPurchased: 1,
                    productId: "flight-1",
                    requiresShipping: false,
                    category: "Flights",
                    legId: "leg-1",
                    departurePortCode: "TLV",
                    departureCity: "Tel Aviv",
                    departureCountryCode: "IL",
                    arrivalPortCode: "JFK",
                    arrivalCity: "New York",
                    arrivalCountryCode: "US",
                    departureDate: new DateTime(2026, 6, 1, 6, 0, 0, DateTimeKind.Utc),
                    arrivalDate: new DateTime(2026, 6, 1, 18, 0, 0, DateTimeKind.Utc),
                    carrierName: "El Al",
                    carrierCode: "LY",
                    routeIndex: 0,
                    legIndex: 0,
                    ticketClass: "economy",
                    transportMethod: TransportMethodType.Plane),
                new AccommodationLineItem(
                    title: "Hotel Arcadia - Standard Room",
                    price: 476,
                    quantityPurchased: 1,
                    productId: "123",
                    requiresShipping: false,
                    category: "Hotels",
                    roomType: "standard",
                    city: "London",
                    countryCode: "GB",
                    checkInDate: new DateTime(2026, 6, 2, 15, 0, 0, DateTimeKind.Utc),
                    checkOutTime: new DateTime(2026, 6, 5, 11, 0, 0, DateTimeKind.Utc),
                    rating: "5",
                    numberOfGuests: 2,
                    cancellationPolicy: "free cancellation",
                    accommodationType: "hotel"),
                new RideTicketLineItem(
                    title: "Ride to JFK airport",
                    price: 74,
                    quantityPurchased: 1,
                    productId: "ride-1",
                    requiresShipping: false,
                    category: "Rides",
                    pickupDate: new DateTime(2026, 6, 1, 3, 0, 0, DateTimeKind.Utc),
                    pickupLatitude: 40.5f,
                    pickupLongitude: -74.25f,
                    pickupAddress: shipping,
                    dropoffDate: new DateTime(2026, 6, 1, 4, 0, 0, DateTimeKind.Utc),
                    dropoffLatitude: 40.75f,
                    dropoffLongitude: -73.75f,
                    dropoffAddress: billing,
                    transportMethod: "taxi",
                    priceBy: "fixed",
                    vehicleClass: "executive",
                    carrierName: "Best Taxi",
                    driverId: "15EGT701",
                    tariff: "day",
                    noteToDriver: "call on arrival",
                    meetNGreet: "yes",
                    cancellationPolicy: "24 hours in advance",
                    authorizedPayments: 74,
                    routeIndex: 1,
                    legIndex: 1)
            };
        }
    }
}
