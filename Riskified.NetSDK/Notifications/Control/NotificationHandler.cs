using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Riskified.NetSDK.Exceptions;
using Riskified.NetSDK.Logging;
using Riskified.NetSDK.Utils;

namespace Riskified.NetSDK.Notifications
{
    public class NotificationsHandler
    {
        private readonly HttpListener _listener;
        private readonly Action<Notification> _notificationReceivedCallback;
        private bool _isStopped;
        private readonly string _localListeningEndpoint;
        private readonly string _authToken,_shopDomain;
        // TODO add test class
        public NotificationsHandler(string localListeningEndpoint, Action<Notification> notificationReceived,string authToken, string shopDomain)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(localListeningEndpoint);
            _isStopped = true;
            _notificationReceivedCallback = notificationReceived;
            _localListeningEndpoint = localListeningEndpoint;
            _authToken = authToken;
            _shopDomain = shopDomain;
        }

        /// <summary>
        /// Registers (maps) the merchant's webhook for notification messages  
        /// Will replace all previous endpoints registered by that specific merchant
        /// This method doesn't test or verify that the webhook exists and responsive
        /// </summary>
        /// <param name="riskifiedHostUrl">Riskified registration webhook as received from Riskified</param>
        /// <param name="merchantNotificationsWebhook">The merchant webhook that will receive notifications on orders status</param>
        /// <param name="authToken">The agreed authentication token between the merchant and Riskified</param>
        /// <param name="shopDomain">The shop domain url as registered to Riskified with</param>
        /// <exception cref="RiskifiedTransactionException">thrown if an error occured in the connection to the server (timeout/error response/500 status code)</exception>
        /// <returns>Registration result - can be successful or failed</returns>
        public static NotificationRegistrationResult RegisterMerchantNotificationsWebhook(string riskifiedHostUrl,
            string merchantNotificationsWebhook, string authToken, string shopDomain)
        {
            string createJson = "{\"action_type\" : \"create\" , \"webhook_url\" : \"" + merchantNotificationsWebhook + "\"}";
            var regRes =  SendMerchantRegistrationRequest(createJson,riskifiedHostUrl,authToken,shopDomain);
            if (regRes.IsSuccessful)
            {
                LoggingServices.Info("Registration Successful: " + regRes.SuccessfulResult.Message);
            }
            else
            {
                LoggingServices.Error("Registration Unsuccessful: " + regRes.FailedResult.Message);
            }
            return regRes;
        }

        /// <summary>
        /// Un-Registers (erases mapping) of any webhooks existed for notification messages for the merchant at Riskified
        /// </summary>
        /// <param name="riskifiedRegistrationEndpoint">Riskified registration webhook as received from Riskified</param>
        /// <param name="authToken">The agreed authentication token between the merchant and Riskified</param>
        /// <param name="shopDomain">The shop domain url as registered to Riskified with</param>
        /// <exception cref="RiskifiedTransactionException">thrown if an error occured in the connection to the server (timeout/error response/500 status code)</exception>
        /// <returns>Registration result - can be successful or failed</returns>
        public static NotificationRegistrationResult UnRegisterMerchantNotificationWebhooks(string riskifiedRegistrationEndpoint, string authToken,
            string shopDomain)
        {
            string deleteJson = "{\"action_type\" : \"delete\"}";

            var unregisterRes =  SendMerchantRegistrationRequest(deleteJson,riskifiedRegistrationEndpoint, authToken, shopDomain);
            if (unregisterRes.IsSuccessful)
            {
                LoggingServices.Info("Unregistration Successful: " + unregisterRes.SuccessfulResult.Message);
            }
            else
            {
                LoggingServices.Error("Unregistration Unsuccessful: " + unregisterRes.FailedResult.Message);
            }
            return unregisterRes;
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
                    LoggingServices.Fatal(errorMsg, e);
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
                        LoggingServices.Error("Received HTTP notification with no body - shouldn't happen");
                        continue;
                    }

                    string notificationBody = HttpUtils.ExtractAndVerifyRequestBody(request);

                    // parsing the notification body to extract id and status of order
                    var regex = new Regex(@"^id=(?<id>\d+?)&status=(?<status>approved|declined)$",
                        RegexOptions.IgnoreCase);
                    Match m = regex.Match(notificationBody);
                    string responseString;
                    bool isActionSucceeded = true;
                    try
                    {
                        int id = int.Parse(m.Groups["id"].Value);
                        var status =
                            (OrderStatus) Enum.Parse(typeof (OrderStatus), m.Groups["status"].Value, true);
                        var n = new Notification(id, status);
                        // running callback to call merchant code on the notification
                        _notificationReceivedCallback(n);
                        responseString =
                            string.Format(
                                "<HTML><BODY>Merchant Received Notification For Order {0} With status {1} </BODY></HTML>",
                                n.OrderId, n.Status);
                    }
                    catch (Exception e)
                    {
                        LoggingServices.Error(
                                "Unable to parse the notification. Was not in the correct format. Data was: " +
                                notificationBody, e);
                        responseString = "<HTML><BODY>Merchant couldn't parse notification message</BODY></HTML>";
                        isActionSucceeded = false;
                    }

                    // Obtain a response object to write back a ack response to the riskified server
                    HttpListenerResponse response = context.Response;
                    // Construct a simple response. 
                    HttpUtils.BuildAndSendResponse(response, _authToken,_shopDomain, responseString,isActionSucceeded);
                }
                catch (Exception e)
                {
                    LoggingServices.Error("An error occured will receiving notification. Specific request was skipped", e);
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

            LoggingServices.Info("HttpListener is crushed. Waiting 30 seconds before restarting");
            while (retriesMade < 3)
            {
                Thread.Sleep(30000);
                retriesMade++;
                LoggingServices.Info("Trying to restart HttpListener for the " + retriesMade + "time");
                try
                {
                    _listener.Start();
                }
                catch (Exception e)
                {
                    LoggingServices.Error("Restart # "+ retriesMade +" failed",e);
                }
            
            }
            string errorMsg = "Failed to restart HttpListener after " + retriesMade +
                              " attempts. Notifications will not be received. Please check the connection and configuration of the server";
            LoggingServices.Fatal(errorMsg);
            throw new NotifierServerFailedToStartException(errorMsg);
        }

        private static NotificationRegistrationResult SendMerchantRegistrationRequest(string jsonBody, string riskifiedHostUrl, string authToken, string shopDomain)
        {
            Uri riskifiedRegistrationWebhookUrl = HttpUtils.BuildUrl(riskifiedHostUrl, "/webhooks/merchant_register_notification_webhook");
            NotificationRegistrationResult registerResult = HttpUtils.PostAndParseResponseToObject<NotificationRegistrationResult>(riskifiedRegistrationWebhookUrl,jsonBody, authToken, shopDomain);
            return registerResult;
        }

    }


}
