using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riskified.NetSDK.Model;

namespace Riskified.NetSDK.Control
{
    public class NotificationHandler
    {
        private HttpListener _listener;
        private NotificationHandlingMethodDelegate _notificationReceivedCallback;
        private bool _isStopped;

        public delegate void NotificationHandlingMethodDelegate(Notification notification);

        public NotificationHandler(string endpoint, NotificationHandlingMethodDelegate notificationReceived)
        {
            _listener = new HttpListener();
            //String prefix = @"http://server1:1234";
            _listener.Prefixes.Add(endpoint);
            _isStopped = true;
            _notificationReceivedCallback = notificationReceived;

        }

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
                    var n = JsonConvert.DeserializeObject<Notification>(notificationString);
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
