using System.Net;
using Riskified.SDK.Model;
using Riskified.SDK.Model.Internal;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Notifications
{
    public static class NotificationParser
    {
        public static string HmacHeaderName => HttpUtils.HmacHeaderName;

        public static OrderNotification ParseListenerRequest(HttpListenerRequest request, string authToken)
        {
            var notificationData = HttpUtils.ParsePostRequestToObject<OrderWrapper<Notification>>(request, authToken);
            return new OrderNotification(notificationData);
        }

        public static OrderNotification ParseRequestComponents(string hmacHeader, string requestBody, string authToken)
        {
            var notificationData = HttpUtils.ParsePostRequestComponentsToObject<OrderWrapper<Notification>>(hmacHeader, requestBody, authToken);
            return new OrderNotification(notificationData);
        }
    }
}