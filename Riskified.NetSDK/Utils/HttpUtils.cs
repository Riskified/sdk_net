using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Riskified.NetSDK.Control;
using Riskified.NetSDK.Exceptions;
using Riskified.NetSDK.Logging;

namespace Riskified.NetSDK.Utils
{
    internal enum HttpBodyType
    {
        JSON,
        XML,
        Text
    }

    internal static class HttpUtils
    {
        private const string ShopDomainHeaderName = "X_RISKIFIED_SHOP_DOMAIN";
        private const string SubmitHeaderName = "X_RISKIFIED_SUBMIT_NOW";
        private const string HmacHeaderName = "X-RISKIFIED-HMAC-SHA256";

        private static readonly string AssemblyVersion;

        static HttpUtils()
        {
            // Extracting the product version for later use
            AssemblyVersion = typeof (RiskifiedGateway).Assembly.GetName().Version.ToString();
        }

        private static string CalcHmac(string data, string authToken)
        {
            byte[] key = Encoding.ASCII.GetBytes(authToken);
            var myhmacsha256 = new HMACSHA256(key);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            var stream = new MemoryStream(byteArray);
            string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            return result;
        }

        public static WebRequest GeneratePostRequest(Uri url, string body, string authToken,string shopDomain, HttpBodyType bodyType,bool shouldIncludeSubmitHeader = false)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            // Set custom Riskified headers
            AddDefaultHeaders(request.Headers,authToken,shopDomain,body);
            if (shouldIncludeSubmitHeader)
            {
                request.Headers.Add(SubmitHeaderName,"true");
            }
            
            request.Method = "POST";
            request.ContentType = "application/"+ Enum.GetName(typeof(HttpBodyType),bodyType).ToLower();
            request.UserAgent = "Riskified.NetSDK/" + AssemblyVersion;
            request.Accept = "*/*";

            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            request.ContentLength = bodyBytes.Length;

            Stream bodyStream = request.GetRequestStream();
            bodyStream.Write(bodyBytes, 0, bodyBytes.Length);
            bodyStream.Close();

            return request;
        }

        private static void AddDefaultHeaders(WebHeaderCollection headers, string authToken, string shopDomain, string body)
        {
            string hashCode = CalcHmac(body, authToken);
            headers.Add(HmacHeaderName, hashCode);
            headers.Add(ShopDomainHeaderName, shopDomain);
            // TODO add support for gzip compression for non-sandbox env
            headers.Add("Accept-Encoding", "gzip,deflate,sdch");
        }

        public static T ParseObjectFromJsonResponse<T>(WebResponse response) where T : class
        {
            var bodyStream = response.GetResponseStream();
            string responseBody;
            try
            {
                responseBody = ExtractStreamData(bodyStream);
            }
            finally
            {
                response.Close();
            }
            
            T transactionResult;
            try
            {
                transactionResult = JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch (Exception e)
            {
                string errorMsg =
                    "Unable to parse JSON response body to type: " + typeof(T).Name + ". Body was: " + responseBody;
                LoggingServices.Error(errorMsg, e);
                throw new RiskifiedTransactionException(errorMsg, e);
            }
            return transactionResult;
        }

        public static string ExtractAndVerifyRequestBody(HttpListenerRequest request)
        {
            Stream s = request.InputStream;
            return ExtractStreamData(s);
        }

        private static string ExtractStreamData(Stream stream)
        {
            if (stream != null)
            {
                // Open the stream using a StreamReader for easy access.
                var reader = new StreamReader(stream);
                // Read the content.
                string streamData = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                /* no need to verify responses
                if (!IsStringVerified(streamData, authToken, hmacValueToVerify))
                {
                    string err = "Data from Riskified server NOT VERIFIED - ignoring it. Body was: " + streamData;
                    LoggingServices.Error(err);
                    throw new RiskifiedTransactionException(err);
                }
                */
                return streamData;
            }
            string errMsg = "Unknown data from Riskified server - ignoring it. Body was null";
            LoggingServices.Error(errMsg);
            throw new RiskifiedTransactionException(errMsg);
        }

        private static bool IsStringVerified(string data, string authToken, string hmacValueToVerify)
        {
            string calculatedHmac = CalcHmac(data, authToken);
            return calculatedHmac.Equals(hmacValueToVerify);
        }

        public static void BuildAndSendResponse(HttpListenerResponse response, string authToken,string shopDomain, string body)
        {
            AddDefaultHeaders(response.Headers,authToken,shopDomain,body);
            response.ContentType = "HTML";
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        public static Uri BuildUrl(string hostUrl, string relativePath)
        {
            Uri fullUrl = new Uri(new Uri(hostUrl),relativePath);
            return fullUrl;
        }
    }
}
