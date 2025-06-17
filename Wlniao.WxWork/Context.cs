﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Wlniao.WxWork
{
    /// <summary>
    /// 请求线程
    /// </summary>
    public abstract class Context : Wlniao.Handler.Context
    {
        /// <summary>
        /// 接口凭据是否为必须
        /// </summary>
        internal protected bool TokenRequired { get; set; }
        /// <summary>
        /// 请求的Headers参数
        /// </summary>
        public Dictionary<String, String> HttpRequestHeaders;
        /// <summary>
        /// 输出的Headers参数
        /// </summary>
        public Dictionary<String, String> HttpResponseHeaders;
        /// <summary>
        /// 输出检查方法
        /// </summary>
        public abstract void CheckRespose<TResponse>(ApiResult<TResponse> rlt) where TResponse : Wlniao.Handler.IResponse;
    }
}