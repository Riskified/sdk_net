# Riskified .NET SDK

An implementation of the Riskified API in C# for .NET  
Refer to the [documentation](https://developers.riskified.com/reference/api-overview) for more details.

## Running the sample code

All examples are at the _Riskified.SDK.Sample_ folder. For the basics:

1. Edit [`launchSettings.json`](Riskified.SDK.Sample/Properties/launchSettings.json) to include: 
    - **RISKIFIED_MERCHANT_DOMAIN** - The same domain you use for login at riskified.com
    - **RISKIFIED_AUTH_TOKEN** - Auth token to access our API. Can be found in our web app under _'Settings'->'Developers'_ 
    - **RISKIFIED_ENVIRONMENT** - `Sandbox` for Sandbox testing or `Production` for Production live work
    - **RISKIFIED_NOTIFICATIONS_WEBHOOK_URL** - Webhook URL for notifications
2. Build and run the sample project executable

If you wish to send your own data, change the model object (Order) in the `GenerateOrder` method of [`OrderTransmissionExample.cs`](https://github.com/Riskified/sdk_net/blob/master/Riskified.SDK.Sample/OrderTransmissionExample.cs). It also shows the fields added in 5.7.0 and 5.8.0.

## Upgrading to 5.8.0

5.8.0 sends the JSON shapes the [API reference](https://developers.riskified.com/reference/api-overview) documents. Your code keeps compiling, but some requests look different:

* `billing_address` and `shipping_address` are arrays (except on `/checkout_denied`, which documents a single object). Use the new `BillingAddresses` / `ShippingAddresses` for more than one address; `BillingAddress` / `ShippingAddress` set the first one.
* `charge_free_payment_details` and a ride's `pickup_address` / `dropoff_address` are arrays, with the new `ChargeFreePaymentDetailsList`, `PickupAddresses` and `DropoffAddresses`.
* On `/checkout_denied`, an order-level `AuthorizationError` is also sent inside card and wallet payment details that have none of their own.
* `dropoff_latitude`, `authorized_payment` and KYC `update_at` are sent alongside the old `dropoff_latitiude`, `authorized_payments` and `updated_at`.
* `seller.starting_price` is sent as a string (`"120.00"`).
* The `Accept` header is `application/vnd.riskified.com; version=2` again (the OTP endpoint keeps `application/json`).

If your integration depends on the 5.7.x format, set `gateway.UseLegacyWireFormat = true` on your `OrdersGateway` while you move over. It's obsolete and will be removed in 6.0.0.


## Migrating from older versions (prior to: API v2 - v2.0.0.0)

API Version 2 introduces new features (and breaks some old ones).  

### Orders Gateway

This version represents a shift from data-driven order handling to multiple API endpoints and introduces some new Model objects.  
Each endpoint/method designed for a specific purpose:

* `/Create` - served by `ordersGateway.Create(Order)`
* `/Update` - served by `ordersGateway.Update(Order)`
* `/Submit` - served by `ordersGateway.Submit(Order)`
* `/Refund` - served by `ordersGateway.PartlyRefund(OrderPartialRefund)`
* `/Cancel` - served by `ordersGateway.Cancel(OrderCancellation)`
* `/historical` - served by `ordersGateway.SendHistoricalOrders(Orders)`

When migrating from version 1, you'll need to separate the different calls to Riskified's API to support this new process.

