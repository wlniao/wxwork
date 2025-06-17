using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.User.Response
{
    /// <summary>
    /// userid与openid互换 的输出内容
    /// </summary>
    public class ConvertToOpenidResponse : BaseResponse
    {
        /// <summary>
        /// 企业微信成员userid对应的openid
        /// </summary>
        public string openid { get; set; }
    }
}
