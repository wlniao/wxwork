using System;
using System.Collections.Generic;

namespace Wlniao.WxWork.Department
{
    /// <summary>
    /// 获取access_token 的请求参数
    /// </summary>
    public class DepartmentList : Context
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public DepartmentList(String id = "1")
        {
            base.Method = "GET";
            base.ApiPath = "/cgi-bin/department/list?access_token=ACCESS_TOKEN&id=" + id;
            base.TokenRequired = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="rlt"></param>
        public override void CheckRespose<TResponse>(ApiResult<TResponse> rlt)
        {
            var res = rlt.data as Response.DepartmentListResponse;
            if (res == null)
            {
                rlt.message = "内容输出无效";
            }
            else
            {
                rlt.code = res.errcode.ToString();
                rlt.message = res.errmsg;
                if (res.errcode == "0")
                {
                    if (res.department == null)
                    {
                        res.department = new List<Models.Department>();
                    }
                    rlt.success = true;
                }
            }
        }
    }
}