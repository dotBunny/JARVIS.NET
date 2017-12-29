using System;
using System.Collections.Generic;

namespace JARVIS.Client
{
    public class OAuthNotification : Shared.INotification
    {
        string URI = string.Empty;
        string Title = string.Empty;
        string Message = string.Empty;
        public string State = string.Empty;

        public OAuthNotification(string uri, string title, string message)
        {
            URI = uri;
            Title = title;
            Message = message;
        }

        public Dictionary<string,string> GetDictionary()
        {
            return new Dictionary<string, string>() { { "Shared.Platform.Run.Path", URI } };
        }

        public string GetTitle()
        {
            return Title;
        }

        public string GetMessage()
        {
            return Message;
        }
    }
}
