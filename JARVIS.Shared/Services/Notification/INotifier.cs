namespace JARVIS.Shared.Services.Notification
{
    /// <summary>
    /// Notifier Interface
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Show a new notification.
        /// </summary>
        /// <param name="notification">The notification object.</param>
        void Notify(INotification notification);

        /// <summary>
        /// Show a new notification.
        /// </summary>
        /// <param name="title">The notification title.</param>
        /// <param name="description">The notification description.</param>
        void Notify(string title, string description);
    }
}