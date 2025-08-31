using System;

namespace LocalNotifications
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, int id, DateTime? notifyTime = null);
        void ReceiveNotification(string title, string message);
        void CancelNotification(int id);
    }
}
