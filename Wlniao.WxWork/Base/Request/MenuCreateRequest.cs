using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.Base.Request
{
    /// <summary>
    /// 创建应用自定义菜单 的请求参数
    /// </summary>
    public class MenuCreateRequest
    {
        /// <summary>
        /// 企业内部应用Id
        /// </summary>
        public string agentid { get; set; }
        /// <summary>
        /// 调用接口凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 自定义菜单数据
        /// </summary>
        public string meuncontent { get; set; }
    }
}