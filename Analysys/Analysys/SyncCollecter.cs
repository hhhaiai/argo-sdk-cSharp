using System;
using System.Collections.Generic;

namespace Analysys
{
    public class SyncCollecter : ICollecter
    {
        private string serverUrl;
        private bool interrupt;
        private bool debug;

        public SyncCollecter(string serverUrl, bool interrupt = false)
        {
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new Exception("Server URL is empty");
            }
            if (serverUrl.Contains("/up"))
            {
                serverUrl = serverUrl.Substring(0, serverUrl.Length - serverUrl.IndexOf("/up"));
            }
            this.serverUrl = serverUrl + "/up";
            this.interrupt = interrupt;
        }
        public bool Send(Dictionary<string, object> egCollectMessage)
        {
            bool flag = false;
            try
            {
                List<Dictionary<string, object>> lstEgMessage = new List<Dictionary<string, object>>();
                lstEgMessage.Add(egCollectMessage);
                string jsonData = JsonHelper.Serialize(lstEgMessage);
                var headParam = new Dictionary<string, string>(1);
                if (debug)
                {
                    Console.WriteLine(string.Format("Send message to server: {0} \ndata: {1}", serverUrl, jsonData));
                }
                string retMsg = new MessageSender(serverUrl, headParam, jsonData).Send();
                Console.WriteLine(retMsg);
                if (debug)
                {
                    Console.WriteLine(string.Format("Send message success,response: {0}", retMsg));
                }
                flag = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (interrupt)
                {
                    Console.WriteLine("AnalysysException：" + ex.Message);
                    throw ex;
                }
                else
                    Console.WriteLine("AnalysysException：" + ex.Message);

            }
            return flag;

        }

        public void Close()
        {

        }
        public void Debug(bool isDebugModel)
        {
            this.debug = isDebugModel;
        }
        public void Flush()
        {

        }
        public void Upload()
        {

        }
    }
}
