using System;
using System.Threading.Tasks;
using Riskified.SDK.Model;
using Riskified.SDK.Notifications;

namespace Riskified.SDK.Sample
{
    public static class NotificationServerExample
    {
        private static NotificationsHandler _notificationServer;

        public static void ReceiveNotificationsExample()
        {
            // Configuration via environment variable (recommended for .NET 8)
            // Set RISKIFIED_NOTIFICATIONS_WEBHOOK_URL environment variable
            string merchantNotificationsWebhook = Environment.GetEnvironmentVariable("RISKIFIED_NOTIFICATIONS_WEBHOOK_URL") ?? "http://localhost:8080/notifications/";
            
            Console.WriteLine("Local Notifications server url (from env var RISKIFIED_NOTIFICATIONS_WEBHOOK_URL): " + merchantNotificationsWebhook);
            Console.WriteLine("'s' to start the notifications server, else to skip all");
            string key = Console.ReadLine();
            switch(key)
            {
                case "s":
                    StartServer(merchantNotificationsWebhook);
                    break;
                default:
                    Console.WriteLine("Unknown key - skipping notifications webhook");
                    break;
            }

            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            
        }

        public static void StopNotificationServer()
        {
            // make sure you shut down the notification server on system shut down
            if (_notificationServer != null)
                _notificationServer.StopReceiveNotifications();
        }
        
        private static void StartServer(string merchantNotificationsWebhook)
        {
            string domain = Environment.GetEnvironmentVariable("RISKIFIED_MERCHANT_DOMAIN") ?? "your_merchant_domain.com";
            string authToken = Environment.GetEnvironmentVariable("RISKIFIED_AUTH_TOKEN") ?? "your_auth_token";

            // setup of a notification server listening to incoming notification from riskified
            // the webhook is the url on the local server which the httpServer will be listening at
            // make sure the url is correct (internet reachable ip/address and port, firewall rules etc.)
            _notificationServer = new NotificationsHandler(merchantNotificationsWebhook, NotificationReceived, authToken, domain);
            // the call to notifier.ReceiveNotifications() is blocking and will not return until we call StopReceiveNotifications 
            // so we run it on a different task in this example
            var t = new Task(_notificationServer.ReceiveNotifications);
            t.Start();
            Console.WriteLine("Notification server up and running and listening to notifications on webhook: " + merchantNotificationsWebhook);
        }

        /// <summary>
        /// A sample notifications callback from the NotificationHandler
        /// Will be called each time a new notification is received at the local webhook
        /// </summary>
        /// <param name="notification">The notification object that was received</param>
        private static void NotificationReceived(OrderNotification notification)
        {
            Console.WriteLine("\n\nNew " + notification.Status + " Notification Received for order with ID:" + notification.Id + " With description: " + notification.Description + " With app_dom_id: " + notification.Custom.AppDomId +
                (notification.Warnings == null ? "" : ("Warnings:\n" + string.Join("\n",notification.Warnings))) + "\n\n");
        }
    }
}
