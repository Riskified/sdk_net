﻿using Riskified.SDK.Model.Internal;
using System;

namespace Riskified.SDK.Model
{
    public class OrderNotification
    {
        internal OrderNotification(OrderWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            Description = notificationInfo.Order.Description;
            UpdatedAt = notificationInfo.Order.UpdatedAt;
            Warnings = notificationInfo.Warnings;
        }

        internal OrderNotification(OrderCheckoutWrapper<Notification> notificationInfo)
        {
            Id = notificationInfo.Order.Id;
            Status = notificationInfo.Order.Status;
            Description = notificationInfo.Order.Description;
            UpdatedAt = notificationInfo.Order.UpdatedAt;
            Warnings = notificationInfo.Warnings;
        }

        public string Id { get; private set; }
        public string Status { get; private set; }
        public string Description { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string[] Warnings { get; private set; }
    }
}
