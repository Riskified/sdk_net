using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Riskified.NetSDK.Exceptions;
using Riskified.NetSDK.Logging;
using Riskified.NetSDK.Model;
using Riskified.NetSDK.Utils;

namespace Riskified.NetSDK.Control
{
    public class NotificationHandler
    {
        private readonly HttpListener _listener;
        private readonly Action<Notification> _notificationReceivedCallback;
        private bool _isStopped;
        private readonly string _localListeningEndpoint;
        private readonly string _authToken,_shopDomain;
        // TODO add test class
        public NotificationHandler(string localListeningEndpoint, Action<Notification> notificationReceived,string authToken, string shopDomain, ILogger logger = null)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(localListeningEndpoint);
            _isStopped = true;
            _notificationReceivedCallback = notificationReceived;
            _localListeningEndpoint = localListeningEndpoint;
            _authToken = authToken;
            _shopDomain = shopDomain;
            LogWrapper.InitializeLogger(logger);
        }

        /// <summary>
        /// Registers a local (client) endpoint for notification messages 
        /// Will replace all previous endpoints registered by that specific merchant
        /// </summary>
        /// <param name="riskifiedRegistrationEndpoint"></param>
        /// <exception cref="RiskifiedException"></exception>
        public void RegisterEndPointForNotifications(string riskifiedRegistrationEndpoint)
        {
            string tmpJsonStr = "{\"action_type\" : \"create\" , \"webhook_url\" : \"" + _localListeningEndpoint + "\"}";

            HttpWebRequest request = WebRequest.CreateHttp(riskifiedRegistrationEndpoint);
            // Set custom Riskified headers
            string hashCode = HttpDefinitions.CalcHmac(tmpJsonStr,_authToken);
            request.Headers.Add(HttpDefinitions.HmacHeaderName, hashCode);
            request.Headers.Add(HttpDefinitions.ShopDomainHeaderName, _shopDomain);
            // TODO add support for gzip compression for non-sandbox env
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = "Riskified.NetSDK/";
            request.Accept = "*/*";

            byte[] bodyBytes = Encoding.UTF8.GetBytes(tmpJsonStr);
            request.ContentLength = bodyBytes.Length;

            Stream body = request.GetRequestStream();
            body.Write(bodyBytes, 0, bodyBytes.Length);
            body.Close();
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (Exception e)
            {
                const string errorMsg = "There was an error sending order to server";
                LogWrapper.GetInstance().Error(errorMsg, e);
                throw new OrderTransactionException("There was an error sending order to server", e);
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
                string resultStr;
                try
                {
                    resultStr = responseFromServer;
                    //transactionResult = JsonConvert.DeserializeObject<OrderTransactionResult>(responseFromServer);
                }
                catch (Exception e)
                {
                    string errorMsg =
                        "Unable to parse response body - Notifications webhook in riskified servers not changed. Verification of data integrity result was: " +
                        isValidatedResponse + ". Body was: " +
                        responseFromServer;
                    LogWrapper.GetInstance().Error(errorMsg, e);
                    throw new RiskifiedException(errorMsg, e);
                }
                /*
                if (transactionResult.IsSuccessful)
                {
                    if (transactionResult.SuccessfulResult == null ||
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
                 */
            }
            throw new OrderTransactionException("Received bad response from riskified server - contact Riskified");
        }

        public void StopReceiveNotifications()
        {
            _isStopped = true;
            _listener.Stop();
        }

