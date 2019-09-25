using System;
using System.ComponentModel;
using System.Reflection;

namespace Analysys
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>枚举的描述</returns>
        public static string GetEnumDescription(this Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
    }
}
