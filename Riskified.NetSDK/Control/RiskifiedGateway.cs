using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Riskified.NetSDK.Model;

namespace Riskified.NetSDK.Control
{
    // TODO documantation
    public class RiskifiedGateway
    {
        private readonly Uri _riskifiedServer;
        private readonly string _signature;
        private readonly string _shopDomain;

        public RiskifiedGateway(Uri riskifiedServerAddress, string authToken, string shopDomain)
        {
            _riskifiedServer = riskifiedServerAddress;
            // TODO make sure signature and domain are of valid structure
            _signature = authToken;
            _shopDomain = shopDomain;
        }

        public int CreateOrUpdateOrder(Order order)
        {
            return SendOrder(order, false);
        }

        public int SubmitOrder(Order order)
        {
            return SendOrder(order, true);
        }

        private int SendOrder(Order order,bool isSubmit)
        {
            string jsonOrder = JsonConvert.SerializeObject(order);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonOrder);

            HttpWebRequest request = HttpWebRequest.CreateHttp(_riskifiedServer);
            // Set custom Riskified headers
            string hashCode = calcHmac(jsonOrder,_signature);
            request.Headers.Add("X_RISKIFIED_HMAC_SHA256", hashCode);
            request.Headers.Add("X_RISKIFIED_SHOP_DOMAIN", _shopDomain);
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            if(isSubmit)
                request.Headers.Add("HTTP_X_RISKIFIED_SUBMIT_NOW", "");
            request.Method = "POST";
            request.ContentType = "application/json";
            // TODO change version control and numbering 
            request.UserAgent = "Riskified.NetSDK - Version 0.9";
            request.Accept = "*/*";
            request.ContentLength = bodyBytes.Length;
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
                throw e;
            }
            // TODO handle exceptions and errors with receiving response (try-catch and response properties)

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            body = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(body);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Display the content.
            Console.WriteLine(responseFromServer);

            dynamic arr = JsonConvert.DeserializeObject(responseFromServer);
            
            int id = int.Parse(arr[0].obj.order.id);
            
            reader.Close();
            body.Close();
            response.Close();

            return id;
        }

        static string calcHmac(string data,string authToken)
        {
            byte[] key = Encoding.ASCII.GetBytes(authToken);
            HMACSHA256 myhmacsha256 = new HMACSHA256(key);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            MemoryStream stream = new MemoryStream(byteArray);
            string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            Console.WriteLine(result);
            return result;
        }

    }
}
