using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Analysys
{
    public class ValidHandle
    {
        private const string KEY_PATTERN = "^((?!^xwhat$|^xwhen$|^xwho$|^appid$|^xcontext$|^\\$lib$|^\\$lib_version$)^[$a-zA-Z][$a-zA-Z0-9_]{0,98})$";
        private const string KEY_PATTERN_CONTEXT = "^((?!^xwhat$|^xwhen$|^xwho$|^appid$|^xcontext$|^\\$lib$|^\\$lib_version$)^[$a-zA-Z][$a-zA-Z0-9_]{0,124})$";
        /**
         * 属性参数格式校验
         * @param eventName 事件名称
         * @param properties 属性
         * @throws AnalysysException 自定义异常
         */
        public static void CheckParam(string eventName, Dictionary<string, object> properties)
        {

            int idLength = 255;
            int keyLength = 125;
            int valueListLen = 100;
            string piEventName = "$profile_increment";
            string paEventName = "$profile_append";
            if (properties == null)
            {
                properties = new Dictionary<string, object>(1);
            }
            foreach (KeyValuePair<string, object> property in properties)
            {
                if (property.Value == null)
                    continue;
                //key约束 符合java命名规则： 开头约束:字母或者$ 字符类型:大小写字母、数字、下划线和 $ 最大长度125字符
                if (property.Key.Length > keyLength)
                {
                    throw new Exception(string.Format("The property key {0} is too long, max length is {1}.", property.Key, keyLength));
                }
                if (!Regex.Match(property.Key, KEY_PATTERN_CONTEXT).Success)
                {
                    throw new Exception(string.Format("The property key {0} is invalid.", property.Key));
                }
                int temp = 0;//没有任何意思，只是为了满足语法的要求
                if (!int.TryParse(property.Value.ToString(), out temp) &&
                        !(property.Value is bool) &&
                        !(property.Value is string) &&
                        !(property.Value is Array) &&
                        !(property.Value.GetType().GetGenericTypeDefinition() == typeof(List<>)))
                {
                    throw new Exception(string.Format("The property {0} is not Number, String, Boolean, List<String>.", property.Value.ToString()));
                }
                if (property.Value is string && property.Value.ToString().Length > idLength)
                {
                    throw new Exception(string.Format("The property String value {0} is too long, max length is {1}.", property.Value, idLength));
                }
                //数组集合约束 数组或集合内最多包含100条,若为字符串数组或集合,每条最大长度255个字符
                if (property.Value.GetType().IsGenericType && property.Value.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    IList valueList = (IList)property.Value;
                    if (valueList.Count > valueListLen)
                    {
                        throw new Exception(string.Format("The property {0} value {1}, max number should be {2}.", property.Key, property.Value, valueListLen));
                    }
                    foreach (object vals in valueList)
                    {
                        if (!(vals is string))
                        {
                            throw new Exception(string.Format("The property {0} should be a list of String.", property.Key));
                        }
                        if (vals.ToString().Length > idLength)
                        {
                            throw new Exception(string.Format("The property {0} some value is too long, max length is {1}.", property.Key, idLength));
                        }
                    }
                }
                if (piEventName.Equals(eventName) && !(int.TryParse(property.Value.ToString(), out temp)))
                {
                    throw new Exception(string.Format("The property value of {0} should be a Number.", property.Key));
                }
                if (paEventName.Equals(eventName))
                {
                    if (!(property.Value.GetType().IsGenericType && property.Value.GetType().GetGenericTypeDefinition() == typeof(List<>)) && !(property.Value is Array))
                    {
                        throw new Exception(string.Format("The property value of {0} should be a List<String>.", property.Key));
                    }
                }
            }
        }
        /**
         * 格式校验
         * @param distinctId 用户标识
         * @param eventName 事件名称
         * @param properties 属性
         * @param commProLen 公共属性长度
         * @throws AnalysysException 自定义异常
         */
        public static void CheckProperty(string distinctId, string eventName, Dictionary<string, object> properties, int commProLen)
        {
            string aliasEventName = "$alias";
            string profileEventName = "$profile";
            string originalId = "$original_id";
            int eventNameLen = 99;
            int connonParamLen = 5;
            int idLength = 255;
            int totalParamLen = 300;
            if (properties == null)
            {
                properties = new Dictionary<string, object>(1);
            }
            if (distinctId == null || distinctId.Length == 0)
            {
                throw new Exception(string.Format("aliasId {0} is empty.", distinctId));
            }
            if (distinctId.Length > idLength)
            {
                throw new Exception(string.Format("aliasId {0} is too long, max length is {1}.", properties[originalId], idLength));
            }
            if (aliasEventName.Equals(eventName))
            {
                if (properties[originalId] == null || properties[originalId].ToString().Length == 0)
                {
                    throw new Exception(string.Format("original_id {0} is empty.", properties[originalId].ToString()));
                }
                if (properties[originalId].ToString().Length > idLength)
                {
                    throw new Exception(string.Format("original_id {0} is too long, max length is {1}.", properties[originalId].ToString(), idLength));
                }
            }

            if (eventName == null || eventName.Length == 0)
            {
                throw new Exception(string.Format("EventName {0} is empty.", eventName));
            }
            if (eventName.Length > eventNameLen)
            {
                throw new Exception(string.Format("EventName {0} is too long, max length is {1}.", eventName, eventNameLen));
            }

            if (!Regex.Match(eventName, KEY_PATTERN).Success)
            {
                throw new Exception(string.Format("EventName {0} is invalid.", eventName));
            }
            //xcontext属性值不大于300个
            if (!eventName.StartsWith(profileEventName) && !eventName.StartsWith(aliasEventName))
            {
                if (properties.Count + commProLen + connonParamLen > totalParamLen)
                {
                    throw new Exception(string.Format("Too many attributes. max number is {0}.", (totalParamLen - commProLen - connonParamLen)));
                }
            }
            else
            {
                if (properties.Count + connonParamLen > totalParamLen)
                {
                    throw new Exception(string.Format("Too many attributes. max number is {0}.", (totalParamLen - connonParamLen)));
                }
            }
            CheckParam(eventName, properties);
        }
    }
}
