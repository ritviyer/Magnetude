using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Notifications.Android;
public class NotificationManager : MonoBehaviour
{
    private void Start()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        CreateNotificationChannel();
    }
    public void CreateNotificationChannel() 
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Reward Notifications",
            Importance = Importance.High,
            Description = "Reward Notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        for (int i = 1; i < 9; i++)
        {
            DateTime notTime = DateTime.Today.AddDays(i).AddHours(16.5f);
            SendNotification(notTime);
        }
    }

    public void SendNotification(DateTime notificationTime)
    {
        var notification = new AndroidNotification();
        notification.Title = "Don't Miss Out!";
        notification.Text = "Your daily reward and challenges await.";
        notification.SmallIcon = "small_icon";
        notification.LargeIcon = "large_icon";
        notification.FireTime = notificationTime;

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}
