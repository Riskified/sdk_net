using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Riskified.NetSDK.Definitions;
using Riskified.NetSDK.Model;
using Riskified.NetSDK.Exceptions;

namespace Riskified.NetSDK.Control
{
    /// <summary>
    /// Main class to handle order creation and submittion to Riskified Servers
    /// </summary>
    public class RiskifiedGateway
    {
        private static readonly string ProductVersion;
        private readonly Uri _riskifiedOrdersTransferAddr;
        private readonly string _signature;
        private readonly string _shopDomain;
        // TODO add test class
        //TODO add Logging messages
        static RiskifiedGateway()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            ProductVersion = fileVersionInfo.ProductVersion;
        }

        public RiskifiedGateway(Uri riskifiedOrdersTransferAddrAddress, string authToken, string shopDomain)
        {
            _riskifiedOrdersTransferAddr = riskifiedOrdersTransferAddrAddress;
            // TODO make sure signature and domain are of valid structure
            _signature = authToken;
            _shopDomain = shopDomain;
        }

        /// <summary>
        /// Sends an order created/updated to Riskified Servers (without Submit for analysis)
        /// </summary>
        /// <param name="order">The Order to create or update</param>
        /// <returns>an order transfer unique id on riskified servers</returns>
        public int CreateOrUpdateOrder(Order order)
        {
            return SendOrder(order, false);
        }

        /// <summary>
        /// Sends an order to Riskified Servers and submits it for analysis
        /// </summary>
        /// <param name="order">The Order to submit</param>
        /// <returns>an order transfer unique id on riskified servers</returns>
        public int SubmitOrder(Order order)
        {
            return SendOrder(order, true);
        }

        private int SendOrder(Order order,bool isSubmit)
        {
            string jsonOrder = JsonConvert.SerializeObject(order);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonOrder);

            HttpWebRequest request = HttpWebRequest.CreateHttp(_riskifiedOrdersTransferAddr);
            // Set custom Riskified headers
            string hashCode = HttpDefinitions.CalcHmac(jsonOrder,_signature);
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            // TODO add support for gzip compression for non-sandbox env
            request.Method = "POST";
            request.ContentType = "application/json";
            
            request.UserAgent = "Riskified.NetSDK/" + ProductVersion;
            request.Accept = "*/*";
            request.ContentLength = bodyBytes.Length;
            request.Headers.Add(HttpDefinitions.HmacHeaderName, hashCode);
            request.Headers.Add(HttpDefinitions.ShopDomainHeaderName, _shopDomain);
            if (isSubmit)
                request.Headers.Add(HttpDefinitions.SubmitHeaderName, "true");
            // TODO set other http request fields if required

            Stream body = request.GetRequestStream();
            body.Write(bodyBytes,0,bodyBytes.Length);
            body.Close();
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (Exception e)
            {
                throw new OrderTransactionException("There was an error sending order to server. More Info: "+ e.Message,e);
            }
            // todo validate the response data
            body = response.GetResponseStream();
            if (body != null)
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(body);

                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                var transactionResult = JsonConvert.DeserializeObject<OrderTransactionResult>(responseFromServer);

                if (transactionResult.IsSuccessful)
                {
                    if(transactionResult.SuccessfulResult == null ||
                        (transactionResult.SuccessfulResult.Status != "submitted" &&
                         transactionResult.SuccessfulResult.Status != "created" &&
                         transactionResult.SuccessfulResult.Status != "updated"))
                        throw new OrderTransactionException("Error receiving valid response from riskified server - contact Riskified");
                }
                else
                {
                    //TODO handle case of unsuccessful tranaction of order
                }

                reader.Close();
                body.Close();
                response.Close();
                return transactionResult.SuccessfulResult.Id;
            }
            throw new OrderTransactionException("Received empty response from riskified server - contact Riskified");

        }

        

    }

    
}
