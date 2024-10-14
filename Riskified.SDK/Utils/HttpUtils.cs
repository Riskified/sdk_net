using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Logging;

namespace Riskified.SDK.Utils
{
    internal enum HttpBodyType
    {
        JSON,
        XML,
        Text
    }

    internal static class HttpUtils
    {
        private const string ShopDomainHeaderName = "X-RISKIFIED-SHOP-DOMAIN";
        private const string HmacHeaderName = "X-RISKIFIED-HMAC-SHA256";
        private const int ServerApiVersion = 2;

        private static readonly string AssemblyVersion;

        static HttpUtils()
        {
            // Extracting the product version for later use
            AssemblyVersion = typeof (HttpUtils).Assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// Sends an HTTP Post request with the json-serialized data of jsonObj as body to the received url (Riskified server),
        /// blocks and returns after successful response from server (200-OK)
        /// Throws exceptions on network errors or serialization problems
        /// </summary>        
        /// <typeparam name="TReqObj">The type of class to be received as parameter and serialized to JSON as request body</typeparam>
        /// <param name="riskifiedWebhookUrl">The full url with internal path of the relevant riskified webhook</param>
        /// <param name="jsonObj">The object to be json serialized as body of the HTTP request</param>
        /// <param name="authToken">The merchant authentication Token</param>
        /// <param name="shopDomain">The shop domain url of the merchant at Riskified</param>
        /// <exception cref="OrderFieldBadFormatException">On bad format of an order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public static void JsonPostAndParseResponseToObject<TReqObj>(Uri riskifiedWebhookUrl, TReqObj jsonObj, string authToken, string shopDomain)
        {
            var response = PostObject<TReqObj>(riskifiedWebhookUrl, jsonObj, authToken, shopDomain);

            if(response != null)
            {
                response.Close();
            }
        }

        /// <summary>
        /// Sends an HTTP Post request with the json-serialized data of jsonObj as body to the received url (Riskified server),
        /// blocks and returns after successful response from server (200-OK)
        /// Throws exceptions on network errors or serialization problems
        /// Returns the expected object representation (of type TRespObj) deserialized from the response json body
        /// Throws exceptions on object serialization or network errors
        /// </summary>
        /// <typeparam name="TRespObj">The type of class expected to be received in the response body as JSON</typeparam>
        /// <typeparam name="TReqObj">The type of class to be received as parameter and serialized to JSON as request body</typeparam>
        /// <param name="riskifiedWebhookUrl">The full url with internal path of the relevant riskified webhook</param>
        /// <param name="jsonObj">The object to be json serialized as body of the HTTP request</param>
        /// <param name="authToken">The merchant authentication Token</param>
        /// <param name="shopDomain">The shop domain url of the merchant at Riskified</param>
        /// <returns>'TRespObj' typed response object</returns>
        /// <exception cref="OrderFieldBadFormatException">On bad format of an order (missing fields data or invalid data)</exception>
        /// <exception cref="RiskifiedTransactionException">On errors with the transaction itself (network errors, bad response data)</exception>
        public static TRespObj JsonPostAndParseResponseToObject<TRespObj,TReqObj>(Uri riskifiedWebhookUrl, TReqObj jsonObj, string authToken, string shopDomain) 
            where TRespObj : class
            where TReqObj  : class 
        {
            var response = PostObject<TReqObj>(riskifiedWebhookUrl, jsonObj, authToken, shopDomain);
            var resObj = ParseObjectFromJsonResponse<TRespObj>(response);
            return resObj;
        }



        private static HttpWebResponse PostObject<TReqObj>(Uri riskifiedWebhookUrl, TReqObj jsonObj, string authToken, string shopDomain)
        {
            string jsonStr;
            try
            {
                //jsonStr = JsonConvert.SerializeObject(jsonObj,new JsonSerializerSettings{NullValueHandling = NullValueHandling.Ignore});
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> { new StringEnumConverter() }  // Adding the StringEnumConverter
                };

                jsonStr = JsonConvert.SerializeObject(jsonObj, settings);
            }
            catch (Exception e)
            {
                throw new OrderFieldBadFormatException("The order could not be serialized to JSON: " + e.Message, e);
            }

            HttpWebResponse response;
            try
            {
                WebRequest request = GeneratePostRequest(riskifiedWebhookUrl, jsonStr, authToken, shopDomain, HttpBodyType.JSON);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                string error = "There was an unknown error sending data to server";
                if (wex.Response != null)
                {
                    HttpWebResponse errorResponse = (HttpWebResponse)wex.Response;
                    HttpStatusCode statusCode = errorResponse.StatusCode;
              
                    
                    try
                    {
                        var errorObj = ParseObjectFromJsonResponse<ErrorResponse>(errorResponse);
                        error = errorObj.Error.Message + " (Http Status code: " + statusCode + ")";
                       
                    }
                    catch (Exception parseEx)
                    {
                        if (statusCode == HttpStatusCode.InternalServerError)
                            error = "Server side error (500): ";
                        else if (statusCode == HttpStatusCode.BadRequest)
                            error = "Client side error (400): ";
                        else
                            error = "Error occurred. Http status code " + statusCode + ":";
                        error += parseEx.Message;
                    }


                }
                LoggingServices.Error(error, wex);
                throw new RiskifiedTransactionException(error, wex);
            }
            catch (Exception e)
            {
                const string errorMsg = "There was an unknown error connecting to Riskified server";
                LoggingServices.Error(errorMsg, e);
                throw new RiskifiedTransactionException(errorMsg, e);
            }
            return response;
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

        private static WebRequest GeneratePostRequest(Uri url, string body, string authToken,string shopDomain, HttpBodyType bodyType)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            // Set custom Riskified headers
            AddDefaultHeaders(request.Headers,authToken,shopDomain,body);
            
            request.Method = "POST";
            request.ContentType = "application/"+ Enum.GetName(typeof(HttpBodyType),bodyType).ToLower();
            request.UserAgent = "Riskified.SDK_NET/" + AssemblyVersion;
            request.Accept = string.Format("application/vnd.riskified.com; version={0}", ServerApiVersion);
            request.Accept = string.Format("application/json");
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
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
            headers.Add("Accept-Encoding", "gzip,deflate,sdch");
        }

