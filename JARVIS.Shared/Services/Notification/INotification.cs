using System.Collections.Generic;

namespace JARVIS.Shared.Services.Notification
{
    /// <summary>
    /// A Notification
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Get a the parameters of the notification.
        /// </summary>
        /// <returns>A string based <see cref="T:System.Collections.Generic.KeyValuePair"/> <see cref="T:System.Collections.Generic.Dictionary"/>.</returns>
        Dictionary<string, string> GetParameters();

        /// <summary>
        /// Gets the message of the notification.
        /// </summary>
        /// <returns>The message.</returns>
        string GetMessage();

        /// <summary>
        /// Get the title of the notification.
        /// </summary>
        /// <returns>The title.</returns>
        string GetTitle();
    }
}