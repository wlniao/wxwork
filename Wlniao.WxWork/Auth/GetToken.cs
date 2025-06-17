using System;
using System.Collections.Generic;

namespace Wlniao.WxWork.Auth
{
    /// <summary>
    /// 获取access_token 的请求参数
    /// </summary>
    public class GetToken : Context
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public GetToken(Request.GetTokenRequest obj)
        {
            base.Method = "GET";
            base.ApiPath = "/cgi-bin/gettoken?corpid=" + obj.corpid + "&corpsecret=" + obj.corpsecret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="rlt"></param>
        public override void CheckRespose<TResponse>(ApiResult<TResponse> rlt)
        {
            var res = rlt.data as Response.GetTokenResponse;
            if (res == null)
            {
                rlt.message = "内容输出无效";
            }
            else
            {
                rlt.code = res.errcode.ToString();
                rlt.message = res.errmsg;
                if (res.errcode == "0" && !string.IsNullOrEmpty(res.access_token))
                {
                    rlt.success = true;
                }
            }
        }
    }
}