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

If you wish to send your own data, change the model object (Order) in the `OrderTransmissionExample.cs` [GenerateOrder method](https://github.com/Riskified/sdk_net/blob/master/Riskified.SDK.Sample/OrderTransmissionExample.cs#L650)


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

