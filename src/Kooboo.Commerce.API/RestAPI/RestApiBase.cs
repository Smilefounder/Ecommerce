using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;

namespace Kooboo.Commerce.API.RestAPI
{
    public abstract class RestApiBase
    {
        public RestApiBase()
        {
            QueryParameters = new Dictionary<string, string>();
        }

        public T Get<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "GET", data);
        }

        public T Post<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "POST", data);
        }

        public T Put<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "PUT", data);
        }

        public T Delete<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "DELETE", data);
        }

        public T Request<T>(string apiUrl, string method = "GET", object data = null)
        {
            string jsonData = null;
            if (data != null)
            {
                jsonData = JsonConvert.SerializeObject(data);
            }

            string url = string.Join("/", WebAPIHost, ApiControllerPath, apiUrl).TrimEnd('/');
            if (QueryParameters != null && QueryParameters.Count > 0)
            {
                string spliter = url.IndexOf('?') > 0 ? "&" : "?";
                string qs = string.Join("&", QueryParameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)));
                url = url + spliter + qs;
            }

            if (HttpContext.Current != null)
            {
                Uri uri = new Uri(url);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query))
                {
                    NameValueCollection queryString = HttpUtility.ParseQueryString(uri.Query);
                    NameValueCollection oldQueryString = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query);
                    foreach (var qs in oldQueryString.AllKeys)
                    {
                        var val = oldQueryString[qs];
                        if (!queryString.AllKeys.Contains(qs) && !string.IsNullOrEmpty(val))
                            queryString.Add(qs, val);
                    }
                    url = string.Join("?", string.IsNullOrEmpty(uri.Query) ? uri.AbsoluteUri : uri.AbsoluteUri.Replace(uri.Query, ""), ToFormatString(queryString));
                }
            }
            var json = HttpHelper.Request(method, url, jsonData);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        private static string ToFormatString(NameValueCollection dic, string comma = "&", string equal = "=", string leftComma = "", string rightComma = "", Func<object, string> keyToString = null, Func<object, string> valToString = null)
        {
            if (dic == null || dic.Count <= 0)
                return string.Empty;
            if (keyToString == null)
                keyToString = o => o.ToString();
            if (valToString == null)
                valToString = o => o == null ? null : o.ToString();
            StringBuilder sb = new StringBuilder();
            int idx = 0;
            foreach (string key in dic.Keys)
            {
                sb.AppendFormat("{0}{1}{2}{3}{4}", keyToString(key), equal, leftComma, valToString(dic[key]), rightComma);
                if (idx < dic.Count - 1)
                    sb.Append(comma);
                idx++;
            }
            return sb.ToString();
        }

        protected abstract string ApiControllerPath { get; }

        public string WebAPIHost { get; set; }

        public Dictionary<string, string> QueryParameters { get; set; }
    }
}
