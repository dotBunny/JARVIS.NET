using System;
using Grapevine.Interfaces.Server;

namespace JARVIS.Core.Services
{
    public interface IService
    {
        string GetName();

        void HandleCallbackAsync(IHttpRequest request);

        void Start();
        void Stop();

        void Tick();
    }
}