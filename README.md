Riskified .NET SDK
=======

## 📚 Documentation

For complete API documentation, visit [Riskified API Reference](http://apiref.riskified.com).

## 🏁 Getting Started

### Installation

#### NuGet Package Manager

```powershell
Install-Package Riskified.SDK
```

### .NET CLI
```bash
dotnet add package Riskified.SDK
```

### Configuration
Configure your `shopurl` and `authtoken` in the ``App.config`` file:
```xml
<appSettings>
  <add key="MerchantDomain" value="your-shopUrl" />
  <add key="MerchantAuthenticationToken" value="your-auth-token" />
  <add key="RiskifiedEnvironment" value="Sandbox" />
</appSettings>
```

## 🔍 Orders Gateway

Each endpoint/method designed for a specific purpose:

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/Create` | `ordersGateway.Create(Order)` | Create a new order |
| `/Update` | `ordersGateway.Update(Order)` | Update an existing order |
| `/Submit` | `ordersGateway.Submit(Order)` | Submit an order for analysis |
| `/Refund` | `ordersGateway.PartlyRefund(OrderPartialRefund)` | Process a partial refund |
| `/Cancel` | `ordersGateway.Cancel(OrderCancellation)` | Cancel an order |
| `/historical` | `ordersGateway.SendHistoricalOrders(Orders)` | Send historical orders data |


## Examples
### Creating an Order
```csharp
csharp
// Initialize the gateway
var gateway = new OrdersGateway();

// Create an order
var order = new Order
{
    Id = "123",
    Email = "customer@example.com",
    // Add all required order properties
};

// Send the order to Riskified
OrderResponse response = gateway.Create(order);
```

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


