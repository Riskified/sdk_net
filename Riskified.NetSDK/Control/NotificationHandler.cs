using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riskified.NetSDK.Model;

namespace Riskified.NetSDK.Control
{
    public class NotificationHandler
    {
        private readonly HttpListener _listener;
        private readonly Action<Notification> _notificationReceivedCallback;
        private bool _isStopped;
        // TODO add test class
        public NotificationHandler(string localListeningEndpoint, Action<Notification> notificationReceived)
        {
            _listener = new HttpListener();
            //String prefix = @"http://server1:1234";
            _listener.Prefixes.Add(localListeningEndpoint);
            _isStopped = true;
            _notificationReceivedCallback = notificationReceived;

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
        public void Start()
        {
            // TODO catch exceptions and handle
            _listener.Start();
            _isStopped = false;
            var t = new Task(ReceiveNotifications);
            t.Start();
        }

        public void Stop()
        {
            _listener.Stop();
            _isStopped = true;
        }

        private void ReceiveNotifications()
        {
            while (!_isStopped)
            {
                if (!_listener.IsListening)
                {
                    //TODO try to reactivate listen for 3 times before breaking with error?
                }
                HttpListenerContext context = _listener.GetContext();
                HttpListenerRequest request = context.Request;

                if (request.HasEntityBody)
                {
                    var stream = request.InputStream;
                    StreamReader reader = new StreamReader(stream);
                    string notificationString = reader.ReadToEnd();
                    //TODO read the notification data from the parameters of the http request
                    var n = new Notification();
                    _notificationReceivedCallback(n);
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response. 
                    string responseString = string.Format("<HTML><BODY> Client Received Notification For Order {0} With status {1} </BODY></HTML>",n.OrderId,n.Status);
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                }
                
            }
        }
    }
}
