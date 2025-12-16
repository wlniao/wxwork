using System;
using System.Collections.Generic;
namespace Wlniao.WxWork
{
    /// <summary>
    /// 基础输出
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 出错返回码，为0表示成功，非0表示调用失败
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 返回码提示语
        /// </summary>
        public string errmsg { get; set; }
    }
}
