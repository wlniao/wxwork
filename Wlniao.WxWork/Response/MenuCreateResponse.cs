using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.Response
{
    /// <summary>
    /// 创建应用自定义菜单 的输出内容
    /// </summary>
    public class MenuCreateResponse : Wlniao.Handler.IResponse
    {
        /// <summary>
        /// 出错返回码，为0表示成功，非0表示调用失败
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 返回码提示语
        /// </summary>
        public string errmsg { get; set; }
    }
}
