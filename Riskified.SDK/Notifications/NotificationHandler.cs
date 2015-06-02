using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Logging;
using Riskified.SDK.Model;
using Riskified.SDK.Utils;
using Riskified.SDK.Model.Internal;

namespace Riskified.SDK.Notifications
{
    public class NotificationsHandler
    {
        private readonly HttpListener _listener;
        private readonly Action<OrderNotification> _notificationReceivedCallback;
        private bool _isStopped;
        private readonly string _localListeningEndpoint;
        private readonly string _authToken,_shopDomain;
        
        /// <summary>
        /// Creates a notification handler on the specified endpoint
        /// </summary>
        /// <param name="localListeningEndpoint">The endpoint for the notification server listener</param>
        /// <param name="notificationReceived">Callback to be called on each notification arrival</param>
        /// <param name="authToken">The authentication token key of the merchant</param>
        /// <param name="shopDomain">The shop domain string as used for registration to Riskified</param>
        public NotificationsHandler(string localListeningEndpoint, Action<OrderNotification> notificationReceived, string authToken, string shopDomain)
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
        /// Stops the notifications server listener
        /// </summary>
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

                    string responseString;
                    bool acionSucceeded = false;
                    try
                    {
                        var notificationData = HttpUtils.ParsePostRequestToObject<OrderWrapper<Notification>>(request,_authToken);
                        OrderNotification n = new OrderNotification(notificationData);
                        responseString =
                                string.Format(
                                    "<HTML><BODY>Merchant Received Notification For Order {0} with status {1} and description {2}</BODY></HTML>",
                                    n.Id, n.Status, n.Description);
                        // running callback to call merchant code on the notification
                        Task.Factory.StartNew(() => _notificationReceivedCallback(n));
                        acionSucceeded = true;
                    }
                    catch (RiskifiedAuthenticationException uae)
                    {
                        LoggingServices.Error("Notification message authentication failed",uae);
                        responseString = "<HTML><BODY>Merchant couldn't authenticate notification message</BODY></HTML>";
                    }
                    catch(Exception e)
                    {
                        LoggingServices.Error("Unable to parse notification message. Some or all of the post params are missing or invalid",e);
                        responseString = "<HTML><BODY>Merchant couldn't parse notification message</BODY></HTML>";
                    }
                    
                    // Obtain a response object to write back a ack response to the riskified server
                    HttpListenerResponse response = context.Response;
                    // Construct a simple response. 
                    HttpUtils.BuildAndSendResponse(response, _authToken,_shopDomain, responseString,acionSucceeded);
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
    }


}
