using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Wlniao.Handler;
namespace Wlniao.WxWork
{
    /// <summary>
    /// 
    /// </summary>
    public class WxWorkHandler : PipelineHandler
    {
        private Dictionary<string, ResponseEncoder> EncoderMap;
        private Dictionary<string, ResponseDecoder> DecoderMap;
        private delegate void ResponseEncoder(Context ctx);
        private delegate void ResponseDecoder(Context ctx);

        /// <summary>
        /// 
        /// </summary>
        public WxWorkHandler(PipelineHandler handler) : base(handler)
        {
            EncoderMap = new Dictionary<string, ResponseEncoder>() {
                { "gettoken", GetTokenEncode },
                { "getuserinfo", GetUserinfoEncode },
                { "convert_to_openid", ConvertToOpenidEncode },
                { "menu_create", MenuCreateEncode },
            };
            DecoderMap = new Dictionary<string, ResponseDecoder>() {
                { "gettoken", GetTokenDecode },
                { "getuserinfo", GetUserinfoDecode },
                { "convert_to_openid", ConvertToOpenidDecode },
                { "menu_create", MenuCreateDecode },
            };
        }

        #region Handle
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        public override void HandleBefore(IContext ctx)
        {
            var _ctx = (Context)ctx;
            EncoderMap[_ctx.Operation](_ctx);
            if (string.IsNullOrEmpty(_ctx.RequestPath))
            {
                _ctx.RequestPath = _ctx.Operation;
            }
            inner.HandleBefore(ctx);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        public override void HandleAfter(IContext ctx)
        {
            inner.HandleAfter(ctx);
            var _ctx = (Context)ctx;
            DecoderMap[_ctx.Operation](_ctx);
        }
        #endregion

        #region GetToken
        private void GetTokenEncode(Context ctx)
        {
            var req = ctx.Request as Request.GetTokenRequest;
            if (req != null)
            {
                ctx.Method = System.Net.Http.HttpMethod.Get;
                ctx.RequestPath = "/cgi-bin/gettoken"
                    + "?corpid=" + req.corpid + "&corpsecret=" + req.agentsecret;
            }
        }
        private void GetTokenDecode(Context ctx)
        {
            try
            {
                ctx.Response = JsonConvert.DeserializeObject<Response.GetTokenResponse>(ctx.HttpResponseString);
            }
            catch
            {
                ctx.Response = new Error() { errmsg = "InvalidJsonString" };
            }
        }
        #endregion

        #region GetUserinfo
        private void GetUserinfoEncode(Context ctx)
        {
            var req = ctx.Request as Request.GetUserinfoRequest;
            if (req != null)
            {
                if (string.IsNullOrEmpty(req.access_token))
                {
                    ctx.Response = new Error() { errmsg = "missing access_token" };
                    return;
                }
                ctx.Method = System.Net.Http.HttpMethod.Get;
                ctx.RequestPath = "/cgi-bin/user/getuserinfo"
                    + "?code=" + req.code + "&access_token=" + req.access_token;
            }
        }
        private void GetUserinfoDecode(Context ctx)
        {
            try
            {
                ctx.Response = JsonConvert.DeserializeObject<Response.GetUserinfoResponse>(ctx.HttpResponseString);
            }
            catch
            {
                ctx.Response = new Error() { errmsg = "InvalidJsonString" };
            }
        }
        #endregion

        #region ConvertToOpenid
        private void ConvertToOpenidEncode(Context ctx)
        {
            var req = ctx.Request as Request.ConvertToOpenidRequest;
            if (req != null)
            {
                if (string.IsNullOrEmpty(req.access_token))
                {
                    ctx.Response = new Error() { errmsg = "missing access_token" };
                    return;
                }
                ctx.Method = System.Net.Http.HttpMethod.Post;
                ctx.HttpRequestString = JsonConvert.SerializeObject(new { req.userid });
                ctx.RequestPath = "cgi-bin/user/convert_to_openid"
                    + "?access_token=" + req.access_token;
            }
        }
        private void ConvertToOpenidDecode(Context ctx)
        {
            try
            {
                ctx.Response = JsonConvert.DeserializeObject<Response.ConvertToOpenidResponse>(ctx.HttpResponseString);
            }
            catch
            {
                ctx.Response = new Error() { errmsg = "InvalidJsonString" };
            }
        }
        #endregion

        #region MenuCreate
        private void MenuCreateEncode(Context ctx)
        {
            var req = ctx.Request as Request.MenuCreateRequest;
            if (req != null)
            {
                if (string.IsNullOrEmpty(req.meuncontent))
                {
                    ctx.Response = new Error() { errmsg = "missing meuncontent" };
                    return;
                }
                ctx.HttpRequestString = req.meuncontent;
                ctx.RequestPath = "/cgi-bin/menu/create"
                    + "?access_token=" + req.access_token
                    + "&agentid=" + req.agentid;
            }
        }
        private void MenuCreateDecode(Context ctx)
        {
            try
            {
                ctx.Response = JsonConvert.DeserializeObject<Response.MenuCreateResponse>(ctx.HttpResponseString);
            }
            catch
            {
                ctx.Response = new Error() { errmsg = "InvalidJsonString" };
            }
        }
        #endregion

    }
}