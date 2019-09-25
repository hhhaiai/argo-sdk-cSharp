using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Analysys
{
    public class MessageSender
    {
        public int Timeout { get; set; } = 100000;
        private bool isEncode;
        private string serverUrl;
        private IDictionary<string, string> egHeaderParams;
        private string jsonData;

        public MessageSender(string serverUrl, IDictionary<string, string> egHeaderParams, string jsonData, bool isEncode = true)
        {
            this.serverUrl = serverUrl;
            this.egHeaderParams = egHeaderParams;
            this.jsonData = jsonData;
            this.isEncode = isEncode;
        }
        public string Send()
        {
            HttpWebRequest req = GetWebRequest(serverUrl, "POST");
            //添加头文件
            if (egHeaderParams != null && egHeaderParams.Count > 0)
            {
                IEnumerator<KeyValuePair<string, string>> dem = egHeaderParams.GetEnumerator();
                while (dem.MoveNext())
                {
                    req.Headers.Add(dem.Current.Key, dem.Current.Value);
                }
            }
            byte[] postData = null;
            if (isEncode)
            {
                string temp = AnalysysEncoder.Compress(jsonData);//Gzip压缩
                postData = Encoding.UTF8.GetBytes(jsonData);
            }
            else
            {
                postData = Encoding.UTF8.GetBytes(jsonData);
                req.ContentType = "application/json";
            }
            Console.WriteLine("Http Request Data: " + jsonData);
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            string response = GetResponseAsString(rsp, encoding);
            Console.WriteLine("Http Response Data: "+ response);
            return response;
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = method;
            //DotNet
            req.UserAgent = "Analysys DotNet SDK";
            req.Timeout = Timeout;
            return req;
        }
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            //return result.ToString();
            return AnalysysEncoder.Decompress(result.ToString());
        }

    }
}
