using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Riskified.NetSDK.Definitions;
using Riskified.NetSDK.Exceptions;
using Riskified.NetSDK.Logging;
using Riskified.NetSDK.Model;

namespace Riskified.NetSDK.Control
{
    public class NotificationHandler
    {
        private readonly HttpListener _listener;
        private readonly Action<Notification> _notificationReceivedCallback;
        private bool _isStopped;
        private readonly string _localListeningEndpoint;
        private readonly string _authToken;
        // TODO add test class
        public NotificationHandler(string localListeningEndpoint, Action<Notification> notificationReceived,string authToken, ILogger logger = null)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(localListeningEndpoint);
            _isStopped = true;
            _notificationReceivedCallback = notificationReceived;
            _localListeningEndpoint = localListeningEndpoint;
            _authToken = authToken;
            LogWrapper.InitializeLogger(logger);
        }

        /// <summary>
        /// Registers a local (client) endpoint for notification messages 
        /// Will replace all previous endpoints registered by that specific merchant
        /// </summary>
        /// <param name="riskifiedRegistrationEndpoint"></param>
        /// <param name="newLocalEndpoint"></param>
        /// <param name="shopUrl"></param>
        /// <param name="password"></param>
        /// <exception cref=""></exception>
        public void RegisterEndPointForNotifications(string riskifiedRegistrationEndpoint, string newLocalEndpoint, string shopUrl, string password)
        {
            //TODO implement this on server side as well as here
            throw new Exception("Not Implemented Yet");
        }

        /// <summary>
        /// Starts a new 
        /// </summary>
        /// <exception cref="RiskifiedException"></exception>
        public void Start()
        {
            try
            {
                _listener.Start();
            }
            catch (Exception e)
            {
                string errorMsg =
                    string.Format(
                        "Unable to start the HTTP webhook listener on: {0}. Check firewall configuration and make sure the app is running under admin privleges",_localListeningEndpoint);
                LogWrapper.GetInstance().Fatal(errorMsg, e);
                throw new RiskifiedException(errorMsg, e);
            }
            var t = new Task(ReceiveNotifications);
            _isStopped = false;
            t.Start();
        }

        public void Stop()
        {
            _isStopped = true;
            _listener.Stop();
        }

        private void ReceiveNotifications()
        {
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

                    StreamReader reader = new StreamReader(stream);
                    string notificationString = reader.ReadToEnd();
                    // verify that the notification is valid and authentic - even if not we still try to parse it
                    string calculatedHmac = HttpDefinitions.CalcHmac(notificationString, _authToken);
                    bool isValidatedNotification =
                        calculatedHmac.Equals(request.Headers.Get(HttpDefinitions.HmacHeaderName));
                    
                    // parsing the notification body to extract id and status of order
                    Regex regex = new Regex(@"^id=(?<id>\d+?)&status=(?<status>approved|declined)$",
                        RegexOptions.IgnoreCase);
                    Match m = regex.Match(notificationString);
                    try
                    {
                        int id = int.Parse(m.Groups["id"].Value);
                        OrderStatus status =
                            (OrderStatus) Enum.Parse(typeof (OrderStatus), m.Groups["status"].Value, true);
                        var n = new Notification(id, status, isValidatedNotification);
                        // running callback to call merchant code on the notification
                        _notificationReceivedCallback(n);
                        //TODO is it neccessary to write back a response to the server - if so headers should be added and modified
                        // Obtain a response object to write back a ack response to the riskified server
                        HttpListenerResponse response = context.Response;
                        // Construct a simple response. 
                        string responseString =
                            string.Format(
                                "<HTML><BODY> Client Received Notification For Order {0} With status {1} </BODY></HTML>",
                                n.OrderId, n.Status);
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                    catch (Exception e)
                    {
                        LogWrapper.GetInstance().Error("Unable to parse the notification. Was not in the correct format. Data was: "+ notificationString);
                        // TODO use the exception in the log message
                        //TODO send erronous reponse back
                    }
                    
                    
                }
                catch (Exception e)
                {
                    LogWrapper.GetInstance().Error("An error occured will receiving notification. Specific request was skipped",e);
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
            throw new RiskifiedException(errorMsg);
        }
    }
}
