using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wlniao.WxWork.Auth;
using Wlniao.WxWork.Auth.Request;
using Wlniao.WxWork.Auth.Response;
namespace Wlniao.WxWork
{
    /// <summary>
    /// 企业微信内部开发客户端
    /// </summary>
    public class Client : Wlniao.Handler.IClient
    {
        #region 企业内部开发配置信息
        internal static string _ApiHost = null;     //企业团体Id
        internal static string _CorpId = null;      //组织机构Id
        internal static string _AgentId = null;     //应用安装Id
        internal static string _Token = null;       //接口通讯凭据
        internal static string _AesKey = null;      //接口通讯密钥
        internal static string _AppKey = null;      //应用开发标识
        internal static string _AppSecret = null;   //企业内部应用开发密钥
        /// <summary>
        /// 开放平台接口前缀
        /// </summary>
        public static string CfgApiHost
        {
            get
            {
                if (_ApiHost == null)
                {
                    _ApiHost = Config.GetSetting("WxWorkHost", "https://qyapi.weixin.qq.com");
                }
                return _ApiHost;
            }
        }
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public static string CfgCorpId
        {
            get
            {
                if (_CorpId == null)
                {
                    _CorpId = Config.GetSetting("WxWorkCorpId");
                }
                return _CorpId;
            }
        }
        /// <summary>
        /// 应用安装Id
        /// </summary>
        public static string CfgAgentId
        {
            get
            {
                if (_AgentId == null)
                {
                    _AgentId = Config.GetSetting("WxWorkAgentId");
                }
                return _AgentId;
            }
        }
        /// <summary>
        /// 接口通讯凭据
        /// </summary>
        public static string CfgToken
        {
            get
            {
                if (_Token == null)
                {
                    _Token = Config.GetSetting("WxWorkToken");
                }
                return _Token;
            }
        }
        /// <summary>
        /// 接口通讯密钥
        /// </summary>
        public static string CfgAesKey
        {
            get
            {
                if (_AesKey == null)
                {
                    _AesKey = Config.GetSetting("WxWorkAesKey");
                }
                return _AesKey;
            }
        }
        /// <summary>
        /// 应用开发标识
        /// </summary>
        public static string CfgAppKey
        {
            get
            {
                if (_AppKey == null)
                {
                    _AppKey = Config.GetSetting("WxWorkAppKey");
                }
                return _AppKey;
            }
        }
        /// <summary>
        /// 应用开发密钥
        /// </summary>
        public static string CfgAppSecret
        {
            get
            {
                if (_AppSecret == null)
                {
                    _AppSecret = Config.GetSetting("WxWorkAppSecret");
                }
                return _AppSecret;
            }
        }
        #endregion

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public string CorpId { get; set; }
        /// <summary>
        /// 应用安装Id
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// 接口通讯凭据
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 接口通讯密钥
        /// </summary>
        public string AesKey { get; set; }
        /// <summary>
        /// 应用开发标识
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用开发密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 是否有AccessToken
        /// </summary>
        /// <returns></returns>
        public Boolean HasAccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(CorpId) && !string.IsNullOrEmpty(AppSecret))
                {
                    var rlt = Handle<GetTokenResponse>(new GetToken(new GetTokenRequest
                    {
                        corpid = CorpId,
                        corpsecret = AppSecret
                    }));
                    if (rlt.success)
                    {
                        AccessToken = rlt.data.access_token;
                    }
                }
                return string.IsNullOrEmpty(AccessToken) ? false : true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Client()
        {
            this.CorpId = CfgCorpId;
            this.AgentId = CfgAgentId;
            this.AesKey = CfgAesKey;
            this.AppKey = CfgAppKey;
            this.AppSecret = CfgAppSecret;
        }
        /// <summary>
        /// 
        /// </summary>
        public Client(String accessToken)
        {
            this.CorpId = CfgCorpId;
            this.AgentId = CfgAgentId;
            this.AesKey = CfgAesKey;
            this.AppKey = CfgAppKey;
            this.AppSecret = CfgAppSecret;
            this.AccessToken = accessToken;
        }
        /// <summary>
        /// 
        /// </summary>
        public Client(String corpId, String agentId, String appSecret)
        {
            this.CorpId = corpId;
            this.AgentId = agentId;
            this.AppSecret = appSecret;
        }
        /// <summary>
        /// 
        /// </summary>
        public Client(String accessToken, String corpId, String agentId, String appSecret)
        {
            this.CorpId = corpId;
            this.AgentId = agentId;
            this.AppSecret = appSecret;
            this.AccessToken = accessToken;
        }

