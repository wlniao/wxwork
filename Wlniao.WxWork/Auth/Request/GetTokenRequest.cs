using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.Auth.Request
{
    /// <summary>
    /// 获取access_token 的请求参数
    /// </summary>
    public class GetTokenRequest : Wlniao.Handler.IRequest
    {
        /// <summary>
        /// 企业ID，获取方式参考：https://work.weixin.qq.com/api/doc#90000/90135/90665/corpid
        /// </summary>
        public string corpid { get; set; }
        /// <summary>
        /// 应用的凭证密钥，获取方式参考：https://work.weixin.qq.com/api/doc#90000/90135/90665/secret
        /// </summary>
        public string corpsecret { get; set; }
    }
}