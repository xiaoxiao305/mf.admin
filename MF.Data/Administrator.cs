using System;
namespace MF.Data
{
    /// <summary>
    /// 管理员实体类
    /// </summary>
    [Serializable]
   public class Administrator
    {
       /// <summary>
       /// 获取或设置管理员ID
       /// </summary>
        public int ID{get;set;}
        
        /// <summary>
        ///获取或设置系统管理员账号 
        /// </summary>
        public string Account
        { get; set; }
        /// <summary>
        /// 获取或设置系统管理员密码
        /// </summary>
        public string Password
        { get; set; }
        /// <summary>
        /// 获取或设置系统管理员状态
        /// 1正常，0冻结
        /// </summary>
        public int Flag
        { get; set; }
        /// <summary>
        /// 获取或设置系统管理员姓名
        /// </summary>
        public string Name
        { get; set; }
        /// <summary>
        /// 获取或设置系统管理员上一次登录IP
        /// </summary>
        public string LastIP
        { get; set; }
        /// <summary>
        /// 获取或设置系统管理员上一次登录时间
        /// </summary>
        public int LastLogin
        { get; set; }
        /// <summary>
        /// 获取或设置系统管理员绑定的手机安全令
        /// </summary>
        public string Token
        { get; set; }
        /// <summary>
        /// 获取或设置系统管理员权限
        /// </summary>
        public string Powers
        { get; set; }
        /// <summary>
        /// 账号类型
        /// 1超级管理员
        /// 2客服管理员
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 当前账号type下的管理员与否
        /// </summary>
        public int IsAdmin { get; set; }
        /// <summary>
        /// 用户设备信息
        /// </summary>
        public string hd { get; set; }

    }
}
