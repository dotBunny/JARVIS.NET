using System;
namespace JARVIS.Server.Services
{
    public interface IService
    {
        string GetName();
        void Start();
        void Stop();
    }
}
