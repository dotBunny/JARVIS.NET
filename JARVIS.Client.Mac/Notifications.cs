using System;
using Foundation;

namespace JARVIS.Client.Mac
{
    public class Notifications
    {
        NSUserNotificationCenter NotificationCenter { get; set; }

        public Notifications()
        {
            // Tell notiication center to display notifications even if the app is topmost
            NotificationCenter = NSUserNotificationCenter.DefaultUserNotificationCenter;
            NotificationCenter.ShouldPresentNotification = (c, n) => true;

            // Handler
            NotificationCenter.DidActivateNotification += (s, e) =>
            {
                switch (e.Notification.ActivationType)
                {
                    case NSUserNotificationActivationType.ContentsClicked:
                        //Console.WriteLine("Notification Touched");
                        break;
                    case NSUserNotificationActivationType.ActionButtonClicked:
                        Console.WriteLine(e.Notification.UserInfo.ValueForKey(new NSString("OP")).ToString());

                        break;
                    default:
                        break;
                }
            };
        }

        public void Notify(NSUserNotification notification)
        {
            NotificationCenter.DeliverNotification(notification);
        }
    }
}
