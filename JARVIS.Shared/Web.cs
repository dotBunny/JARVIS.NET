using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace JARVIS.Shared
{
    public static class Web
    {
        public static Dictionary<string, string> GetStringDictionary(System.Collections.Specialized.NameValueCollection parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            foreach(string s in parameters.AllKeys)
            {
                returnParameters.Add(s, parameters[s]);
            }

            return returnParameters;
        }

        public static void Touch(string URI) {

            // The downloaded resource ends up in the variable named content.  
    var content = new MemoryStream();  

    // Initialize an HttpWebRequest for the current URL.  
    var webReq = (HttpWebRequest)WebRequest.Create(URI);  

    // Send the request to the Internet resource and wait for  
    // the response.  
    // Note: you can't use HttpWebRequest.GetResponse in a Windows Store app.  
    using (WebResponse response = webReq.GetResponse())  
    {  
        // Get the data stream that is associated with the specified URL.  
        using (Stream responseStream = response.GetResponseStream())  
        {  
            // Read the bytes in responseStream and copy them to content.    
            responseStream.CopyTo(content);  
        }  
    }  

    //// Return the result as a byte array.  
    //return content.ToArray();  


            //// Don't block just go do it
            //WebRequest.Create(URI).GetResponseAsync();
        }
    }
}