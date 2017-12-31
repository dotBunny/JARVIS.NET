using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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

        public static string GET(Uri endpoint, System.Collections.Specialized.NameValueCollection headers = null)
        {
            return GetWebResponse(endpoint, "GET", string.Empty, headers);
        }

        public static string POST(Uri endpoint, string requestBody = "", System.Collections.Specialized.NameValueCollection headers = null)
        {
            return GetWebResponse(endpoint, "POST", requestBody, headers);
        }

        static string GetWebResponse(Uri endpoint, string method = "GET", string requestBody = "", System.Collections.Specialized.NameValueCollection headers = null)
        {
            string responseString = string.Empty;

            // Setup web request
            HttpWebRequest request = WebRequest.Create(endpoint) as HttpWebRequest;

            // Headers
            if (headers != null)
            {
                request.Headers.Add(headers);
            }

            // Setup Request
            //request.Date = DateTime.Now;
            request.Method = method;
            request.Timeout = 30000;
            request.Accept = "application/json";

            if (request.Method == "POST")
            {
                byte[] data = Encoding.UTF8.GetBytes(requestBody);

                request.ContentLength = data.LongLength;
                request.ContentType = "application/json";

                // Populate requests data
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }

            // We use a try catch for safety right now
            try
            {
                HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse;

                using (Stream responseStream = webResponse.GetResponseStream())

                using (StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    responseString = responseStreamReader.ReadToEnd();
                }
            }
            catch ( Exception e )
            {
                responseString = "{'error':911, 'error_description':'" + e.Message + "'}";
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