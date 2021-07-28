using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wlniao.WxWork.Request;
using Wlniao.WxWork.Response;
namespace Wlniao.WxWork
{
    /// <summary>
    /// 企业微信内部开发客户端
    /// </summary>
    public class Client : Wlniao.Handler.IClient
    {
        #region 企业内部开发配置信息
        internal static string _CorpId = null;      //企业团体Id
        internal static string _AgentId = null;     //企业内部应用Id
        internal static string _AgentSecret = null; //企业内部应用开发密钥
        /// <summary>
        /// 企业团体Id
        /// </summary>
        public static string CfgCorpId
        {
            get
            {
                if (_CorpId == null)
                {
                    _CorpId = Config.GetSetting("CorpId");
                }
                return _CorpId;
            }
        }
        /// <summary>
        /// 企业内部应用Id
        /// </summary>
        public static string CfgAgentId
        {
            get
            {
                if (_AgentId == null)
                {
                    _AgentId = Config.GetSetting("AgentId");
                }
                return _AgentId;
            }
        }
        /// <summary>
        /// 企业内部应用开发密钥
        /// </summary>
        public static string CfgAgentSecret
        {
            get
            {
                if (_AgentSecret == null)
                {
                    _AgentSecret = Config.GetSetting("AgentSecret");
                }
                return _AgentSecret;
            }
        }
        #endregion

        /// <summary>
        /// 企业团体Id
        /// </summary>
        public string CorpId { get; set; }
        /// <summary>
        /// 企业内部应用Id
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// 企业内部开发密钥
        /// </summary>
        public string AgentSecret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Handler handler = null;
        /// <summary>
        /// 
        /// </summary>
        public Client()
        {
            this.CorpId = CfgCorpId;
            this.AgentId = CfgAgentId;
            this.AgentSecret = CfgAgentSecret;
            handler = new Handler();
        }
        /// <summary>
        /// 
        /// </summary>
        public Client(String CorpId, String AgentId, String AgentSecret)
        {
            this.CorpId = CorpId;
            this.AgentId = AgentId;
            this.AgentSecret = AgentSecret;
            handler = new Handler();
        }


        private Task<ApiResult<TResponse>> CallAsync<TRequest, TResponse>(string operation, TRequest request, System.Net.Http.HttpMethod method)
            where TResponse : Wlniao.Handler.IResponse, new()
            where TRequest : Wlniao.Handler.IRequest
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            var ctx = new Context();
            ctx.CorpId = CorpId;
            ctx.AgentId = AgentId;
            ctx.AgentSecret = AgentSecret;
            ctx.Method = method == null ? System.Net.Http.HttpMethod.Get : method;
            ctx.Operation = operation;
            ctx.Request = request;
            ctx.RequestHost = "https://qyapi.weixin.qq.com";

            handler.HandleBefore(ctx);
            if (ctx.Response == null)
            {
                return ctx.HttpTask.ContinueWith((t) =>
                {
                    handler.HandleAfter(ctx);
                    if (ctx.Response is Error)
                    {
                        var err = (Error)ctx.Response;
                        return new ApiResult<TResponse>() { success = false, message = err.errmsg, code = err.errcode.ToString() };
                    }
                    return new ApiResult<TResponse>() { success = true, message = "success", data = (TResponse)ctx.Response };
                });
            }
            else
            {
                if (ctx.Response is Error)
                {
                    var err = (Error)ctx.Response;
                    return Task<ApiResult<TResponse>>.Run(() =>
                    {
                        return new ApiResult<TResponse>() { success = false, message = err.errmsg, code = err.errcode.ToString() };
                    });
                }
                else
                {
                    return Task<ApiResult<TResponse>>.Run(() =>
                    {
                        return new ApiResult<TResponse>() { success = true, message = "error", data = (TResponse)ctx.Response };
                    });
                }
            }
        }
        private TResponse GetResponseFromAsyncTask<TResponse>(Task<TResponse> task)
        {
            try
            {
                task.Wait();
            }
            catch (System.AggregateException e)
            {
                log.Error(e.Message);
                throw e.GetBaseException();
            }

            return task.Result;
        }



        #region GetToken 获取access_token
        /// <summary>
        /// 获取access_token
        /// </summary>
        public string GetToken(String secret = null)
        {
            var token = Wlniao.Cache.Get("wxwork_access_token_" + CorpId);
            if (string.IsNullOrEmpty(token))
            {
                var rlt = GetResponseFromAsyncTask(CallAsync<GetTokenRequest, GetTokenResponse>("gettoken"
                    , new GetTokenRequest() { corpid = CorpId, agentsecret = string.IsNullOrEmpty(secret) ? AgentSecret : secret }
                    , System.Net.Http.HttpMethod.Get));
                if (rlt.success && rlt.data != null && rlt.data.expires_in > 0)
                {
                    token = rlt.data.access_token;
                    Wlniao.Cache.Set("wxwork_access_token_" + CorpId, token, rlt.data.expires_in);
                }
            }
            return token;
        }
        #endregion 

        #region GetUserinfo 获取访问用户身份
        /// <summary>
        /// 获取access_token
        /// </summary>
        public ApiResult<GetUserinfoResponse> GetUserinfo(String code, String token = null)
        {
            return GetResponseFromAsyncTask(CallAsync<GetUserinfoRequest, GetUserinfoResponse>("getuserinfo"
                , new GetUserinfoRequest() { code = code, access_token = string.IsNullOrEmpty(token) ? GetToken() : token }
                , System.Net.Http.HttpMethod.Get));
        }
        #endregion 

        #region ConvertToOpenid userid与openid互换
        /// <summary>
        /// 获取access_token
        /// </summary>
        public string ConvertToOpenid(String userid, String token = null)
        {
            var rlt = GetResponseFromAsyncTask(CallAsync<ConvertToOpenidRequest, ConvertToOpenidResponse>("convert_to_openid"
                , new ConvertToOpenidRequest() { userid = userid, access_token = string.IsNullOrEmpty(token) ? GetToken() : token }
                , System.Net.Http.HttpMethod.Get));
            if (rlt.success && rlt.data != null && !string.IsNullOrEmpty(rlt.data.openid))
            {
                return rlt.data.openid;
            }
            return "";
        }
        #endregion 


        #region MenuCreate 创建应用自定义菜单
        /// <summary>
        /// 创建应用自定义菜单
        /// </summary>
        public ApiResult<MenuCreateResponse> MenuCreate(String menucontent, String token = null)
        {
            return GetResponseFromAsyncTask(CallAsync<MenuCreateRequest, MenuCreateResponse>("menu_create"
                , new MenuCreateRequest() {meuncontent= menucontent, agentid = AgentId, access_token = string.IsNullOrEmpty(token) ? GetToken() : token }
                , System.Net.Http.HttpMethod.Post));
        }
        #endregion 
    }
}