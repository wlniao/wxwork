using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.Request
{
    /// <summary>
    /// 获取访问用户身份 的请求参数
    /// </summary>
    public class GetUserinfoRequest : Wlniao.Handler.IRequest
    {
        /// <summary>
        /// 通过成员授权获取到的code，最大为512字节。每次成员授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 调用接口凭证
        /// </summary>
        public string access_token { get; set; }
    }
}