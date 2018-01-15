using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Enums;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services.Singletons
{
    public class WebServicesStatus : IWebServicesStatus
    {
        private readonly Dictionary<WebServiceType, Tuple<bool,string>> _statusDictionary;

        public WebServicesStatus()
        {
            _statusDictionary = new Dictionary<WebServiceType, Tuple<bool, string>>();
            foreach (var type in Enum.GetValues(typeof(WebServiceType)).OfType<WebServiceType>())
            {
                AddDefaultStatus(type);
            }
        }

        private void AddDefaultStatus(WebServiceType type)
        {
            _statusDictionary.Add(type, CreateStatus());
        }

        private static Tuple<bool, string> CreateStatus(bool online = true, string errorMessage = null)
        {
            return new Tuple<bool, string>(online, errorMessage);
        }

        public string GetErrorMessage(WebServiceType type)
        {
            return _statusDictionary[type].Item2;
        }

        public bool IsOnline(WebServiceType type)
        {
            return _statusDictionary[type].Item1;
        }

        public void SetStatus(WebServiceType type, bool online, string errorMessage)
        {
            _statusDictionary[type] = CreateStatus(online, errorMessage);
        }

        public bool IsAnyServiceOffline()
        {
            return _statusDictionary.Any(s => !s.Value.Item1);
        }
    }
}
