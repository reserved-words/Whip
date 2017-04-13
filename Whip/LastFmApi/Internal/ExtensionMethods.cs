using LastFmApi.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi.Internal
{
    internal static class ExtensionMethods
    {
        public static string GetParameterName(this Enum en)
        {
            var memberInfo = en.GetType().GetMember(en.ToString());

            if (memberInfo == null || !memberInfo.Any())
                return null;

            var attrs = memberInfo[0].GetCustomAttributes(typeof(ParameterNameAttribute), false).OfType<ParameterNameAttribute>();

            return attrs.FirstOrDefault()?.Name ?? en.ToString();
        }

        public static void AddApiSignature(this Dictionary<ParameterKey, string> parameters, string secret)
        {
            var apiSigBuilder = new StringBuilder();
            foreach (var kv in parameters.OrderBy(kv => kv.Key.GetParameterName(), StringComparer.Ordinal))
            {
                apiSigBuilder.Append(kv.Key);
                apiSigBuilder.Append(kv.Value);
            }
            apiSigBuilder.Append(secret);

            parameters.Add(ParameterKey.ApiSig, MD5Hasher.Hash(apiSigBuilder.ToString()));
        }

        public static TResult GetResult<TResult>(this ApiMethodBase<TResult> method)
        {
            var response = WebHelper.HttpGetAsync(method.Parameters).Result;
            return method.ParseResult(response);
        }

        public async static Task<TResult> GetResultAsync<TResult>(this ApiMethodBase<TResult> method)
        {
            var response = await WebHelper.HttpGetAsync(method.Parameters);
            return method.ParseResult(response);
        }

        public async static Task PostAsync(this ApiMethodBase method)
        {
            await WebHelper.HttpPostAsync(method.Parameters);
        }

        public static string GetQueryKey(this KeyValuePair<ParameterKey, string> kvp)
        {
            return kvp.Key.GetParameterName();
        }

        public static string GetQueryValue(this KeyValuePair<ParameterKey, string> kvp)
        {
            return kvp.Value.Replace("&", "%26");
        }

        public static string GetQueryString(this KeyValuePair<ParameterKey, string> kvp)
        {
            return string.Format("{0}={1}", kvp.GetQueryKey(), kvp.GetQueryValue());
        }

        public static Dictionary<string, string> ToStringKeys(this Dictionary<ParameterKey, string> parameters)
        {
            var strings = parameters.OrderBy(p => p.GetQueryKey())
                .Select(p => new KeyValuePair<string, string>(p.GetQueryKey(), p.GetQueryValue()))
                .ToList();

            var dictionary = new Dictionary<string, string>();

            foreach (var kvp in strings)
            {
                dictionary.Add(kvp.Key, kvp.Value);
            }

            return dictionary;
        }
    }
}
