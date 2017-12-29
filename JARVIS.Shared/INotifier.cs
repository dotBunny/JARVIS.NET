using System;
namespace JARVIS.Shared
{
    public interface INotifier
    {
        void Notify(string title, string description);
        void Notify(INotification notification);
    }
}
