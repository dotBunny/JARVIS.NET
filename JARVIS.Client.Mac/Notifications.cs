using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace JARVIS.Client.Mac
{
    public class Notifications : JARVIS.Shared.INotifier
    {
        NSUserNotificationCenter NotificationCenter { get; set; }

        NSString runPathKey = new NSString("Shared.Platform.Run.Path");
        NSString runArguementsKey = new NSString("Shared.Platform.Run.Arguements");
        NSString runHideKey = new NSString("Shared.Platform.Run.Hide");

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

                        if ( e.Notification.UserInfo.ContainsKey(runPathKey) ){
                            Shared.Platform.Run(e.Notification.UserInfo.ValueForKey(runPathKey).ToString(), string.Empty, false);
                        }

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

        public void Notify(string title, string description)
        {
            // Trigger a local notification after the time has elapsed
            var notification = new NSUserNotification();

            // Add text and sound to the notification
            notification.Title = title;
            notification.InformativeText = description;
            notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
            notification.HasActionButton = false;
            notification.HasReplyButton = false;

            Notify(notification);
        }

        public void Notify(Shared.INotification notification)
        {
            // Trigger a local notification after the time has elapsed
            var sysNotification = new NSUserNotification();

            // Add text and sound to the notification
            sysNotification.Title = notification.GetTitle();
            sysNotification.InformativeText = notification.GetMessage();
            sysNotification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
            sysNotification.HasActionButton = true;
            sysNotification.HasReplyButton = false;

            // Create Key Data (how we pass things around unfortunately)
            foreach(KeyValuePair<string,string> item in notification.GetDictionary())
            {
                sysNotification.SetValueForKey(new NSString(item.Key), new NSString(item.Value));
            }

            Notify(sysNotification);
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