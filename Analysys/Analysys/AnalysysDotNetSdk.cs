using System;
using System.Collections.Generic;

namespace Analysys
{
    public class AnalysysDotNetSdk
    {
        private string SDK_VERSION = "4.0.8";
        private ICollecter collecter;
        private string appId;
        private Dictionary<string, object> egBaseProperties;
        private Dictionary<string, object> xcontextSuperProperties;
        private DEBUG debugMode = (int)DEBUG.CLOSE;

        public AnalysysDotNetSdk(ICollecter collecter, string appId)
        {
            this.collecter = collecter;
            this.appId = appId;
            this.egBaseProperties = new Dictionary<string, object>(3);
            this.xcontextSuperProperties = new Dictionary<string, object>();
            InitBaseProperties();
        }
        public void SetDebugMode(DEBUG debug)
        {
            this.debugMode = debug;
        }
        private bool IsDebug()
        {
            switch (debugMode)
            {
                case DEBUG.OPENNOSAVE:
                case DEBUG.OPENANDSAVE:
                    return true;
                default:
                    return false;
            }
        }
        public void InitBaseProperties()
        {
            this.egBaseProperties.Clear();
            this.egBaseProperties.Add("$lib", PlatForm.DotNet.GetEnumDescription());
            this.egBaseProperties.Add("$lib_version", SDK_VERSION);
        }
        public void RegisterSuperProperties(Dictionary<string, object> superProperties)
        {
            int maxNum = 100;
            if (superProperties.Count > maxNum)
                throw new Exception("Too many super properties. max number is 100.");
            ValidHandle.CheckParam("", superProperties);
            IEnumerator<KeyValuePair<string, object>> dem = superProperties.GetEnumerator();
            while (dem.MoveNext())
            {
                this.xcontextSuperProperties.Add(dem.Current.Key, dem.Current.Value);
            }
            if (IsDebug())
            {
                Console.WriteLine("RegisterSuperProperties success");
            }
        }
        public void UnRegisterSuperProperty(string key)
        {
            if (this.xcontextSuperProperties.ContainsKey(key))
            {
                this.xcontextSuperProperties.Remove(key);
            }
            if (IsDebug())
            {
                Console.WriteLine(string.Format("UnRegisterSuperProperty Key[{0}] success", key));
            }
        }

        public object GetSuperProperty(string key)
        {
            if (this.xcontextSuperProperties.ContainsKey(key))
            {
                bool flag = this.xcontextSuperProperties.TryGetValue(key, out object superPropertie);
                return flag ? superPropertie : null;
            }
            return null;
        }

        public Dictionary<string, object> GetSuperProperties()
        {
            return this.xcontextSuperProperties;
        }

        public void ClearSuperProperties()
        {
            this.xcontextSuperProperties.Clear();
            if (IsDebug())
            {
                Console.WriteLine("ClearSuperProperties success");
            }
        }

        public void Flush()
        {
            this.collecter.Flush();
        }

        public void Shutdown()
        {
            this.collecter.Close();
        }

        /**
	    * 设置用户的属性
	    * @param distinctId 用户ID
	    * @param isLogin 用户ID是否是登录 ID
	    * @param properties 用户属性
	    * @param platform 平台类型
	    */
        public void ProfileSet(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform)
        {
            Upload(distinctId, isLogin, EventName.P_SET.GetEnumDescription(), properties, platform);
        }

        /**
         * 首次设置用户的属性,该属性只在首次设置时有效
         * @param distinctId 用户ID
         * @param isLogin 用户ID是否是登录 ID
         * @param properties 用户属性
         * @param platform 平台类型
         */
        public void ProfileSetOnce(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform)
        {
            Upload(distinctId, isLogin, EventName.P_SET_ONE.GetEnumDescription(), properties, platform);
        }

        /**
         * 为用户的一个或多个数值类型的属性累加一个数值
         * @param distinctId 用户ID
         * @param isLogin 用户ID是否是登录 ID
         * @param properties 用户属性
         * @param platform 平台类型
         */
        public void ProfileIncrement(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform)
        {
            Upload(distinctId, isLogin, EventName.P_IN.GetEnumDescription(), properties, platform);
        }

        /**
         * 追加用户列表类型的属性
         * @param distinctId 用户ID
         * @param isLogin 用户ID是否是登录 ID
         * @param properties 用户属性
         * @param platform 平台类型
         */
        public void ProfileAppend(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform)
        {
            Upload(distinctId, isLogin, EventName.P_APP.GetEnumDescription(), properties, platform);
        }

