using System;
using System.Configuration;
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
            string merchantNotificationsWebhook = ConfigurationManager.AppSettings["NotificationsWebhookUrl"];
            
            Console.WriteLine("Local Notifications server url set in the config file: " + merchantNotificationsWebhook);
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
            string domain = ConfigurationManager.AppSettings["MerchantDomain"];
            string authToken = ConfigurationManager.AppSettings["MerchantAuthenticationToken"];

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
