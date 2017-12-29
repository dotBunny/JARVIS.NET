using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace JARVIS.Client.Mac
{
    public class Notifications
    {
        NSUserNotificationCenter NotificationCenter { get; set; }

        //List<NSUserNotification> ActiveNotifications = new List<NSUserNotification>();
        //int CachedNotificationCount;

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
                       
                        Console.WriteLine("Notification Touched");
                       
                        break;
                    case NSUserNotificationActivationType.ActionButtonClicked:
                        
                        Console.WriteLine(e.Notification.UserInfo.ValueForKey(new NSString("OP")));

                        break;
                    default:
                        break;
                }

                //if (ActiveNotifications.Contains(e.Notification))
                //{
                //    ActiveNotifications.Remove(e.Notification);
                //    UpdateBadge();
                //}

            };

            //NotificationCenter.DidDeliverNotification += (sender, e) => {

            //    Console.WriteLine(e.Notification.ActivationType.ToString());

            //};


        }

        public void Notify(NSUserNotification notification)
        {
            
            NotificationCenter.DeliverNotification(notification);

            //ActiveNotifications.Add(notification);
            //UpdateBadge();
        }

        //void UpdateBadge()
        //{
        //    CachedNotificationCount = ActiveNotifications.Count;
        //    var dockTile = NSApplication.SharedApplication.DockTile;

        //    if ( CachedNotificationCount > 0 )
        //    {
        //        dockTile.BadgeLabel = CachedNotificationCount.ToString();
        //    }
        //    else 
        //    {
        //        dockTile.BadgeLabel = string.Empty;
        //    }
        //}

        //public int GetActiveNotificationCount()
        //{
        //    return ActiveNotifications.Count;
        //}

    }
}