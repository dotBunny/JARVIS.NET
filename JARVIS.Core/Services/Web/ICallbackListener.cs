using System;
using Grapevine.Interfaces.Server;

namespace JARVIS.Core.Services.Web
{
    public interface ICallbackListener
    {
        void Callback(IHttpRequest request);
    }
}
