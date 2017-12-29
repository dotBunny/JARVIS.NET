using System;
using System.Collections.Generic;

namespace JARVIS.Shared
{
    public interface INotification
    {
        string GetTitle();
        string GetMessage();

        Dictionary<string, string> GetDictionary();
    }
}
