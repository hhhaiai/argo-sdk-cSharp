using System;
using System.Collections.Generic;
using System.Timers;

namespace Analysys
{
    public class BatchCollecter : ICollecter
    {
        private DateTime sendTimer = default(DateTime);
        private string serverUrl;
        private static int DEFAULT_BATCH_NUM = 20;
        private static long DEFAULT_BATCH_SEC = 10;
        private int batchNum;
        private long batchSec;
        private bool interrupt;
        private List<Dictionary<string, object>> batchMsgList;
        private bool debug;
        private Timer timer;
        private static object objLock = new object();


        public BatchCollecter(string serverUrl) : this(serverUrl, DEFAULT_BATCH_NUM, DEFAULT_BATCH_SEC)
        {
        }
        public BatchCollecter(string serverUrl, bool interrupt) : this(serverUrl, DEFAULT_BATCH_NUM, DEFAULT_BATCH_SEC, interrupt)
        {
        }
        public BatchCollecter(string serverUrl, int batchNum) : this(serverUrl, batchNum, DEFAULT_BATCH_SEC)
        {
        }
        public BatchCollecter(string serverUrl, int batchNum, long batchSec) : this(serverUrl, batchNum, batchSec, false)
        {
        }
        /**
        * 构造方法
        * @param serverUrl 数据接收服务地址
        * @param batchNum 批量发送数量
        * @param batchSec 批量发送等待时间(秒)
        * @param interrupt 是否中断程序
        */
        public BatchCollecter(string serverUrl, int batchNum, long batchSec, bool interrupt)
        {
            if (serverUrl == null || serverUrl.Trim().Length == 0)
            {
                throw new Exception("Server URL is empty");
            }
            else
            {
                if (serverUrl.Contains("/up"))
                {
                    serverUrl = serverUrl.Substring(0, serverUrl.Length - serverUrl.IndexOf("/up"));
                }
            }
            this.serverUrl = serverUrl + "/up";
            this.interrupt = interrupt;
            this.batchNum = batchNum;
            this.batchSec = batchSec * 1000;
            this.batchMsgList = new List<Dictionary<string, object>>(this.batchNum);
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            Init();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sendTimer != default(DateTime) && (DateTime.Now - sendTimer).TotalSeconds >= batchSec)
            {
                Upload();
            }
        }

        private void Init()
        {
            timer.Start();
        }
        public bool Send(Dictionary<string, object> egCollectMessage)
        {
            if (sendTimer == default(DateTime))
            {
                sendTimer = DateTime.Now;
            }
            batchMsgList.Add(egCollectMessage);
            string xWhat = "xwhat";
            if (batchMsgList.Count >= batchNum || EventName.ALIAS.GetEnumDescription().Equals(egCollectMessage[xWhat]))
            {
                Upload();
            }
            return true;
        }
        public void Upload()
        {
            string jsonData = null;
            lock (objLock)
            {
                if (batchMsgList != null && batchMsgList.Count > 0)
                {
                    try
                    {
                        jsonData = JsonHelper.Serialize(batchMsgList);
                        var headParam = new Dictionary<string, string>(1);
                        if (debug)
                        {
                            Console.WriteLine(String.Format("Send message to server: {0} \ndata: {1}", serverUrl, jsonData));
                        }
                        string retMsg = new MessageSender(serverUrl, headParam, jsonData).Send();
                        if (debug)
                        {
                            Console.WriteLine(String.Format("Send message success,response: {0}", retMsg));
                        }
                    }
                    catch (Exception e)
                    {
                        if (interrupt)
                        {
                            Shutdown();
                            throw new Exception("Upload Data Error", e);
                        }
                        else
                        {
                            Console.WriteLine("Upload Data Error" + e);
                        }
                    }
                    finally
                    {
                        batchMsgList.Clear();
                        ResetTimer();
                    }
                }
            }
        }

        private void Shutdown()
        {
            try
            {
                this.timer.Stop();
                this.timer.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("shut down excetion :" + e.Message);
            }
        }
        public void Flush()
        {
            Upload();
        }
        public void Close()
        {
            Flush();
            Shutdown();
        }
        private void ResetTimer()
        {
            sendTimer = default(DateTime);
        }
        public void Debug(bool isDebugModel)
        {
            this.debug = isDebugModel;
        }
    }
}


