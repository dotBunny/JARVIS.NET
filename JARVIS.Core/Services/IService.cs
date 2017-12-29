using System;
namespace JARVIS.Core.Services
{
    public interface IService
    {
        string GetName();
        void SetValue(string key, string data);
        void Start();
        void Stop();

        void Tick();
    }
}