using System;
namespace JARVIS.Core.Services
{
    public interface IService
    {
        string GetName();
        void Start();
        void Stop();

        void Tick();
    }
}