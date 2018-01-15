using System;
using Whip.Common.Enums;

namespace Whip.Services.Interfaces.Singletons
{
    public interface IWebServicesStatus
    {
        string GetErrorMessage(WebServiceType type);

        bool IsAnyServiceOffline();

        bool IsOnline(WebServiceType type);

        void SetStatus(WebServiceType type, bool online, string errorMessage = null);
    }
}
