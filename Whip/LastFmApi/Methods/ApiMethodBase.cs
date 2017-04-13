using LastFmApi.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LastFmApi.Methods
{
    internal abstract class ApiMethodBase
    {
        private readonly string _apiSecret;

        public ApiMethodBase(ApiClient client, string methodName)
        {
            _apiSecret = client.ApiSecret;

            Parameters = new Dictionary<ParameterKey, string>();
            Parameters.Add(ParameterKey.Method, methodName);
            Parameters.Add(ParameterKey.ApiKey, client.ApiKey);
        }

        public ApiMethodBase(AuthorizedApiClient client, string methodName)
        {
            _apiSecret = client.ApiSecret;

            Parameters = new Dictionary<ParameterKey, string>();
            Parameters.Add(ParameterKey.Method, methodName);
            Parameters.Add(ParameterKey.ApiKey, client.ApiKey);
            Parameters.Add(ParameterKey.SessionKey, client.SessionKey);
        }

        public Dictionary<ParameterKey, string> Parameters { get; protected set; }

        protected void AddApiSignature()
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
    }

    internal abstract class ApiMethodBase<TResult> : ApiMethodBase
    {
        public ApiMethodBase(ApiClient client, string methodName)
            :base(client, methodName)
        {
        }

        public abstract TResult ParseResult(string result);
    }
}
