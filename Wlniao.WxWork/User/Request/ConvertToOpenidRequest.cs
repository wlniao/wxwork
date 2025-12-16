using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.User.Request
{
    /// <summary>
    /// userid与openid互换 的请求参数
    /// </summary>
    public class ConvertToOpenidRequest
    {
        /// <summary>
        /// 企业内的成员id
        /// </summary>
        public string userid { get; set; }
    }
}