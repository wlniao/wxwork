using System;
using System.Collections.Generic;

namespace Wlniao.WxWork.User
{
    /// <summary>
    /// 获取access_token 的请求参数
    /// </summary>
    public class ConvertToOpenid : Context
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public ConvertToOpenid(Request.ConvertToOpenidRequest obj)
        {
            base.Method = "POST";
            base.ApiPath = "/cgi-bin/user/convert_to_openid?access_token=ACCESS_TOKEN";
            base.ResponseBody = obj;
            base.TokenRequired = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="rlt"></param>
        public override void CheckRespose<TResponse>(ApiResult<TResponse> rlt)
        {
            var res = rlt.data as Response.ConvertToOpenidResponse;
            if (res == null)
            {
                rlt.message = "内容输出无效";
            }
            else
            {
                rlt.code = res.errcode.ToString();
                rlt.message = res.errmsg;
                if (res.errcode == "0" && !string.IsNullOrEmpty(res.openid))
                {
                    rlt.success = true;
                }
            }
        }
    }
}