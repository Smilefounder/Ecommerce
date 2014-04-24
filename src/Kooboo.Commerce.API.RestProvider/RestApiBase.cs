using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;
using Kooboo.Commerce.API.HAL.Serialization;

namespace Kooboo.Commerce.API.RestProvider
{
    /// <summary>
    /// rest api base class
    /// </summary>
    public abstract class RestApiBase
    {
        public string ContentType { get; set; }

        public string Accept { get; set; }

        public RestApiBase()
        {
            ContentType = "application/json";
            Accept = "application/json";
            QueryParameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// http get request to the api url
        /// </summary>
        /// <typeparam name="T">return result type</typeparam>
        /// <param name="apiUrl">api url (action name)</param>
        /// <param name="data">data for sending to remote server</param>
        /// <returns>return the result</returns>
        public T Get<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "GET", data);
        }

        /// <summary>
        /// http post request to the api url
        /// </summary>
        /// <typeparam name="T">return result type</typeparam>
        /// <param name="apiUrl">api url (action name)</param>
        /// <param name="data">data for sending to remote server</param>
        /// <returns>return the result</returns>
        public T Post<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "POST", data);
        }

        /// <summary>
        /// http put request to the api url
        /// </summary>
        /// <typeparam name="T">return result type</typeparam>
        /// <param name="apiUrl">api url (action name)</param>
        /// <param name="data">data for sending to remote server</param>
        /// <returns>return the result</returns>
        public T Put<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "PUT", data);
        }

        /// <summary>
        /// http delete request to the api url
        /// </summary>
        /// <typeparam name="T">return result type</typeparam>
        /// <param name="apiUrl">api url (action name)</param>
        /// <param name="data">data for sending to remote server</param>
        /// <returns>return the result</returns>
        public T Delete<T>(string apiUrl, object data = null)
        {
            return Request<T>(apiUrl, "DELETE", data);
        }
        /// <summary>
        /// http request to the api url
        /// </summary>
        /// <typeparam name="T">return result type</typeparam>
        /// <param name="apiUrl">api url (action name)</param>
        /// <param name="method">request method</param>
        /// <param name="data">data for sending to remote server</param>
        /// <returns>return the result</returns>
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
            var json = HttpHelper.Request(method, url, jsonData, accept: Accept, contentType: ContentType);

            if (ContentType == "application/hal+json")
            {
                return JsonConvert.DeserializeObject<T>(json, new ResourceConverter());
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// formate the name value collection to string
        /// </summary>
        /// <param name="dic">name value collection</param>
        /// <param name="comma">split comma between a pair of name value</param>
        /// <param name="equal">equal used to split the name and value</param>
        /// <param name="leftComma">left comma enclose the value on the left side</param>
        /// <param name="rightComma">right comma enclose the value on the right side</param>
        /// <param name="keyToString">get key from name</param>
        /// <param name="valToString">get value from value</param>
        /// <returns>formated string</returns>
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

        /// <summary>
        /// set the api controller path
        /// </summary>
        protected abstract string ApiControllerPath { get; }
        /// <summary>
        /// the web api host address
        /// </summary>
        public string WebAPIHost { get; set; }
        /// <summary>
        /// query string parameters, which will appned to the request url.
        /// </summary>
        public Dictionary<string, string> QueryParameters { get; set; }
    }
}
