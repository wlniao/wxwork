using System;
using System.Collections.Generic;
namespace Wlniao.WxWork.Department.Response
{
    /// <summary>
    /// 获取部门列表 的输出内容
    /// </summary>
    public class DepartmentListResponse : BaseResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Models.Department> department { get; set; }
    }
}