        /// <summary>
        /// Runs the notification server
        /// This method is blocking and will not return until StopReceiveNotifications is called
        /// </summary>
        /// <exception cref="NotifierServerFailedToStartException">Thrown when the listener was unable to start due to server network configuration errors</exception>
        /// <exception cref="NotifierAlreadyRunningException">Thrown when server is already running</exception>
        public void ReceiveNotifications()
        {
            if (!_isStopped)
                throw new NotifierAlreadyRunningException("Notification handler already running");

            if (!_listener.IsListening)
            {
                try
                {
                    _listener.Start();
                }
                catch (Exception e)
                {
                    string errorMsg =
                        string.Format(
                            "Unable to start the HTTP webhook listener on: {0}. Check firewall configuration and make sure the app is running under admin privleges",
                            _localListeningEndpoint);
                    LogWrapper.GetInstance().Fatal(errorMsg, e);
                    throw new NotifierServerFailedToStartException(errorMsg, e);
                }
            }
            
            _isStopped = false;

            while (!_isStopped)
            {
                try
                {
                    //blocking call
                    var context = _listener.GetContext();
                    // reaches here when a connection was made
                    var request = context.Request;


                    if (!request.HasEntityBody)
                    {
                        LogWrapper.GetInstance().Error("Received HTTP notification with no body - shouldn't happen");
                        continue;
                    }

                    var stream = request.InputStream;

                    var reader = new StreamReader(stream);
                    string notificationString = reader.ReadToEnd();
                    // verify that the notification is valid and authentic - even if not we still try to parse it
                    string calculatedHmac = HttpDefinitions.CalcHmac(notificationString, _authToken);
                    bool isValidatedNotification =
                        calculatedHmac.Equals(request.Headers.Get(HttpDefinitions.HmacHeaderName));

                    // parsing the notification body to extract id and status of order
                    var regex = new Regex(@"^id=(?<id>\d+?)&status=(?<status>approved|declined)$",
                        RegexOptions.IgnoreCase);
                    Match m = regex.Match(notificationString);
                    string responseString;
                    try
                    {
                        int id = int.Parse(m.Groups["id"].Value);
                        var status =
                            (OrderStatus) Enum.Parse(typeof (OrderStatus), m.Groups["status"].Value, true);
                        var n = new Notification(id, status, isValidatedNotification);
                        // running callback to call merchant code on the notification
                        _notificationReceivedCallback(n);
                        responseString =
                            string.Format(
                                "<HTML><BODY> Merchant Received Notification For Order {0} With status {1} </BODY></HTML>",
                                n.OrderId, n.Status);
                    }
                    catch (Exception e)
                    {
                        LogWrapper.GetInstance()
                            .Error(
                                "Unable to parse the notification. Was not in the correct format. Data was: " +
                                notificationString, e);
                        responseString = "<HTML><BODY> Merchant couldn't parse notification message</BODY></HTML>";
                    }

                    // Obtain a response object to write back a ack response to the riskified server
                    HttpListenerResponse response = context.Response;
                    // Construct a simple response. 
                    string hashCode = HttpDefinitions.CalcHmac(responseString, _authToken);
                    response.Headers.Add(HttpDefinitions.HmacHeaderName, hashCode);
                    response.Headers.Add(HttpDefinitions.ShopDomainHeaderName, _shopDomain);
                    // TODO add support for gzip compression for non-sandbox env
                    response.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                    response.ContentType = "HTML";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                catch (Exception e)
                {
                    LogWrapper.GetInstance()
                        .Error("An error occured will receiving notification. Specific request was skipped", e);
                    // trying to restart listening - maybe connection was cut shortly
                    if (!_listener.IsListening)
                    {
                        RestartHttpListener();
                    }
                }
            }
        }

        private void RestartHttpListener()
        {
            int retriesMade = 0;

            LogWrapper.GetInstance().Info("HttpListener is crushed. Waiting 30 seconds before restarting");
            while (retriesMade < 3)
            {
                Thread.Sleep(30000);
                retriesMade++;
                LogWrapper.GetInstance().Info("Trying to restart HttpListener for the " + retriesMade + "time");
                try
                {
                    _listener.Start();
                }
                catch (Exception e)
                {
                    LogWrapper.GetInstance().Error("Restart # "+ retriesMade +" failed",e);
                }
            
            }
            string errorMsg = "Failed to restart HttpListener after " + retriesMade +
                              " attempts. Notifications will not be received. Please check the connection and configuration of the server";
            LogWrapper.GetInstance().Fatal(errorMsg);
            throw new NotifierServerFailedToStartException(errorMsg);
        }
    }
}
