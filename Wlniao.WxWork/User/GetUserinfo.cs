using System;
using System.Collections.Generic;

namespace Wlniao.WxWork.User
{
    /// <summary>
    /// 获取访问用户身份
    /// </summary>
    public class GetUserinfo : Context
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public GetUserinfo(string code)
        {
            base.Method = "GET";
            base.ApiPath = "/cgi-bin/user/getuserinfo?access_token=ACCESS_TOKEN&code=" + code;
            base.TokenRequired = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public GetUserinfo(Request.GetUserinfoRequest obj)
        {
            base.Method = "GET";
            base.ApiPath = "/cgi-bin/user/getuserinfo?access_token=" + obj.access_token + "&code=" + obj.code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="rlt"></param>
        public override void CheckRespose<TResponse>(ApiResult<TResponse> rlt)
        {
            var res = rlt.data as Response.GetUserinfoResponse;
            if (res == null)
            {
                rlt.message = "内容输出无效";
            }
            else
            {
                rlt.code = res.errcode.ToString();
                rlt.message = res.errmsg;
                if (res.errcode == "0" && !string.IsNullOrEmpty(res.UserId))
                {
                    rlt.success = true;
                }
            }
        }

    }
}