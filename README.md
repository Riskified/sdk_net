sdk_net
=======

An implementation of the Riskified API in C# for .NET  
Refer to the [documentation](http://apiref.riskified.com) for more details.

Running the sample code
-----------------------
All examples are at the _Riskified.SDK.Sample_ folder. For the basics:

1. Copy the ```Riskified.SDK.Sample.config.example``` and rename to ```Riskified.SDK.Sample.config```
1. Edit ```Riskified.SDK.Sample.config``` to include your credentials: 
  - **MerchantDomain** - The same domain you use for login at riskified.com
  - **MerchantAuthenticationToken** - The one you will see in the 'Settings'->'Advanced Settings' tab in the Riskified webapp
  - **RiskifiedEnvironment** - `Sandbox` for Sandbox testing or `Production` for Production live work
1. Build and run the sample project executable

If you wish to send your own data - Change the model object (Order) in the ```OrderTransmissionExample.cs``` [GenerateOrder method] (https://github.com/Riskified/sdk_net/blob/master/Riskified.SDK.Sample/OrderTransmissionExample.cs#L93)


Migrating from older versions (prior to: API v2 - v2.0.0.0)
-----------------------------------------------------------

API Version 2 introduces new features (and breaks some old ones).  

### Orders Gateway ###

This version represents a shift from data-driven order handling to multiple API endpoints and introduces some new Model objects.  
Each endpoint/method designed for a specific purpose:

* `/Create` - served by `ordersGateway.Create(Order)`
* `/Update` - served by `ordersGateway.Update(Order)`
* `/Submit` - served by `ordersGateway.Submit(Order)`
* `/Refund` - served by `ordersGateway.PartlyRefund(OrderPartialRefund)`
* `/Cancel` - served by `ordersGateway.Cancel(OrderCancellation)`
* `/historical` - served by `ordersGateway.SendHistoricalOrders(Orders)`

When migrating from version 1, you'll need to separate the different calls to Riskified's API to support this new process.

