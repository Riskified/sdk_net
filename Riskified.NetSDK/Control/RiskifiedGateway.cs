using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Riskified.NetSDK.Definitions;
using Riskified.NetSDK.Logging;
using Riskified.NetSDK.Model;
using Riskified.NetSDK.Exceptions;

namespace Riskified.NetSDK.Control
{
    /// <summary>
    /// Main class to handle order creation and submittion to Riskified Servers
    /// </summary>
    public class RiskifiedGateway
    {
        private static readonly string AssemblyVersion;
        private readonly Uri _riskifiedOrdersTransferAddr;
        private readonly string _authToken;
        private readonly string _shopDomain;
        // TODO add test class
        
        static RiskifiedGateway()
        {
            // Extracting the product version for later use
            AssemblyVersion = typeof(RiskifiedGateway).Assembly.GetName().Version.ToString();
        }

        public RiskifiedGateway(Uri riskifiedOrdersTransferAddrAddress, string authToken, string shopDomain,ILogger logger=null)
        {
            _riskifiedOrdersTransferAddr = riskifiedOrdersTransferAddrAddress;
            // TODO make sure signature and domain are of valid structure
            _authToken = authToken;
            _shopDomain = shopDomain;
            LogWrapper.InitializeLogger(logger);
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends an order created/updated to Riskified Servers (without Submit for analysis)
        /// </summary>
        /// <param name="order">The Order to create or update</param>
        /// <returns>The order ID in riskified servers (for followup only - not used latter)</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="OrderTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        /// <exception cref="OrderTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        public int CreateOrUpdateOrder(Order order)
        {
            return SendOrder(order, false);
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends an order to Riskified Servers and submits it for analysis
        /// </summary>
        /// <param name="order">The Order to submit</param>
        /// <returns>The order ID in riskified servers (for followup only - not used latter)</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="OrderTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        public int SubmitOrder(Order order)
        {
            return SendOrder(order, true);
        }

        /// <summary>
        /// Validates the Order object fields
        /// Sends the order to riskified server endpoint as configured in the ctor
        /// </summary>
        /// <param name="order">The order object to send</param>
        /// <param name="isSubmit">if the order should be submitted for inspection/analysis, flag should be true </param>
        /// <returns>The order ID in riskified servers (for followup only - not used latter)</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of the order (missing fields data or invalid data)</exception>
        /// <exception cref="OrderTransactionException">On errors with the transaction itself (netwwork errors, bad response data)</exception>
        private int SendOrder(Order order,bool isSubmit)
        {
            string jsonOrder;
            try
            {
                jsonOrder = JsonConvert.SerializeObject(order);
            }
            catch (Exception e)
            {
                throw new OrderFieldBadFormatException("The order could not be serialized to JSON: "+e.Message, e);
            }
            
            HttpWebRequest request = WebRequest.CreateHttp(_riskifiedOrdersTransferAddr);
            // Set custom Riskified headers
            string hashCode = HttpDefinitions.CalcHmac(jsonOrder, _authToken);
            request.Headers.Add(HttpDefinitions.HmacHeaderName, hashCode);
            if (isSubmit)
                request.Headers.Add(HttpDefinitions.SubmitHeaderName, "true");
            request.Headers.Add(HttpDefinitions.ShopDomainHeaderName, _shopDomain);
            // TODO add support for gzip compression for non-sandbox env
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = "Riskified.NetSDK/" + AssemblyVersion;
            request.Accept = "*/*";
            
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonOrder);
            request.ContentLength = bodyBytes.Length;
            
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
                const string errorMsg = "There was an error sending order to server";
                LogWrapper.GetInstance().Error(errorMsg,e);
                throw new OrderTransactionException("There was an error sending order to server",e);
            }
            
            body = response.GetResponseStream();
            if (body != null)
            {
                // Open the stream using a StreamReader for easy access.
                var reader = new StreamReader(body);

                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                body.Close();
                response.Close();

                string calculatedHmac = HttpDefinitions.CalcHmac(responseFromServer, _authToken);
                bool isValidatedResponse =
                        calculatedHmac.Equals(response.Headers.Get(HttpDefinitions.HmacHeaderName));
                OrderTransactionResult transactionResult;
                try
                {
                    transactionResult = JsonConvert.DeserializeObject<OrderTransactionResult>(responseFromServer);
                }
                catch (Exception e)
                {
                    string errorMsg =
                        "Unable to parse response body - order state in riskified servers unknown. Verification of data integrity result was: " +
                        isValidatedResponse + ". Body was: " +
                        responseFromServer;
                    LogWrapper.GetInstance().Error(errorMsg,e);
                    throw new OrderTransactionException(errorMsg,e);
                }
                
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
                    throw new OrderTransactionException("Case of failed response not implemented yet");
                }
                
                if (transactionResult.SuccessfulResult.Id != null) return transactionResult.SuccessfulResult.Id.Value;
            }
            throw new OrderTransactionException("Received bad response from riskified server - contact Riskified");

        }

        

    }

    
}
