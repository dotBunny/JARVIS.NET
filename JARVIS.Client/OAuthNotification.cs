using System.Collections.Generic;

namespace JARVIS.Client
{
    /// <summary>
    /// An OAuth Notification
    /// </summary>
    public class OAuthNotification : Shared.Services.Notification.INotification
    {
        /// <summary>
        /// The OAuth URI to hit to trigger authentication.
        /// </summary>
        readonly string _endpoint = string.Empty;

        /// <summary>
        /// The title to be used with the notification.
        /// </summary>
        readonly string _title = string.Empty;

        /// <summary>
        /// The message to display with the notification.
        /// </summary>
        readonly string _message = string.Empty;

        /// <summary>
        /// The associated state key to be referenced by the notification.
        /// </summary>
        public string State = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Client.OAuthNotification"/> class.
        /// </summary>
        /// <param name="uri">The URL to open.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="message">The message of the notification.</param>
        public OAuthNotification(string uri, string title, string message)
        {
            _endpoint = uri;
            _title = title;
            _message = message;
        }

        /// <summary>
        /// Gets the message of the notification.
        /// </summary>
        /// <returns>The message.</returns>
        public string GetMessage()
        {
            return _message;
        }

        /// <summary>
        /// Get a the parameters of the notification.
        /// </summary>
        /// <returns>A string based <see cref="T:System.Collections.Generic.KeyValuePair"/> <see cref="T:System.Collections.Generic.Dictionary"/>.</returns>
        public Dictionary<string,string> GetParameters()
        {
            return new Dictionary<string, string> { { "Shared.Platform.Run.executablePath", _endpoint } };
        }

        /// <summary>
        /// Get the title of the notification.
        /// </summary>
        /// <returns>The title.</returns>
        public string GetTitle()
        {
            return _title;
        }
    }
}