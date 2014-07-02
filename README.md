sdk_net
=======

An implementation of the Riskified API in C# for .NET

# Running the examples
All examples are in the _Riskified.SDK.Sample_ folder. For the basics:

1. Copy the ```Riskified.SDK.Sample.config.example``` to ```Riskified.SDK.Sample.config```
1. Edit Riskified.SDK.Sample.config to include your credentials: 
  - **merchant domain** - The same domain you use for login
  - **auth_token** - The one you will see in the 'Settings'->'Advanced Settings' tab in the Riskified webapp
  - **Riskified host url** - https://sandbox.riskified.com for Sandbox or  https://wh.riskified.com for Production
1. Build and run the sample project exe

If you want to send your own data - Change the details in the ```OrderTransmissionExample.cs``` [GenerateOrder method] (https://github.com/Riskified/sdk_net/blob/master/Riskified.SDK.Sample/OrderTransmissionExample.cs#L93)