        /// <summary>
        /// 接口调用
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public ApiResult<TResponse> Handle<TResponse>(Context ctx)
            where TResponse : Wlniao.Handler.IResponse
        {
            if (string.IsNullOrEmpty(ctx.ApiHost))
            {
                ctx.ApiHost = CfgApiHost;
            }
            var task = HandleAsync<TResponse>(ctx);
            task.Wait();
            return task.Result;
        }
        /// <summary>
        /// 接口调用（异步）
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public Task<ApiResult<TResponse>> HandleAsync<TResponse>(Context ctx)
            where TResponse : Wlniao.Handler.IResponse
        {
            var result = new ApiResult<TResponse> { node = XCore.WebNode, code = "-1", success = false, message = "unkown error" };
            if (string.IsNullOrEmpty(ctx.ApiHost))
            {
                result.code = "400";
                result.message = "request host not set";
            }
            else if (string.IsNullOrEmpty(ctx.ApiPath))
            {
                result.code = "400";
                result.message = "request path not set";
            }
            else if (ctx.TokenRequired && string.IsNullOrEmpty(AccessToken))
            {
                result.code = "400";
                result.message = "client access_token not set";
            }
            else
            {
                System.Net.Http.HttpClient http = null;
                Task<System.Net.Http.HttpResponseMessage> task = null;
                if (ctx.Certificate == null)
                {
                    http = new System.Net.Http.HttpClient();
                }
                else
                {
                    var handler = new System.Net.Http.HttpClientHandler();
                    handler.ClientCertificateOptions = System.Net.Http.ClientCertificateOption.Manual;
                    handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    handler.ClientCertificates.Add(ctx.Certificate);
                    http = new System.Net.Http.HttpClient(handler);
                }
                http.BaseAddress = new System.Uri(ctx.ApiHost);
                if (ctx.TokenRequired)
                {
                    ctx.ApiPath = ctx.ApiPath.Replace("ACCESS_TOKEN", AccessToken);
                }
                if (ctx.RequestBody == null)
                {
                    task = http.GetAsync(ctx.ApiPath);
                }
                else if (ctx.Method == "GET" && ctx.RequestBody is string)
                {
                    var query = ctx.RequestBody as string;
                    if (!string.IsNullOrEmpty(query))
                    {
                        var link = ctx.ApiPath.IndexOf('?') < 0 ? '?' : '&';
                        ctx.ApiPath += query[0] == '?' || query[0] == '&' ? query : link + query;
                    }
                    task = http.GetAsync(ctx.ApiPath);
                }
                else
                {
                    var text = ctx.RequestBody as string;
                    var bytes = ctx.RequestBody as byte[];
                    if (text == null && bytes == null && ctx.RequestBody != null)
                    {
                        text = Json.ToString(ctx.RequestBody);
                    }
                    if (bytes != null)
                    {
                        task = http.PostAsync(ctx.ApiPath, new System.Net.Http.ByteArrayContent(bytes));
                    }
                    else if (!string.IsNullOrEmpty(text))
                    {
                        task = http.PostAsync(ctx.ApiPath, new System.Net.Http.StringContent(text));
                    }
                    else
                    {
                        text = Newtonsoft.Json.JsonConvert.SerializeObject(ctx.RequestBody);
                        task = http.PostAsync(ctx.ApiPath, new System.Net.Http.StringContent(text));
                    }
                }
                task.Result.Content.ReadAsStringAsync().ContinueWith((res) =>
                {
                    ctx.ResponseBody = res.Result;
                    ctx.HttpResponseHeaders = new Dictionary<String, String>();
                    var status = (int)task.Result.StatusCode;
                    if (status >= 200 && status < 300)
                    {
                        try
                        {
                            result.code = "0";
                            result.data = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(res.Result);
                            result.message = task.Result.ReasonPhrase;
                            foreach (var item in task.Result.Headers)
                            {
                                var em = item.Value.GetEnumerator();
                                if (em.MoveNext())
                                {
                                    ctx.HttpResponseHeaders.Add(item.Key.ToLower(), em.Current);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.code = "-1";
                            result.message = ex.Message;
                        }
                    }
                    else
                    {
                        result.code = task.Result.StatusCode.ToString();
                        result.message = task.Result.ReasonPhrase;
                    }
                }).Wait();
                if (result.code == "0")
                {
                    ctx.CheckRespose(result);
                }
            }
            Wlniao.Log.Loger.Topic("wxwork", "\r\nRequest:+\r\n" + (ctx.RequestBody as string) + "\r\nResponse:+\r\n" + (ctx.ResponseBody as string));
            return Task<ApiResult<TResponse>>.Run(() =>
            {
                return result;
            });
        }



        //#region GetToken 获取access_token
        ///// <summary>
        ///// 获取access_token
        ///// </summary>
        //public string GetToken(String secret = null)
        //{
        //    var token = Wlniao.Cache.Get("wxwork_access_token_" + CorpId);
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        var rlt = GetResponseFromAsyncTask(CallAsync<GetToken, GetTokenResponse>("gettoken"
        //            , new GetToken() { corpid = CorpId, agentsecret = string.IsNullOrEmpty(secret) ? AgentSecret : secret }
        //            , System.Net.Http.HttpMethod.Get));
        //        if (rlt.success && rlt.data != null && rlt.data.expires_in > 0)
        //        {
        //            token = rlt.data.access_token;
        //            Wlniao.Cache.Set("wxwork_access_token_" + CorpId, token, rlt.data.expires_in);
        //        }
        //    }
        //    return token;
        //}
        //#endregion 

        //#region GetUserinfo 获取访问用户身份
        ///// <summary>
        ///// 获取access_token
        ///// </summary>
        //public ApiResult<GetUserinfoResponse> GetUserinfo(String code, String token = null)
        //{
        //    return GetResponseFromAsyncTask(CallAsync<GetUserinfoRequest, GetUserinfoResponse>("getuserinfo"
        //        , new GetUserinfoRequest() { code = code, access_token = string.IsNullOrEmpty(token) ? GetToken() : token }
        //        , System.Net.Http.HttpMethod.Get));
        //}
        //#endregion 

        //#region ConvertToOpenid userid与openid互换
        ///// <summary>
        ///// 获取access_token
        ///// </summary>
        //public string ConvertToOpenid(String userid, String token = null)
        //{
        //    var rlt = GetResponseFromAsyncTask(CallAsync<ConvertToOpenidRequest, ConvertToOpenidResponse>("convert_to_openid"
        //        , new ConvertToOpenidRequest() { userid = userid, access_token = string.IsNullOrEmpty(token) ? GetToken() : token }
        //        , System.Net.Http.HttpMethod.Get));
        //    if (rlt.success && rlt.data != null && !string.IsNullOrEmpty(rlt.data.openid))
        //    {
        //        return rlt.data.openid;
        //    }
        //    return "";
        //}
        //#endregion 


        //#region MenuCreate 创建应用自定义菜单
        ///// <summary>
        ///// 创建应用自定义菜单
        ///// </summary>
        //public ApiResult<MenuCreateResponse> MenuCreate(String menucontent, String token = null)
        //{
        //    return GetResponseFromAsyncTask(CallAsync<MenuCreateRequest, MenuCreateResponse>("menu_create"
        //        , new MenuCreateRequest() {meuncontent= menucontent, agentid = AgentId, access_token = string.IsNullOrEmpty(token) ? GetToken() : token }
        //        , System.Net.Http.HttpMethod.Post));
        //}
        //#endregion 
    }
}