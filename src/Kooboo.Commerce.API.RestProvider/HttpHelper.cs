using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace Kooboo.Commerce.API.RestProvider
{
    public class HttpHelper
    {
        /// <summary>
        /// request the uri with http method ans sending the json.
        /// </summary>
        /// <param name="method">http method, e.g. GET, PUT, POST, DELETE</param>
        /// <param name="uri">the request uri with query string</param>
        /// <param name="jsonData">json data</param>
        /// <param name="proxy">use proxy</param>
        /// <param name="credentials">need credentials for the request.</param>
        /// <returns>the response content</returns>
        public static string Request(string method, string uri, string jsonData, string accept = "application/json", string contentType = "application/json",
            CookieCollection cookies = null, string userAgent = null, string referer = null, IDictionary<string, string> otherHeaders = null, IWebProxy proxy = null, ICredentials credentials = null, int timeOut = 300000)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);

                request.Accept = accept;
                request.ContentType = contentType;
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.Method = method;
                if (!string.IsNullOrEmpty(userAgent))
                    request.UserAgent = userAgent;
                request.Referer = referer;
                if (cookies != null)
                {
                    if (request.CookieContainer == null)
                        request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                if (otherHeaders != null)
                {
                    foreach (var kvp in otherHeaders)
                    {
                        request.Headers.Add(kvp.Key, kvp.Value);
                    }
                }
                request.Proxy = proxy;
                request.Credentials = credentials;
                request.ContentLength = 0;

                // Add request payload if any.
                if (!string.IsNullOrEmpty(jsonData))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(jsonData);
                    request.ContentLength = buffer.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(buffer, 0, buffer.Length);
                    }
                }

                // Execute request.
                using (WebResponse response = request.GetResponse())
                {
                    var result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    return result;
                }

            }
            catch (WebException ex)
            {
                //var message = ex.Message;
                //var response = ex.Response;
                //if (response != null)
                //{
                //    using (var responseStream = response.GetResponseStream())
                //    {
                //        message = new StreamReader(responseStream, true).ReadToEnd();
                //    }
                //}

                //int statusCode = 0;
                //if (response is HttpWebResponse)
                //    statusCode = (int)((HttpWebResponse)response).StatusCode;

                throw ex;
            }
        }
    }
}
