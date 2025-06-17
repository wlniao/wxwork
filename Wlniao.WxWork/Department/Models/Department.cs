using System;
using System.Collections.Generic;
using System.Text;

namespace Wlniao.WxWork.Department.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Department
    {
        /// <summary>
        /// 创建的部门id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 部门名称，代开发自建应用需要管理员授权才返回；第三方应用不返回
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 部门负责人的UserID；第三方仅通讯录应用可获取
        /// </summary>
        public List<string> department_leader { get; set; }
        /// <summary>
        /// 父部门id。根部门为1
        /// </summary>
        public string parentid { get; set; }
        /// <summary>
        /// 在父部门中的次序值
        /// </summary>
        public string order { get; set; }
    }
}