using System.Collections.Generic;

namespace Analysys
{
    public interface ICollecter
    {
        /// <summary>
        /// 发送/缓存消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <returns>是否成功</returns>
        bool Send(Dictionary<string, object> message);
        /// <summary>
        /// 上报数据
        /// </summary>
        void Upload();
        /// <summary>
        /// 刷新缓存
        /// </summary>
        void Flush();
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
        /// <summary>
        /// 设定调试模式DEBUG
        /// </summary>
        /// <param name="isDebugModel"></param>
        void Debug(bool isDebugModel);
    }
}
