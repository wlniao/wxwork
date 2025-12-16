using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Wlniao.WxWork
{
    /// <summary>
    /// 请求线程
    /// </summary>
    public abstract class Context
    {
        /// <summary>
        /// 接口凭据是否为必须
        /// </summary>
        protected internal bool TokenRequired { get; set; }
        /// <summary>
        /// 请求的Headers参数
        /// </summary>
        public new Dictionary<string, string> HttpRequestHeaders;
        /// <summary>
        /// 输出的Headers参数
        /// </summary>
        public new Dictionary<string, string> HttpResponseHeaders;
        /// <summary>
        /// 输出检查方法
        /// </summary>
        public abstract void CheckRespose<TResponse>(ApiResult<TResponse> rlt);
    }
}