using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JARVIS.Shared
{
    /// <summary>
    /// Web Related Helpers
    /// </summary>
    public static class Web
    {
        /// <summary>
        /// The fail message to respond when something goes wrong (we do not provide info).
        /// </summary>
        public const string FailCode = "{ \"status\": 400, \"message\": \"Operation Failed\"}";

        /// <summary>
        /// The string dictionary deliminator.
        /// </summary>
        public const string StringDeliminator = "||";

        /// <summary>
        /// The success message to respond when everything is OK.
        /// </summary>
        public const string SuccessCode = "{ \"status\": 200, \"message\": \"OK\"}";

        /// <summary>
        /// Add query parameter to a <see cref="T:System.Uri"/>.
        /// </summary>
        /// <returns>The updated <see cref="T:System.Uri"/></returns>
        /// <param name="uri">Target <see cref="T:System.Uri"/>.</param>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            if (httpValueCollection.Count == 0)
                ub.Query = String.Empty;
            else
            {
                var sb = new StringBuilder();

                for (int i = 0; i < httpValueCollection.Count; i++)
                {
                    string text = httpValueCollection.GetKey(i);
                    {
                        text = HttpUtility.UrlEncode(text);

                        string val = (text != null) ? (text + "=") : string.Empty;
                        string[] vals = httpValueCollection.GetValues(i);

                        if (sb.Length > 0)
                            sb.Append('&');

                        if (vals == null || vals.Length == 0)
                            sb.Append(val);
                        else
                        {
                            if (vals.Length == 1)
                            {
                                sb.Append(val);
                                sb.Append(HttpUtility.UrlEncode(vals[0]));
                            }
                            else
                            {
                                for (int j = 0; j < vals.Length; j++)
                                {
                                    if (j > 0)
                                    {
                                        sb.Append('&');
                                    }

                                    sb.Append(val);
                                    sb.Append(HttpUtility.UrlEncode(vals[j]));
                                }
                            }
                        }
                    }
                }

                ub.Query = sb.ToString();
            }
            return ub.Uri;
        }

        /// <summary>
        /// GET a <see cref="T:System.Byte"/> array of the data returned from the specified endpoint.
        /// </summary>
        /// <returns>A <see cref="T:System.Byte"/> array.</returns>
        /// <param name="endpoint">The URL to request a response.</param>
        /// <param name="headers">Any headers required to make the request.</param>
        public static byte[] GetBytes(string endpoint, Dictionary<string, string> headers = null)
        {
            return GetBytesResponse(endpoint, "GET", string.Empty, headers);
        }

        /// <summary>
        /// Get a <see cref="T:System.Byte"/> array of the data returned from the specified endpoint via GET or POST.
        /// </summary>
        /// <returns>A <see cref="T:System.Byte"/> array.</returns>
        /// <param name="endpoint">The URL to request a response.</param>
        /// <param name="method">GET/POST method to make the request.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="headers">Any headers required to make the request.</param>
        static byte[] GetBytesResponse(string endpoint, string method = "GET", string requestBody = "", Dictionary<string, string> headers = null)
        {
            byte[] responseBytes = new byte[0];

            using (HttpClient client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> headerPair in headers)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(headerPair.Key, headerPair.Value);
                    }
                }

                // Stub Request
                HttpRequestMessage message;

                if (!string.IsNullOrEmpty(requestBody) || requestBody != "")
                {
                    message = new HttpRequestMessage(new HttpMethod(method), new Uri(endpoint))
                    {
                        Content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded")
                    };
                }
                else
                {
                    message = new HttpRequestMessage(new HttpMethod(method), new Uri(endpoint));
                }

                // Get Response
                using (HttpResponseMessage response = Task.Run(() => client.SendAsync(message)).Result)
                {
                    responseBytes = response.Content.ReadAsByteArrayAsync().Result;
                }
            }
            return responseBytes;
        }

        /// <summary>
        /// GET a string of the data returned from the specified endpoint.
        /// </summary>
        /// <returns>The content returned from the request.</returns>
        /// <param name="endpoint">The URL to request a response.</param>
        /// <param name="headers">Any headers required to make the request.</param>
        public static string GetJSON(string endpoint, Dictionary<string, string> headers = null)
        {
            return GetStringResponse(endpoint, "GET", string.Empty, headers);
        }

        /// <summary>
        /// Get a <see cref="T:System.Collections.Generic.Dictionary"/> from a <see cref="T:System.Collections.Specialized.NameValueCollection"/>.
        /// </summary>
        /// <returns>A string based <see cref="T:System.Collections.Generic.KeyValuePair"/> <see cref="T:System.Collections.Generic.Dictionary"/>.</returns>
        /// <param name="parameters">The <see cref="T:System.Collections.Specialized.NameValueCollection"/>.</param>
        public static Dictionary<string, string> GetStringDictionary(System.Collections.Specialized.NameValueCollection parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            foreach(string s in parameters.AllKeys)
            {
                returnParameters.Add(s, parameters[s]);
            }

            return returnParameters;
        }

        /// <summary>
        /// Gets a <see cref="T:System.Collections.Generic.Dictionary"/> from a string delimited.
        /// </summary>
        /// <returns>A string based KVP <see cref="T:System.Collections.Generic.Dictionary"/>.</returns>
        /// <param name="parameters">The delimited string.</param>
        public static Dictionary<string, string> GetStringDictionaryEscaped(string parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            string[] splitParameters = parameters.Split(new[] { StringDeliminator }, StringSplitOptions.None);

            for (int i = 0; i < splitParameters.Length; i+=2)
            {
                if ((i + 1) < splitParameters.Length)
                {
                    returnParameters.Add(splitParameters[i].Trim(), splitParameters[i + 1].Trim());
                }
            }

            return returnParameters;
        }

        /// <summary>
        /// Get a string data returned from the specified endpoint via GET or POST.
        /// </summary>
        /// <returns>The content returned from the request.</returns>
        /// <param name="endpoint">The URL to request a response.</param>
        /// <param name="method">GET/POST method to make the request.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="headers">Any headers required to make the request.</param>
        static string GetStringResponse(string endpoint, string method = "GET", string requestBody = "", Dictionary<string,string> headers = null)
        {
            string responseString = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                // Accept JSON
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> headerPair in headers)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(headerPair.Key, headerPair.Value);
                    }
                }

                // Stub Request
                HttpRequestMessage message;

                if (!string.IsNullOrEmpty(requestBody) && requestBody != "")
                {
                    message = new HttpRequestMessage(new HttpMethod(method), new Uri(endpoint))
                    {
                        Content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded")
                    };
                } 
                else 
                {
                    message = new HttpRequestMessage(new HttpMethod(method), new Uri(endpoint));
                }
               
                // Get Response
                using (HttpResponseMessage response = Task.Run(() => client.SendAsync(message)).Result)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                }
            }
            return responseString;   
        }

        /// <summary>
        /// POST to a specified endpoint, and get a string of the data returned.
        /// </summary>
        /// <returns>The content returned from the request.</returns>
        /// <param name="endpoint">The URL to request a response.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="headers">Any headers required to make the request.</param>
        public static string PostJSON(string endpoint, string requestBody = "", Dictionary<string, string> headers = null)
        {
            return GetStringResponse(endpoint, "POST", requestBody, headers);
        }

        /// <summary>
        /// POST to a specified endpoint, and get a <see cref="T:System.Byte"/> array of the data returned.
        /// </summary>
        /// <returns>A <see cref="T:System.Byte"/> array.</returns>
        /// <param name="endpoint">The URL to request a response.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="headers">Any headers required to make the request.</param>
        public static byte[] PostBytes(string endpoint, string requestBody = "", Dictionary<string, string> headers = null)
        {
            return GetBytesResponse(endpoint, "POST", requestBody, headers);
        }

        /// <summary>
        /// Touch the specified URL.
        /// </summary>
        /// <param name="URI">The address to touch.</param>
        public static void Touch(string URI)
        {
            HttpWebRequest request = WebRequest.Create(URI) as HttpWebRequest;
            request.GetResponse();
        }
    }
}