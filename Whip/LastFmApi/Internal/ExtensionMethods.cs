using LastFmApi.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LastFmApi.Internal.Helpers;

namespace LastFmApi.Internal
{
    internal static class ExtensionMethods
    {
        public static string GetStringValue(this Enum en)
        {
            var memberInfo = en.GetType().GetMember(en.ToString());

            if (memberInfo == null || !memberInfo.Any())
                return null;

            var attrs = memberInfo[0].GetCustomAttributes(typeof(StringValueAttribute), false).OfType<StringValueAttribute>();

            return attrs.FirstOrDefault()?.Value ?? en.ToString();
        }

        public static void AddApiSignature(this Dictionary<ParameterKey, string> parameters, string secret)
        {
            var apiSigBuilder = new StringBuilder();
            foreach (var kv in parameters.OrderBy(kv => kv.Key.GetStringValue(), StringComparer.Ordinal))
            {
                apiSigBuilder.Append(kv.Key);
                apiSigBuilder.Append(kv.Value);
            }
            apiSigBuilder.Append(secret);

            parameters.Add(ParameterKey.ApiSig, MD5Helper.Hash(apiSigBuilder.ToString()));
        }

        public async static Task<TResult> GetResultAsync<TResult>(this ApiMethodBase<TResult> method)
        {
            var response = await WebHelper.HttpGetAsync(method.Parameters);
            return method.ParseResult(XmlHelper.Validate(response));
        }

        public async static Task PostAsync(this ApiMethodBase method)
        {
            var response = await WebHelper.HttpPostAsync(method.Parameters);
            XmlHelper.Validate(response);
        }

        public static string GetQueryKey(this KeyValuePair<ParameterKey, string> kvp)
        {
            return kvp.Key.GetStringValue();
        }

        public static string GetQueryValue(this KeyValuePair<ParameterKey, string> kvp, bool urlEncode)
        {
            return kvp.Value == null
                ? string.Empty
                : urlEncode
                ? HttpUtility.UrlEncode(kvp.Value)
                : kvp.Value;
        }

        public static string GetQueryString(this KeyValuePair<ParameterKey, string> kvp)
        {
            return string.Format("{0}={1}", kvp.GetQueryKey(), kvp.GetQueryValue(true));
        }

        public static Dictionary<string, string> ToStringKeys(this Dictionary<ParameterKey, string> parameters)
        {
            var strings = parameters.OrderBy(p => p.GetQueryKey())
                .Select(p => new KeyValuePair<string, string>(p.GetQueryKey(), p.GetQueryValue(false)))
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
