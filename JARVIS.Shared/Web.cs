using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JARVIS.Shared
{
    public static class Web
    {
        public const string SuccessCode = "OK";
        public const string FailCode = "FAIL";
        public const string Deliminator = "||";

        public static Dictionary<string, string> GetStringDictionary(System.Collections.Specialized.NameValueCollection parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            foreach(string s in parameters.AllKeys)
            {
                returnParameters.Add(s, parameters[s]);
            }

            return returnParameters;
        }

        public static Dictionary<string, string> GetStringDictionaryEscaped(string parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            string[] splitParameters = parameters.Split(new[] { Deliminator }, StringSplitOptions.None);

            for (int i = 0; i < splitParameters.Length; i+=2)
            {
                if ((i + 1) < splitParameters.Length)
                {
                    returnParameters.Add(splitParameters[i].Trim(), splitParameters[i + 1].Trim());
                }
            }

            return returnParameters;
        }


        public static void Touch(string URI) {
            HttpWebRequest request = WebRequest.Create(URI) as HttpWebRequest;
            request.GetResponse();
        }

        public static string GET(string endpoint, Dictionary<string, string> headers = null)
        {
            return GetWebResponse(endpoint, "GET", string.Empty, headers);
        }

        public static string POST(string endpoint, string requestBody = "", Dictionary<string, string> headers = null)
        {
            return GetWebResponse(endpoint, "POST", requestBody, headers);
        }

        static string GetWebResponse(string endpoint, string method = "GET", string requestBody = "", Dictionary<string,string> headers = null)
        {
            string responseString = string.Empty;

            // Setup web request
            using (HttpClient client = new HttpClient())
            {

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> headerPair in headers)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(headerPair.Key, headerPair.Value);
                    }
                }

                HttpRequestMessage message = new HttpRequestMessage(new HttpMethod(method), new Uri(endpoint))
                {
                    Content = new StringContent(requestBody, Encoding.UTF8)
                };

                using (HttpResponseMessage response = Task.Run(() => client.SendAsync(message)).Result)
                {
                    
                    responseString = response.Content.ReadAsStringAsync().Result;
                }
            }
          
            return responseString;   
        }

        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);

            // this code block is taken from httpValueCollection.ToString() method
            // and modified so it encodes strings with HttpUtility.UrlEncode
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
                                        sb.Append('&');

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
    }
}