        /**
         * 删除用户某一个属性
         * @param distinctId 用户ID
         * @param isLogin 用户ID是否是登录 ID
         * @param property 用户属性名称
         * @param platform 平台类型
         */
        public void ProfileUnSet(string distinctId, bool isLogin, string property, string platform)
        {
            Dictionary<String, Object> properties = new Dictionary<string, object>(2);
            properties.Add(property, "");
            Upload(distinctId, isLogin, EventName.P_UN.GetEnumDescription(), properties, platform);
        }

        /**
         * 删除用户所有属性
         * @param distinctId 用户ID
         * @param isLogin 用户ID是否是登录 ID
         * @param platform 平台类型
         * @throws AnalysysException 自定义异常
         */
        public void ProfileDelete(string distinctId, bool isLogin, string platform)
        {
            Upload(distinctId, isLogin, EventName.P_DEL.GetEnumDescription(), new Dictionary<string, object>(1), platform);
        }

        /**
         * 关联用户匿名ID和登录ID
         * @param aliasId 用户登录ID
         * @param distinctId 用户匿名ID
         * @param platform 平台类型
         */
        public void Alias(string aliasId, string distinctId, string platform)
        {
            Dictionary<string, object> param = new Dictionary<string, object>(2);
            param.Add("$original_id", distinctId);
            Upload(aliasId, true, EventName.ALIAS.GetEnumDescription(), param, platform);
        }

        /**
         * 追踪用户多个属性的事件
         * @param distinctId 用户ID
         * @param isLogin 用户ID是否是登录 ID
         * @param eventName 事件名称
         * @param properties 事件属性
         * @param platform 平台类型k
         * @param platform 平台类型k
         */
        public void Track(string distinctId, bool isLogin, string eventName, Dictionary<string, object> properties, string platform)
        {
            Upload(distinctId, isLogin, eventName, properties, platform);
        }

        private void Upload(string distinctId, bool isLogin, string eventName, Dictionary<string, object> properties, string platform)
        {
            ValidHandle.CheckProperty(distinctId, eventName, properties, this.xcontextSuperProperties.Count);
            Dictionary<string, object> eventMap = new Dictionary<string, object>(8);
            eventMap.Add("xwho", distinctId);
            eventMap.Add("xwhen", (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            eventMap.Add("xwhat", eventName);
            eventMap.Add("appid", appId);
            Dictionary<string, object> newProperties = new Dictionary<string, object>(16);
            string profile = "$profile";
            if (!eventName.StartsWith(profile) && !eventName.StartsWith(EventName.ALIAS.GetEnumDescription()))
            {
                AddDictionary(newProperties, xcontextSuperProperties);
            }
            newProperties.Add("$debug", (int)debugMode);
            if (properties != null)
            {
                AddDictionary(newProperties, properties);
            }
            AddDictionary(newProperties, egBaseProperties);
            newProperties.Add("$is_login", isLogin);
            string newPlatForm = GetPlatForm(platform);
            if (newPlatForm != null && newPlatForm.Trim().Length > 0)
            {
                newProperties.Add("$platform", newPlatForm);
            }
            eventMap.Add("xcontext", newProperties);
            this.collecter.Debug(IsDebug());
            bool ret = this.collecter.Send(eventMap);
            if (eventName.StartsWith(profile) && IsDebug() && ret)
            {
                Console.WriteLine(string.Format("{0} success.", eventName.Substring(1)));
            }
        }

        private string GetPlatForm(string platform)
        {
            if (PlatForm.JS.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.JS.GetEnumDescription(); }
            if (PlatForm.WeChat.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.WeChat.GetEnumDescription(); }
            if (PlatForm.Android.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.Android.GetEnumDescription(); }
            if (PlatForm.iOS.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.iOS.GetEnumDescription(); }
            if (PlatForm.DotNet.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.DotNet.GetEnumDescription(); }
            Console.WriteLine(string.Format("Warning: param platform:{0}  Your input are not:iOS/Android/JS/WeChat/DotNet.", platform == null ? "" : platform));
            if (PlatForm.Java.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.Java.GetEnumDescription(); }
            if (PlatForm.Python.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.Python.GetEnumDescription(); }
            if (PlatForm.Node.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.Node.GetEnumDescription(); }
            if (PlatForm.PHP.GetEnumDescription().Equals(platform, StringComparison.CurrentCultureIgnoreCase)) { return PlatForm.PHP.GetEnumDescription(); }
            if (platform == null || platform.Trim().Length == 0) { return ""; }
            return platform;
        }


        private void AddDictionary(Dictionary<string, object> src1, Dictionary<string, object> src2)
        {
            if (src2 == null) return;
            if (src1 == null) src1 = new Dictionary<string, object>();
            foreach (var kv in src2)
            {
                src1.Add(kv.Key,kv.Value);
            }
        }
    }
}
