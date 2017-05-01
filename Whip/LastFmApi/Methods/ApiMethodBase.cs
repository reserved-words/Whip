using LastFmApi.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LastFmApi.Methods
{
    internal abstract class ApiMethodBase
    {
        private readonly string _apiSecret;
        private readonly bool _authorized;

        public ApiMethodBase(ApiClient client, string methodName)
        {
            _apiSecret = client.ApiSecret;

            Parameters = new Dictionary<ParameterKey, string>();
            Parameters.Add(ParameterKey.Method, methodName);
            Parameters.Add(ParameterKey.ApiKey, client.ApiKey);

            _authorized = false;
        }

        public ApiMethodBase(AuthorizedApiClient client, string methodName)
        {
            _apiSecret = client.ApiSecret;

            Parameters = new Dictionary<ParameterKey, string>();
            Parameters.Add(ParameterKey.Method, methodName);
            Parameters.Add(ParameterKey.ApiKey, client.ApiKey);
            Parameters.Add(ParameterKey.SessionKey, client.SessionKey);

            _authorized = true;
        }

        public Dictionary<ParameterKey, string> Parameters { get; protected set; }

        private void AddApiSignature()
        {
            StringBuilder apiSigBuilder = new StringBuilder();
            foreach (var kv in Parameters.OrderBy(kv => kv.Key.GetParameterName(), StringComparer.Ordinal))
            {
                apiSigBuilder.Append(kv.Key.GetParameterName());
                apiSigBuilder.Append(kv.Value);
            }
            apiSigBuilder.Append(_apiSecret);

            Parameters.Add(ParameterKey.ApiSig, MD5Hasher.Hash(apiSigBuilder.ToString()));
        }

        protected void SetParameters(Dictionary<ParameterKey, string> parameters = null)
        {
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    Parameters.Add(kvp.Key, kvp.Value);
                }
            }

            if (_authorized)
            {
                AddApiSignature();
            }
        }
    }

    internal abstract class ApiMethodBase<TResult> : ApiMethodBase
    {
        public ApiMethodBase(ApiClient client, string methodName)
            :base(client, methodName)
        {
        }

        public ApiMethodBase(AuthorizedApiClient client, string methodName)
            : base(client, methodName)
        {
        }

        public abstract TResult ParseResult(XElement xmlResult);
    }
}