        private static T ParseObjectFromJsonResponse<T>(WebResponse response) where T : class
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
            return JsonStringToObject<T>(responseBody);
        }

        private static T JsonStringToObject<T>(string responseBody) where T : class
        {
            T transactionResult;
            try
            {
                transactionResult = JsonConvert.DeserializeObject<T>(responseBody);
                ErrorResponse response = JsonConvert.DeserializeObject<ErrorResponse>(responseBody);
                
                if (transactionResult.GetType().Name == "ErrorResponse") {
                    foreach (var propInfo in transactionResult.GetType().GetProperties())
                    {
                        propInfo.SetValue(transactionResult, response.Error);
                    }
                }
            }
            catch (Exception e)
            {
                string errorMsg =
                    "Unable to parse JSON response body to type: " + typeof(T).Name + ". Body was: " + responseBody;
                throw new RiskifiedTransactionException(errorMsg, e); // should be replaced with a better named exception
            }
            return transactionResult;
        }

        internal class ErrorResponse
        {
            [JsonProperty(PropertyName = "error")]
            public ErrorMessage Error { get; set; }
        }

        internal class ErrorMessage
        {
            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }
        }

        /// <summary>
        /// Authenticates the http request using the received auth token and tries to parse it into an object of of type T
        /// In case of failed authentication or parsing an exception will be thrown
        /// </summary>
        /// <typeparam name="T">The expected object type to be parsed from the request content</typeparam>
        /// <param name="request">The request</param>
        /// <param name="authToken">The merchant authentication token</param>
        /// <returns>An object of type T containing data parsed from the request</returns>
        /// <exception cref="RiskifiedAuthenticationException">On missing/bad HMAC signature that doesn't match the given auth token</exception>
        /// <exception cref="RiskifiedTransactionException">On parsing error from string into the relevant object of type T</exception>
        public static T ParsePostRequestToObject<T>(HttpListenerRequest request,string authToken) where T : class
        {
            if (!request.HasEntityBody)
            {
                return null;
            }
            string postData = AuthorizeAndExtractContent(request, authToken);
            T obj = JsonStringToObject<T>(postData);
            return obj;
        }

        private static string AuthorizeAndExtractContent(HttpListenerRequest request, string authToken)
        {
            if (!string.IsNullOrEmpty(request.Headers[HmacHeaderName]))
            {
                Stream s = request.InputStream;
                string postData = ExtractStreamData(s);
                if (string.Equals(request.Headers[HmacHeaderName], CalcHmac(postData, authToken), StringComparison.Ordinal))
                {
                    return postData;
                }
            }
            throw new RiskifiedAuthenticationException("Request HMAC signature was either missing or incorrect");
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

                return streamData;
            }
            const string errMsg = "Unknown data from Riskified server - ignoring it. Body was null";
            LoggingServices.Error(errMsg);
            throw new RiskifiedTransactionException(errMsg);
        }
        /*
        private static bool IsStringVerified(string data, string authToken, string hmacValueToVerify)
        {
            string calculatedHmac = CalcHmac(data, authToken);
            return calculatedHmac.Equals(hmacValueToVerify);
        }
        */
        public static void BuildAndSendResponse(HttpListenerResponse response, string authToken,string shopDomain, string body,bool isActionSucceeded)
        {
            AddDefaultHeaders(response.Headers,authToken,shopDomain,body);
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            if (isActionSucceeded)
                response.StatusCode = (int) HttpStatusCode.OK;
            else
                response.StatusCode = (int) HttpStatusCode.BadRequest;

            byte[] buffer = Encoding.UTF8.GetBytes(body);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        public static Uri BuildUrl(RiskifiedEnvironment env, string relativePath, FlowStrategy flow = FlowStrategy.Default)
        {
            var hostUrl = EnvironmentsUrls.GetEnvUrl(env, flow);
            Uri fullUrl = new Uri(new Uri(hostUrl),relativePath);
            return fullUrl;
        }
    }
}
