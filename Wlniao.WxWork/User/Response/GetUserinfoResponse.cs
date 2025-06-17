using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.User.Response
{
    /// <summary>
    /// 获取访问用户身份 的输出内容
    /// </summary>
    public class GetUserinfoResponse : BaseResponse
    {
        /// <summary>
        /// 成员UserID。若需要获得用户详情信息，可调用通讯录接口：读取成员
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 非企业成员的标识，对当前企业唯一
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 手机设备号(由企业微信在安装时随机生成，删除重装会改变，升级不受影响)
        /// </summary>
        public string DeviceId { get; set; }
    }
}
